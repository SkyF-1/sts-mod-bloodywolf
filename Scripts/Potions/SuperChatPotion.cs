using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Potions;

[Pool(typeof(BloodywolfPotionPool))]
public sealed class SuperChatPotion : CustomPotionModel
{
	public override PotionRarity Rarity => PotionRarity.Common;

	public override PotionUsage Usage => PotionUsage.CombatOnly;

	public override TargetType TargetType => TargetType.AnyEnemy;

	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new PowerVar<SuperChatPower>(3m) };
    public override string? PackedImagePath => $"res://StsModBloodywolf/images/potions/{Id.Entry.ToLowerInvariant()}.png";
    public override string? PackedOutlinePath => $"res://StsModBloodywolf/images/potions/{Id.Entry.ToLowerInvariant()}_outline.png";
	public override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromPower<SuperChatPower>() };

	protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
	{
		PotionModel.AssertValidForTargetedPotion(target);
		NCombatRoom.Instance?.PlaySplashVfx(target, new Color("fd2155"));
		await PowerCmd.Apply<SuperChatPower>(target, base.DynamicVars[SuperChatPower.Key].BaseValue, base.Owner.Creature, null);
	}
}
