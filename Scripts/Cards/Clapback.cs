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
using MegaCrit.Sts2.Core.Combat;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]

public sealed class Clapback : BloodywolfCardModel
{    /// 回怼
	private decimal _baseCloutLoss = 1m;
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(11m, ValueProp.Move),
        new CloutLossVar(_baseCloutLoss)
    };
    
    protected override bool IsPlayable => (base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0) >= base.DynamicVars[CloutLossVar.Key].BaseValue;

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    
	public Clapback()
		: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{		
		// 失去影响
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
		
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);

		base.DynamicVars[CloutLossVar.Key].BaseValue += 1;
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side == base.Owner.Creature.Side)
		{
			base.DynamicVars[CloutLossVar.Key].BaseValue = _baseCloutLoss;
		}
	}
	protected override PileType GetResultPileTypeForCardPlay()
	{
		PileType resultPileTypeForCardPlay = base.GetResultPileTypeForCardPlay();
		if (resultPileTypeForCardPlay != PileType.Discard)
		{
			return resultPileTypeForCardPlay;
		}
		return PileType.Hand;
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(5m);
	}
}
