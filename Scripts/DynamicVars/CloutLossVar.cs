using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class CloutLossVar : DynamicVar
{
    public const string Key = "Bloodywolf-CloutLoss";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public CloutLossVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }

    public CloutLossVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithTooltip(LocKey);
    }
}