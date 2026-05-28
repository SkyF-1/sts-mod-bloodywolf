using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Factories;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class HemAndHaw : CustomCardModel
{/// 支支吾吾
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>{
	new CardsVar(2),
	new PowerVar<HemAndHawPower>(3m)
	};

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public HemAndHaw()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<HemAndHawPower>(base.Owner.Creature, base.DynamicVars["HemAndHawPower"].BaseValue, base.Owner.Creature, this);
		IEnumerable<CardModel> forCombat = CardFactory.GetForCombat(base.Owner, base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint).Where(delegate(CardModel c)
		{
			CardEnergyCost energyCost = c.EnergyCost;
			return energyCost != null && energyCost.Canonical == 0 && !energyCost.CostsX;
		}), base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardGeneration);
		foreach (CardModel item in forCombat)
		{
			// if (base.IsUpgraded)
			// {
			// 	CardCmd.Upgrade(item);
			// }
			await CardPileCmd.AddGeneratedCardToCombat(item, PileType.Hand, addedByPlayer: true);
		}
	}
	protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1m);
		base.DynamicVars["HemAndHawPower"].UpgradeValueBy(2m);
    }
}
