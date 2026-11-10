using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Relics;

public class MartialSoul : BloodywolfRelicModel
{
    // 稀有度
    public override RelicRarity Rarity => RelicRarity.Ancient;
    // 小图标
    public override string PackedIconPath => AssetProfile.IconPath!;
    // 轮廓图标
    protected override string PackedIconOutlinePath => AssetProfile.IconOutlinePath!;
    // 大图标
    protected override string BigIconPath => AssetProfile.BigIconPath!;

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new RateVar(7m)
    };
    public override async Task BeforeCombatStart()
	{
		Flash();
		await BloodywolfResources.GainClout(base.Owner, (int)base.DynamicVars[RateVar.Key].BaseValue, this);
	}
}
