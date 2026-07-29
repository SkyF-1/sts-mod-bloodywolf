using System.Collections.Generic;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace StsModBloodywolf.Scripts.CombatHistory
{
    /// <summary>
    /// 记录一次 Troll 技能被使用的战斗历史条目。
    /// </summary>
    public class TrollUsedEntry : CombatHistoryEntry
    {
        /// <summary>伤害来源</summary>
        public Creature? Dealer { get; }

        /// <summary>造成的伤害量</summary>
        public decimal Damage { get; }

        /// <summary>使用的卡牌（可能为 null）</summary>
        public CardModel? Card { get; }

        public override string Description
        {
            get
            {
                string dealerName = (base.Actor != null) ? base.Actor.Name : "Unknown";
                string targetName = (Dealer != null) ? Dealer.Name : "Unknown";
                return $"{dealerName} trolled {targetName}, dealt {Damage} damage.";
            }
        }

        public TrollUsedEntry(
            Creature? dealer,      // 发起攻击的生物
            Creature target,
            decimal damage,
            CardModel? card,
            int roundNumber,
            CombatSide currentSide,
            MegaCrit.Sts2.Core.Combat.History.CombatHistory history,
            IEnumerable<Player> players)
            : base(target, roundNumber, currentSide, history, players)
        {
            Dealer = dealer;
            Damage = damage;
            Card = card;
        }
    }
}