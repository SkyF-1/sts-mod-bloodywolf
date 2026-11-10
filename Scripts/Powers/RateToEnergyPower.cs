using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class RateToEnergyPower : BloodywolfPowerModel, ISecondaryResourceHookListener
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;

	public async Task AfterSecondaryResourceChanged(SecondaryResourceChangeContext context)
	{
		if (context.Delta > 0 &&
			context.Definition.Id == BloodywolfResources.CloutId &&
			context.Player == base.Owner.Player)
		{
			Flash();
			await PlayerCmd.GainEnergy(Amount, base.Owner.Player);
		}
	}
	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side == base.Owner.Side)
		{
			await PowerCmd.Remove(this);
		}
	}
}
