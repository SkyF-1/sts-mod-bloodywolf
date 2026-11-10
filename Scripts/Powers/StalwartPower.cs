using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class StalwartPower : BloodywolfPowerModel, ISecondaryResourceHookListener
{
	public override PowerType Type => PowerType.Debuff;

	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;
	public async Task AfterSecondaryResourceChanged(SecondaryResourceChangeContext context)
	{
		if (context.Delta != 0 &&
			context.Definition.Id == BloodywolfResources.CloutId &&
			context.Player == base.Applier?.Player)
		{
			Flash();
			await CreatureCmd.Damage(
				new BlockingPlayerChoiceContext(),
				base.Owner,
				Amount,
				ValueProp.Unblockable | ValueProp.Unpowered,
				null,
				null);
		}
	}
	public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (side == base.Owner.Side)
		{
			await PowerCmd.Remove(this);
		}
	}
}
