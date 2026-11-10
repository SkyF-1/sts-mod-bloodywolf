using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Relics;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.CardSelection;
using StsModBloodywolf.Scripts.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Relics;

public class LiveRoomRule : BloodywolfRelicModel, ISecondaryResourceHookListener
{
    // 稀有度
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    // 小图标
    public override string PackedIconPath => AssetProfile.IconPath!;
    // 轮廓图标
    protected override string PackedIconOutlinePath => AssetProfile.IconOutlinePath!;
    // 大图标
    protected override string BigIconPath => AssetProfile.BigIconPath!;
	public async Task AfterSecondaryResourceChanged(SecondaryResourceChangeContext context)
	{
		if (context.Delta < 0 &&
			context.Definition.Id == BloodywolfResources.CloutId &&
			context.Player == base.Owner)
		{
			Flash();
			await CreatureCmd.Damage(
				new BlockingPlayerChoiceContext(),
				base.Owner.Creature.CombatState.HittableEnemies,
				-context.Delta,
				ValueProp.Unblockable | ValueProp.Unpowered,
				base.Owner.Creature,
				null,
				null);
		}
	}
}
