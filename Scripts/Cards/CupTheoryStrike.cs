using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;
using MegaCrit.Sts2.Core.Commands.Builders;
using StsModBloodywolf.Scripts.Commands;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class CupTheoryStrike : BloodywolfCardModel
{/// 杯论打击
	protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new TrollVar(10m)
    };

	public CupTheoryStrike()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
    public override string PortraitPath => AssetProfile.PortraitPath!;
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await TrollCmd.Troll(base.DynamicVars[TrollVar.Key].BaseValue)
			.FromCard(this)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_bloody_impact")
			.Execute(choiceContext);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars[TrollVar.Key].UpgradeValueBy(4m);
	}
}
