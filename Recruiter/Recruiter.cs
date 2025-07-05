using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using System.Runtime;
using MCM.Abstractions.FluentBuilder;
using MCM.Common;
using MCM.Abstractions.Base.Global;

namespace Recruiter
{
    public class Recruiter : MBSubModuleBase
    {
        bool AllowRecruitElite = true;
        private bool hasAddedMenuToTown = false;

        public static RecruiterSettings Settings { get; private set; }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            Settings = GlobalSettings<RecruiterSettings>.Instance;
            Settings.recruiter = this;
        }
        protected override void OnSubModuleLoad()
        {
            Settings = new RecruiterSettings();
            base.OnSubModuleLoad();
        }
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
        }

        private void getEliteTroops(CharacterObject troop, List<CharacterObject> list)
        {
            foreach (var upgraded in troop.UpgradeTargets)
            {
                if (upgraded.UpgradeTargets.Count() == 0)
                {
                    list.Add(upgraded);
                }
                else
                {
                    getEliteTroops(upgraded, list);
                }
            }
        }

        private void addTroopsMenu(CampaignGameStarter campaignGameStarter, string menu, CharacterObject troop, bool isElite)
        {
            if (isElite && troop.Tier > Settings.MaxRankElite) return;
            if (isElite ==false && troop.Tier > Settings.MaxRankRegular) return;

            string troopMenu = String.Format("{0}_{1}", menu, troop.ToString().Replace(" ", ""));

            campaignGameStarter.AddGameMenu(troopMenu, "How many troops do you want to hire", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, this);

            foreach (var t in troop.UpgradeTargets)
            {
                addTroopsMenu(campaignGameStarter, menu, t, isElite);
            }

            campaignGameStarter.AddGameMenuOption(menu, troopMenu, troop.ToString(),
                (x) =>
                {
                    x.optionLeaveType = GameMenuOption.LeaveType.Recruit;
                    if(isElite)
                    {
                        int clanEliteTier = Hero.MainHero.Clan.Tier - Settings.ClanRankForElite;
                        return clanEliteTier >= 0 && Settings.MaxRankElite >= troop.Tier &&
                               ((Settings.TierRecruitablePerClanRankElite == 0 && troop.Tier == 2) || troop.Tier <= (((clanEliteTier + 1)  * Settings.TierRecruitablePerClanRankElite) + 1)) &&
                               Hero.MainHero.Gold >= getRecruitmentCost(troop, 1, isElite);
                    }
                    else
                    {
                        int clanTier = Hero.MainHero.Clan.Tier - Settings.ClanRankForRegular;
                        return clanTier >= 0 && Settings.MaxRankRegular >= troop.Tier &&
                               ((Settings.TierRecruitablePerClanRank == 0 && troop.Tier == 1) || troop.Tier <= ((clanTier + 1) * Settings.TierRecruitablePerClanRank)) &&
                               Hero.MainHero.Gold >= getRecruitmentCost(troop, 1, isElite);
                    }
                },
                (x) =>
                {
                    GameMenu.SwitchToMenu(troopMenu);
                }, relatedObject: this);

            Action<string, int> addTroopOption = (menuName, amount) =>
            {
                campaignGameStarter.AddGameMenuOption(menuName,
                    String.Format("{0}_{1}", menuName, amount),
                    String.Format("Recruit {0} {1} for {2}", amount, troop.ToString(), getRecruitmentCost(troop, amount, isElite)),
                    (x) =>
                    {
                        x.optionLeaveType = GameMenuOption.LeaveType.Recruit;
                        return Hero.MainHero.Gold >= getRecruitmentCost(troop, amount, isElite);
                    },
                    (x) =>
                    {
                        MobileParty.MainParty.AddElementToMemberRoster(troop, amount);
                        GiveGoldAction.ApplyForCharacterToSettlement(Hero.MainHero, MobileParty.MainParty.CurrentSettlement, getRecruitmentCost(troop, amount, isElite));
                    },
                    false, -1, true, this);
            };

            addTroopOption(troopMenu, 1);
            addTroopOption(troopMenu, 10);
            addTroopOption(troopMenu, 100);
            campaignGameStarter.AddGameMenuOption(troopMenu, "recruit_back", "Leave", (x) => { x.optionLeaveType = GameMenuOption.LeaveType.Leave; return true; }, (x) => GameMenu.SwitchToMenu(menu), true, relatedObject: this);
        }

        private int getRecruitmentCost(CharacterObject troop, int amount, bool isElite)
        {
            DefaultPartyWageModel wageModel = new DefaultPartyWageModel();
            float multiplier = isElite ? Settings.CostMultiplierElite : Settings.CostMultiplierRegular;
            return (int)(wageModel.GetTroopRecruitmentCost(troop, Hero.MainHero) * (multiplier + (Settings.CostTierMultiplier * troop.Tier)) * amount);
        }

        public override void OnAfterGameInitializationFinished(Game game, object starterObject)
        {
            RebuildMenu(game, (CampaignGameStarter)starterObject);
        }
        public void RebuildMenu(Game game, CampaignGameStarter starterObject)
        {
            if (game.GameType is Campaign)
            {
                CampaignGameStarter campaignGameStarter = (CampaignGameStarter)starterObject;
                Campaign campaign = (Campaign)game.GameType;
                campaign.GameMenuManager.RemoveRelatedGameMenus(this);
                campaign.GameMenuManager.RemoveRelatedGameMenuOptions(this);
                try
                {
                    GameMenuOption.OnConditionDelegate hireRecruitsDelegate = delegate (MenuCallbackArgs args)
                    {
                        args.optionLeaveType = GameMenuOption.LeaveType.Recruit;
                        return true;
                    };
                    GameMenuOption.OnConsequenceDelegate hireRecruitsConsequenceDelegate = delegate (MenuCallbackArgs args)
                    {
                        GameMenu.SwitchToMenu("recruit_menu_" + Hero.MainHero.CurrentSettlement.Culture.ToString());
                    };

                    int clanTier = Hero.MainHero.Clan.Tier;

                    if (!hasAddedMenuToTown)
                    {
                        //hasAddedMenuToTown = true;
                        campaignGameStarter.AddGameMenuOption("town", "recruiter_hire_recruits", "Hire some recruits", hireRecruitsDelegate, hireRecruitsConsequenceDelegate, false, 7, false, this);
                        campaignGameStarter.AddGameMenuOption("recruit_select_culture", "recruiter_back", "Leave", (x) => { x.optionLeaveType = GameMenuOption.LeaveType.Leave; return true; }, (x) => GameMenu.SwitchToMenu("town"), true, relatedObject: this);
                    }

                    campaignGameStarter.AddGameMenu("recruit_select_culture", "Select the culture to hire from.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, this);

                    foreach (var kingdom in Kingdom.All.DistinctBy(k => k.Culture))
                    {
                        CultureObject culture = kingdom.Culture;

                        string menu = "recruit_menu_" + culture.ToString();
                        campaignGameStarter.AddGameMenu(menu, "Select troop type to recruit.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, this);

                        if (AllowRecruitElite) addTroopsMenu(campaignGameStarter, menu, culture.EliteBasicTroop, true);
                        addTroopsMenu(campaignGameStarter, menu, culture.BasicTroop, false);

                        //culture selection
                        campaignGameStarter.AddGameMenuOption(menu, "recruit_select_culture", "Select culture", hireRecruitsDelegate, (x) => GameMenu.SwitchToMenu("recruit_select_culture"), true, relatedObject: this);
                        campaignGameStarter.AddGameMenuOption("recruit_select_culture", "recruiter_select_" + culture.ToString(), culture.ToString(), hireRecruitsDelegate, (x) => GameMenu.SwitchToMenu("recruit_menu_" + culture.ToString()), true, relatedObject: this);

                        //back
                        campaignGameStarter.AddGameMenuOption(menu, "recruit_back", "Leave", (x) => { x.optionLeaveType = GameMenuOption.LeaveType.Leave; return true; }, (x) => GameMenu.SwitchToMenu("town"), true, relatedObject: this);
                    }

                    CharacterObject basicCustom = Game.Current.ObjectManager.GetObject<CharacterObject>("_basic_root");
                    CharacterObject eliteBasicCustom = Game.Current.ObjectManager.GetObject<CharacterObject>("_elite_root");
                    if (basicCustom != null && eliteBasicCustom != null)
                    {
                        string menu = "recruit_menu_custom";
                        campaignGameStarter.AddGameMenu(menu, "How many recruits do you want to hire.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, this);

                        if (AllowRecruitElite) addTroopsMenu(campaignGameStarter, menu, eliteBasicCustom, true);
                        addTroopsMenu(campaignGameStarter, menu, basicCustom, false);

                        //culture selection
                        campaignGameStarter.AddGameMenuOption(menu, "recruit_select_culture", "Select culture", hireRecruitsDelegate, (x) => GameMenu.SwitchToMenu("recruit_select_culture"), true, relatedObject: this);
                        campaignGameStarter.AddGameMenuOption("recruit_select_culture", "recruiter_select_custom", "Custom", hireRecruitsDelegate, (x) => GameMenu.SwitchToMenu("recruit_menu_custom"), true, relatedObject: this);

                        //back
                        campaignGameStarter.AddGameMenuOption(menu, "recruit_back", "Leave", (x) => { x.optionLeaveType = GameMenuOption.LeaveType.Leave; return true; }, (x) => GameMenu.SwitchToMenu("town"), true, relatedObject: this);
                    }
                    //List<CharacterObject> eliteTroops = new List<CharacterObject>();
                    //getEliteTroops(eliteBasicCustom, eliteTroops);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }

        public void RebuildMenu()
        {
            if(Game.Current != null && SandBoxManager.Instance != null && SandBoxManager.Instance.GameStarter != null)
                RebuildMenu(Game.Current, SandBoxManager.Instance.GameStarter);
        }
    }
}