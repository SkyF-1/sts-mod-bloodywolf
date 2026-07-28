// using BaseLib.Abstracts;
// using BaseLib.Utils;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.Entities.Creatures;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.ValueProps;
// using StsModBloodywolf.Scripts.DynamicVars;
// using StsModBloodywolf.Scripts.Pools;
// using StsModBloodywolf.Scripts.Commands;

// namespace StsModBloodywolf.Scripts.Cards;

// [Pool(typeof(BloodywolfCardPool))]
// public sealed class LoveAndHate : BloodywolfCardModel
// {/// 爱恨交加
//     public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

// 	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
// 	{
//         new GivenBlockVar(7m)
// 	};

// 	public LoveAndHate()
// 		: base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
// 	{
// 	}

// 	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
// 	{
// 		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
//         await MyCmd.GiveBlock(cardPlay.Target, base.DynamicVars[GivenBlockVar.Key].BaseValue, cardPlay);
// 		await CreatureCmd.Damage(choiceContext, base.CombatState.Enemies.ToList(), base.DynamicVars[GivenBlockVar.Key].BaseValue, ValueProp.Unpowered | ValueProp.Unblockable | ValueProp.Move, base.Owner.Creature, this);
// 	}

// 	protected override void OnUpgrade()
// 	{
//         base.DynamicVars[GivenBlockVar.Key].UpgradeValueBy(2m);
// 	}
// }
