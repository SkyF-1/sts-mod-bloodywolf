using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Combat;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class FakeAccountPower : BloodywolfPowerModel
{

	public override PowerType Type => PowerType.Buff;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;

	public override PowerStackType StackType => PowerStackType.Counter;

public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (side == base.Owner.Side)
		{
			await PowerCmd.Apply<RealAccountPower>(new BlockingPlayerChoiceContext(), base.Owner, Amount, base.Owner, null);
			await PowerCmd.Remove(this);
		}
	}
}
