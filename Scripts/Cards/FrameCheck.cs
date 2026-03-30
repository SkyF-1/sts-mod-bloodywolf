using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class FrameCheck : CustomCardModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<VulnerablePower>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(6m, ValueProp.Move),
        new PowerVar<VulnerablePower>(1m)
    };

	public FrameCheck()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
        await PowerCmd.Apply<VulnerablePower>(
            cardPlay.Target, 
            base.DynamicVars.Vulnerable.BaseValue, 
            base.Owner.Creature, 
            this);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(3m);
	}
}
