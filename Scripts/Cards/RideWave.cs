using BaseLib.Abstracts;
using BaseLib.Utils;
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

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class RideWave : BloodywolfCardModel
{/// 热点话题
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<StrengthPower>()
    };
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };

	public RideWave()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal CloutValue = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
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
