using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Reckoning : CustomCardModel
{/// 秋后算账
	public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
	{
		new CalculationBaseVar(0m),
		new ExtraDamageVar(1m),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => PileType.Discard.GetPile(card.Owner).Cards.Count),
		new HotTakeVar(5m),
		new CloutLossVar(2m)
	};

	public Reckoning()
		: base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}
    protected override bool ShouldGlowGoldInternal => base.Owner.Creature.GetPower<CloutPower>()?.Amount >= base.DynamicVars[HotTakeVar.Key].IntValue;
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(choiceContext);
        decimal cloutAmount = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        if (cloutAmount >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        {
            await PowerCmd.Apply<CloutPower>(
                base.Owner.Creature,
                -base.DynamicVars[CloutLossVar.Key].BaseValue,
                base.Owner.Creature,
                this);
            if (!base.Keywords.Contains(CardKeyword.Exhaust) && !base.ExhaustOnNextPlay)
            {
                await CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Random);
            }
        }
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}
