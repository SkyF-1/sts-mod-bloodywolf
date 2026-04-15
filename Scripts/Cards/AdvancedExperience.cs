using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Factories;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class AdvancedExperience : CustomCardModel
{/// 先进经验
	public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
	private CardModel? _mockGeneratedCard;
	public AdvancedExperience()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		CardModel cardModel;
		if (_mockGeneratedCard == null)
		{
			List<CardPoolModel> list = base.Owner.UnlockState.CharacterCardPools.ToList();
			if (list.Count > 1)
			{
				list.Remove(base.Owner.Character.CardPool);
			}
			IEnumerable<CardModel> cards = from c in list.SelectMany((CardPoolModel c) => c.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint))
				where c.Type == CardType.Skill
				select c;
			List<CardModel> list2 = CardFactory.GetDistinctForCombat(base.Owner, cards, 3, base.Owner.RunState.Rng.CombatCardGeneration).ToList();
			if (base.IsUpgraded)
			{
				foreach (CardModel item in list2)
				{
					CardCmd.Upgrade(item);
				}
			}
			cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, list2, base.Owner, canSkip: true);
		}
		else
		{
			cardModel = _mockGeneratedCard;
			if (base.IsUpgraded)
			{
				CardCmd.Upgrade(cardModel);
			}
		}
		if (cardModel != null)
		{
			cardModel.SetToFreeThisTurn();
			await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, addedByPlayer: true);
		}
	}

	public void MockGeneratedCard(CardModel card)
	{
		AssertMutable();
		_mockGeneratedCard = card;
	}

	protected override void OnUpgrade()
	{
	}
}
