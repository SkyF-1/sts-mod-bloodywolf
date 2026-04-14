using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class AxisOutput : CustomCardModel
{
    /// 控轴输出
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(6m, ValueProp.Move)
    };

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    public AxisOutput()
        : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取弃牌堆中费用为0的牌
        CardPile discardPile = PileType.Discard.GetPile(base.Owner);
        var zeroCostCards = discardPile.Cards.Where(card => card.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && !card.EnergyCost.CostsX).ToList();

        if (zeroCostCards.Count == 0)
        {
            return; // 如果没有符合条件的牌，直接返回
        }

        foreach (CardModel card in zeroCostCards)
        {
            // 将牌洗入抽牌堆
            await CardPileCmd.Add(card, PileType.Draw);

            // 每洗入一张牌，造成伤害
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}