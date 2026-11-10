using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class StrategicSetup : BloodywolfCardModel
{
    /// 思路构建
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new RateVar(3m)
    };

    public override string PortraitPath => AssetProfile.PortraitPath!;

    public StrategicSetup()
        : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 应用评定效果
        await BloodywolfResources.GainClout(
            base.Owner,
            (int)base.DynamicVars[RateVar.Key].BaseValue,
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
