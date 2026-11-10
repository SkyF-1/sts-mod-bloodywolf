using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class CloutLossVar : SecondaryResourceVar
{
    public const string Key = "Bloodywolf-CloutLoss";

    public CloutLossVar(decimal baseValue) : base(Key, BloodywolfResources.CloutId, baseValue) { }

    public CloutLossVar(string name, decimal baseValue)
        : base(name, BloodywolfResources.CloutId, baseValue) { }
}
