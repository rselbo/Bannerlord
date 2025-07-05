using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace BoostMod
{
    internal class BoostSiegeEventModel : DefaultSiegeEventModel
    {
        public override float GetConstructionProgressPerHour(
          SiegeEngineType type,
          SiegeEvent siegeEvent,
          ISiegeEventSide side)
        {
            if (siegeEvent.IsPlayerSiegeEvent && 
                ((siegeEvent.BesiegedSettlement.Owner.IsHumanPlayerCharacter && side.BattleSide == BattleSideEnum.Defender) ||
                (!siegeEvent.BesiegedSettlement.Owner.IsHumanPlayerCharacter && side.BattleSide == BattleSideEnum.Attacker)))
                return base.GetConstructionProgressPerHour(type, siegeEvent, side) * BoostModModule.Settings.SiegeConstructionMultiplier;
            else
                return base.GetConstructionProgressPerHour(type, siegeEvent, side);
        }
    }
}
