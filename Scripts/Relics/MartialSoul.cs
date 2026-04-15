using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Relics;

[Pool(typeof(BloodywolfRelicPool))]
public class MartialSoul : CustomRelicModel
{
    // 稀有度
    public override RelicRarity Rarity => RelicRarity.Ancient;
    // 小图标
    public override string PackedIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
    // 轮廓图标
    protected override string PackedIconOutlinePath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
    // 大图标
    protected override string BigIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";

    public override async Task BeforeCombatStart()
	{
		Flash();
		await PowerCmd.Apply<CloutPower>(base.Owner.Creature, 5m, base.Owner.Creature, null);
	}
}