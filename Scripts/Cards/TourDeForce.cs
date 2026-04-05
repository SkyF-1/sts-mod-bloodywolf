using BaseLib.Abstracts;
using BaseLib.Utils;
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
public sealed class TourDeForce : CustomCardModel
{/// 绝活
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        new List<IHoverTip>
        {
            HoverTipFactory.FromPower<CloutPower>()
        };
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
	protected override IEnumerable<DynamicVar> CanonicalVars => [new RateVar(7m)];
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public TourDeForce()
		: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<CloutPower>(
        base.Owner.Creature, 
        base.DynamicVars[RateVar.Key].IntValue, 
        base.Owner.Creature, 
        this);
    }

	protected override void OnUpgrade()
    {
        base.DynamicVars[RateVar.Key].UpgradeValueBy(2m);
    }
}
