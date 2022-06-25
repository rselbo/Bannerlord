using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Recruiter
{
	public class Recruiter : MBSubModuleBase
	{
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
					
					campaignGameStarter.AddGameMenuOption("town", "recruiter_hire_recruits", "Hire some recruits", hireRecruitsDelegate, hireRecruitsConsequenceDelegate, false, 7, false);

					campaignGameStarter.AddGameMenu("recruit_select_culture", "Select the culture to hire from.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);

					DefaultPartyWageModel wageModel = new DefaultPartyWageModel();
					foreach (var kingdom in Kingdom.All.DistinctBy(k => k.Culture))
					{
						CultureObject culture = kingdom.Culture;

						Func<CharacterObject, int, int> getRecruitmentCost = (troop, amount) => {
							return wageModel.GetTroopRecruitmentCost(troop, Hero.MainHero) * 5 * amount;
						};
						Action<string, CharacterObject, int> addTroopOption = (menuName, troop, amount) =>
						{
							campaignGameStarter.AddGameMenuOption(menuName,
								String.Format("recruit_{0}_{1}", amount, troop.ToString().Replace(" ", "")),
								String.Format("Recruit {0} {1} for {2}", amount, troop.ToString(), getRecruitmentCost(troop, amount)),
								(x) =>
								{
									x.optionLeaveType = GameMenuOption.LeaveType.Recruit;
									return Hero.MainHero.Gold >= getRecruitmentCost(troop, amount);
								},
								(x) =>
								{
									MobileParty.MainParty.AddElementToMemberRoster(troop, amount);
									GiveGoldAction.ApplyForCharacterToSettlement(Hero.MainHero, MobileParty.MainParty.CurrentSettlement, getRecruitmentCost(troop, amount));
								},
								false, -1, true);
						};

						string menu = "recruit_menu_" + culture.ToString();
						campaignGameStarter.AddGameMenu(menu, "How many recruits do you want to hire.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);

						List<CharacterObject> eliteTroops = new List<CharacterObject>();
						getEliteTroops(culture.EliteBasicTroop, eliteTroops);

						addTroopOption(menu, culture.BasicTroop, 1);
						addTroopOption(menu, culture.EliteBasicTroop, 1);
						foreach(var eliteTroop in eliteTroops) { addTroopOption(menu, eliteTroop, 1); }
						addTroopOption(menu, culture.BasicTroop, 10);
						addTroopOption(menu, culture.EliteBasicTroop, 10);
						foreach (var eliteTroop in eliteTroops) { addTroopOption(menu, eliteTroop, 10); }
						addTroopOption(menu, culture.BasicTroop, 100);
						addTroopOption(menu, culture.EliteBasicTroop, 100);
						foreach (var eliteTroop in eliteTroops) { addTroopOption(menu, eliteTroop, 100); }

						//culture selection
						campaignGameStarter.AddGameMenuOption(menu, "recruit_select_culture", "Select culture", hireRecruitsDelegate, (x) => GameMenu.SwitchToMenu("recruit_select_culture"), true);
						campaignGameStarter.AddGameMenuOption("recruit_select_culture", "recruiter_select_" + culture.ToString(), culture.ToString(), hireRecruitsDelegate, (x) => GameMenu.SwitchToMenu("recruit_menu_" + culture.ToString()), true);

						//back
						campaignGameStarter.AddGameMenuOption(menu, "recruit_back", "Leave", (x) => { x.optionLeaveType = GameMenuOption.LeaveType.Leave; return true; }, (x) => GameMenu.SwitchToMenu("town"), true);
					}
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