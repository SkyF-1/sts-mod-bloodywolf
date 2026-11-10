using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class ConclusionFirst : BloodywolfCardModel
{/// 先说结论
	public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>{CardKeyword.Exhaust};
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new RateVar(2m),
        new CardsVar(2),
        new PowerVar<CupLossPower>(3m)
    };
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CupLossPower>()
    };
    public override string PortraitPath => AssetProfile.PortraitPath!;

	public ConclusionFirst()
		: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await BloodywolfResources.GainClout(
            base.Owner,
            (int)base.DynamicVars[RateVar.Key].BaseValue,
            this);
        await PowerCmd.Apply<CupLossPower>(choiceContext,
        base.CombatState.HittableEnemies,
        base.DynamicVars[CupLossPower.Key].BaseValue,
        base.Owner.Creature,
        this);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
    }

	protected override void OnUpgrade()
    {
        base.AddKeyword(CardKeyword.Innate);
    }
}
