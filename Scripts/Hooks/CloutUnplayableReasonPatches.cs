using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Cards;
using StsModBloodywolf.Scripts.Char;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Cards.FreePlay;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Hooks;

internal static class CloutUnplayableReason
{
    internal const UnplayableReason CostTooHigh =
        (UnplayableReason)(1 << 30);

    private const UnplayableReason HigherPriorityReasons =
        UnplayableReason.NoLivingAllies |
        UnplayableReason.EnergyCostTooHigh |
        UnplayableReason.StarCostTooHigh |
        UnplayableReason.BlockedByHook |
        UnplayableReason.HasUnplayableKeyword;

    private static bool _internalPatchesReady;

    internal static bool InternalPatchesReady
    {
        get => Volatile.Read(ref _internalPatchesReady);
        set => Volatile.Write(ref _internalPatchesReady, value);
    }

    internal static bool IsCloutCostUnpayable(CardModel card)
    {
        if (!InternalPatchesReady ||
            card.IsCanonical ||
            card is not BloodywolfCardModel ||
            card.Owner.Character is not Bloodywolf ||
            FreePlayBindingRegistry.IsCardFreeForUpcomingPlay(card))
            return false;

        var plan = SecondaryResourcePaymentResolver.Plan(card);
        return plan.Lines.Any(line =>
            line.ResourceId == BloodywolfResources.CloutId &&
            line.Kind == SecondaryResourceUseKind.RequiredCost &&
            !line.CanPlay);
    }

    internal static bool HasHigherPriorityReason(UnplayableReason reason)
    {
        return (reason & HigherPriorityReasons) != UnplayableReason.None;
    }
}

internal static class CloutUnplayableReasonInstaller
{
    private const string UnplayableReasonExtensionsType =
        "MegaCrit.Sts2.Core.Entities.Cards.UnplayableReasonExtensions";

    internal static bool Install(Harmony harmony)
    {
        if (CloutUnplayableReason.InternalPatchesReady)
            return true;

        var extensionsType = AccessTools.TypeByName(UnplayableReasonExtensionsType);
        if (extensionsType == null)
        {
            Entry.Logger.Warn(
                $"Could not install Clout unplayable-reason patches: " +
                $"type {UnplayableReasonExtensionsType} was not found.");
            return false;
        }

        var hasResourceCostReasonMethod = AccessTools.Method(
            extensionsType,
            "HasResourceCostReason",
            [typeof(UnplayableReason)]);
        var getPlayerDialogueLineMethod = AccessTools.Method(
            extensionsType,
            "GetPlayerDialogueLine",
            [typeof(UnplayableReason), typeof(AbstractModel)]);
        var cardModelCanPlayMethod = AccessTools.Method(
            typeof(CardModel),
            nameof(CardModel.CanPlay),
            [typeof(UnplayableReason).MakeByRefType(), typeof(AbstractModel).MakeByRefType()]);

        if (hasResourceCostReasonMethod == null ||
            getPlayerDialogueLineMethod == null ||
            cardModelCanPlayMethod == null)
        {
            Entry.Logger.Warn(
                "Could not install Clout unplayable-reason patches: " +
                "one or more required methods were not found.");
            return false;
        }

        var cardModelCanPlayPostfix = new HarmonyMethod(
            AccessTools.Method(
                typeof(CloutUnplayableReasonPatches),
                nameof(CloutUnplayableReasonPatches.CardModelCanPlayPostfix)))
        {
            priority = Priority.Last,
        };
        var hasResourceCostReasonPostfix = new HarmonyMethod(
            AccessTools.Method(
                typeof(CloutUnplayableReasonPatches),
                nameof(CloutUnplayableReasonPatches.HasResourceCostReasonPostfix)))
        {
            priority = Priority.Low,
        };
        var getPlayerDialogueLinePrefix = new HarmonyMethod(
            AccessTools.Method(
                typeof(CloutUnplayableReasonPatches),
                nameof(CloutUnplayableReasonPatches.GetPlayerDialogueLinePrefix)))
        {
            priority = Priority.Low,
        };

        var installed = new List<(MethodInfo Original, MethodInfo Patch)>();
        try
        {
            harmony.Patch(
                cardModelCanPlayMethod,
                postfix: cardModelCanPlayPostfix);
            installed.Add((cardModelCanPlayMethod, cardModelCanPlayPostfix.method));

            harmony.Patch(
                hasResourceCostReasonMethod,
                postfix: hasResourceCostReasonPostfix);
            installed.Add((hasResourceCostReasonMethod, hasResourceCostReasonPostfix.method));

            harmony.Patch(
                getPlayerDialogueLineMethod,
                prefix: getPlayerDialogueLinePrefix);
            installed.Add((getPlayerDialogueLineMethod, getPlayerDialogueLinePrefix.method));
        }
        catch (Exception ex)
        {
            foreach (var (original, patch) in installed)
                harmony.Unpatch(original, patch);

            Entry.Logger.Warn(
                $"Could not install Clout unplayable-reason patches: {ex}");
            return false;
        }

        CloutUnplayableReason.InternalPatchesReady = true;
        Entry.Logger.Info("Installed Clout unplayable-reason compatibility patches.");
        return true;
    }
}

internal static class CloutUnplayableReasonPatches
{
    internal static void CardModelCanPlayPostfix(
        CardModel __instance,
        ref bool __result,
        ref UnplayableReason __0)
    {
        if (!CloutUnplayableReason.IsCloutCostUnpayable(__instance))
            return;

        __0 |= CloutUnplayableReason.CostTooHigh;
        __result = false;
    }

    internal static void HasResourceCostReasonPostfix(
        UnplayableReason __0,
        ref bool __result)
    {
        if ((__0 & CloutUnplayableReason.CostTooHigh) != UnplayableReason.None)
            __result = true;
    }

    internal static bool GetPlayerDialogueLinePrefix(
        UnplayableReason __0,
        ref LocString? __result)
    {
        if ((__0 & CloutUnplayableReason.CostTooHigh) == UnplayableReason.None ||
            CloutUnplayableReason.HasHigherPriorityReason(__0))
            return true;

        __result = new LocString(
            "combat_messages",
            "STS_MOD_BLOODYWOLF_NOT_ENOUGH_CLOUT");
        return false;
    }
}
