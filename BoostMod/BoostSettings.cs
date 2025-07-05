using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCM.Abstractions.Base.Global;

namespace BoostMod
{
    internal class BoostSettings : AttributeGlobalSettings<BoostSettings>
    {
        public override string Id => "RSBoost";

        public override string DisplayName => "RS Boost";

        public override string FolderName => "RSBoost";

        public override string FormatType => "json";

        [SettingPropertyFloatingInteger("Siege construction multiplier", 0.1f, 20, "0.00", RequireRestart = false, HintText = "Multiplier for siege camp and engines construction", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public float SiegeConstructionMultiplier { get; set; } = 10;

        [SettingPropertyFloatingInteger("Movement boost", 0.1f, 20, "0.00", RequireRestart = false, HintText = "How much speed to add to the main party", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public float MovementBoost { get; set; } = 4;

        [SettingPropertyFloatingInteger("Settlement gold multiplier", 0.1f, 200, "0.00", RequireRestart = false, HintText = "Multiplier for how much gold a settlement will have. Only applies next time the gold updates, can be a few ingame days", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public float SettlementGoldMultiplier { get; set; } = 10;
    }
}
