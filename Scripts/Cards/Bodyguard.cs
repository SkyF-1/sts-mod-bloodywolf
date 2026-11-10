using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class Bodyguard : BloodywolfCardModel
{/// 护至身前
    public override string PortraitPath => AssetProfile.PortraitPath!;

	public Bodyguard()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
		this.SecondaryCosts().Set(BloodywolfResources.CloutId, 3);
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardSelectorPrefs prefs;
        if(PileType.Draw.GetPile(base.Owner).Cards.Count() == 1)
            prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 0, 1);
        else
            prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
		List<CardModel> cardsIn = (from c in PileType.Draw.GetPile(base.Owner).Cards
			orderby c.Rarity, c.Id
			select c).ToList();
		CardModel ?cardModel = (await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, base.Owner, prefs)).FirstOrDefault();

        if (!base.Keywords.Contains(CardKeyword.Exhaust) && !base.ExhaustOnNextPlay)
        {
            await CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Random);
        }
        if(cardModel != null)
        await CardCmd.AutoPlay(choiceContext, cardModel, null);
    }

	protected override void OnUpgrade()
    {
        this.SecondaryCosts().Set(BloodywolfResources.CloutId, 2);
    }
}
