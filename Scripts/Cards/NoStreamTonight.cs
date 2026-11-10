using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class NoStreamTonight : BloodywolfCardModel
{
    /// 今晚不播
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new EnergyVar(2)
    };

    public NoStreamTonight()
        : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        this.SecondaryCosts().Set(BloodywolfResources.CloutId, 2);
    }

    public override string PortraitPath => AssetProfile.PortraitPath!;

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Energy.UpgradeValueBy(1);
    }
}
