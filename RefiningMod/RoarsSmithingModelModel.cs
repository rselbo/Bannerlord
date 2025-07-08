using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using Shokuho.CustomCampaign.Models.CampaignModels;

namespace RefiningMod
{
    internal class RoarsSmithingModelModel : ShokuhoSmithingModel
    {
        public override IEnumerable<TaleWorlds.Core.Crafting.RefiningFormula> GetRefiningFormulas(Hero weaponsmith)
        {
            int charcoalMultiplier = 10;
            int normal = 2;
            int medium = 20;
            int large = 100;

            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Wood, normal , CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal, weaponsmith.GetPerkValue(DefaultPerks.Crafting.CharcoalMaker) ? (normal * charcoalMultiplier * 3) / 2  : normal * charcoalMultiplier / 2 );
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Wood, medium, CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal, weaponsmith.GetPerkValue(DefaultPerks.Crafting.CharcoalMaker) ? (medium * charcoalMultiplier * 3) / 2 : medium * charcoalMultiplier / 2);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Wood, large, CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal, weaponsmith.GetPerkValue(DefaultPerks.Crafting.CharcoalMaker) ? (large * charcoalMultiplier * 3) / 2 : large * charcoalMultiplier / 2);
            
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.IronOre, 1, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron1, weaponsmith.GetPerkValue(DefaultPerks.Crafting.IronMaker) ? 3 : 2);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.IronOre, 10, CraftingMaterials.Charcoal, 10, CraftingMaterials.Iron1, weaponsmith.GetPerkValue(DefaultPerks.Crafting.IronMaker) ? 30 : 20);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.IronOre, 100, CraftingMaterials.Charcoal, 50, CraftingMaterials.Iron1, weaponsmith.GetPerkValue(DefaultPerks.Crafting.IronMaker) ? 150 : 100);

            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron1, normal, CraftingMaterials.Charcoal, normal/2, CraftingMaterials.Iron2, normal);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron1, medium, CraftingMaterials.Charcoal, medium/2, CraftingMaterials.Iron2, medium);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron1, large, CraftingMaterials.Charcoal, large/2, CraftingMaterials.Iron2, large);

            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron2, normal, CraftingMaterials.Charcoal, normal / 2, CraftingMaterials.Iron3, normal);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron2, medium, CraftingMaterials.Charcoal, medium / 2, CraftingMaterials.Iron3, medium);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron2, large, CraftingMaterials.Charcoal, large / 2, CraftingMaterials.Iron3, large);

            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron3, normal, CraftingMaterials.Charcoal, normal / 2, CraftingMaterials.Iron4, normal);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron3, medium, CraftingMaterials.Charcoal, medium / 2, CraftingMaterials.Iron4, medium);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron3, large, CraftingMaterials.Charcoal, large / 2, CraftingMaterials.Iron4, large);

            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron4, normal, CraftingMaterials.Charcoal, normal / 2, CraftingMaterials.Iron5, normal);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron4, medium, CraftingMaterials.Charcoal, medium / 2, CraftingMaterials.Iron5, medium);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron4, large, CraftingMaterials.Charcoal, large / 2, CraftingMaterials.Iron5, large);

            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron5, normal, CraftingMaterials.Charcoal, normal / 2, CraftingMaterials.Iron6, normal);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron5, medium, CraftingMaterials.Charcoal, medium / 2, CraftingMaterials.Iron6, medium);
            yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron5, large, CraftingMaterials.Charcoal, large / 2, CraftingMaterials.Iron6, large);

            //yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron2, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron3, 2);
            ////if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker))
            //{
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron1, 2, CraftingMaterials.Charcoal, 3, CraftingMaterials.Iron4, 2);
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron2, 2, CraftingMaterials.Charcoal, 2, CraftingMaterials.Iron4, 2);
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron3, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron4, 2);
            //}
            ////if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker2))
            //{
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron1, 2, CraftingMaterials.Charcoal, 4, CraftingMaterials.Iron5, 2);
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron2, 2, CraftingMaterials.Charcoal, 3, CraftingMaterials.Iron5, 2);
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron3, 2, CraftingMaterials.Charcoal, 2, CraftingMaterials.Iron5, 2);
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron4, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron5, 2);
            //}
            ////if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker3))
            //{
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron1, 2, CraftingMaterials.Charcoal, 5, CraftingMaterials.Iron6, 2);
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron2, 2, CraftingMaterials.Charcoal, 4, CraftingMaterials.Iron6, 2);
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron3, 2, CraftingMaterials.Charcoal, 3, CraftingMaterials.Iron6, 2);
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron4, 2, CraftingMaterials.Charcoal, 2, CraftingMaterials.Iron6, 2);
            //    yield return new TaleWorlds.Core.Crafting.RefiningFormula(CraftingMaterials.Iron5, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron6, 2);
            //}
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
