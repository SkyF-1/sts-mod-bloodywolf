using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.CombatHistory;
using StsModBloodywolf.Scripts.Singletons;

namespace StsModBloodywolf.Scripts.Commands;

public static class MyCmd
{
    // ---------------------------------------------------------------------
    // 洗牌前减费单例入口
    // ---------------------------------------------------------------------

    private static CostReductionBeforeShuffledSingleton CostReductionBeforeShuffled
        => ModelDb.Singleton<CostReductionBeforeShuffledSingleton>();

    // ---------------------------------------------------------------------
    // 洗牌前减费命令
    // ---------------------------------------------------------------------

    /// <summary>
    /// 让一张卡在下次洗牌前减 amount 费（默认 1）。可重复调用累加。
    /// </summary>
    public static void ReduceCostBeforeShuffled(CardModel card, int amount = 1)
    {
        CostReductionBeforeShuffled.ReduceCost(card, amount);
    }

    /// <summary>
    /// 批量：让一组卡在下次洗牌前减 amount 费（默认 1）。
    /// </summary>
    public static void ReduceCostBeforeShuffled(IEnumerable<CardModel> cards, int amount = 1)
    {
        if (cards == null) return;

        foreach (var card in cards)
        {
            CostReductionBeforeShuffled.ReduceCost(card, amount);
        }
    }

    /// <summary>
    /// 让一张卡在下次洗牌前费用直接设为 cost。
    /// </summary>
    public static void SetCostBeforeShuffled(CardModel card, int cost)
    {
        CostReductionBeforeShuffled.SetCost(card, cost);
    }

    /// <summary>
    /// 让一张卡在下次洗牌前费用变为 0。
    /// </summary>
    public static void SetFreeBeforeShuffled(CardModel card)
    {
        CostReductionBeforeShuffled.SetCost(card, 0);
    }

    /// <summary>
    /// 清除一张卡的洗牌前减费标记。
    /// </summary>
    public static void ClearCostReductionBeforeShuffled(CardModel card)
    {
        CostReductionBeforeShuffled.ClearCostReduction(card);
    }

    // ---------------------------------------------------------------------
    // 原有命令
    // ---------------------------------------------------------------------

    public static async Task<decimal> GiveBlock(Creature creature, decimal amount, CardPlay? cardPlay, bool fast = false)
    {
        if (!creature.IsAlive) return -1;
        return await CreatureCmd.GainBlock(creature, amount, ValueProp.Unpowered, cardPlay, fast);
    }

    public static async Task<IEnumerable<DamageResult>> Troll(PlayerChoiceContext choiceContext, Creature target, decimal amount, Creature? dealer, CardModel? cardSource)
    {
        decimal modifiedAmount = MyHook.ModifyTrollDamage(target.CombatState, target, amount, dealer, cardSource);
        var damageResult = await CreatureCmd.Damage(choiceContext, target, modifiedAmount, ValueProp.Unpowered | ValueProp.Unblockable, dealer, cardSource, null);
        if (target.IsAlive)
        {
            decimal totalDamage = damageResult.Sum(r => r.TotalDamage);
            await CreatureCmd.GainBlock(target, totalDamage, ValueProp.Unpowered, null);
            var combat = target.CombatState;
            var entry = new TrollUsedEntry(
                dealer: dealer,
                target: target,
                damage: totalDamage,
                card: cardSource,
                roundNumber: combat?.RoundNumber ?? 0,
                currentSide: combat?.CurrentSide ?? CombatSide.Player,
                history: null!,                    // 我们用自定义列表管理，不依赖官方历史
                players: combat?.Players ?? Array.Empty<Player>()
            );
            TrollHistoryManager.Record(entry);
            await MyHook.AfterTroll(target.CombatState, choiceContext, target, totalDamage, dealer, cardSource);
        }
        return damageResult;
    }

    public static async Task<IEnumerable<DamageResult>> Troll(PlayerChoiceContext choiceContext, List<Creature> targets, decimal amount, Creature? dealer, CardModel? cardSource)
    {
        var allDamageResults = new List<DamageResult>();

        foreach (var target in targets)
        {
            IEnumerable<DamageResult> damageResults;
            if (dealer is null)
            {
                damageResults = await Troll(choiceContext, target, amount, dealer, cardSource);
            }
            else
            {
                damageResults = await Troll(choiceContext, target, amount, dealer, cardSource);
            }

            allDamageResults.AddRange(damageResults);
        }

        return allDamageResults;
    }
}