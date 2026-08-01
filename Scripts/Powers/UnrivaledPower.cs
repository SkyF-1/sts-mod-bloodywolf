using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.DynamicVars;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class UnrivaledPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new HotTakeVar(9m)
	];

	private bool ShouldGetReward
	{
		get
		{
			return base.DynamicVars[HotTakeVar.Key].BaseValue <= (base.Owner.GetPower<CloutPower>()?.Amount ?? 0m);
		}
	}
	public override PowerStackType StackType => PowerStackType.Counter;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	public override Task AfterCombatEnd(CombatRoom room)
	{
		if(!ShouldGetReward) return Task.CompletedTask;
		room.AddExtraReward(base.Owner.Player, new RelicReward(RelicRarity.Rare, base.Owner.Player));
		return Task.CompletedTask;
	}
}
