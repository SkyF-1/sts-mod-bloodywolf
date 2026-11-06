using StsModBloodywolf.Scripts.Pools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace StsModBloodywolf.Scripts.Relics;

[RegisterRelic(typeof(BloodywolfRelicPool), Inherit = true)]
public abstract class BloodywolfRelicModel : ModRelicTemplate
{
    public override RelicAssetProfile AssetProfile
    {
        get
        {
            var name = Id.Entry.Replace("STS_MOD_BLOODYWOLF_RELIC_", "").ToLowerInvariant();
            var path = $"res://StsModBloodywolf/images/relics/stsmodbloodywolf-{name}.png";
            return new RelicAssetProfile(path, path, path);
        }
    }
}
