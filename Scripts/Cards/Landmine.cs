using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Landmine : CustomCardModel
{/// 爆雷
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(42m, ValueProp.Move),
        new DamageVar("SelfDamage", 21m, ValueProp.Unpowered)
    };

	public Landmine()
		: base(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
            .TargetingAllOpponents(base.CombatState)
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(choiceContext);
        var canonicalPower = ModelDb.Power<LandminePower>();          // 获取规范实例
        var mutablePower = canonicalPower.ToMutable() as LandminePower; // 创建可变副本
        if(mutablePower != null) 
        {
            mutablePower.SourceCard = this;
            await PowerCmd.Apply(mutablePower, base.Owner.Creature, base.DynamicVars["SelfDamage"].BaseValue, base.Owner.Creature, this);
        }
    }

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(10m);
	}
}

