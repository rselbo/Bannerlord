using TaleWorlds.CampaignSystem.GameComponents;

namespace LevelingTweaks
{
    internal class RoarsCharacterDevelopmentModel : DefaultCharacterDevelopmentModel
    {
        public override int LevelsPerAttributePoint => 1;
        public override int FocusPointsPerLevel => 2;

    }
}
