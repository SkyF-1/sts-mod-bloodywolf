using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class VerticalStrike : CustomCardModel
{/// 深度打击
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new DamageVar(8m, ValueProp.Move)
	};

	public VerticalStrike()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (target == null || dealer == null || cardSource == null)
		{
        	return 1m;
		}
		if (!(cardSource == this))
		{
			return 1m;
		}
		if (!props.IsPoweredAttack())
		{
			return 1m;
		}
		if (target.GetPowerAmount<CupLossPower>() <= 0)
		{
			return 1m;
		}
		return 2m;
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
        base.DynamicVars.Damage.UpgradeValueBy(3m);
	}
}
