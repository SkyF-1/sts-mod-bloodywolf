using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class FlourishingPower : BloodywolfPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;

	public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (participants.Contains(base.Owner))
		{
			Flash();
			await PowerCmd.Apply<RateToDrawPower>(new ThrowingPlayerChoiceContext(), base.Owner, base.Amount, base.Owner, null);
			// await PowerCmd.Apply<RateToEnergyPower>(new ThrowingPlayerChoiceContext(), base.Owner, base.Amount, base.Owner, null);
			await PowerCmd.Remove(this);
		}
	}
}
