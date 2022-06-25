using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.Core;

namespace RefiningMod
{
    internal class RoarsSmithingModelModel : DefaultSmithingModel
    {
        public override IEnumerable<TaleWorlds.Core.Crafting.RefiningFormula> GetRefiningFormulas(Hero weaponsmith)
        {
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Wood, 2, CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal); 
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.CharcoalMaker))
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Wood, 2, CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal, 3);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.IronOre, 1, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron1, weaponsmith.GetPerkValue(DefaultPerks.Crafting.IronMaker) ? 3 : 2);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron1, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron2, 2);

            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron1, 2, CraftingMaterials.Charcoal, 2, CraftingMaterials.Iron3, 2);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron2, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron3, 2);
            //if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker))
            {
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron1, 2, CraftingMaterials.Charcoal, 3, CraftingMaterials.Iron4, 2);
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron2, 2, CraftingMaterials.Charcoal, 2, CraftingMaterials.Iron4, 2);
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron3, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron4, 2);
            }
            //if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker2))
            {
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron1, 2, CraftingMaterials.Charcoal, 4, CraftingMaterials.Iron5, 2);
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron2, 2, CraftingMaterials.Charcoal, 3, CraftingMaterials.Iron5, 2);
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron3, 2, CraftingMaterials.Charcoal, 2, CraftingMaterials.Iron5, 2);
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron4, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron5, 2);
            }
            //if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker3))
            {
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron1, 2, CraftingMaterials.Charcoal, 5, CraftingMaterials.Iron6, 2);
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron2, 2, CraftingMaterials.Charcoal, 4, CraftingMaterials.Iron6, 2);
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron3, 2, CraftingMaterials.Charcoal, 3, CraftingMaterials.Iron6, 2);
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron4, 2, CraftingMaterials.Charcoal, 2, CraftingMaterials.Iron6, 2);
                yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron5, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron6, 2);
            }
        }

        public override int GetSkillXpForRefining(ref TaleWorlds.Core.Crafting.RefiningFormula refineFormula) => base.GetSkillXpForRefining(ref refineFormula) * 4;
        public override int GetEnergyCostForRefining(ref TaleWorlds.Core.Crafting.RefiningFormula refineFormula, Hero hero)
        {
            if (hero.GetPerkValue(DefaultPerks.Crafting.PracticalRefiner))
                return 0;
            return 0;
        }

        public override int GetEnergyCostForSmithing(ItemObject item, Hero hero)
        {
            return 0;//base.GetEnergyCostForSmithing(item, hero) / 5;
        }

        public override int GetEnergyCostForSmelting(ItemObject item, Hero hero)
        {
            if (hero.GetPerkValue(DefaultPerks.Crafting.PracticalSmelter))
                return 0;
            return 0;
        }

    }
}
