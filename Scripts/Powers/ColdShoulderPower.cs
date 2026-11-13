using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using StsModBloodywolf.Scripts.Powers;
using MegaCrit.Sts2.Core.Combat.History.Entries;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class ColdShoulderPower : BloodywolfPowerModel
{
	public override PowerType Type => PowerType.Buff;
    private bool _enable = false;
    public bool Enable
	{
		get
		{
			return _enable;
		}
		set
		{
			AssertMutable();
			_enable = value;
		}
	}
	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;
    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (participants.Contains(base.Owner))
		{
			if ((decimal)CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.HappenedThisTurn(Owner.CombatState) && e.CardPlay.Card.Owner.Creature == Owner) <= Amount)
			{
				Flash();
				Enable = true;
			}
		}
        else
        {
            Enable = false;
        }
	}
#if STS2_API_0_107_1
	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
#elif STS2_API_0_113_0
	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
#else
#error Unsupported Sts2ApiCompat. Add an explicit ColdShoulderPower damage branch.
#endif
	{
        if(Enable == false)
        {
            return 1m;
        }
		if (target != base.Owner)
		{
			return 1m;
		}
		if (!props.IsPoweredAttack())
		{
			return 1m;
		}
		return 0.5m;
	}
}
