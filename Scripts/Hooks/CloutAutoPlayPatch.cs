using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Cards.FreePlay;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Hooks;

[HarmonyPatch(typeof(Hook), nameof(Hook.BeforeCardAutoPlayed))]
internal static class CloutAutoPlayPatch
{
    private static readonly ConditionalWeakTable<CardModel, object> ResumeOriginal = new();

    [HarmonyPrefix]
    private static bool Prefix(
        ICombatState combatState,
        CardModel card,
        Creature? target,
        AutoPlayType type,
        ref Task __result)
    {
        if (ResumeOriginal.TryGetValue(card, out _))
        {
            ResumeOriginal.Remove(card);
            return true;
        }

        __result = PayThenRunOriginal(combatState, card, target, type);
        return false;
    }

    private static async Task PayThenRunOriginal(
        ICombatState combatState,
        CardModel card,
        Creature? target,
        AutoPlayType type)
    {
        try
        {
            await PayCloutCost(card);
        }
        catch (Exception ex)
        {
            Entry.Logger.Warn($"Auto-play Clout payment failed for {card.Id.Entry}: {ex}");
        }

        var token = new object();
        ResumeOriginal.Add(card, token);
        try
        {
            await Hook.BeforeCardAutoPlayed(combatState, card, target, type);
        }
        finally
        {
            if (ResumeOriginal.TryGetValue(card, out var current) && ReferenceEquals(current, token))
                ResumeOriginal.Remove(card);
        }
    }

    private static async Task PayCloutCost(CardModel card)
    {
        if (card.Owner == null || FreePlayBindingRegistry.IsCardFreeForUpcomingPlay(card))
            return;

        var plan = SecondaryResourcePaymentResolver.Plan(card);
        int cost = plan.Lines
            .Where(line => !line.IsFree && line.ResourceId == BloodywolfResources.CloutId)
            .Sum(line => line.Cost);
        if (cost <= 0)
            return;

        int amountToSpend = Math.Min(BloodywolfResources.GetClout(card.Owner), cost);
        if (amountToSpend > 0)
            await BloodywolfResources.SpendClout(card.Owner, amountToSpend, card, card);
    }
}
