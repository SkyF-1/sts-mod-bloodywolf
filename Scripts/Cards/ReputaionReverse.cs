using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class ReputationReverse : BloodywolfCardModel
{
    /// 风评反转
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new CardsVar(3)
    };
    public ReputationReverse()
        : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        this.SecondaryCosts().Set(BloodywolfResources.CloutId, 2);
    }

    public override string PortraitPath => AssetProfile.PortraitPath!;

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
