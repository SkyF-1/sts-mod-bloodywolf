using BaseLib.Abstracts;
using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Entities.Cards;


namespace StsModBloodywolf.Scripts.Powers;

public sealed class CupLossPower : CustomPowerModel, IHealthBarForecastSource
{
	public override PowerType Type => PowerType.Debuff;
	public override PowerStackType StackType => PowerStackType.Counter;
	public const string Key = "CupLossPower";
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? __)
	{
		if (dealer == base.Owner && result.UnblockedDamage > 0)
		{
			await PowerCmd.TickDownDuration(this);
		}
        if (dealer != null && dealer.IsPlayer && target == base.Owner && result.BlockedDamage > 0)
        {
			Flash();
            await CreatureCmd.GainBlock(dealer, new BlockVar(result.BlockedDamage, ValueProp.Unpowered), null);
        }
	}
	public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
	{
		if (!CombatManager.Instance.IsOverOrEnding && side == base.Owner.Side && !base.Owner.IsDead )
		{
			var amount = base.Amount;
			List<DamageResult> list = (await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner, amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null)).ToList();
			DamageResult damageResult = list.FirstOrDefault();
			if(base.Owner.IsAlive && damageResult != null)
			{
				var block = new BlockVar(damageResult.UnblockedDamage, ValueProp.Unpowered);
				await CreatureCmd.GainBlock(base.Owner, block, null);
			}
		}
	}

	public int CalculateTotalDamageNextTurn()
	{
		
		decimal damage = base.Amount;
		damage = Hook.ModifyDamage(base.Owner.CombatState.RunState, base.Owner.CombatState, base.Owner, null, damage, ValueProp.Unblockable | ValueProp.Unpowered, null, ModifyDamageHookType.All, CardPreviewMode.None, out IEnumerable<AbstractModel> _);
		return (int)damage;
	}
	public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        yield return new HealthBarForecastSegment(
            amount: this.CalculateTotalDamageNextTurn(),
            color: new Color("#65cbf3"),
            direction: HealthBarForecastDirection.FromRight
			//order: 0,
			//overlayMaterial: PreloadManager.Cache.GetMaterial("res://StsModBloodywolf/materials/cup_loss_bar_overlay.tres")
        );
    }
}
