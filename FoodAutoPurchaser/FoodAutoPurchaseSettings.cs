using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodAutoPurchaser
{
    internal class FoodAutoPurchaseSettings : AttributeGlobalSettings<FoodAutoPurchaseSettings>
    {
        public override string Id => "FoodAutoPurchaser";

        public override string DisplayName => "RS Food Manager";

        public override string FolderName => "FoodAutoPurchaser";

        public override string FormatType => "json";

        [SettingPropertyInteger("Minimum denari to autopurchase", 0, 1000000, "0", RequireRestart = false, HintText = "Do not purchase food if cash is below this", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public int MinimumCash { get; set; } = 20000;

        [SettingPropertyInteger("Minimum food to keep", 0, 200, "0", RequireRestart = false, HintText = "Buy up to this amount of food", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public int MinimumFood { get; set; } = 20;

        [SettingPropertyBool("Enable buying food", RequireRestart = false)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public bool EnableBuying { get; set; } = true;

        [SettingPropertyInteger("Maximum food to keep", 0, 200, "0", RequireRestart = false, HintText = "Sell any food above this level", Order = 0)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public int MaximumFood { get; set; } = 60;

        [SettingPropertyBool("Enable selling food", RequireRestart = false)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public bool EnableSelling{ get; set; } = true;

        [SettingPropertyBool("Enable selling Animals", RequireRestart = false)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public bool EnableSellingAnimals { get; set; } = false;

        [SettingPropertyBool("Enable selling Goods", RequireRestart = false)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public bool EnableSellingGoods { get; set; } = false;

        [SettingPropertyBool("Exclude selling tools", RequireRestart = false)]
        [SettingPropertyGroup("General", GroupOrder = 0)]
        public bool ExcludeSellingTools { get; set; } = false;

    }
}
