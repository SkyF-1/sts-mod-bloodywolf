using BaseLibToRitsu.Generated;
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

[Pool(typeof(BloodywolfCardPool))]
public sealed class Optimize : CustomCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>{new CardsVar(1), new RateVar(0m)};

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>{HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromPower<CloutPower>()};
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public Optimize()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
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
            await PowerCmd.Apply<CloutPower>(
                base.Owner.Creature, 
                rateCount, 
                base.Owner.Creature, 
                this);
		}
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Cards.UpgradeValueBy(1m);
	}
}

