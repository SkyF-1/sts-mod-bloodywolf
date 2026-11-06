using STS2RitsuLib.Cards.DynamicVars;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class GivenBlockVar : DynamicVar
{
    public const string Key = "Bloodywolf-GivenBlock";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public GivenBlockVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithSharedTooltip(LocKey);
    }

    public GivenBlockVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithSharedTooltip(LocKey);
    }
}