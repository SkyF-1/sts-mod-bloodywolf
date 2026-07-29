using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

public static class MyHook
{
    public interface IAfterTrollListener
    {
        Task AfterTroll(PlayerChoiceContext choiceContext, Creature target, decimal damageDealt, Creature? dealer, CardModel? cardSource);
    }

    public interface IModifyTrollDamageListener
    {
        decimal ModifyTrollDamage(ICombatState combatState, Creature? target, decimal originalDamage, Creature? dealer, CardModel? cardSource);
    }

    /// <summary>
    /// 在 Troll 命令效果执行完毕后调用。
    /// </summary>
    public static async Task AfterTroll(ICombatState combatState, PlayerChoiceContext choiceContext, Creature target, decimal damageDealt, Creature? dealer, CardModel? cardSource)
    {
        foreach (var model in SafeIterate(combatState))
        {
            if (model is IAfterTrollListener listener)
            {
                await listener.AfterTroll(choiceContext, target, damageDealt, dealer, cardSource);
                model.InvokeExecutionFinished();
            }
        }
    }

    public static decimal ModifyTrollDamage(ICombatState combatState, Creature? target, decimal originalDamage, Creature? dealer, CardModel? cardSource)
    {
        decimal modified = originalDamage;
        foreach (var model in SafeIterate(combatState))
        {
            if (model is IModifyTrollDamageListener modifier)
            {
                modified = modifier.ModifyTrollDamage(combatState, target, modified, dealer, cardSource);
                model.InvokeExecutionFinished();
            }
        }
        return modified;
    }

    private static IEnumerable<AbstractModel> SafeIterate(ICombatState combatState)
    {
        if (CombatManager.Instance.IsOverOrEnding && !CombatManager.Instance.IsStarting)
            yield break;
        foreach (var model in combatState.IterateHookListeners())
            yield return model;
    }
}