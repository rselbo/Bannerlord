using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace BoostMod
{
    public class BoostModModel : DefaultBuildingConstructionModel
    {
        public override int GetBoostAmount(Town town)
        {
            int num = base.GetBoostAmount(town);
            return num * 100;
        }
    }
}
