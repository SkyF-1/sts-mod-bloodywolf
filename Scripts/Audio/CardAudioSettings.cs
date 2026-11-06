using StsModBloodywolf.Scripts.Config;
using STS2RitsuLib.Content;

namespace StsModBloodywolf.Scripts.Audio;

public static class CardAudioSettings
{
    public static void Initialize()
    {
        BloodywolfSettingsPage.Register();
    }

    public static CardAudioOverride GetOverride(string cardId)
    {
        var settings = BloodywolfSettingsPage.Read();
        return settings.CardAudioOverrides.GetValueOrDefault(
            NormalizeCardId(cardId),
            CardAudioOverride.FollowDefault);
    }

    public static void SetOverride(string cardId, CardAudioOverride value)
    {
        var key = NormalizeCardId(cardId);
        BloodywolfSettingsPage.Modify(settings =>
        {
            if (value == CardAudioOverride.FollowDefault)
                settings.CardAudioOverrides.Remove(key);
            else
                settings.CardAudioOverrides[key] = value;
        });
    }

    public static void ClearOverrides()
    {
        BloodywolfSettingsPage.Modify(static settings => settings.CardAudioOverrides.Clear());
    }

    public static CardAudioVariant? ResolveVariant(string cardId)
    {
        var settings = BloodywolfSettingsPage.Read();
        return settings.CardAudioOverrides.GetValueOrDefault(
            NormalizeCardId(cardId),
            CardAudioOverride.FollowDefault) switch
        {
            CardAudioOverride.Disabled => null,
            CardAudioOverride.CardName => CardAudioVariant.CardName,
            CardAudioOverride.Flavor => CardAudioVariant.Flavor,
            _ => settings.DefaultCardAudioVariant
        };
    }

    public static string GetCardKey(Type cardType)
    {
        return NormalizeCardId(
            ModContentRegistry.GetFixedPublicEntry(Entry.ModId, cardType));
    }

    public static string NormalizeCardId(string cardId)
    {
        return cardId.Trim().ToUpperInvariant();
    }
}
