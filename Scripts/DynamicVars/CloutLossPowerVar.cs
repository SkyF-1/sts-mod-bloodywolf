using STS2RitsuLib.Cards.DynamicVars;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class CloutLossPowerVar : DynamicVar
{
    public const string Key = "Bloodywolf-CloutLossPower";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public CloutLossPowerVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithSharedTooltip(LocKey);
    }

    public CloutLossPowerVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithSharedTooltip(LocKey);
    }
}