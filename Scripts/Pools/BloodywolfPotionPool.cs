using STS2RitsuLib.Scaffolding.Content;

namespace StsModBloodywolf.Scripts.Pools;
public class BloodywolfPotionPool : TypeListPotionPoolModel
{
    public override string EnergyColorName => "Bloodywolf";
    public override string? TextEnergyIconPath => "res://StsModBloodywolf/images/energy_bloodywolf.png";
    public override string? BigEnergyIconPath => "res://StsModBloodywolf/images/energy_bloodywolf_big.png";
}
