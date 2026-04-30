// using BaseLib.Abstracts;
// using BaseLib.Utils;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MegaCrit.Sts2.Core.ValueProps;
// using MegaCrit.Sts2.Core.Combat;
// using MegaCrit.Sts2.Core.Combat.History.Entries;
// using MegaCrit.Sts2.Core.Entities.Creatures;
// using MegaCrit.Sts2.Core.HoverTips;
// using MegaCrit.Sts2.Core.Models;
// using StsModBloodywolf.Scripts.Pools;

// namespace StsModBloodywolf.Scripts.Cards;

// [Pool(typeof(BloodywolfCardPool))]
// public sealed class EndOfTunnel : CustomCardModel
// {
// 	private const string _calculatedHitsKey = "CalculatedHits";
//     public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
// 	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
// 	{
// 		new DamageVar(8m, ValueProp.Move),
// 		new CalculationBaseVar(0m),
// 		new CalculationExtraVar(1m),
// 		new CalculatedVar("CalculatedHits").WithMultiplier(delegate(CardModel card, Creature? _)
// 		{
// 			if (card.Pile == null)
// 			{
// 				return 0m;
// 			}
// 			int num = (from e in CombatManager.Instance.History.Entries.OfType<EnergySpentEntry>()
// 				where e.HappenedThisTurn(card.CombatState) && e.Actor.Player == card.Owner
// 				select e).Sum((EnergySpentEntry c) => c.Amount);
// 			if (card.Pile.Type == PileType.Play)
// 			{
// 				num -= card.EnergyCost.GetWithModifiers(CostModifiers.All);
// 			}
// 			return num;
// 		})
// 	};

// 	protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>{base.EnergyHoverTip};

// 	public EndOfTunnel()
// 		: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
// 	{
// 	}

// 	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
// 	{
// 		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
//         await PlayerCmd.GainEnergy(((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target), base.Owner);
// 		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount((int)((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard(this)
// 			.Targeting(cardPlay.Target)
// 			.WithHitFx("vfx/vfx_attack_slash")
// 			.Execute(choiceContext);
// 	}

// 	protected override void OnUpgrade()
// 	{
// 		base.DynamicVars.Damage.UpgradeValueBy(2m);
// 	}
// }
