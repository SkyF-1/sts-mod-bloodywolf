using Godot;
using BaseLib.Abstracts;

namespace StsModBloodywolf.Scripts.Pools;
public class BloodywolfCardPool : CustomCardPoolModel
{
    // 卡池的ID。必须唯一防撞车。
    public override string Title => "BloodywolfCardPool";

    // 卡池的能量图标。暂时不支持加载，建议暂时使用原版，或者通过更改CardModel的EnergyIcon修改。
    public override string EnergyColorName => "ironclad";

    // 卡池的主题色。
    public override Color DeckEntryCardColor => new("#1f97a7");

    // 卡池是否是无色。例如事件、状态等卡池就是无色的。
    public override bool IsColorless => false;
}