using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class DoOrDie : CustomCardModel
{
    /// 背水一战
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };

    public DoOrDie()
        : base(3, CardType.Skill, CardRarity.Ancient, TargetType.Self)
    {
    }

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Clear all Clout
        decimal cloutAmount = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        if (cloutAmount > 0)
        {
            await PowerCmd.Remove<CloutPower>(base.Owner.Creature);
        }

        // Exhaust draw pile and discard pile
        var drawPile = PileType.Draw.GetPile(base.Owner).Cards;
        var discardPile = PileType.Discard.GetPile(base.Owner).Cards;
        if(drawPile.Count == 0 && discardPile.Count == 0)
        {
            return;
        }
        foreach(CardModel card in drawPile.ToList())
        {
            await CardCmd.Exhaust(choiceContext, card, false, true);
        }
        foreach(CardModel card in discardPile.ToList())
        {
            await CardCmd.Exhaust(choiceContext, card, false, true);
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}

