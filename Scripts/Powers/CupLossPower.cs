using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;


namespace StsModBloodywolf.Scripts.Powers;

public sealed class CupLossPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Debuff;
	public override PowerStackType StackType => PowerStackType.Single;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? creature, CardModel? __)
	{
		if (creature == base.Owner && result.UnblockedDamage > 0)
		{
			await PowerCmd.Remove(this);
		}
        if (creature.IsPlayer && target == base.Owner && result.BlockedDamage > 0)
        {
			Flash();
            await CreatureCmd.GainBlock(creature, new BlockVar(result.BlockedDamage, ValueProp.Unpowered), null);
        }
	}
}
