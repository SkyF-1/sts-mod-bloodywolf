using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class RetreatAll : BloodywolfCardModel
{/// 全撤�?
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
	protected override IEnumerable<DynamicVar> CanonicalVars => [new RateVar(1m)];
    public override string PortraitPath => AssetProfile.PortraitPath!;
	public RetreatAll()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        IEnumerable<CardModel> cards = PileType.Hand.GetPile(base.Owner).Cards;
        int cardCount = cards.Count();
        await CardCmd.Discard(choiceContext, cards);
        await BloodywolfResources.GainClout(base.Owner, cardCount, this);
	}

	protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
