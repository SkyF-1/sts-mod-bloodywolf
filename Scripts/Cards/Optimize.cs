using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.CardSelection;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class Optimize : BloodywolfCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>{new CardsVar(1), new RateVar(0m)};

	protected override IEnumerable<IHoverTip> AdditionalHoverTips => new List<IHoverTip>{HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromPower<CloutPower>()};
    public override string PortraitPath => AssetProfile.PortraitPath!;

	public Optimize()
		: base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		IEnumerable<CardModel> cardModels = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1, base.DynamicVars.Cards.IntValue), context: choiceContext, player: base.Owner, filter: null, source: this);
		if (cardModels != null)
		{
            decimal rateCount = DynamicVars[RateVar.Key].BaseValue;
            foreach(CardModel cardModel in cardModels)
            {
                rateCount += cardModel.EnergyCost.GetAmountToSpend();
			    await CardCmd.Exhaust(choiceContext, cardModel);
            }
            int ratingCount = base.IsUpgraded ? 2 : 1;
            for (int i = 0; i < ratingCount; i++)
            {
                await PowerCmd.Apply<CloutPower>(choiceContext,
                    base.Owner.Creature,
                    rateCount,
                    base.Owner.Creature,
                    this);
            }
		}
	}
}
