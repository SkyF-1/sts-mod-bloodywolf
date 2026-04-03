using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class CrossCompare : CustomCardModel
{/// 横向比较
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new CalculationBaseVar(0m),
		new CalculationExtraVar(1m),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? target) => target?.Block ?? 0)
	};
	public CrossCompare()
		: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<Creature> targets = base.CombatState.Enemies.Where(creature => creature != cardPlay.Target);
        foreach (Creature target in targets)
        {
            await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this).Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }
    }

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}
