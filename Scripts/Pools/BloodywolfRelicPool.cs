using STS2RitsuLib.Scaffolding.Content;

namespace StsModBloodywolf.Scripts.Pools;
public class BloodywolfRelicPool : TypeListRelicPoolModel
{
    public override string EnergyColorName => "Bloodywolf";
    public override string? TextEnergyIconPath => "res://StsModBloodywolf/images/resources/bloodywolf_energy/energy_bloodywolf_small.png";
    public override string? BigEnergyIconPath => "res://StsModBloodywolf/images/resources/bloodywolf_energy/energy_bloodywolf_big.png";
}
