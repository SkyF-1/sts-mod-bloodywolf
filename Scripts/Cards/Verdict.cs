using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Entities.Creatures;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Verdict : CustomCardModel
{/// 定论
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new DamageVar(24m, ValueProp.Move) };

	public Verdict()
		: base(4, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash", null, "heavy_attack.mp3")
			.Execute(choiceContext);
	}
	public override Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{        
        if (applier == base.Owner.Creature && !(amount <= 0m) && power is CloutPower)
		{
            base.EnergyCost.AddThisCombat(-1);
        }
		return Task.CompletedTask;
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(4m);
	}

}
