using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class RetreatAll : CustomCardModel
{/// 全撤了
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
	protected override IEnumerable<DynamicVar> CanonicalVars => [new RateVar(1m)];
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CloutPower>()
    };
	public RetreatAll()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{  
        IEnumerable<CardModel> cards = PileType.Hand.GetPile(base.Owner).Cards;
        decimal cardCount = cards.Count();
        await CardCmd.Discard(choiceContext, cards);
        await PowerCmd.Apply<CloutPower>(base.Owner.Creature, cardCount, base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
