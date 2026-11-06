using System.Reflection;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib;
using STS2RitsuLib.Data;
using STS2RitsuLib.Settings;
using STS2RitsuLib.Utils.Persistence;
using StsModBloodywolf.Scripts.Audio;
using StsModBloodywolf.Scripts.Cards;
using StsModBloodywolf.Scripts.Services;

namespace StsModBloodywolf.Scripts.Config;

public sealed class BloodywolfSettings
{
    public bool EnableCardVoice { get; set; } = true;
    public double MasterVolume { get; set; } = 0.8d;
    public CardAudioVariant DefaultCardAudioVariant { get; set; } = CardAudioVariant.CardName;
    public Dictionary<string, CardAudioOverride> CardAudioOverrides { get; set; } =
        new(StringComparer.OrdinalIgnoreCase);
}

public static class BloodywolfSettingsPage
{
    public const string DataKey = "settings";
    private const string FileName = "bloodywolf.settings.json";
    private const string MainPageId = "bloodywolf_main";
    private const string AudioOverridesPageId = "bloodywolf_audio_overrides";
    private static readonly SaveScope Scope = SaveScope.Global;
    private static bool _registered;

    private static readonly ModSettingsValueBinding<BloodywolfSettings, bool> EnableCardVoiceBinding = new(
        Entry.ModId,
        DataKey,
        Scope,
        static settings => settings.EnableCardVoice,
        static (settings, value) => settings.EnableCardVoice = value);

    private static readonly ModSettingsValueBinding<BloodywolfSettings, double> MasterVolumeBinding = new(
        Entry.ModId,
        DataKey,
        Scope,
        static settings => settings.MasterVolume,
        static (settings, value) => settings.MasterVolume = value);

    private static readonly ModSettingsValueBinding<BloodywolfSettings, CardAudioVariant>
        DefaultCardAudioVariantBinding = new(
            Entry.ModId,
            DataKey,
            Scope,
            static settings => settings.DefaultCardAudioVariant,
            static (settings, value) => settings.DefaultCardAudioVariant = value);

    public static void Register()
    {
        if (_registered)
            return;

        ModDataStore.For(Entry.ModId).Register<BloodywolfSettings>(
            DataKey,
            FileName,
            Scope,
            static () => new BloodywolfSettings(),
            autoCreateIfMissing: true);

        RitsuLibFramework.RegisterModSettings(Entry.ModId, ConfigureMainPage, MainPageId);
        RitsuLibFramework.RegisterModSettings(Entry.ModId, ConfigureAudioOverridesPage, AudioOverridesPageId);
        _registered = true;
    }

    public static BloodywolfSettings Read()
    {
        return RitsuLibFramework.GetDataStore(Entry.ModId).Get<BloodywolfSettings>(DataKey);
    }

    public static void Modify(Action<BloodywolfSettings> modifier, bool save = true)
    {
        var store = RitsuLibFramework.GetDataStore(Entry.ModId);
        store.Modify(DataKey, modifier);
        if (save)
            store.Save(DataKey);
    }

    public static void ResetToDefaults()
    {
        Modify(static settings =>
        {
            var defaults = new BloodywolfSettings();
            settings.EnableCardVoice = defaults.EnableCardVoice;
            settings.MasterVolume = defaults.MasterVolume;
            settings.DefaultCardAudioVariant = defaults.DefaultCardAudioVariant;
            settings.CardAudioOverrides = defaults.CardAudioOverrides;
        });
    }

    private static void ConfigureMainPage(ModSettingsPageBuilder page)
    {
        page.WithTitle(Text("STS_MOD_BLOODYWOLF_MOD.title", "血狼破军"))
            .WithModDisplayName(Text("STS_MOD_BLOODYWOLF_MOD.title", "血狼破军"))
            .WithVisibleOnHostSurfaces(
                ModSettingsHostSurface.MainMenu | ModSettingsHostSurface.RunPause)
            .AddSection("audio", section => section
                .WithTitle(Text("STS_MOD_BLOODYWOLF_AUDIO_SETTINGS.title", "音频设置"))
                .AddToggle(
                    "enable_card_voice",
                    Text("STS_MOD_BLOODYWOLF_ENABLE_CARD_VOICE.title", "启用卡牌音效"),
                    EnableCardVoiceBinding)
                .AddSlider(
                    "master_volume",
                    Text("STS_MOD_BLOODYWOLF_MASTER_VOLUME.title", "主音量"),
                    MasterVolumeBinding,
                    0d,
                    1d,
                    0.01d,
                    static value => $"{value:0%}")
                .AddEnumChoice(
                    "default_card_audio",
                    Text("STS_MOD_BLOODYWOLF_DEFAULT_CARD_AUDIO_VARIANT.title", "默认卡牌音效"),
                    DefaultCardAudioVariantBinding,
                    static value => Text(
                        $"STS_MOD_BLOODYWOLF_DEFAULT_CARD_AUDIO_VARIANT.{value}",
                        value.ToString()),
                    presentation: ModSettingsChoicePresentation.Dropdown)
                .AddSubpage(
                    "card_audio_overrides",
                    Text("STS_MOD_BLOODYWOLF_CARD_AUDIO_OVERRIDES.title", "逐卡音效设置"),
                    AudioOverridesPageId,
                    Text("STS_MOD_BLOODYWOLF_CARD_AUDIO_OVERRIDES.title", "逐卡音效设置"),
                    Text(
                        "STS_MOD_BLOODYWOLF_CARD_AUDIO_OVERRIDES.description",
                        "未单独设置的卡牌跟随默认音效。"))
                .AddButton(
                    "restore_defaults",
                    ModSettingsText.Literal("恢复默认"),
                    ModSettingsText.Literal("重置"),
                    static host =>
                    {
                        ResetToDefaults();
                        host.RequestRefresh();
                    }));
    }

