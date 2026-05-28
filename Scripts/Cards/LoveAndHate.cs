using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class LoveAndHate : CustomCardModel
{/// 爱恨交加
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
        new BlockVar(7m, ValueProp.Unpowered)
	};

	public LoveAndHate()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await CreatureCmd.GainBlock(cardPlay.Target, base.DynamicVars.Block, cardPlay);
		await CreatureCmd.Damage(choiceContext, cardPlay.Target, DynamicVars.Block.BaseValue, ValueProp.Unpowered | ValueProp.Unblockable | ValueProp.Move, this);
	}

	protected override void OnUpgrade()
	{
        base.DynamicVars.Block.UpgradeValueBy(2m);
	}
}
