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
public sealed class NewGodArrives : CustomCardModel
{/// 新神已至
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
	{
		HoverTipFactory.Static(StaticHoverTip.ReplayStatic)
	};
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(4m, ValueProp.Move),
        new RepeatVar(2),
        new HotTakeVar(8m)
    };

	public NewGodArrives()
		: base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
    protected override bool ShouldGlowGoldInternal => base.Owner.Creature.GetPower<CloutPower>()?.Amount >= base.DynamicVars[HotTakeVar.Key].BaseValue;
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal CloutValue = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        //言论条件
        if (CloutValue >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue + 4).FromCard(this)
			.TargetingAllOpponents(base.CombatState)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
        }else
        {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue).FromCard(this)
			.TargetingAllOpponents(base.CombatState)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
        }
    }

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(2m);
	}
}
