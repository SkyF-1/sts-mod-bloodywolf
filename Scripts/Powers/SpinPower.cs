using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using static MyHook;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class SpinPower : CustomPowerModel, IModifyTrollDamageListener
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	public decimal ModifyTrollDamage(ICombatState combatState, Creature? target, decimal originalDamage, Creature? dealer, CardModel? cardSource)
    {
		if (dealer != base.Owner)
		{
			return originalDamage;
		}
        return originalDamage + Amount;
    }

}
