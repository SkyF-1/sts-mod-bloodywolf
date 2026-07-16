using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class SentimentStorm : BloodywolfCardModel
{/// 舆论风暴
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { 
        new DamageVar(15m, ValueProp.Move),
        new CardsVar(1)
    };
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public SentimentStorm()
		: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
        await PowerCmd.Apply<SentimentStormPower>(choiceContext, 
            base.Owner.Creature, 
            base.DynamicVars.Cards.BaseValue, 
            base.Owner.Creature, 
            this);
    }

	protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(6m);
    }
}
