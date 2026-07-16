using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class StrategicSetup : BloodywolfCardModel
{
    /// 思路构建
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CloutPower>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new RateVar(3m)
    };

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    public StrategicSetup()
        : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 应用评定效果
        await PowerCmd.Apply<CloutPower>(choiceContext, 
            base.Owner.Creature,
            base.DynamicVars[RateVar.Key].BaseValue,
            base.Owner.Creature,
            this);

        CardModel clone = CreateClone();
        clone.EnergyCost.SetThisCombat(0);
        var cardPosition = CardPilePosition.Random;
        await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Draw, base.Owner, position: cardPosition);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars[RateVar.Key].UpgradeValueBy(1m);
    }
}