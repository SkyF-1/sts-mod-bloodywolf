using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class FakeNews : BloodywolfCardModel
{
    /// 不要不要
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new BlockVar(9m, ValueProp.Move)
    };

    public FakeNews()
        : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        this.SecondaryCosts().Set(BloodywolfResources.CloutId, 1);
    }

    public override string PortraitPath => AssetProfile.PortraitPath!;

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        if (!base.Keywords.Contains(CardKeyword.Exhaust) && !base.ExhaustOnNextPlay)
        {
            await CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Top);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3m);
    }
}
