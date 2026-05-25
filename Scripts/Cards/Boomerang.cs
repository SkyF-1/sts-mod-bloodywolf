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

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Boomerang : CustomCardModel
{/// 回旋镖
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(6m, ValueProp.Move),
        new RepeatVar(3),
        new HotTakeVar(3m),
        new DynamicVar("enemyAttack", 6m)
    };

	public Boomerang()
		: base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}
    protected override bool ShouldGlowRedInternal => base.Owner.Creature.GetPower<CloutPower>()?.Amount >= base.DynamicVars[HotTakeVar.Key].BaseValue;
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue).FromCard(this)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
        decimal CloutValue = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        // 言论条件
        if (CloutValue >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        {
            Creature targetCreature = cardPlay.Target;
            MonsterModel monsterModel = targetCreature.Monster;
            if (monsterModel == null) return;
            
            MoveState originalMove = monsterModel.NextMove;
            if (originalMove == null) return;

            // 1. 修改意图列表（通过反射设置 Intents 的支持字段）
            var intentsField = typeof(MoveState).GetField("<Intents>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
            if (intentsField == null) throw new Exception("Cannot find Intents backing field");
            var oldIntents = (IReadOnlyList<AbstractIntent>)intentsField.GetValue(originalMove);
            var newIntents = new List<AbstractIntent>(oldIntents) { new SingleAttackIntent(6) }.AsReadOnly();
            intentsField.SetValue(originalMove, newIntents);

            // 2. 修改执行委托（组合原委托）
            var performField = typeof(MoveState).GetField("_onPerform", BindingFlags.NonPublic | BindingFlags.Instance);
            if (performField == null) throw new Exception("Cannot find _onPerform field");
            var originalPerform = (Func<IReadOnlyList<Creature>, Task>)performField.GetValue(originalMove);
            
            async Task CombinedPerform(IReadOnlyList<Creature> targets)
            {
                await originalPerform(targets);
                // 额外造成6点伤害，目标为原招式的目标（通常为全体玩家）
                foreach (var player in targets)
                {
                    await DamageCmd.Attack(6)
                        .FromMonster(monsterModel)
                        .WithHitFx("vfx/vfx_attack_slash")
                        .Execute(choiceContext);
                }
            }
            performField.SetValue(originalMove, (Func<IReadOnlyList<Creature>, Task>)CombinedPerform);

            // 3. 刷新 UI 显示
            var creatureNode = NCombatRoom.Instance?.GetCreatureNode(targetCreature);
            if (creatureNode != null)
                await creatureNode.RefreshIntents();

            // 可选：防止重复添加（记录已修改的怪物）
            // _boostedMonsters.Add(targetCreature);
        }
    }

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(2m);
	}
}
