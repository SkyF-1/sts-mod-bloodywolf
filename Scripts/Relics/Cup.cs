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
using MegaCrit.Sts2.Core.Combat;
using StsModBloodywolf.Scripts.Commands;
using StsModBloodywolf.Scripts.DynamicVars;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace StsModBloodywolf.Scripts.Relics;

[Pool(typeof(BloodywolfRelicPool))]
public class Cup : CustomRelicModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { 
        new TrollVar(7)
    };
    // 稀有度
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    // 小图标
    public override string PackedIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
    // 轮廓图标
    protected override string PackedIconOutlinePath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
    // 大图标
    protected override string BigIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player == base.Owner)
		{
			ICombatState combatState = player.Creature.CombatState;
			if (base.Owner.PlayerCombatState.TurnNumber == 1)
			{
				Flash();
				VfxCmd.PlayOnCreatureCenters(combatState.HittableEnemies, "vfx/vfx_attack_slash");
				await MyCmd.Troll(choiceContext, combatState.HittableEnemies.ToList(), base.DynamicVars[TrollVar.Key].BaseValue, base.Owner.Creature, null);
			}
		}
	}
}