using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System.Reflection;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.CombatHistory;
using StsModBloodywolf.Scripts.Commands;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Boomerang : BloodywolfCardModel
{/// 回旋�?
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new TrollVar(3m),
		new CalculationBaseVar(1m),
		new CalculationExtraVar(1m),
		new CalculatedVar("CalculatedHits").WithMultiplier((CardModel _, Creature? __) => TrollHistoryManager.Count)
    };

	public Boomerang()
		: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}
    // protected override bool ShouldGlowRedInternal => base.Owner.Creature.GetPower<CloutPower>()?.Amount >= base.DynamicVars[HotTakeVar.Key].BaseValue;
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        int count = (int)((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target);
        for(int i = 0; i < count; i++)
        {
            if(!cardPlay.Target.IsAlive) break;
            await MyCmd.Troll(choiceContext, cardPlay.Target, base.DynamicVars[TrollVar.Key].BaseValue, base.Owner.Creature, this);
        }

        return;
        
        decimal CloutValue = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        // 言论条件（现已弃用�?
        //if (cardPlay.Target.IsAlive && CloutValue >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        // {
        //     Creature targetCreature = cardPlay.Target;
        //     MonsterModel monster = targetCreature.Monster;
        //     if (monster == null) return;

        //     MoveState originalMove = monster.NextMove;
        //     if (originalMove == null) return;

        //     // 1. 获取原状态的意图列表并复制一�?
        //     var oldIntents = originalMove.Intents;
        //     var newIntents = new List<AbstractIntent>(oldIntents) { new SingleAttackIntent(6) };

        //     // 2. 获取原状态的执行委托
        //     var performField = typeof(MoveState).GetField("_onPerform", BindingFlags.NonPublic | BindingFlags.Instance);
        //     if (performField == null) throw new Exception("Cannot find _onPerform field");
        //     var originalPerform = (Func<IReadOnlyList<Creature>, Task>)performField.GetValue(originalMove);

        //     // 3. 组合新委托：原动�?+ 额外伤害
        //     async Task CombinedPerform(IReadOnlyList<Creature> targets)
        //     {
        //         await originalPerform(targets);
        //         // 额外造成6点伤害，目标为原招式的目标（通常为全体玩家）
        //         foreach (var player in targets)
        //         {
        //             await DamageCmd.Attack(6)
        //                 .FromMonster(monster)
        //                 .WithHitFx("vfx/vfx_attack_slash")
        //                 .Execute(choiceContext);
        //         }
        //     }

        //     // 创建临时状态，使用对象初始化器设置 FollowUpStateId
        //     string tempStateId = originalMove.StateId + "_BOOSTED";
        //     var tempMove = new MoveState(tempStateId, CombinedPerform, newIntents.ToArray())
        //     {
        //         FollowUpStateId = originalMove.FollowUpStateId ?? originalMove.FollowUpState?.Id,
        //         MustPerformOnceBeforeTransitioning = originalMove.MustPerformOnceBeforeTransitioning
        //     };
        //     // 5. 强制替换当前状�?
        //     monster.SetMoveImmediate(tempMove, forceTransition: true);

        //     // 6. 刷新UI
        //     var creatureNode = NCombatRoom.Instance?.GetCreatureNode(targetCreature);
        //     if (creatureNode != null)
        //         await creatureNode.RefreshIntents();
        // }
    }

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}
