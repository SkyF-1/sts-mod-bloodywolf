using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class EndOfTunnel : BloodywolfCardModel
{//苦尽甘来
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>{ CardKeyword.Innate };
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new DamageVar(14m, ValueProp.Move),
		new CalculationBaseVar(0m),
		new CalculationExtraVar(1m),
		new CalculatedVar("CalculatedHits").WithMultiplier((CardModel c, Creature? __) => c.Owner.Creature.CombatState.RoundNumber)
	};

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>{base.EnergyHoverTip};

	public EndOfTunnel()
		: base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .WithHitCount((int)((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard(this)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(4m);
	}
}
