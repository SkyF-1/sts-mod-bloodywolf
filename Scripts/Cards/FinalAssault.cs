using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using StsModBloodywolf.Scripts.Pools;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class FinalAssault : BloodywolfCardModel
{
    /// 最终攻�?
	protected override bool IsPlayable => false;
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new DamageVar(24m, ValueProp.Move)
	};

	public FinalAssault()
		: base(-1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
			.Execute(choiceContext);
	}

	public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
	{
		await TryPlay(creature);
	}
	public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		foreach(Creature creature in base.CombatState.HittableEnemies)
			await TryPlay(creature);
	}
	public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
	{
		foreach(Creature creature in base.CombatState.HittableEnemies)
			await TryPlay(creature);
	}
	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		foreach(Creature creature in base.CombatState.HittableEnemies)
			await TryPlay(creature);
	}
	private async Task TryPlay(Creature target)
	{
		if (!target.IsEnemy) return;
		if (!CanBeKilled(target)) return;
		if (base.Pile.Type != PileType.Hand) return;
		await CardCmd.AutoPlay(new BlockingPlayerChoiceContext(), this, target);
	}

	private bool CanBeKilled(Creature target)
	{
		return DamagePreview(CardPreviewMode.Normal, target, true) >= target.CurrentHp + target.Block;
	}

	private decimal DamagePreview(CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal num = base.DynamicVars.Damage.BaseValue;
        EnchantmentModel? enchantment = base.Enchantment;
        if (enchantment != null)
        {
            num += enchantment.EnchantDamageAdditive(num, base.DynamicVars.Damage.Props);
            num *= enchantment.EnchantDamageMultiplicative(num, base.DynamicVars.Damage.Props);
            if (!base.IsEnchantmentPreview)
            {
                base.DynamicVars.Damage.EnchantedValue = num;
            }
        }

        if (runGlobalHooks)
        {
            num = Hook.ModifyDamage(base.Owner.RunState, base.CombatState, target, base.Owner.Creature, base.DynamicVars.Damage.BaseValue, base.DynamicVars.Damage.Props, this, ModifyDamageHookType.All, previewMode, out IEnumerable<AbstractModel> _);
        }
        return num;
    }
    protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(8m);
	}
}