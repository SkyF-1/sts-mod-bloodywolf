using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class NoStreamTonight : CustomCardModel
{
    /// 今晚不播
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new CloutLossVar(1m),
        new EnergyVar(1)
    };
    protected override bool IsPlayable => (base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0) >= base.DynamicVars[CloutLossVar.Key].BaseValue;

    public NoStreamTonight()
        : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal cloutAmount = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        if (cloutAmount >= base.DynamicVars[CloutLossVar.Key].BaseValue)
        {
            await PowerCmd.Apply<CloutPower>(
                base.Owner.Creature,
                -base.DynamicVars[CloutLossVar.Key].BaseValue,
                base.Owner.Creature,
                this);
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Energy.UpgradeValueBy(1);
    }
}