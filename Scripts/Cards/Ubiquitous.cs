using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class Ubiquitous : BloodywolfCardModel
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public override string PortraitPath => AssetProfile.PortraitPath!;

    public Ubiquitous()
        : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var rng = base.Owner.RunState.Rng.CombatCardGeneration;
        List<CardPoolModel> viablePools = ModelDb.AllCharacterCardPools
            .Where(pool => GetEligibleRares(pool).Any())
            .ToList();
        viablePools = viablePools.UnstableShuffle(rng);

        List<CardModel> choices = [];
        foreach (CardPoolModel pool in viablePools.Take(3))
        {
            CardModel? canonicalCard = rng.NextItem(GetEligibleRares(pool));
            if (canonicalCard != null)
            {
                choices.Add(base.CombatState.CreateCard(canonicalCard, base.Owner));
            }
        }

        if (choices.Count < 3)
        {
            return;
        }

        CardModel? selectedCard = await CardSelectCmd.FromChooseACardScreen(
            choiceContext,
            choices,
            base.Owner);
        if (selectedCard == null)
        {
            return;
        }

        foreach (Player ally in base.CombatState.Players.Where(player => player.Creature.IsAlive))
        {
            CardModel copy = base.CombatState.CreateCard(selectedCard.CanonicalInstance, ally);
            copy.EnergyCost.SetThisCombat(0);
            await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, ally);
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }

    private IEnumerable<CardModel> GetEligibleRares(CardPoolModel pool)
    {
        return CardFactory.FilterForCombat(
                pool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint))
            .Where(card => card.Rarity == CardRarity.Rare);
    }
}
