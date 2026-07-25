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

namespace StsModBloodywolf.Scripts.Relics;

[Pool(typeof(BloodywolfRelicPool))]
public class RabbitTeeth : CustomRelicModel
{
	protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>{HoverTipFactory.FromCard<IBiteYou>()};
    // 稀有度
    public override RelicRarity Rarity => RelicRarity.Common;
    // 小图标
    public override string PackedIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
    // 轮廓图标
    protected override string PackedIconOutlinePath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
    // 大图标
    protected override string BigIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";

    public override async Task<Task> AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player != base.Owner)
		{
			return Task.CompletedTask;
		}
		if (base.Owner.PlayerCombatState.TurnNumber > 1)
		{
			return Task.CompletedTask;
		}
		Flash();
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		List<CardModel> list = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, 0, 9999999), context: choiceContext, player: base.Owner, filter: null, source: this)).ToList();
		foreach (CardModel item in list)
		{
			CardModel cardModel = base.Owner.Creature.CombatState.CreateCard<IBiteYou>(base.Owner);
			await CardCmd.Transform(item, cardModel);
		}
		return Task.CompletedTask;
	}
}