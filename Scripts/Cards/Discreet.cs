using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Discreet : BloodywolfCardModel
{// 谨言慎行
	public override bool GainsBlock => true;
    protected override bool ShouldGlowGoldInternal => base.Owner.Creature.GetPower<CloutPower>()?.Amount >= base.DynamicVars[HotTakeVar.Key].BaseValue;
	protected override IEnumerable<DynamicVar> CanonicalVars => [
    new BlockVar("BaseBlock", 7m, ValueProp.Move),
    new HotTakeVar(3m),
    new BlockVar("BonusBlock", 4m, ValueProp.Move),
    ];
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public Discreet()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{  
        decimal CloutValue = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars["BaseBlock"].BaseValue, ValueProp.Move, cardPlay);

        if (CloutValue >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        {
            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars["BonusBlock"].BaseValue, ValueProp.Move, cardPlay);
        }

	}

	protected override void OnUpgrade()
	{
		base.DynamicVars["BaseBlock"].UpgradeValueBy(2m);
        base.DynamicVars["BonusBlock"].UpgradeValueBy(2m);
	}
}
