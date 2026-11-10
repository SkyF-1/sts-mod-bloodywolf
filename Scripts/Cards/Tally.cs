using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class Tally : BloodywolfCardModel
{
    /// 清点计算
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(2m, ValueProp.Move),
        new RepeatVar(3),
        new RateVar(1m)
    };

    public Tally()
        : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    public override string PortraitPath => AssetProfile.PortraitPath!;

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue)
            .FromCard(this, cardPlay).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_scratch")
            .Execute(choiceContext);

    }
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
	{
		if ( cardSource == this && result.UnblockedDamage > 0)
		{
			await BloodywolfResources.GainClout(
                base.Owner,
                (int)base.DynamicVars[RateVar.Key].BaseValue,
                this);
		}
	}
    protected override void OnUpgrade()
    {
        base.DynamicVars.Repeat.UpgradeValueBy(1m);
    }
}
