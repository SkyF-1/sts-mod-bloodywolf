using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class StirUp : BloodywolfCardModel
{/// 带节�?
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(27m, ValueProp.Move),
        new CloutLossVar(3m),
        new CardsVar(1)
    };
    protected override bool IsPlayable => (base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0) >= base.DynamicVars[CloutLossVar.Key].BaseValue;

	public StirUp()
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal cloutAmount = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        var cloutPower = base.Owner.Creature.GetPower<CloutPower>();
        if (cloutAmount >= base.DynamicVars[CloutLossVar.Key].BaseValue)
        {
            await PowerCmd.Apply<CloutPower>(choiceContext, 
                base.Owner.Creature, 
                -base.DynamicVars[CloutLossVar.Key].BaseValue,
                base.Owner.Creature, 
                null);
        }
        else if (cloutPower != null)
        {
            await PowerCmd.Remove(cloutPower);
        }
        
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
        

        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
    }

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(6m);
	}
}
