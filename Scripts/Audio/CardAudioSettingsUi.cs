using System;
using System.Collections.Generic;
using System.Linq;
using BaseLib.Config;
using BaseLib.Config.UI;
using Godot;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Cards;
using StsModBloodywolf.Scripts.Services;

namespace StsModBloodywolf.Scripts.Audio;

public static class CardAudioSettingsUi
{
    private const string DescriptionKey =
        "STSMODBLOODYWOLF-CARD_AUDIO_OVERRIDES.description";
    private const string SearchKey =
        "STSMODBLOODYWOLF-CARD_AUDIO_OVERRIDES-SEARCH.placeholder";
    private const string NoMatchesKey =
        "STSMODBLOODYWOLF-CARD_AUDIO_OVERRIDES-NO_MATCHES";

    public static void Attach(VBoxContainer container)
    {
        var description = ModConfig.CreateRawLabelControl(Loc(DescriptionKey,
            "每张卡默认跟随上方的“默认卡牌音效”。需要单独指定时，在此处覆盖。"), 26);
        description.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        container.AddChild(description);

        var search = new LineEdit
        {
            PlaceholderText = Loc(SearchKey, "搜索卡牌"),
            CustomMinimumSize = new Vector2(0, 52),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            ClearButtonEnabled = true
        };
        container.AddChild(search);

        var rows = new VBoxContainer
        {
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };
        container.AddChild(rows);

        var cards = GetBloodywolfCards();
        var resetHandler = new Action(() => Rebuild(rows, cards, search.Text));
        CardAudioSettings.OverridesCleared += resetHandler;
        container.TreeExiting += () => CardAudioSettings.OverridesCleared -= resetHandler;

        search.TextChanged += query => Rebuild(rows, cards, query);
        Rebuild(rows, cards, string.Empty);
    }

    private static void Rebuild(
        VBoxContainer container,
        IReadOnlyList<BloodywolfCardModel> cards,
        string query)
    {
        if (!GodotObject.IsInstanceValid(container))
            return;

        foreach (var child in container.GetChildren())
        {
            container.RemoveChild(child);
            child.QueueFree();
        }

        var filteredCards = cards.Where(card =>
            string.IsNullOrWhiteSpace(query)
            || GetCardTitle(card).Contains(query.Trim(), StringComparison.CurrentCultureIgnoreCase));

        foreach (var card in filteredCards)
            container.AddChild(CreateCardRow(card));

        if (container.GetChildCount() == 0)
        {
            container.AddChild(ModConfig.CreateRawLabelControl(
                Loc(NoMatchesKey, "没有匹配的卡牌。"), 24));
        }
    }

    private static Control CreateCardRow(BloodywolfCardModel card)
    {
        var row = new HBoxContainer
        {
            CustomMinimumSize = new Vector2(0, 58),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };
        row.AddThemeConstantOverride("separation", 16);

        var hasCardName = CardAudioCatalog.Has(card.Id.Entry, CardAudioVariant.CardName)
            || BloodywolfAudioService.HasLegacyAudio(card.GetType().Name);
        var hasFlavor = CardAudioCatalog.Has(card.Id.Entry, CardAudioVariant.Flavor);
        var availability = $"{(hasCardName ? "✓" : "—")} / {(hasFlavor ? "✓" : "—")}";
        var label = ModConfig.CreateRawLabelControl(
            $"{GetCardTitle(card)}  [color=#999999]({availability})[/color]", 28);
        label.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        row.AddChild(label);

        var choices = new[]
        {
            CardAudioOverride.FollowDefault,
            CardAudioOverride.CardName,
            CardAudioOverride.Flavor,
            CardAudioOverride.Disabled
        };
        var dropdown = new OptionButton
        {
            CustomMinimumSize = new Vector2(300, 52),
            SizeFlagsHorizontal = Control.SizeFlags.ShrinkEnd,
            FocusMode = Control.FocusModeEnum.All,
            TooltipText = $"卡名: {(hasCardName ? "有" : "无")}\n风味: {(hasFlavor ? "有" : "无")}"
        };

        foreach (var choice in choices)
            dropdown.AddItem(GetChoiceLabel(choice), (int)choice);

        var selectedChoice = CardAudioSettings.GetOverride(card.Id.Entry);
        dropdown.Select(Math.Max(0, Array.IndexOf(choices, selectedChoice)));
        dropdown.ItemSelected += index =>
        {
            var choice = (CardAudioOverride)dropdown.GetItemId((int)index);
            CardAudioSettings.SetOverride(card.Id.Entry, choice);
        };

        row.AddChild(dropdown);
        return row;
    }

    private static IReadOnlyList<BloodywolfCardModel> GetBloodywolfCards()
    {
        try
        {
            return ModelDb.AllCards
                .OfType<BloodywolfCardModel>()
                .GroupBy(card => card.Id.Entry, StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .OrderBy(GetCardTitle, StringComparer.CurrentCulture)
                .ToList();
        }
        catch (Exception exception)
        {
            GD.PushWarning($"[BloodywolfAudio] Could not enumerate cards for settings: {exception.Message}");
            return Array.Empty<BloodywolfCardModel>();
        }
    }

    private static string GetCardTitle(BloodywolfCardModel card)
    {
        return card.TitleLocString.GetFormattedText();
    }

    private static string GetChoiceLabel(CardAudioOverride choice)
    {
        var suffix = choice switch
        {
            CardAudioOverride.FollowDefault => "FOLLOW_DEFAULT",
            CardAudioOverride.CardName => "CARD_NAME",
            CardAudioOverride.Flavor => "FLAVOR",
            CardAudioOverride.Disabled => "DISABLED",
            _ => "FOLLOW_DEFAULT"
        };
        return Loc($"STSMODBLOODYWOLF-CARD_AUDIO_OVERRIDES-{suffix}", choice.ToString());
    }

    private static string Loc(string key, string fallback)
    {
        return LocString.GetIfExists("settings_ui", key)?.GetFormattedText() ?? fallback;
    }
}
