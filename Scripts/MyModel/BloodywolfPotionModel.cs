using StsModBloodywolf.Scripts.Pools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace StsModBloodywolf.Scripts.Potions;

[RegisterPotion(typeof(BloodywolfPotionPool), Inherit = true)]
public abstract class BloodywolfPotionModel : ModPotionTemplate
{
    public override PotionAssetProfile AssetProfile
    {
        get
        {
            var name = Id.Entry.Replace("STS_MOD_BLOODYWOLF_POTION_", "").ToLowerInvariant();
            var path = $"res://StsModBloodywolf/images/potions/stsmodbloodywolf-{name}.png";
            return new PotionAssetProfile(path, path);
        }
    }
}
