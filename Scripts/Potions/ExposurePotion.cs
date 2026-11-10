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
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Potions;

public sealed class ExposurePotion : BloodywolfPotionModel
{
	public override PotionRarity Rarity => PotionRarity.Uncommon;
	public override PotionUsage Usage => PotionUsage.CombatOnly;
	public override TargetType TargetType => TargetType.Self;
    public override string? CustomImagePath => AssetProfile.ImagePath!;
    public override string? CustomOutlinePath => AssetProfile.OutlinePath!;
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => new List<IHoverTip> { BloodywolfResources.CreateCloutHoverTip() };
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new RateVar(5m)
    };
	protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? __)
	{
        await BloodywolfResources.GainClout(base.Owner, (int)base.DynamicVars[RateVar.Key].BaseValue, this);
	}
}
