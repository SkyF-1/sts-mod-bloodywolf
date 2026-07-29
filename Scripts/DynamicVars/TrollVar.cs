using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace StsModBloodywolf.Scripts.DynamicVars;

public class TrollVar : DynamicVar
{
    public const string Key = "Bloodywolf-Troll";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public TrollVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }

    public TrollVar(string name, decimal baseValue) : base(name, baseValue)
    {
        this.WithTooltip(LocKey);
    }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal num = BaseValue;

        // 1. 如果有附魔（Enchantment），先应用附魔提供的加算/乘算修正
        EnchantmentModel? enchantment = card.Enchantment;
        if (enchantment != null)
        {
            // 如果你的附魔也为 Troll 值提供了修正方法，可以在这里调用
            // 如果没有，可以跳过这段。假设你后续会添加类似 EnchantTrollAdditive 等方法。
            // num += enchantment.EnchantTrollAdditive(num);
            // num *= enchantment.EnchantTrollMultiplicative(num);
            // 暂时先不处理附魔，直接使用 BaseValue
        }
        // 2. 如果允许全局钩子，调用你的自定义修改钩子
        if (runGlobalHooks && card.CombatState != null)
        {
            // 注意：ModifyTrollDamage 需要在 MyHook 中定义
            num = MyHook.ModifyTrollDamage(card.CombatState, target, num, card.Owner.Creature, card);
        }

        // 3. 更新预览值
        base.PreviewValue = num;
    }
}