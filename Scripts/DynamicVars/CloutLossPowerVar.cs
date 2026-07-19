using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class CloutLossPowerVar : DynamicVar
{
    public const string Key = "Bloodywolf-CloutLossPower";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public CloutLossPowerVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }

    public CloutLossPowerVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithTooltip(LocKey);
    }
}