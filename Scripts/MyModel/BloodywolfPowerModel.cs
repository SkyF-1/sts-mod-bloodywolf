using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace StsModBloodywolf.Scripts.Powers;

[RegisterPower(Inherit = true)]
public abstract class BloodywolfPowerModel : ModPowerTemplate
{
    public override PowerAssetProfile AssetProfile
    {
        get
        {
            var name = Id.Entry.Replace("STS_MOD_BLOODYWOLF_POWER_", "").ToLowerInvariant();
            var path = $"res://StsModBloodywolf/images/powers/stsmodbloodywolf-{name}.png";
            return new PowerAssetProfile(path, path);
        }
    }
}
