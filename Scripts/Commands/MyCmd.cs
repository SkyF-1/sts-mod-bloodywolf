using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.CombatHistory;

namespace StsModBloodywolf.Scripts.Commands;

public static class MyCmd
{
    public static async Task<decimal> GiveBlock(Creature creature, decimal amount, CardPlay? cardPlay, bool fast = false)
    {
        return await CreatureCmd.GainBlock(creature, amount, ValueProp.Unpowered, cardPlay, fast);
    }

	public static async Task<IEnumerable<DamageResult>> Troll(PlayerChoiceContext choiceContext, Creature target, decimal amount, Creature? dealer, CardModel? cardSource)
	{
        decimal modifiedAmount = MyHook.ModifyTrollDamage(target.CombatState, target, amount, dealer, cardSource);
        var damageResult = await CreatureCmd.Damage(choiceContext, target, modifiedAmount, ValueProp.Unpowered | ValueProp.Unblockable, dealer, cardSource);
        if (target.IsAlive)
        {
            decimal totalDamage = damageResult.Sum(r => r.TotalDamage);
            await CreatureCmd.GainBlock(target, totalDamage, ValueProp.Unpowered, null);
            var combat = target.CombatState;
            var entry = new TrollUsedEntry(
                dealer: dealer,
                target: target,
                damage: totalDamage,
                card: cardSource,
                roundNumber: combat?.RoundNumber ?? 0,
                currentSide: combat?.CurrentSide ?? CombatSide.Player,
                history: null!,                    // 我们用自定义列表管理，不依赖官方历史
                players: combat?.Players ?? Array.Empty<Player>()
            );
            TrollHistoryManager.Record(entry);
            await MyHook.AfterTroll(target.CombatState, choiceContext, target, totalDamage, dealer, cardSource);
        }
        return damageResult;
	}
    public static async Task<IEnumerable<DamageResult>> Troll(PlayerChoiceContext choiceContext, List<Creature> targets, decimal amount, Creature? dealer, CardModel? cardSource)
    {
        var allDamageResults = new List<DamageResult>();

        foreach (var target in targets)
        {
            IEnumerable<DamageResult> damageResults;
            if (dealer is null)
            {
                damageResults = await Troll(choiceContext, target, amount, dealer, cardSource);
            }
            else
            {
                damageResults = await Troll(choiceContext, target, amount, dealer, cardSource);
            }

            allDamageResults.AddRange(damageResults);
        }

        return allDamageResults;
    }
}
