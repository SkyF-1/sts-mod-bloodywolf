using BaseLib.Abstracts;
using BaseLib.Utils;
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

namespace StsModBloodywolf.Scripts.Relics;

[Pool(typeof(BloodywolfRelicPool))]
public class LiveRoomRule : CustomRelicModel
{
    // 稀有度
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    // 小图标
    public override string PackedIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
    // 轮廓图标
    protected override string PackedIconOutlinePath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
    // 大图标
    protected override string BigIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (!(amount >= 0m) && power.Owner == base.Owner.Creature && power is CloutPower)
		{
			Flash();
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature.CombatState.HittableEnemies, -amount, ValueProp.Unblockable | ValueProp.Unpowered, base.Owner.Creature, null);
		}
	}
}