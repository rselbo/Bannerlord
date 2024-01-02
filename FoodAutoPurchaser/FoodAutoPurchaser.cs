using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Party;

namespace FoodAutoPurchaser
{
    class AutoPurchaserSettings
    {
        public int FoodBuyLimit = 0;
    }

    public class AutoPurchaser : MBSubModuleBase
    {
        public AutoPurchaser()
        {
            autoPurchaser = this;
        }

        private static AutoPurchaser autoPurchaser = null;
        private int BuyLimit { get; set; } = 20;

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (!(game.GameType is Campaign))
                return;

            CampaignEvents.SettlementEntered.AddNonSerializedListener((object)this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
        }

        public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
        {
            if (hero == null || !hero.IsHumanPlayerCharacter)
                return;
            for (int index = 0; index < settlement.ItemRoster.Count; ++index)
            {
                if (hero.Gold < 10000)
                    return;

                ItemRosterElement elementCopyAtIndex = settlement.ItemRoster.GetElementCopyAtIndex(index);
                EquipmentElement equipmentElement = ((ItemRosterElement)elementCopyAtIndex).EquipmentElement;
                ItemObject itemObject1 = ((EquipmentElement)equipmentElement).Item;
                if (itemObject1.IsFood)
                {
                    int val1 = this.BuyLimit;
                    int indexOfItem = mobileParty.ItemRoster.FindIndexOfItem(itemObject1);
                    if (indexOfItem >= 0)
                    {
                        int buyLimit = this.BuyLimit;
                        ItemRosterElement itemRosterElement = mobileParty.ItemRoster[indexOfItem];
                        int amount = ((ItemRosterElement)itemRosterElement).Amount;
                        val1 = buyLimit - amount;
                    }
                    int num = Math.Min(val1, ((ItemRosterElement)elementCopyAtIndex).Amount - 1);
                    if (num > 0)
                    {
                        SettlementComponent component = settlement.SettlementComponent;
                        MobileParty mobileParty1 = mobileParty;
                        ItemObject itemObject2 = itemObject1;
                        MobileParty mobileParty2 = mobileParty1;
                        if (component.GetItemPrice(itemObject2, mobileParty2, false)*num <= mobileParty.LeaderHero.Gold)
                            SellItemsAction.Apply(settlement.Party, mobileParty.Party, elementCopyAtIndex, num, (Settlement)null);
                    }
                }
            }
        }

    }
}