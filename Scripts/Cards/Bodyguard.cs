using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Bodyguard : BloodywolfCardModel
{/// 护至身前
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> 
    { 
        new CloutLossVar(3m)
    };
    protected override bool IsPlayable => (base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0) >= base.DynamicVars[CloutLossVar.Key].BaseValue;

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public Bodyguard()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal cloutAmount = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        var cloutPower = base.Owner.Creature.GetPower<CloutPower>();
        if (cloutAmount >= base.DynamicVars[CloutLossVar.Key].BaseValue)
        {
            await PowerCmd.Apply<CloutPower>(choiceContext,
                base.Owner.Creature,
                -base.DynamicVars[CloutLossVar.Key].BaseValue,
                base.Owner.Creature,
                this);
        }
        else if (cloutPower != null)
        {
            await PowerCmd.Remove(cloutPower);
        }

        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
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
        base.DynamicVars[CloutLossVar.Key].UpgradeValueBy(-1m);
    }
}
