using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class HotTakeVar : DynamicVar
{
    public const string Key = "Bloodywolf-HotTake";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public HotTakeVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }

    public HotTakeVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithTooltip(LocKey);
    }
}