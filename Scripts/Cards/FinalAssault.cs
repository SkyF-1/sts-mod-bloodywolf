using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Runs;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class FinalAssault : BloodywolfCardModel
{
    /// 最终攻�?
	protected override bool IsPlayable => false;
	private Creature? _deferredTarget;
	private bool _deferredAutoPlayScheduled;
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
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_dramatic_stab", null, "slash_attack.mp3")
			.Execute(choiceContext);
	}

	public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
	{
		await TryPlay(creature);
	}
	public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
	{
		if (card != this || oldPileType == PileType.Hand || base.Pile?.Type != PileType.Hand)
			return;

		await TryPlayHittableEnemies();
	}
	public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await TryPlayHittableEnemies();
	}
	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		await TryPlayHittableEnemies();
	}
	private async Task TryPlayHittableEnemies()
	{
		ICombatState? combatState = base.CombatState;
		if (combatState == null) return;

		foreach (Creature creature in combatState.HittableEnemies.ToList())
		{
			if (!creature.IsAlive) continue;
			await TryPlay(creature);
		}
	}
	private async Task TryPlay(Creature target)
	{
		if (!target.IsEnemy) return;
		if (!target.IsAlive) return;
		if (!CanBeKilled(target)) return;
		if (base.Pile?.Type != PileType.Hand) return;

		// Effects triggered outside a GameAction, such as the debug console, have
		// no ActionExecutor pass to settle a lethal auto-play afterwards.
		if (RunManager.Instance?.ActionExecutor.CurrentlyRunningAction == null)
		{
			ScheduleDeferredAutoPlay(target);
			return;
		}

		await CardCmd.AutoPlay(new BlockingPlayerChoiceContext(), this, target);
	}

	private void ScheduleDeferredAutoPlay(Creature target)
	{
		if (_deferredAutoPlayScheduled) return;

		_deferredTarget = target;
		_deferredAutoPlayScheduled = true;
		TaskHelper.RunSafely(ResolveDeferredAutoPlay());
	}

	private async Task ResolveDeferredAutoPlay()
	{
		try
		{
			// Let the current console command or hook chain finish before playing.
			if (Engine.GetMainLoop() is SceneTree sceneTree)
			{
				await sceneTree.ToSignal(sceneTree, SceneTree.SignalName.ProcessFrame);
			}
			else
			{
				await Task.Yield();
			}

			Creature? target = _deferredTarget;
			_deferredTarget = null;

			if (target == null || !target.IsAlive || !target.IsHittable) return;
			if (!CombatManager.Instance.IsInProgress || CombatManager.Instance.IsOverOrEnding) return;
			if (base.Pile?.Type != PileType.Hand || !CanBeKilled(target)) return;

			await CardCmd.AutoPlay(new BlockingPlayerChoiceContext(), this, target);

			if (CombatManager.Instance.IsInProgress && CombatManager.Instance.IsEnding)
			{
				await CombatManager.Instance.CheckWinCondition();
			}
		}
		finally
		{
			_deferredAutoPlayScheduled = false;
		}
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
			num = Hook.ModifyDamage(base.Owner.RunState, base.CombatState, target, base.Owner.Creature, base.DynamicVars.Damage.BaseValue, base.DynamicVars.Damage.Props, this, null, ModifyDamageHookType.All, previewMode, out IEnumerable<AbstractModel> _);
        }
        return num;
    }
    protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(8m);
	}
}
