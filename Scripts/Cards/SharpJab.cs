using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class SharpJab : BloodywolfCardModel
{/// 锐气
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>{ CardKeyword.Exhaust };
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CupLossPower>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<CupLossPower>(7m)
    ];
	public SharpJab()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await PowerCmd.Apply<CupLossPower>(choiceContext, cardPlay.Target, DynamicVars[CupLossPower.Key].BaseValue, base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars["CupLossPower"].UpgradeValueBy(2m);
	}
}
