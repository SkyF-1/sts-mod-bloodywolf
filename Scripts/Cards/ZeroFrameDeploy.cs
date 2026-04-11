using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class ZeroFrameDeploy : CustomCardModel
{
    /// 零帧部署
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    public ZeroFrameDeploy()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取抽牌堆
        CardPile drawPile = PileType.Draw.GetPile(base.Owner);
        // 筛选符合条件的0费牌（非X费用，且为攻击/技能/能力）
        var zeroCostCards = drawPile.Cards.Where(Filter).ToList();
        if (zeroCostCards.Count == 0)
            return;

        // 计算手牌剩余空间（手牌上限10）
        int handCount = CardPile.GetCards(base.Owner, PileType.Hand).Count();
        int space = 10 - handCount;
        if (space <= 0)
            return;

        // 只取能容纳的数量
        var cardsToHand = zeroCostCards.Take(space);
        foreach (CardModel card in cardsToHand)
        {
            await CardPileCmd.Add(card, PileType.Hand);
        }
    }

    // 筛选逻辑：0费、非X费用、且类型为攻击/技能/能力
    private bool Filter(CardModel card)
    {
        // 最终费用为0且不是X费用
        bool isZeroCost = card.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && !card.EnergyCost.CostsX;
        if (!isZeroCost)
            return false;

        // 类型限制：Attack, Skill, Power
        CardType type = card.Type;
        return type == CardType.Attack || type == CardType.Skill || type == CardType.Power;
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}