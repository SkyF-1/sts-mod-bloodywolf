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
public sealed class FluidSentimentPotion : CustomPotionModel
{
	public override PotionRarity Rarity => PotionRarity.Common;
	public override PotionUsage Usage => PotionUsage.CombatOnly;
	public override TargetType TargetType => TargetType.AllEnemies;
    public override string? CustomPackedImagePath => $"res://StsModBloodywolf/images/potions/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomPackedOutlinePath => $"res://StsModBloodywolf/images/potions/{Id.Entry.ToLowerInvariant()}_outline.png";
	public override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromPower<SuperChatPower>() };

	protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? _)
	{
        foreach (Creature enemy in base.Owner.Creature.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<CupLossPower>(enemy, 1m, base.Owner.Creature, null);
        }
	}
}