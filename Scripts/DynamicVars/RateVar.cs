using STS2RitsuLib.Cards.DynamicVars;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class RateVar : DynamicVar
{
    public const string Key = "Bloodywolf-Rate";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public RateVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithSharedTooltip(LocKey);
    }

    public RateVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithSharedTooltip(LocKey);
    }
}