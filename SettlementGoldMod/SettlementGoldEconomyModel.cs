using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace SettlementGoldMod
{
    public class GoldSettlementEconomyModel : DefaultSettlementEconomyModel
    {
        private readonly int Factor = 100;
        public override int GetTownGoldChange(Town town) => MathF.Round(0.25f * (10000f + town.Prosperity * 12f * Factor - (float)town.Gold));
    }
}