using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace StsModBloodywolf.Scripts.Singletons;

/// <summary>
/// 洗牌前减费单例。
/// 局内调用 ReduceCost(card, n) 给卡减 n 费，洗牌时全部清空。
/// </summary>
public class CostReductionBeforeShuffledSingleton : CustomSingletonModel
{
    // 累积减费量：值是正数，表示减多少费
    private readonly Dictionary<CardModel, int> _costReductions = new();

    // 绝对费用覆盖：值是最终费用，优先级高于 _costReductions
    private readonly Dictionary<CardModel, int> _absoluteCosts = new();

    public CostReductionBeforeShuffledSingleton() : base(true, true) { }

    /// <summary>
    /// 让这张卡减 amount 费，可累加。
    /// ReduceCost(card, 2) 然后 ReduceCost(card, 3) → 总共减 5。
    /// </summary>
    public void ReduceCost(CardModel card, int amount = 1)
    {
        if (!CanModify(card) || amount <= 0) return;

        _costReductions.TryGetValue(card, out int current);
        _costReductions[card] = current + amount;
        card.InvokeEnergyCostChanged();
    }

    /// <summary>
    /// 把这张卡的费用直接设成 cost。
    /// 与 ReduceCost 混用时，SetCost 优先。
    /// </summary>
    public void SetCost(CardModel card, int cost)
    {
        if (!CanModify(card)) return;

        _absoluteCosts[card] = Math.Max(0, cost);
        card.InvokeEnergyCostChanged();
    }

    /// <summary>
    /// 清除单张卡的减费标记。
    /// </summary>
    public void ClearCostReduction(CardModel card)
    {
        if (card == null) return;

        bool changed = _costReductions.Remove(card);
        changed |= _absoluteCosts.Remove(card);
        if (changed) card.InvokeEnergyCostChanged();
    }

    /// <summary>
    /// 洗牌时清空所有减费标记。
    /// </summary>
    public override void ModifyShuffleOrder(Player player, List<CardModel> cards, bool isInitialShuffle)
    {
        if (_costReductions.Count == 0 && _absoluteCosts.Count == 0) return;

        foreach (var card in _costReductions.Keys) card.InvokeEnergyCostChanged();
        foreach (var card in _absoluteCosts.Keys) card.InvokeEnergyCostChanged();

        _costReductions.Clear();
        _absoluteCosts.Clear();
    }

    public override bool TryModifyEnergyCostInCombat(
        CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card == null) return false;

        // 绝对覆盖优先
        if (_absoluteCosts.TryGetValue(card, out int abs))
        {
            modifiedCost = abs;
            return modifiedCost != originalCost;
        }

        // 相对减费
        if (_costReductions.TryGetValue(card, out int reduction))
        {
            modifiedCost = Math.Max(0, originalCost - reduction);
            return modifiedCost != originalCost;
        }

        return false;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card?.CloneOf == null) return Task.CompletedTask;

        bool changed = false;

        if (_costReductions.TryGetValue(card.CloneOf, out int r))
        {
            _costReductions[card] = r;
            changed = true;
        }
        if (_absoluteCosts.TryGetValue(card.CloneOf, out int a))
        {
            _absoluteCosts[card] = a;
            changed = true;
        }

        if (changed) card.InvokeEnergyCostChanged();
        return Task.CompletedTask;
    }

    private static bool CanModify(CardModel card)
    {
        return card != null
            && !card.IsCanonical
            && !card.EnergyCost.CostsX;
    }
}