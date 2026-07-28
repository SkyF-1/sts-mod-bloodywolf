using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class TrollVar : DynamicVar
{
    public const string Key = "Bloodywolf-Troll";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public TrollVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }

    public TrollVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithTooltip(LocKey);
    }
}