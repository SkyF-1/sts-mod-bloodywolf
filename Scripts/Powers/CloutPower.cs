using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class CloutPower : BloodywolfPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;

	public override async Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side == base.Owner.Side)
		{
			await PowerCmd.Decrement(this);
		}
	}
}
