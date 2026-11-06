// using MegaCrit.Sts2.Core.Combat;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Powers;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Entities.Creatures;
// using MegaCrit.Sts2.Core.ValueProps;
// using MegaCrit.Sts2.Core.Models;

// namespace StsModBloodywolf.Scripts.Powers;

// public sealed class SuperChatPower : BloodywolfPowerModel
// {
// 	public override PowerType Type => PowerType.Debuff;
// 	public const string Key = "SuperChatPower";
// 	public override PowerStackType StackType => PowerStackType.Counter;
// 	public override string? CustomIconPath => AssetProfile.IconPath!;
//     public override string? CustomBigIconPath => AssetProfile.BigIconPath!;
//     public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
// 	{
// 		if (target != base.Owner)
// 		{
// 			return 1m;
// 		}
//         if ((props & ValueProp.Move) == 0)
//         {
//             return 1m;
//         }
//         if ((props & ValueProp.Unpowered) != 0)
//         {
//             return 1m;
//         }
// 		if (target.Block > 0m)
//         {
//             return 2m;
//         }
// 		return 1m;
// 	}
// 	public override async Task 	AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
// 	{
// 		if (side == CombatSide.Enemy)
// 		{
// 			await PowerCmd.TickDownDuration(this);
// 		}
// 	}

// }
