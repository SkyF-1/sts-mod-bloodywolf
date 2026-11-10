using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;


namespace StsModBloodywolf.Scripts.Powers;

public sealed class ShutUpPower : BloodywolfPowerModel
{
	private const int CloutCost = 3;
	private bool _shouldConsume;

	public override PowerType Type => PowerType.Buff;
	public static string Key => "ShutUpPower";
	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;
	protected override IEnumerable<DynamicVar> CanonicalVars =>
	[
		SecondaryResourceVars.For("CloutSpend", BloodywolfResources.CloutId, CloutCost)
	];
	public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		_shouldConsume = false;
		if (target != base.Owner || amount <= 0m)
		{
			return amount;
		}
		if (BloodywolfResources.GetClout(base.Owner) < CloutCost)
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
		if (base.Owner.Player is not { } player)
			return;

		if (await BloodywolfResources.SpendClout(player, CloutCost, source: this))
		{
			Flash();
			await PowerCmd.Decrement(this);
		}
	}
}
