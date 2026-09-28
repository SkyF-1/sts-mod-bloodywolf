using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;


namespace StsModBloodywolf.Scripts.Powers;

public sealed class ShutUpPower : CustomPowerModel
{
	private bool _shouldConsume;

	public override PowerType Type => PowerType.Buff;
	public static string Key => "ShutUpPower";
	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new CloutLossPowerVar(3m)
    };
	public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		_shouldConsume = false;
		if (target != base.Owner || amount <= 0m)
		{
			return amount;
		}
		decimal cloutLoss = base.DynamicVars[CloutLossPowerVar.Key].BaseValue;
		decimal cloutValue = base.Owner.GetPower<CloutPower>()?.Amount ?? 0m;
		if (cloutValue < cloutLoss)
		{
			return amount;
		}
		_shouldConsume = true;
		return 0m;
	}
	public override async Task AfterModifyingHpLostAfterOsty()
	{
		if (!_shouldConsume)
		{
			return;
		}
		_shouldConsume = false;
		await PowerCmd.Apply<CloutPower>(new BlockingPlayerChoiceContext(), base.Owner, -base.DynamicVars[CloutLossPowerVar.Key].BaseValue, base.Owner, null);
		Flash();
		await PowerCmd.Decrement(this);
	}
}
