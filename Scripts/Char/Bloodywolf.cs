using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Godot;
using StsModBloodywolf.Scripts.Pools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Godot;

namespace StsModBloodywolf.Scripts.Char;

[RegisterCharacter]
public class Bloodywolf : ModCharacterTemplate<BloodywolfCardPool, BloodywolfRelicPool, BloodywolfPotionPool>
{
    public override Color NameColor => new(0.5f, 0.5f, 1f);
    public override Color EnergyLabelOutlineColor => new("#1c3f6d");
    public override Color MapDrawingColor => new("#1c3f6d");
    public override CharacterGender Gender => CharacterGender.Masculine;

    public override int StartingHp => 79;
    public override int StartingGold => 99;

    public override CharacterAssetProfile AssetProfile => new(
        Scenes: new CharacterSceneAssetSet(
            VisualsPath: "res://StsModBloodywolf/scenes/bloodywolf_character.tscn",
            EnergyCounterPath: "res://StsModBloodywolf/scenes/bloodywolf_energy_counter.tscn",
            MerchantAnimPath: "res://StsModBloodywolf/scenes/bloodywolf_merchant.tscn",
            RestSiteAnimPath: "res://StsModBloodywolf/scenes/bloodywolf_rest_site.tscn"),
        Ui: new CharacterUiAssetSet(
            IconTexturePath: "res://StsModBloodywolf/icon.png",
            IconPath: "res://StsModBloodywolf/scenes/bloodywolf_icon.tscn",
            CharacterSelectBgPath: "res://StsModBloodywolf/scenes/bloodywolf_bg.tscn",
            CharacterSelectIconPath: "res://StsModBloodywolf/images/char_select_bloodywolf.png",
            CharacterSelectLockedIconPath: "res://StsModBloodywolf/images/char_select_bloodywolf.png"),
        Audio: new CharacterAudioAssetSet(
            CharacterSelectSfx: "res://StsModBloodywolf/audio/char/sfx.ogg",
            CharacterTransitionSfx: "event:/sfx/ui/wipe_ironclad"),
        Multiplayer: new CharacterMultiplayerAssetSet(
            ArmPointingTexturePath: "res://StsModBloodywolf/scenes/bloodywolf_arm_pointing.png",
            ArmRockTexturePath: "res://StsModBloodywolf/scenes/bloodywolf_arm_rock.png",
            ArmPaperTexturePath: "res://StsModBloodywolf/scenes/bloodywolf_arm_paper.png",
            ArmScissorsTexturePath: "res://StsModBloodywolf/scenes/bloodywolf_arm_scissors.png"));

    public override bool RequiresEpochAndTimeline => false;
    public override float AttackAnimDelay => 0f;
    public override float CastAnimDelay => 0f;

    protected override NCreatureVisuals? TryCreateCreatureVisuals() =>
        RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(
            AssetProfile.Scenes!.VisualsPath!);

    public override List<string> GetArchitectAttackVfx() => [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
    ];
}
