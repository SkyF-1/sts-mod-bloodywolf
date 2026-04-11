using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Characters;
using BaseLib.Abstracts;
using Godot;
using StsModBloodywolf.Scripts.Cards;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Relics;

namespace StsModBloodywolf.Scripts.Char;
public class Bloodywolf : PlaceholderCharacterModel
{
    // 角色名称颜色
    public override Color NameColor => new(0.5f, 0.5f, 1f);
    // 能量图标轮廓颜色
    public override Color EnergyLabelOutlineColor => new("#1c3f6d");
    public override Color MapDrawingColor => new("#1c3f6d");
    // 人物性别（男女中立）
    public override CharacterGender Gender => CharacterGender.Masculine;

    // 初始血量
    public override int StartingHp => 79;

    // 人物模型tscn路径。要自定义见下。
    public override string CustomVisualPath => "res://StsModBloodywolf/scenes/bloodywolf_character.tscn";
    // 卡牌拖尾路径。
    // public override string CustomTrailPath => "res://scenes/vfx/card_trail_bloodywolf.tscn";
    // 人物头像路径。
    public override string CustomIconTexturePath => "res://icon.png";
    // 人物头像2号。
    // public override string CustomIconPath => "res://scenes/ui/character_icons/bloodywolf_icon.tscn";
    // 能量表盘tscn路径。要自定义见下。
    public override string CustomEnergyCounterPath => "res://StsModBloodywolf/scenes/bloodywolf_energy_counter.tscn";
    // 篝火休息动画。
    public override string CustomRestSiteAnimPath => "res://StsModBloodywolf/scenes/bloodywolf_rest_site.tscn";
    // 商店人物动画。
    public override string CustomMerchantAnimPath => "res://StsModBloodywolf/scenes/bloodywolf_merchant.tscn";
    // 多人模式-手指。
    // public override string CustomArmPointingTexturePath => null;
    // 多人模式剪刀石头布-石头。
    // public override string CustomArmRockTexturePath => null;
    // 多人模式剪刀石头布-布。
    // public override string CustomArmPaperTexturePath => null;
    // 多人模式剪刀石头布-剪刀。
    // public override string CustomArmScissorsTexturePath => null;

    // 人物选择背景。
    public override string CustomCharacterSelectBg => "res://StsModBloodywolf/scenes/bloodywolf_bg.tscn";
    // 人物选择图标。
    public override string CustomCharacterSelectIconPath => "res://StsModBloodywolf/images/char_select_bloodywolf.png";
    // 人物选择图标-锁定状态。
    public override string CustomCharacterSelectLockedIconPath => "res://StsModBloodywolf/images/char_select_bloodywolf_locked.png";
    // 人物选择过渡动画。
    // public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/ironclad_transition_mat.tres";
    // 地图上的角色标记图标、表情轮盘上的角色头像
    // public override string CustomMapMarkerPath => null;
    // 攻击音效
    // public override string CustomAttackSfx => null;
    // 施法音效
    // public override string CustomCastSfx => null;
    // 死亡音效
    // public override string CustomDeathSfx => null;
    // 角色选择音效
    // public override string CharacterSelectSfx => null;
    // 过渡音效。这个不能删。
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

    public override CardPoolModel CardPool => ModelDb.CardPool<BloodywolfCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<BloodywolfRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<BloodywolfPotionPool>();

    // 初始卡组
    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeWolf>(),
        ModelDb.Card<StrikeWolf>(),
        ModelDb.Card<StrikeWolf>(),
        ModelDb.Card<StrikeWolf>(),
        ModelDb.Card<DefendWolf>(),
        ModelDb.Card<DefendWolf>(),
        ModelDb.Card<DefendWolf>(),
        ModelDb.Card<DefendWolf>(),
        ModelDb.Card<Review>(),
        ModelDb.Card<Liquidate>(),
    ];

    // 初始遗物
    public override IReadOnlyList<RelicModel> StartingRelics => [
        ModelDb.Relic<RabbitHead>(),
    ];

    // 攻击建筑师的攻击特效列表
    public override List<string> GetArchitectAttackVfx() => [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
    ];
}