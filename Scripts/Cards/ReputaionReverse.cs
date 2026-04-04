using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class ReputationReverse : CustomCardModel
{
    /// 风评反转
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new CloutLossVar(2m),
        new CardsVar(4)
    };
    protected override bool IsPlayable => (base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0) >= base.DynamicVars[CloutLossVar.Key].BaseValue;
    public ReputationReverse()
        : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
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
            await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars[CloutLossVar.Key].UpgradeValueBy(-1m);
    }
}