using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class StickPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Single;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>{HoverTipFactory.Static(StaticHoverTip.Block)};

	public override bool ShouldClearBlock(Creature creature)
	{
		if (base.Owner != creature)
		{
			return true;
		}
		return false;
	}

	public override Task AfterPreventingBlockClear(AbstractModel preventer, Creature creature)
	{
		if (this != preventer)
		{
			return Task.CompletedTask;
		}
		Flash();
		return Task.CompletedTask;
	}

	public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
	{
		if (side == CombatSide.Player)
		{
			await PowerCmd.Remove(this);
		}
	}
}

