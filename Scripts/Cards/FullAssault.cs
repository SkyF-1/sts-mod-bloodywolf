using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Services;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class FullAssault : CustomCardModel
{/// 全军出击
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>{CardKeyword.Exhaust};

	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>{new CardsVar(3)};

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>{HoverTipFactory.FromCard<IBiteYou>(base.IsUpgraded)};
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public FullAssault()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		BloodywolfAudioService.PlayCard(GetType().Name.ToLowerInvariant());
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		IEnumerable<CardModel> enumerable = await IBiteYou.CreateInHand(base.Owner, base.DynamicVars.Cards.IntValue, base.CombatState);
		if (!base.IsUpgraded)
		{
			return;
		}
		foreach (CardModel item in enumerable)
		{
			CardCmd.Upgrade(item);
		}

	}

}

