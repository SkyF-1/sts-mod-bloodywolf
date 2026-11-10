using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class RideWave : BloodywolfCardModel
{/// 热点话题
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<StrengthPower>()
    };
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };

	public RideWave()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
    public override string PortraitPath => AssetProfile.PortraitPath!;
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int CloutValue = BloodywolfResources.GetClout(base.Owner);
        await PowerCmd.Apply<RideWavePower>(choiceContext,
            base.Owner.Creature,
            CloutValue,
            base.Owner.Creature,
            null);
    }

	protected override void OnUpgrade()
    {
        base.RemoveKeyword(CardKeyword.Exhaust);
    }
}
