using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Combat.Powers;
using STS2RitsuLib.Scaffolding.Content;

namespace StsModBloodywolf.Scripts.Powers;

public abstract class CustomTemporaryDexterityPower<TOrigin> :
    ModTemporaryAppliedPowerTemplate<TOrigin, DexterityPower>
    where TOrigin : AbstractModel
{
    protected override bool IsPositive => true;

    public override PowerAssetProfile AssetProfile
    {
        get
        {
            var name = Id.Entry.Replace("STS_MOD_BLOODYWOLF_POWER_", "").ToLowerInvariant();
            var path = $"res://StsModBloodywolf/images/powers/stsmodbloodywolf-{name}.png";
            return new PowerAssetProfile(path, path);
        }
    }

    public override LocString Description => new(
        "powers",
        IsPositive ? "TEMPORARY_DEXTERITY_POWER.description" : "TEMPORARY_DEXTERITY_DOWN.description");

    protected override string SmartDescriptionLocKey => IsPositive
        ? "TEMPORARY_DEXTERITY_POWER.smartDescription"
        : "TEMPORARY_DEXTERITY_DOWN.smartDescription";
}
