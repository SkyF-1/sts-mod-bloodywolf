using BaseLib.Abstracts;
using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Entities.Cards;


namespace StsModBloodywolf.Scripts.Powers;

public sealed class CupLossPower : CustomPowerModel, IHealthBarForecastSource
{
	public override PowerType Type => PowerType.Debuff;
	public override PowerStackType StackType => PowerStackType.Counter;
	public const string Key = "CupLossPower";
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	// 注意：“格挡被削减时转移等量格挡”的逻辑不在这里实现，而是放在
	// CupLossBlockTransferSingleton 中。原因是目标被击杀时游戏会跳过
	// Hook.AfterDamageReceived，挂在目标身上的能力拿不到 DamageResult。
	// 基类的 Flash() 是 protected，单例需要通过这个入口触发闪烁提示。
	public void TriggerFlash() => Flash();

	public override async Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (!CombatManager.Instance.IsOverOrEnding && side == base.Owner.Side && !base.Owner.IsDead )
		{
			var amount = base.Amount;
			List<DamageResult> list = (await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner, amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null)).ToList();
			DamageResult damageResult = list.FirstOrDefault();
			if(base.Owner.IsAlive && damageResult != null)
			{
				var block = new BlockVar(damageResult.UnblockedDamage, ValueProp.Unpowered);
				await CreatureCmd.GainBlock(base.Owner, block, null);
			}
		}
	}

	public int CalculateTotalDamageNextTurn()
	{
		
		decimal damage = base.Amount;
		damage = Hook.ModifyDamage(base.Owner.CombatState.RunState, base.Owner.CombatState, base.Owner, null, damage, ValueProp.Unblockable | ValueProp.Unpowered, null, null, ModifyDamageHookType.All, CardPreviewMode.None, out IEnumerable<AbstractModel> _);
		return (int)damage;
	}
	public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        yield return new HealthBarForecastSegment(
            amount: this.CalculateTotalDamageNextTurn(),
            color: new Color("#65cbf3"),
            direction: HealthBarForecastDirection.FromRight
			//order: 0,
			//overlayMaterial: PreloadManager.Cache.GetMaterial("res://StsModBloodywolf/materials/cup_loss_bar_overlay.tres")
        );
    }
}
