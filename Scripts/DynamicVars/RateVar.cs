using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class RateVar : DynamicVar
{
    public const string Key = "Bloodywolf-Rate";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public RateVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }
}