using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Combat.SecondaryResources;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class RateVar : SecondaryResourceVar
{
    public const string Key = "Bloodywolf-Rate";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public RateVar(decimal baseValue) : base(Key, BloodywolfResources.CloutId, baseValue)
    {
        this.WithTooltip(CreateTooltip);
    }

    public RateVar(string name, decimal baseValue)
        : base(name, BloodywolfResources.CloutId, baseValue)
    {
        this.WithTooltip(CreateTooltip);
    }

    private static IHoverTip CreateTooltip(DynamicVar dynamicVar)
    {
        var title = new LocString("static_hover_tips", $"{LocKey}.title");
        var description = new LocString("static_hover_tips", $"{LocKey}.description");
        title.AddObj(Key, dynamicVar);
        description.AddObj(Key, dynamicVar);
        return new HoverTip(title, description);
    }
}
