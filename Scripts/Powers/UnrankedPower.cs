using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;


namespace StsModBloodywolf.Scripts.Powers;

public sealed class UnrankedPower : BloodywolfPowerModel
{
	public override PowerType Type => PowerType.Buff;
	public static string Key => "UnrankedPower";
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<StrengthPower>()
    };
	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomIconPath => AssetProfile.IconPath!;
    public override string? CustomBigIconPath => AssetProfile.BigIconPath!;
	public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (side == CombatSide.Enemy)
		{
			Flash();
			IEnumerable<Creature> enemies = combatState.HittableEnemies;
			foreach(Creature enemy in enemies.Where(e => e.GetPowerAmount<CupLossPower>() > 0))
			{
				await PowerCmd.Apply<StrengthPower>(new BlockingPlayerChoiceContext(), enemy, -Amount, enemy, null);
			}
		}
	}
}
