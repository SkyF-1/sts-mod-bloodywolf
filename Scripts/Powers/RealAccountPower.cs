using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Combat;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class RealAccountPower : CustomPowerModel
{
	private HashSet<CardModel>? _autoplayingCards;

	public override PowerType Type => PowerType.Buff;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";

	public override PowerStackType StackType => PowerStackType.Counter;

	private HashSet<CardModel> AutoplayingCards
	{
		get
		{
			AssertMutable();
			if (_autoplayingCards == null)
			{
				_autoplayingCards = new HashSet<CardModel>();
			}
			return _autoplayingCards;
		}
	}

	public override async Task AfterCardDrawnEarly(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		if (card.Owner.Creature == base.Owner && card.Type == CardType.Attack && !base.Owner.CombatState.HittableEnemies.All((Creature c) => c.ShowsInfiniteHp))
		{
			AutoplayingCards.Add(card);
			await CardCmd.AutoPlay(choiceContext, card, null);
			await PowerCmd.Decrement(this);
			AutoplayingCards.Remove(card);
		}
	}

	public override Task BeforeAttack(AttackCommand command)
	{
		if (!AutoplayingCards.Contains(command.ModelSource))
		{
			return Task.CompletedTask;
		}
		command.WithHitFx("vfx/hellraiser_attack_vfx", command.HitSfx, command.TmpHitSfx).WithAttackerAnim("Cast", command.Attacker.Player.Character.CastAnimDelay).SpawningHitVfxOnEachCreature()
			.WithHitVfxSpawnedAtBase();
		return Task.CompletedTask;
	}

	public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
	{
		if (side == base.Owner.Side)
		{
			await PowerCmd.Remove(this);
		}
	}
}
