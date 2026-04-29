using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;


namespace StsModBloodywolf.Scripts.Powers;

public sealed class UnrankedPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;
	public static string Key => "UnrankedPower";
	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
	{
		if (side == base.Owner.Side)
		{
			Flash();
			IEnumerable<Creature> enemies = combatState.HittableEnemies;
			foreach(Creature enemy in enemies.Where(e => e.GetPowerAmount<CupLossPower>() > 0))
			{
				await PowerCmd.Apply<VulnerablePower>(enemy, Amount, enemy, null);
			}
		}
	}
}
