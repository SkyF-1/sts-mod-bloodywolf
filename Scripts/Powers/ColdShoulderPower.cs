using BaseLib.Abstracts;
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

public sealed class ColdShoulderPower : CustomPowerModel
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
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
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
    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
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
