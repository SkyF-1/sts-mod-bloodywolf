using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.DynamicVars;

namespace StsModBloodywolf.Scripts.CombatHistory;

/// <summary>
/// Records speech-card plays whose extra effect is active.
/// </summary>
public static class HotTakeHistory
{
    private static readonly List<CardPlay> _triggeredPlays = [];

    public static IReadOnlyList<CardPlay> Entries => _triggeredPlays;

    public static CardPlay? LastTriggered(Player player)
    {
        return _triggeredPlays.LastOrDefault(play => play.Player == player);
    }

    internal static void RecordIfTriggered(CardPlay cardPlay)
    {
        CardModel card = cardPlay.Card;
        if (!card.DynamicVars.ContainsKey(HotTakeVar.Key))
        {
            return;
        }

        if (card.ShouldGlowGold || card.ShouldGlowRed)
        {
            _triggeredPlays.Add(cardPlay);
        }
    }

    public static void Clear()
    {
        _triggeredPlays.Clear();
    }
}

[HarmonyPatch(
    typeof(MegaCrit.Sts2.Core.Combat.History.CombatHistory),
    nameof(MegaCrit.Sts2.Core.Combat.History.CombatHistory.CardPlayStarted))]
internal static class HotTakeHistoryPatch
{
    private static void Postfix(CardPlay cardPlay)
    {
        HotTakeHistory.RecordIfTriggered(cardPlay);
    }
}
