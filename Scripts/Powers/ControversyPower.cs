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

public sealed class ControversyPower : BloodywolfPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;
	protected override IEnumerable<DynamicVar> CanonicalVars =>
	[
		SecondaryResourceVars.For("CloutSpend", BloodywolfResources.CloutId, 1m)
	];
	public override async Task AfterEnergyReset(Player player)
	{
		if (player == base.Owner.Player &&
			await BloodywolfResources.SpendClout(player, 1, source: this))
		{
			Flash();
			await PlayerCmd.GainEnergy(Amount, player);
		}
	}
}
