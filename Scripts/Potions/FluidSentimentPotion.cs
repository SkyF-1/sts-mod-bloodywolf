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

public sealed class FluidSentimentPotion : BloodywolfPotionModel
{
	public override PotionRarity Rarity => PotionRarity.Common;
	public override PotionUsage Usage => PotionUsage.CombatOnly;
	public override TargetType TargetType => TargetType.AllEnemies;
    public override string? CustomImagePath => AssetProfile.ImagePath!;
    public override string? CustomOutlinePath => AssetProfile.OutlinePath!;
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => new List<IHoverTip> { HoverTipFactory.FromPower<CupLossPower>() };

	protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? _)
	{
        foreach (Creature enemy in base.Owner.Creature.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<CupLossPower>(choiceContext, enemy, 5m, base.Owner.Creature, null);
        }
	}
}
