using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class IGetYou : CustomCardModel
{/// 懂你意思
	public override bool GainsBlock => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>{ CardKeyword.Retain, CardKeyword.Exhaust };

	protected override IEnumerable<DynamicVar> CanonicalVars => [
    new BlockVar(20m, ValueProp.Move),
    ];
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public IGetYou()
		: base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{   
        BlockVar givenBlock = new BlockVar(base.DynamicVars.Block.BaseValue, ValueProp.Unpowered);
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        await CreatureCmd.GainBlock(cardPlay.Target, givenBlock, cardPlay);
	}

	protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(10m);
    }
}

