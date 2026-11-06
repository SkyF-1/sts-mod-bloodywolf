using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class DataPower : BloodywolfPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;

	public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? giver, out decimal modifiedAmount)
    {
        modifiedAmount = amount;
        if (amount > 0 && canonicalPower is CloutPower && giver == base.Owner && target == base.Owner)
        {
            Flash();
            modifiedAmount = amount + 1m;
            PowerCmd.TickDownDuration(this);
            return true;
        }
        return false;
    }
}
