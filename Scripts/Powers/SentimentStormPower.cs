using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Players;
using StsModBloodywolf.Scripts.Commands;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class SentimentStormPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		if (card.Owner.Creature != base.Owner)
		{
			return;
		}
		ICombatState? combatState = base.Owner.CombatState;
		if (combatState == null)
		{
			return;
		}
		Flash();
		await TrollCmd.Troll(Amount)
			.From(base.Owner)
			.TargetingAllOpponents(combatState)
			.Execute(choiceContext);
	}
	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side == base.Owner.Side)
		{
			await PowerCmd.Remove(this);
		}
	}
	// public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
	// {
	// 	_shouldIgnoreNextInstance = true;
	// }
}