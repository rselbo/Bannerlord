using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.GameComponents;

namespace BoostMod
{
    internal class BoostMovementSpeed : DefaultPartySpeedCalculatingModel
    {
        public override ExplainedNumber CalculateBaseSpeed(
          MobileParty mobileParty,
          bool includeDescriptions = false,
          int additionalTroopOnFootCount = 0,
          int additionalTroopOnHorseCount = 0)
        {
            ExplainedNumber num = base.CalculateBaseSpeed(mobileParty, includeDescriptions, additionalTroopOnFootCount, additionalTroopOnHorseCount);
            if (mobileParty.IsMainParty)
            {
                num.Add(BoostModModule.Settings.MovementBoost, description: new TaleWorlds.Localization.TextObject("Boost"));
            }
            return num;
        }
    }
}
