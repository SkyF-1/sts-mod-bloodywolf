using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class Rally : BloodywolfCardModel
{
    /// 号召
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new RateVar(2m),
        new HotTakeVar(9m)
    };
    protected override bool ShouldGlowGoldInternal => BloodywolfResources.GetClout(base.Owner) + base.DynamicVars[RateVar.Key].BaseValue >= base.DynamicVars[HotTakeVar.Key].BaseValue;

    public Rally()
        : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    public override string PortraitPath => AssetProfile.PortraitPath!;

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await BloodywolfResources.GainClout(base.Owner, (int)base.DynamicVars[RateVar.Key].BaseValue, this);

        int cloutAmount = BloodywolfResources.GetClout(base.Owner);
        if (cloutAmount >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        {
            int num = 10 - base.Owner.PlayerCombatState.Hand.Cards.Count;
		    await CardPileCmd.Draw(choiceContext, num, base.Owner);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars[HotTakeVar.Key].UpgradeValueBy(-1m);
    }
}
