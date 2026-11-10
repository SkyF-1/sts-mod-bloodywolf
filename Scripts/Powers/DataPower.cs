using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class DataPower : BloodywolfPowerModel, ISecondaryResourceHookListener
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;

	public decimal ModifySecondaryResourceGain(SecondaryResourceContext context, decimal amount)
    {
        if (amount > 0 &&
			context.Player == base.Owner.Player &&
			context.Definition.Id == BloodywolfResources.CloutId)
        {
            Flash();
            PowerCmd.TickDownDuration(this);
            return amount + 1m;
        }
        return amount;
    }
}
