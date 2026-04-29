using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class ConclusionFirst : CustomCardModel
{/// 先说结论
	public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>{CardKeyword.Innate, CardKeyword.Exhaust};
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> 
    { 
        new RateVar(2m),
        new CardsVar(1),
        new PowerVar<CupLossPower>(1m)
    };
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CloutPower>(),
        HoverTipFactory.FromPower<CupLossPower>()
    };
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public ConclusionFirst()
		: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<CloutPower>(
        base.Owner.Creature, 
        base.DynamicVars[RateVar.Key].BaseValue, 
        base.Owner.Creature, 
        this);
        await PowerCmd.Apply<CupLossPower>(
        base.CombatState.HittableEnemies, 
        base.DynamicVars[CupLossPower.Key].BaseValue, 
        base.Owner.Creature, 
        this);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
    }

	protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1);
        base.DynamicVars[RateVar.Key].UpgradeValueBy(1m);
    }
}

