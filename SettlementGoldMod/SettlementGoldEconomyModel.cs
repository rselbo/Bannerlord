using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace SettlementGoldMod
{
    public class GoldSettlementEconomyModel : DefaultSettlementEconomyModel
    {
        public override int GetTownGoldChange(Town town) => MathF.Round(0.25f * ((float)(10000.0 + (double)town.Prosperity * 120.0) - (float)town.Gold));
    }
}