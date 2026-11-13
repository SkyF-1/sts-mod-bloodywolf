using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Singletons;
using STS2RitsuLib.Models;

namespace StsModBloodywolf.Scripts.Commands;

public static class MyCmd
{
    // ---------------------------------------------------------------------
    // 洗牌前减费单例入口
    // ---------------------------------------------------------------------

    private static T GetRunModel<T>(CardModel card) where T : HookedSingletonModel
    {
#if STS2_API_0_113_0
        return card.Owner.RunState.RunModels.OfType<T>().SingleOrDefault()
            ?? throw new InvalidOperationException($"Current run does not contain {typeof(T).Name}.");
#elif STS2_API_0_107_1
        return ModelDb.Singleton<T>();
#else
#error Unsupported Sts2ApiCompat. Add an explicit MyCmd run-model branch.
#endif
    }

    // ---------------------------------------------------------------------
    // 洗牌前减费命令
    // ---------------------------------------------------------------------

    /// <summary>
    /// 让一张卡在下次洗牌前减 amount 费（默认 1）。可重复调用累加。
    /// </summary>
    public static void ReduceCostBeforeShuffled(CardModel card, int amount = 1)
    {
        GetRunModel<CostReductionBeforeShuffledSingleton>(card).ReduceCost(card, amount);
    }

    /// <summary>
    /// 批量：让一组卡在下次洗牌前减 amount 费（默认 1）。
    /// </summary>
    public static void ReduceCostBeforeShuffled(IEnumerable<CardModel> cards, int amount = 1)
    {
        if (cards == null) return;

        foreach (var card in cards)
        {
            GetRunModel<CostReductionBeforeShuffledSingleton>(card).ReduceCost(card, amount);
        }
    }

    /// <summary>
    /// 让一张卡在下次洗牌前费用直接设为 cost。
    /// </summary>
    public static void SetCostBeforeShuffled(CardModel card, int cost)
    {
        GetRunModel<CostReductionBeforeShuffledSingleton>(card).SetCost(card, cost);
    }

    /// <summary>
    /// 让一张卡在下次洗牌前费用变为 0。
    /// </summary>
    public static void SetFreeBeforeShuffled(CardModel card)
    {
        GetRunModel<CostReductionBeforeShuffledSingleton>(card).SetCost(card, 0);
    }

    /// <summary>
    /// 清除一张卡的洗牌前减费标记。
    /// </summary>
    public static void ClearCostReductionBeforeShuffled(CardModel card)
    {
        GetRunModel<CostReductionBeforeShuffledSingleton>(card).ClearCostReduction(card);
    }

    // ---------------------------------------------------------------------
    // 原有命令
    // ---------------------------------------------------------------------

    public static async Task<decimal> GiveBlock(Creature creature, decimal amount, CardPlay? cardPlay, bool fast = false)
    {
        if (!creature.IsAlive) return -1;
        return await CreatureCmd.GainBlock(creature, amount, ValueProp.Unpowered, cardPlay, fast);
    }

}
