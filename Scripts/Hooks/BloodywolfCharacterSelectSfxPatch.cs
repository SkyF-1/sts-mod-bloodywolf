using System;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using StsModBloodywolf.Scripts.Services;

namespace StsModBloodywolf.Scripts.Hooks;

[HarmonyPatch(
    typeof(SfxCmd),
    nameof(SfxCmd.Play),
    new[] { typeof(string), typeof(float) })]
internal static class BloodywolfCharacterSelectSfxPatch
{
    [HarmonyPrefix]
    private static bool Prefix(string sfx, float volume)
    {
        if (!string.Equals(
                sfx,
                BloodywolfAudioService.CharacterSelectSfxEvent,
                StringComparison.Ordinal))
        {
            return true;
        }

        BloodywolfAudioService.PlayCharacterSelect(volume);
        return false;
    }
}
