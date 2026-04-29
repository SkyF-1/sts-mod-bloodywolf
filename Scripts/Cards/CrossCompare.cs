using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class CrossCompare : CustomCardModel
{/// 横向比较
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CupLossPower>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(7m, ValueProp.Move),
        new PowerVar<CupLossPower>(7m)
    };
	public CrossCompare()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
    }
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
            .TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
	{
		if ((dealer == base.Owner.Creature || dealer?.PetOwner == base.Owner) && !target.IsPlayer && result.WasBlockBroken && cardSource == this)
		{
			await PowerCmd.Apply<CupLossPower>(target, base.DynamicVars["CupLossPower"].BaseValue, base.Owner.Creature, this);
		}
	}
    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
