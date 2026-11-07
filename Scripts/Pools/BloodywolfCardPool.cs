using Godot;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace StsModBloodywolf.Scripts.Pools;
public class BloodywolfCardPool : TypeListCardPoolModel
{
    public override string Title => "BloodywolfCardPool";
    public override string EnergyColorName => "Bloodywolf";

    public override string? TextEnergyIconPath => "res://StsModBloodywolf/images/energy_bloodywolf.png";
    public override string? BigEnergyIconPath => "res://StsModBloodywolf/images/energy_bloodywolf_big.png";

    public override Color DeckEntryCardColor => new("#1c3f6d");
    public override Color EnergyOutlineColor => new("#1c3f6d");

    private static readonly Material? PoolFrameMaterialValue =
        MaterialUtils.CreateReplaceHueShaderMaterial(0.1098f, 0.2471f, 0.4275f);

    public override Material? PoolFrameMaterial => PoolFrameMaterialValue;
    public override bool IsColorless => false;
}
