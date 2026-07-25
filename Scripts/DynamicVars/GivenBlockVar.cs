using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class GivenBlockVar : DynamicVar
{
    public const string Key = "Bloodywolf-GivenBlock";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public GivenBlockVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }

    public GivenBlockVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithTooltip(LocKey);
    }
}