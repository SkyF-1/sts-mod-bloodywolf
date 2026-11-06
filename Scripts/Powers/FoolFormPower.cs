using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class FoolFormPower : BloodywolfPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        decimal cloutStacks = base.Owner.GetPower<CloutPower>()?.Amount ?? 0;

        if (cloutStacks > 0 && side == CombatSide.Player)
        {
            await CreatureCmd.GainBlock(base.Owner, new BlockVar(cloutStacks * base.Amount, ValueProp.Unpowered), null);
        }
    }
}