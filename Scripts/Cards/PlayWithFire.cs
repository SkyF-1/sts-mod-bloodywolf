using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Commands;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class PlayWithFire : BloodywolfCardModel
{// 引火上身
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>{CardKeyword.Exhaust};
	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new TrollVar(6m)
    ];
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public PlayWithFire()
		: base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        IEnumerable<CardModel> cards = GetCards().ToList();
        int cardCount = cards.Count();
		foreach (CardModel item in cards)
		{
			await CardCmd.Exhaust(choiceContext, item);
		}
        for(int i = 0; i < cardCount; i++)
			await MyCmd.Troll(choiceContext, cardPlay.Target, base.DynamicVars[TrollVar.Key].BaseValue, base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars[TrollVar.Key].UpgradeValueBy(2m);
	}

    private IEnumerable<CardModel> GetCards()
	{
		CardPile pile = PileType.Hand.GetPile(base.Owner);
		return pile.Cards;
	}
}
