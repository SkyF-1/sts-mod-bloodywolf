using BaseLib.Abstracts;
using BaseLib.Config;
using Godot;
using StsModBloodywolf.Scripts.Audio;

namespace StsModBloodywolf.Scripts.Config
{
    [HoverTipsByDefault]  // 可选，让配置项在UI中有悬停提示
    public sealed class BloodywolfModConfig : SimpleModConfig
    {
        // 声音总开关
        [ConfigSection("Audio Settings")]

        // 卡牌语音开关（单独控制）
        public static bool EnableCardVoice { get; set; } = true;

        // 添加主音量控制（0 = 静音, 1 = 最大）
        [SliderRange(0, 1, 0.01)]
        [SliderLabelFormat("{0:0%}")]
        [ConfigHoverTip(false)]
        public static double MasterVolume { get; set; } = 0.8;

        // 未单独覆盖的卡牌默认使用的音频类型。
        public static CardAudioVariant DefaultCardAudioVariant { get; set; }
            = CardAudioVariant.CardName;

        public override void SetupConfigUI(Control optionContainer)
        {
            GenerateOptionsForAllProperties(optionContainer);

            var overridesSection = CreateCollapsibleSection(
                "Card Audio Overrides", collapsedByDefault: true);
            optionContainer.AddChild(overridesSection);
            CardAudioSettingsUi.Attach(overridesSection.ContentContainer);

            AddRestoreDefaultsButton(optionContainer);
            SetupFocusNeighbors(optionContainer);
        }

        protected override void RestoreDefaultsNoConfirm()
        {
            CardAudioSettings.ClearOverrides();
            base.RestoreDefaultsNoConfirm();
        }
    }
}
