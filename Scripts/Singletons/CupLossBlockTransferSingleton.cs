using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Singletons;

/// <summary>
/// 杯级丧失的“格挡被削减时，把等量格挡转移给伤害来源”全局单例。
///
/// 为什么不放在 CupLossPower 内部：
/// 目标被这一击击杀时，CreatureCmd.Damage 会跳过 Hook.AfterDamageReceived 的派发，
/// 挂在目标身上的 Power 永远拿不到 DamageResult（随后 Kill 又会清空它的能力）。
/// Hook.AfterDamageGiven 无论目标是否死亡都会派发，且派发时机在 Kill 之前，
/// 此时 Power 仍在目标身上，因此可以在单例里稳定完成转移。
/// </summary>
public class CupLossBlockTransferSingleton : CustomSingletonModel
{
    // receiveCombatHooks: true  → 注册为战斗内钩子订阅者（ModelDb.Init 时会实例化并订阅）
    // receiveRunHooks: false    → 不需要战斗外的钩子
    public CupLossBlockTransferSingleton() : base(true, false)
    {
    }

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature? target,
        CardModel? cardSource)
    {
        // 与原 Power 内实现保持一致：只处理玩家造成的伤害，且必须真的削减了格挡。
        if (dealer == null || !dealer.IsPlayer) return;
        if (target == null || result == null || result.BlockedDamage <= 0) return;

        CupLossPower? power = target.GetPower<CupLossPower>();
        if (power == null) return;

        power.TriggerFlash();
        await CreatureCmd.GainBlock(dealer, new BlockVar(result.BlockedDamage, ValueProp.Unpowered), null);
    }
}