    private static void ConfigureAudioOverridesPage(ModSettingsPageBuilder page)
    {
        page.WithTitle(Text("STS_MOD_BLOODYWOLF_CARD_AUDIO_OVERRIDES.title", "逐卡音效设置"))
            .WithModDisplayName(Text("STS_MOD_BLOODYWOLF_MOD.title", "血狼破军"))
            .WithVisibleOnHostSurfaces(
                ModSettingsHostSurface.MainMenu | ModSettingsHostSurface.RunPause);

        var cardTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(static type => !type.IsAbstract && typeof(BloodywolfCardModel).IsAssignableFrom(type))
            .OrderBy(static type => type.FullName, StringComparer.Ordinal)
            .ToArray();

        foreach (var group in cardTypes.GroupBy(GetGroupName))
        {
            var sectionId = $"cards_{NormalizeId(group.Key)}";
            page.AddSection(sectionId, section =>
            {
                section.WithTitle(ModSettingsText.Literal(group.Key));
                foreach (var cardType in group)
                    AddCardAudioChoice(section, cardType);
            });
        }
    }

    private static void AddCardAudioChoice(ModSettingsSectionBuilder section, Type cardType)
    {
        var cardKey = CardAudioSettings.GetCardKey(cardType);
        var binding = new ModSettingsValueBinding<BloodywolfSettings, CardAudioOverride>(
            Entry.ModId,
            DataKey,
            Scope,
            settings => settings.CardAudioOverrides.GetValueOrDefault(
                cardKey,
                CardAudioOverride.FollowDefault),
            (settings, value) =>
            {
                if (value == CardAudioOverride.FollowDefault)
                    settings.CardAudioOverrides.Remove(cardKey);
                else
                    settings.CardAudioOverrides[cardKey] = value;
            });

        section.AddChoice(
            cardKey.ToLowerInvariant(),
            ModSettingsText.Dynamic(() => GetCardLabel(cardType, cardKey)),
            binding,
            [
                new(CardAudioOverride.FollowDefault, Text(
                    "STS_MOD_BLOODYWOLF_CARD_AUDIO_OVERRIDES_FOLLOW_DEFAULT", "跟随默认")),
                new(CardAudioOverride.CardName, Text(
                    "STS_MOD_BLOODYWOLF_CARD_AUDIO_OVERRIDES_CARD_NAME", "卡名")),
                new(CardAudioOverride.Flavor, Text(
                    "STS_MOD_BLOODYWOLF_CARD_AUDIO_OVERRIDES_FLAVOR", "风味")),
                new(CardAudioOverride.Disabled, Text(
                    "STS_MOD_BLOODYWOLF_CARD_AUDIO_OVERRIDES_DISABLED", "关闭")),
            ],
            presentation: ModSettingsChoicePresentation.Dropdown);
    }

    private static string GetCardLabel(Type cardType, string cardKey)
    {
        string title;
        try
        {
            var card = ModelDb.GetById<CardModel>(ModelDb.GetId(cardType));
            title = card.TitleLocString.GetFormattedText();
        }
        catch
        {
            title = cardType.Name;
        }

        var audioKey = $"stsmodbloodywolf-{cardKey.Replace("STS_MOD_BLOODYWOLF_CARD_", "").ToLowerInvariant()}";
        var hasName = CardAudioCatalog.Has(cardKey, CardAudioVariant.CardName)
                      || CardAudioCatalog.Has(audioKey, CardAudioVariant.CardName);
        var hasFlavor = CardAudioCatalog.Has(cardKey, CardAudioVariant.Flavor)
                        || CardAudioCatalog.Has(audioKey, CardAudioVariant.Flavor);
        return $"{title}  ({(hasName ? "✓" : "—")} / {(hasFlavor ? "✓" : "—")})";
    }

    private static string GetGroupName(Type cardType)
    {
        if (typeof(IBiteYou) == cardType)
            return "衍生牌";

        return cardType.Namespace?.Contains("Ancient", StringComparison.OrdinalIgnoreCase) == true
            ? "先古牌"
            : "卡牌";
    }

    private static string NormalizeId(string value)
    {
        return new string(value.Select(static ch => char.IsLetterOrDigit(ch) ? ch : '_').ToArray());
    }

    private static ModSettingsText Text(string key, string fallback)
    {
        return ModSettingsText.LocString("settings_ui", key, fallback);
    }
}
