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

// 1.7.*
//using TaleWorlds.CampaignSystem.SandBox.GameComponents.Party;
//1.8.*
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace Recruiter
{
	public class Recruiter : MBSubModuleBase
	{
		int MaxLevelRecruitable = 100;
		int LevelRecruitablePerClanRank = 2;
		int ClanRankForElite = 2;
		bool AllowRecruitElite = true;

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

		public override void OnAfterGameInitializationFinished(Game game, object starterObject)
		{
			if (game.GameType is Campaign)
			{
				CampaignGameStarter campaignGameStarter = (CampaignGameStarter)starterObject;
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
					
					campaignGameStarter.AddGameMenuOption("town", "recruiter_hire_recruits", "Hire some recruits", hireRecruitsDelegate, hireRecruitsConsequenceDelegate, false, 7, false);

                    campaignGameStarter.AddGameMenu("recruit_select_culture", "Select the culture to hire from.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
                    //campaignGameStarter.AddGameMenu("recruit_select_troop", "Select the troop type.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);

                    DefaultPartyWageModel wageModel = new DefaultPartyWageModel();
					Func<CharacterObject, int, int> getRecruitmentCost = (troop, amount) => {
						return wageModel.GetTroopRecruitmentCost(troop, Hero.MainHero) * 1 * amount;
					};
					Action<string, CharacterObject, int, int> addTroopOption = (menuName, troop, amount, requiredTier) =>
					{
						campaignGameStarter.AddGameMenuOption(menuName,
							String.Format("recruit_{0}_{1}", amount, troop.ToString().Replace(" ", "")),
							String.Format("Recruit {0} {1} for {2}", amount, troop.ToString(), getRecruitmentCost(troop, amount)),
							(x) =>
							{
								x.optionLeaveType = GameMenuOption.LeaveType.Recruit;
								return Hero.MainHero.Gold >= getRecruitmentCost(troop, amount) && Hero.MainHero.Clan.Tier >= requiredTier;
							},
							(x) =>
							{
								MobileParty.MainParty.AddElementToMemberRoster(troop, amount);
								GiveGoldAction.ApplyForCharacterToSettlement(Hero.MainHero, MobileParty.MainParty.CurrentSettlement, getRecruitmentCost(troop, amount));
							},
							false, -1, true);
					};
					Action<string, CharacterObject, CharacterObject, List<CharacterObject>> addOptions = (menu, basicTroop, basicEliteTroop, eliteTroops) =>
					{
						addTroopOption(menu, basicTroop, 1, 0);
						addTroopOption(menu, basicEliteTroop, 1, 2);
						foreach (var eliteTroop in eliteTroops) { addTroopOption(menu, eliteTroop, 1, 4); }

						addTroopOption(menu, basicTroop, 10, 0);
						addTroopOption(menu, basicEliteTroop, 10, 2);
						foreach (var eliteTroop in eliteTroops) { addTroopOption(menu, eliteTroop, 10, 4); }

						addTroopOption(menu, basicTroop, 100, 0);
						addTroopOption(menu, basicEliteTroop, 100, 2);
						foreach (var eliteTroop in eliteTroops) { addTroopOption(menu, eliteTroop, 100, 4); }
					};

					foreach (var kingdom in Kingdom.All.DistinctBy(k => k.Culture))
					{
						CultureObject culture = kingdom.Culture;

						string menu = "recruit_menu_" + culture.ToString();
						campaignGameStarter.AddGameMenu(menu, "How many recruits do you want to hire.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);

						List<CharacterObject> eliteTroops = new List<CharacterObject>();
						getEliteTroops(culture.EliteBasicTroop, eliteTroops);

						addOptions(menu, culture.BasicTroop, culture.EliteBasicTroop, eliteTroops);

						//culture selection
						campaignGameStarter.AddGameMenuOption(menu, "recruit_select_culture", "Select culture", hireRecruitsDelegate, (x) => GameMenu.SwitchToMenu("recruit_select_culture"), true);
						campaignGameStarter.AddGameMenuOption("recruit_select_culture", "recruiter_select_" + culture.ToString(), culture.ToString(), hireRecruitsDelegate, (x) => GameMenu.SwitchToMenu("recruit_menu_" + culture.ToString()), true);

						//back
						campaignGameStarter.AddGameMenuOption(menu, "recruit_back", "Leave", (x) => { x.optionLeaveType = GameMenuOption.LeaveType.Leave; return true; }, (x) => GameMenu.SwitchToMenu("town"), true);
					}

					CharacterObject basicCustom = Game.Current.ObjectManager.GetObject<CharacterObject>("_basic_root");
					CharacterObject eliteBasicCustom = Game.Current.ObjectManager.GetObject<CharacterObject>("_elite_root");
					if (basicCustom != null && eliteBasicCustom != null)
                    {
                        List<CharacterObject> eliteTroops = new List<CharacterObject>();
                        getEliteTroops(eliteBasicCustom, eliteTroops);
                        string menu = "recruit_menu_custom";
						campaignGameStarter.AddGameMenu(menu, "How many recruits do you want to hire.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
						addOptions(menu, basicCustom, eliteBasicCustom, eliteTroops);

						//culture selection
						campaignGameStarter.AddGameMenuOption(menu, "recruit_select_culture", "Select culture", hireRecruitsDelegate, (x) => GameMenu.SwitchToMenu("recruit_select_culture"), true);
						campaignGameStarter.AddGameMenuOption("recruit_select_culture", "recruiter_select_custom", "Custom", hireRecruitsDelegate, (x) => GameMenu.SwitchToMenu("recruit_menu_custom"), true);

						//back
						campaignGameStarter.AddGameMenuOption(menu, "recruit_back", "Leave", (x) => { x.optionLeaveType = GameMenuOption.LeaveType.Leave; return true; }, (x) => GameMenu.SwitchToMenu("town"), true);
					}
					//List<CharacterObject> eliteTroops = new List<CharacterObject>();
					//getEliteTroops(eliteBasicCustom, eliteTroops);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
				campaignGameStarter.AddGameMenuOption("recruit_select_culture", "recruiter_back", "Leave", (x) => { x.optionLeaveType = GameMenuOption.LeaveType.Leave; return true; }, (x) => GameMenu.SwitchToMenu("town"), true);

			}
		}


	}
}