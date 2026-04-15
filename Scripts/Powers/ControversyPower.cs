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

public sealed class ControversyPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new CloutLossVar(1m)
    };
	public override async Task AfterEnergyReset(Player player)
	{
		if (player == base.Owner.Player)
		{
			decimal CloutValue = base.Owner.GetPower<CloutPower>()?.Amount ?? 0;
			if (CloutValue >= base.DynamicVars[CloutLossVar.Key].BaseValue)
			{
				await PowerCmd.Apply<CloutPower>(
				base.Owner, 
				-base.DynamicVars[CloutLossVar.Key].BaseValue,
				base.Owner, 
				null);
				Flash();
				await PlayerCmd.GainEnergy(Amount, player);
        	}
		}
	}
}
