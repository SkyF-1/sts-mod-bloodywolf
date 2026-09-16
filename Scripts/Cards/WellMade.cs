using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Commands;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class WellMade : BloodywolfCardModel
{/// 制作精良
    protected override bool ShouldGlowGoldInternal => base.Owner.Creature.GetPower<CloutPower>()?.Amount >= base.DynamicVars[HotTakeVar.Key].BaseValue;
	protected override IEnumerable<DynamicVar> CanonicalVars => [
	new HotTakeVar(5m),
	new CardsVar(2)
    ];
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public WellMade()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{  
		List<CardModel> drawnCards = (await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner)).ToList();
        decimal CloutValue = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
		if (CloutValue >= base.DynamicVars[HotTakeVar.Key].BaseValue && drawnCards.Count > 0)
        {
			CardModel card = drawnCards[Random.Shared.Next(drawnCards.Count)];
			if (IsUpgraded)
			{
				MyCmd.SetFreeBeforeShuffled(card);
			}
			else
			{
				MyCmd.ReduceCostBeforeShuffled(card);
			}
        }

	}
}
