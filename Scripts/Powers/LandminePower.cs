using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class LandminePower : BloodywolfPowerModel
{
    private sealed class Data
    {
        public int cardsPlayed;
    }

    public CardModel? SourceCard { get; set; }
	public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
	public override int DisplayAmount => GetInternalData<Data>().cardsPlayed;
    public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == base.Owner)
        {
			GetInternalData<Data>().cardsPlayed++;
            InvokeDisplayAmountChanged();
        }
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == base.Owner.Side)
        {
			int cardsPlayed = GetInternalData<Data>().cardsPlayed;
			if (cardsPlayed > 0)
            {
				if (base.Owner.Player is not { } player)
					return;

				await BloodywolfResources.GainClout(
					player,
					base.Amount * cardsPlayed,
					(AbstractModel?)SourceCard ?? this);
            }
            await PowerCmd.Remove(this);
        }
    }

}
