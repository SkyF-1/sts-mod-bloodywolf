using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class PlayWithFire : CustomCardModel
{// 引火上身
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>{CardKeyword.Exhaust};
	protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CloutPower>()
    };
	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6m, ValueProp.Move),
        new RateVar(1m)
    ];
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public PlayWithFire()
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        IEnumerable<CardModel> cards = GetCards().ToList();
        int cardCount = cards.Count();
		foreach (CardModel item in cards)
		{
			await CardCmd.Exhaust(choiceContext, item);
            await PowerCmd.Apply<CloutPower>(base.Owner.Creature, base.DynamicVars[RateVar.Key].BaseValue, base.Owner.Creature, this);
		}
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitCount(cardCount)
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(2m);
	}

    private IEnumerable<CardModel> GetCards()
	{
		CardPile pile = PileType.Hand.GetPile(base.Owner);
		return pile.Cards.Where((CardModel c) => c.Type != CardType.Attack);
	}
}
