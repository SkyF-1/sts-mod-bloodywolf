using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class FakeNews : CustomCardModel
{
    /// 不要不要
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new CloutLossVar(1m),
        new BlockVar(9m, ValueProp.Move)
    };
    protected override bool IsPlayable => (base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0) >= base.DynamicVars[CloutLossVar.Key].BaseValue;

    public FakeNews()
        : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
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
            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
            if (!base.Keywords.Contains(CardKeyword.Exhaust) && !base.ExhaustOnNextPlay)
            {
                await CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Top);
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(4m);
    }
}