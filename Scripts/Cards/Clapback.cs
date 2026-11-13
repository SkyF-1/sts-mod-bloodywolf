using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Resources;
using MegaCrit.Sts2.Core.Combat;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Cards;


public sealed class Clapback : BloodywolfCardModel
{    /// 回怼
	private int _cloutCostThisTurn = 1;
	protected override IEnumerable<DynamicVar> CanonicalVars =>
	[
		new DamageVar(11m, ValueProp.Move)
	];

    public override string PortraitPath => AssetProfile.PortraitPath!;

	public Clapback()
		: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
		this.SecondaryCosts().Set(BloodywolfResources.CloutId, _cloutCostThisTurn);
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCardCompat(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(choiceContext);

		_cloutCostThisTurn++;
		this.SecondaryCosts().Set(
			BloodywolfResources.CloutId,
			new SecondaryResourceCost(_cloutCostThisTurn),
			SecondaryResourceCostDuration.ThisTurn);
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side == base.Owner.Creature.Side)
		{
			_cloutCostThisTurn = 1;
			if (this.TryGetSecondaryCosts(out var costs))
				costs.ClearDuration(SecondaryResourceCostDuration.ThisTurn);
		}
	}
#if STS2_API_0_107_1
	protected override PileType GetResultPileTypeForCardPlay()
	{
		PileType resultPileType = base.GetResultPileTypeForCardPlay();
		return resultPileType == PileType.Discard ? PileType.Hand : resultPileType;
	}
#elif STS2_API_0_113_0
	protected override CardLocation GetResultLocationForCardPlay()
	{
		CardLocation resultLocationForCardPlay = base.GetResultLocationForCardPlay();
		if (resultLocationForCardPlay.pileType == PileType.Discard)
		{
			resultLocationForCardPlay.pileType = PileType.Hand;
		}
		return resultLocationForCardPlay;
	}
#else
#error Unsupported Sts2ApiCompat. Add an explicit Clapback result-location branch.
#endif
	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(3m);
	}
}
