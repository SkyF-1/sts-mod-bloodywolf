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

namespace StsModBloodywolf.Scripts.Powers;

public sealed class SentimentStormPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;
	private bool _shouldIgnoreNextInstance;
	public override PowerStackType StackType => PowerStackType.Single;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner == base.Owner.Player && cardPlay.Card.Type == CardType.Attack && !cardPlay.IsAutoPlay)
		{
			if(_shouldIgnoreNextInstance)
			{
				_shouldIgnoreNextInstance = false;
				return;
			}
			Flash();
			await CardPileCmd.AutoPlayFromDrawPile(choiceContext, base.Owner.Player, Amount, CardPilePosition.Top, forceExhaust: false);
		}
	}
	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side == base.Owner.Side)
		{
			await PowerCmd.Remove(this);
		}
	}
	public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
	{
		_shouldIgnoreNextInstance = true;
	}
}