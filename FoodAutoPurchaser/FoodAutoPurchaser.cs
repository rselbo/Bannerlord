using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Party;
using MCM.Abstractions.Base.Global;
using System.Runtime;

namespace FoodAutoPurchaser
{
    public class AutoPurchaser : MBSubModuleBase
    {
        public AutoPurchaser()
        {
            autoPurchaser = this;
        }

        private static AutoPurchaser autoPurchaser = null;
        private FoodAutoPurchaseSettings Settings = null;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            Settings = GlobalSettings<FoodAutoPurchaseSettings>.Instance;
        }

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

            if (Settings != null)
            {
                if (Settings.EnableBuying && hero.Gold >= Settings.MinimumCash)
                {
                    BuyFood(mobileParty, settlement, hero);
                }
                if (Settings.EnableSelling)
                {
                    SellItems(mobileParty, settlement, hero);
                }

            }
        }

        private void BuyFood(MobileParty mobileParty, Settlement settlement, Hero hero)
        {
            foreach(ItemRosterElement item in settlement.ItemRoster)
            {
                ItemObject itemObject = ((EquipmentElement)item.EquipmentElement).Item;
                if(itemObject.IsFood)
                {
                    BuyFoodItem(mobileParty, settlement, hero, item);
                }
            }
        }

        private void BuyFoodItem(MobileParty mobileParty, Settlement settlement, Hero hero, ItemRosterElement foodRosterItem)
        {
            ItemObject foodItem = ((EquipmentElement)foodRosterItem.EquipmentElement).Item;
            int partyAmount = GetPartyAmount(mobileParty, foodItem);
            int wantToBuy = Settings.MinimumFood - partyAmount;

            // Dot buy food that will exceed encumberance
            float remainingCapacity = mobileParty.InventoryCapacity - mobileParty.TotalWeightCarried;
            if(remainingCapacity < foodItem.Weight * wantToBuy) 
            {
                wantToBuy = (int)(remainingCapacity / foodItem.Weight);
            }

            if (wantToBuy > 0)
            {
                SettlementComponent component = settlement.SettlementComponent;

                int pricePer = component.GetItemPrice(foodItem, mobileParty, false);
                //limit by cash
                int canBuy = mobileParty.LeaderHero.Gold / pricePer;
                wantToBuy = Math.Min(wantToBuy, canBuy);
                if (wantToBuy == 0)
                    return;

                SellItemsAction.Apply(settlement.Party, mobileParty.Party, foodRosterItem, wantToBuy, settlement);
            }
        }

        private int GetPartyAmount(MobileParty mobileParty, ItemObject foodItem)
        {
            int indexOfItem = mobileParty.ItemRoster.FindIndexOfItem(foodItem);
            if (indexOfItem >= 0)
            {
                ItemRosterElement itemRosterElement = mobileParty.ItemRoster[indexOfItem];
                return ((ItemRosterElement)itemRosterElement).Amount;
            }
            return 0;
        }

        private void SellItems(MobileParty mobileParty, Settlement settlement, Hero hero)
        {
            foreach(ItemRosterElement item in mobileParty.ItemRoster)
            {
                ItemObject itemObject = ((EquipmentElement)item.EquipmentElement).Item;
                if (itemObject.IsFood && item.Amount > Settings.MaximumFood)
                {
                    SellItemsAction.Apply(mobileParty.Party, settlement.Party, item, item.Amount - Settings.MaximumFood, settlement);
                }

                if (itemObject.IsAnimal && Settings.EnableSellingAnimals)
                {
                    SellItemsAction.Apply(mobileParty.Party, settlement.Party, item, item.Amount, settlement);
                }

                if (itemObject.IsTradeGood && !itemObject.IsFood && Settings.EnableSellingGoods)
                {
                    SellItemsAction.Apply(mobileParty.Party, settlement.Party, item, item.Amount, settlement);
                }

            }
        }
    }
}