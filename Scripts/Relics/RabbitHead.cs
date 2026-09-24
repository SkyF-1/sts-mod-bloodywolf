using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Relics;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using static MyHook;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.DynamicVars;

namespace StsModBloodywolf.Scripts.Relics;

[Pool(typeof(BloodywolfRelicPool))]
public class RabbitHead : CustomRelicModel
{
    public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<MartialSoul>();
    // 稀有度
    public override RelicRarity Rarity => RelicRarity.Starter;
    // 小图标
    public override string PackedIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
    // 轮廓图标
    protected override string PackedIconOutlinePath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
    // 大图标
    protected override string BigIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new RateVar(3m)
    };
    public override async Task BeforeCombatStart()
	{
		Flash();
		await PowerCmd.Apply<CloutPower>(new BlockingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars[RateVar.Key].BaseValue, base.Owner.Creature, null);
	}

}