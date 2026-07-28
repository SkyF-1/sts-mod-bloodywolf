using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Commands;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class WeAreBuddies : BloodywolfCardModel
{// 都是恩人
	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new GivenBlockVar(4m)
    ];
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public WeAreBuddies()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{  
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await MyCmd.GiveBlock(cardPlay.Target, base.DynamicVars[GivenBlockVar.Key].BaseValue, cardPlay);
        await PowerCmd.Apply<TemporaryFreeAttackPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
		RemoveKeyword(CardKeyword.Exhaust);
	}
}
