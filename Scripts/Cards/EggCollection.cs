using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class EggCollection : BloodywolfCardModel
{
    /// 彩蛋征集
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new RateVar(2m),
        new HotTakeVar(3m),
        new RateVar("bonusRate", 3m)
    };
    protected override bool ShouldGlowGoldInternal => BloodywolfResources.GetClout(base.Owner) + base.DynamicVars[RateVar.Key].BaseValue >= base.DynamicVars[HotTakeVar.Key].BaseValue;

    public EggCollection()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    public override string PortraitPath => AssetProfile.PortraitPath!;

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await BloodywolfResources.GainClout(base.Owner, (int)base.DynamicVars[RateVar.Key].BaseValue, this);

        int cloutAmount = BloodywolfResources.GetClout(base.Owner);
        if (cloutAmount >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        {
            await PowerCmd.Apply<CloutNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars["bonusRate"].BaseValue, base.Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["bonusRate"].UpgradeValueBy(1m);
    }
}
