using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using System.Runtime.CompilerServices;

namespace Recruiter
{
    public class RecruiterSettings : AttributeGlobalSettings<RecruiterSettings>
    {
        public Recruiter recruiter = null;
        public override string Id => "Recruiter";

        public override string DisplayName => "RS Recruiter";

        public override string FolderName => "Recruiter";

        public override string FormatType => "json";

        [SettingPropertyInteger("Clan rank for regular", 0, 6, "0", RequireRestart = false, HintText = "Minimum clan rank to hire regular troops", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public int ClanRankForRegular { get; set; } = 2;
        [SettingPropertyInteger("Clan rank for elite", 0, 6, "0", RequireRestart = false, HintText = "Minimum clan rank to hire elite troops", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public int ClanRankForElite { get; set; } = 1;

        [SettingPropertyInteger("Max recruitable tier for regulars", 0, 6, "0", RequireRestart = false, HintText = "Maximum recruitable tier for regular troops", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public int MaxRankRegular { get; set; } = 1;
        [SettingPropertyInteger("Max recruitable tier for elite", 0, 6, "0", RequireRestart = false, HintText = "Maximum recruitable tier for elite troops", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public int MaxRankElite { get; set; } = 1;

        [SettingPropertyInteger("Troop tiers unlocked per Clan rank for regular troops", 0, 6, "0", RequireRestart = false, HintText = "How many tiers of troops are unlocked per Clan rank", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public int TierRecruitablePerClanRank { get; set; } = 2;
        [SettingPropertyInteger("Troop tiers unlocked per Clan rank for elite troops", 0, 6, "0", RequireRestart = false, HintText = "How many tiers of troops are unlocked per Clan rank", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public int TierRecruitablePerClanRankElite { get; set; } = 2;
        [SettingPropertyFloatingInteger("Troop cost multiplier for regular troops", 0.1f, 20, "0", RequireRestart = false, HintText = "Multiplier for hiring regular troops ", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public float CostMultiplierRegular { get; set; } = 1.0f;
        [SettingPropertyFloatingInteger("Troop cost multiplier for elite troops", 0.1f, 20, "0", RequireRestart = false, HintText = "Multiplier for hiring elite troops ", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public float CostMultiplierElite { get; set; } = 1.0f;

        [SettingPropertyFloatingInteger("Troop tier cost extra multiplier", 0, 10, "0", RequireRestart = false, HintText = "Extra multiplier for the troop costs making higher tiers more expensive", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public float CostTierMultiplier { get; set; } = 0;

        public override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if(recruiter != null)
            {
                recruiter.RebuildMenu();

            }

        }
    }
}
