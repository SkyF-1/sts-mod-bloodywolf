using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class JellyLegsPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Debuff;

	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";

	public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? _, CardModel? __)
	{
		if (target == base.Owner && props.IsPoweredAttack() && result.UnblockedDamage > 0)
		{
			await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner, -Amount, base.Owner, null);
		}
	}
}
