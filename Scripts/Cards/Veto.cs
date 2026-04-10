using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Veto : CustomCardModel
{
    /// 不通过
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new CardsVar(1)
    };

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    public Veto()
        : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 0, base.DynamicVars.Cards.IntValue);
        CardPile pile = PileType.Draw.GetPile(base.Owner);
        if (pile.Cards.Count == 0)
        {
            return;
        }
        IEnumerable<CardModel> cardModels = await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, base.Owner, prefs);
        foreach (CardModel cardModel in cardModels)
        {
            await CardPileCmd.Add(cardModel, PileType.Discard, CardPilePosition.Top);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1);
    }
}