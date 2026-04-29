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
	public override PowerType Type => PowerType.Buff;
	public static string Key => "ShutUpPower";
	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new CloutLossVar(3m)
    };
	public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (target != base.Owner)
		{
			return amount;
		}
		decimal CloutValue = base.Owner.GetPower<CloutPower>()?.Amount ?? 0;
		if (CloutValue < base.DynamicVars[CloutLossVar.Key].BaseValue)
		{
			return amount;
		}
		if(amount > 0)
		{
			PowerCmd.Apply<CloutPower>(base.Owner, -base.DynamicVars[CloutLossVar.Key].BaseValue, base.Owner, null);
			Flash();
			PowerCmd.Decrement(this);
			return 0m;
		}
		return amount;
	}
}
