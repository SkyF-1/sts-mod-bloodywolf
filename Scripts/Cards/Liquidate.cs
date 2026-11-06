using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Char;
using STS2RitsuLib.Interop.AutoRegistration;

namespace StsModBloodywolf.Scripts.Cards;

[RegisterArchaicToothTranscendence(typeof(Purge))]
[RegisterCharacterStarterCard(typeof(Bloodywolf))]
public sealed class Liquidate : BloodywolfCardModel
{/// 清算
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>()
    };

	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new DamageVar(8m, ValueProp.Move),
        new HotTakeVar(3m),
		new PowerVar<WeakPower>(1m),
		new PowerVar<VulnerablePower>(1m)
	};

    protected override bool ShouldGlowGoldInternal => base.Owner.Creature.GetPower<CloutPower>()?.Amount >= base.DynamicVars[HotTakeVar.Key].IntValue;
	public Liquidate()
		: base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
	{
	}
    public override string PortraitPath => AssetProfile.PortraitPath!;

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		float num = base.Owner.Character.AttackAnimDelay;
		if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Normal)
		{
			num += 0.2f;
		}
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.WithAttackerAnim("Attack", num)
			.Execute(choiceContext);
        decimal CloutValue = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        //言论条�?
        if(CloutValue >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        {
            await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
        }
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(3m);
	}
}
