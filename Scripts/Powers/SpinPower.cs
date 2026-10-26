using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using static MyHook;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class SpinPower : CustomPowerModel, IModifyTrollDamageListener, IModifyTrollDamageMultiplicativeListener
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Single;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	public decimal ModifyTrollDamage(ICombatState combatState, Creature? target, decimal originalDamage, Creature? dealer, CardModel? cardSource)
    {
		if (dealer != base.Owner)
		{
			return originalDamage;
		}
        return originalDamage + (dealer.GetPower<StrengthPower>()?.ModifyDamageAdditive(
            target,
            originalDamage,
            ValueProp.Move,
            dealer,
            cardSource,
            null) ?? 0m);
    }

    public decimal ModifyTrollDamageMultiplicative(ICombatState combatState, Creature? target, decimal originalDamage, Creature? dealer, CardModel? cardSource)
    {
        if (dealer != base.Owner || target == null)
        {
            return 1m;
        }

        return target.GetPower<VulnerablePower>()?.ModifyDamageMultiplicative(
            target,
            originalDamage,
            ValueProp.Move,
            dealer,
            cardSource,
            null) ?? 1m;
    }
}
