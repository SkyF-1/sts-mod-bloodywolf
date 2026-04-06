using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Players;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(TokenCardPool))]
public sealed class IBiteYou : CustomCardModel
{/// 我咬死你
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new DamageVar(3m, ValueProp.Move),
        new RateVar(1m)
	};
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>{CardKeyword.Exhaust};

	public IBiteYou()
		: base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		AttackCommand attackCommand = DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target);
		await PowerCmd.Apply<CloutPower>(
            base.Owner.Creature, 
            base.DynamicVars[RateVar.Key].BaseValue, 
            base.Owner.Creature, 
            this);
		await attackCommand.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(1m);
	}

	public static async Task<CardModel?> CreateInHand(Player owner, CombatState combatState)
	{
		return (await CreateInHand(owner, 1, combatState)).FirstOrDefault();
	}

	public static async Task<IEnumerable<CardModel>> CreateInHand(Player owner, int count, CombatState combatState)
	{
		if (count == 0)
		{
			return Array.Empty<CardModel>();
		}
		if (CombatManager.Instance.IsOverOrEnding)
		{
			return Array.Empty<CardModel>();
		}
		List<CardModel> bites = new List<CardModel>();
		for (int i = 0; i < count; i++)
		{
			bites.Add(combatState.CreateCard<IBiteYou>(owner));
		}
		await CardPileCmd.AddGeneratedCardsToCombat(bites, PileType.Hand, addedByPlayer: true);
		return bites;
	}
}
