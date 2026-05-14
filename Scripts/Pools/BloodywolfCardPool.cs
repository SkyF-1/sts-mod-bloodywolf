using Godot;
using BaseLib.Abstracts;

namespace StsModBloodywolf.Scripts.Pools;
public class BloodywolfCardPool : CustomCardPoolModel
{
    // 卡池的ID。必须唯一防撞车。
    public override string Title => "BloodywolfCardPool";

    // 卡池的能量图标。暂时不支持加载，建议暂时使用原版，或者通过更改CardModel的EnergyIcon修改。
    // public override string EnergyColorName => "colorless";
    // 描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://StsModBloodywolf/images/energy_bloodywolf.png";
    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://StsModBloodywolf/images/energy_bloodywolf_big.png";

    // 卡池的主题色。
    public override Color DeckEntryCardColor => new("#1f97a7");
    public override Color ShaderColor => new("#1f97a7");

    // 卡池是否是无色。例如事件、状态等卡池就是无色的。
    public override bool IsColorless => false;
}