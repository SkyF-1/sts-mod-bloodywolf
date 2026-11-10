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

public sealed class PrestigePower : BloodywolfPowerModel, ISecondaryResourceHookListener
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
			await Cmd.CustomScaledWait(0.1f, 0.2f);

			Creature creature = base.Owner.Player.RunState.Rng.CombatTargets.NextItem(base.Owner.CombatState.HittableEnemies);
			if (creature != null)
			{
				VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_attack_blunt");
				await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), creature, Amount, ValueProp.Unpowered, base.Owner);
			}
		}
	}
}
