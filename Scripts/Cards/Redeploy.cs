using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]

public sealed class Redeploy : CustomCardModel
{    /// 再部署
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(4m, ValueProp.Move),
        new CloutLossVar(2m)
    };
    
    protected override bool IsPlayable => (base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0) >= base.DynamicVars[CloutLossVar.Key].BaseValue;

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    
	public Redeploy()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{		
		// 失去1影响
		decimal cloutAmount = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
		if (cloutAmount >= base.DynamicVars[CloutLossVar.Key].BaseValue)
		{
			await PowerCmd.Apply<CloutPower>(
				base.Owner.Creature,
				-base.DynamicVars[CloutLossVar.Key].BaseValue,
				base.Owner.Creature,
				this);
		}
		
		
		// 将弃牌堆的1张牌置于抽牌堆顶
		CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
		CardPile pile = PileType.Discard.GetPile(base.Owner);
		if (pile.Cards.Count > 0)
		{
			IEnumerable<CardModel> cardModels = await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, base.Owner, prefs);
			foreach (CardModel cardModel in cardModels)
			{
				await CardPileCmd.Add(cardModel, PileType.Draw, CardPilePosition.Top);
			}
		}
	}
	

	protected override PileType GetResultPileType()
	{
		PileType resultPileType = base.GetResultPileType();
		if (resultPileType != PileType.Discard)
		{
			return resultPileType;
		}
		return PileType.Hand;
	}


	protected override void OnUpgrade()
	{
		base.DynamicVars[CloutLossVar.Key].UpgradeValueBy(-1m);
	}
}
