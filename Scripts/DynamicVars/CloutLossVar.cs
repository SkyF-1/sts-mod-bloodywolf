using STS2RitsuLib.Cards.DynamicVars;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class CloutLossVar : DynamicVar
{
    public const string Key = "Bloodywolf-CloutLoss";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public CloutLossVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithSharedTooltip(LocKey);
    }

    public CloutLossVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithSharedTooltip(LocKey);
    }
}