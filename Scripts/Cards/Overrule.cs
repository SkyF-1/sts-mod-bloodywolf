using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class Overrule : BloodywolfCardModel
{/// 不容分说
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CupLossPower>()
    };
    public override string PortraitPath => AssetProfile.PortraitPath!;

	public Overrule()
		: base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<OverrulePower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
    }

	protected override void OnUpgrade()
    {
        base.AddKeyword(CardKeyword.Innate);
    }
}
