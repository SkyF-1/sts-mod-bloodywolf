using STS2RitsuLib.Cards.DynamicVars;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class HotTakeVar : DynamicVar
{
    public const string Key = "Bloodywolf-HotTake";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public HotTakeVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithSharedTooltip(LocKey);
    }

    public HotTakeVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithSharedTooltip(LocKey);
    }
}