using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Library;

namespace SettlementGoldMod
{
    public class GoldSettlementEconomyModel : DefaultSettlementEconomyModel
    {
        public override int GetTownGoldChange(Town town) => TaleWorlds.Library.MathF.Round((float)(0.200000002980232 * ((double)((Fief)town).Prosperity * 70.0 - (double)((SettlementComponent)town).Gold))) ;
    }
}