using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.CombatHistory;

namespace StsModBloodywolf.Scripts.Commands;

public static class TrollCmd
{
    public static TrollCommand Troll(decimal amount) => new(amount);
}

public sealed class TrollCommand
{
    private readonly decimal _amount;
    private Creature? _dealer;
    private CardModel? _cardSource;
    private Creature? _singleTarget;
    private IReadOnlyList<Creature>? _targets;
    private ICombatState? _combatState;
    private bool _hasTargeting;

    internal TrollCommand(decimal amount)
    {
        _amount = amount;
    }

    public TrollCommand From(Creature? dealer, CardModel? cardSource = null)
    {
        _dealer = dealer;
        _cardSource = cardSource;
        return this;
    }

    public TrollCommand FromCard(CardModel cardSource)
    {
        ArgumentNullException.ThrowIfNull(cardSource);
        return From(cardSource.Owner.Creature, cardSource);
    }

    public TrollCommand Targeting(Creature target)
    {
        ArgumentNullException.ThrowIfNull(target);
        EnsureTargetingNotSet();
        _singleTarget = target;
        _hasTargeting = true;
        return this;
    }

    public TrollCommand TargetingAll(IEnumerable<Creature> targets)
    {
        ArgumentNullException.ThrowIfNull(targets);
        EnsureTargetingNotSet();
        _targets = targets.ToList();
        _hasTargeting = true;
        return this;
    }

    public TrollCommand TargetingAllOpponents(ICombatState combatState)
    {
        ArgumentNullException.ThrowIfNull(combatState);
        EnsureTargetingNotSet();
        _combatState = combatState;
        _hasTargeting = true;
        return this;
    }

    public async Task<IEnumerable<DamageResult>> Execute(PlayerChoiceContext? choiceContext)
    {
        if (!_hasTargeting)
        {
            throw new InvalidOperationException("No targets set.");
        }

        IEnumerable<Creature> targets = GetPossibleTargets().ToList();
        var allDamageResults = new List<DamageResult>();
        PlayerChoiceContext context = choiceContext ?? new BlockingPlayerChoiceContext();

        foreach (Creature target in targets)
        {
            if (!target.IsAlive) continue;
            ICombatState? combat = target.CombatState;
            if (combat == null) continue;

            decimal modifiedAmount = MyHook.ModifyTrollDamage(combat, target, _amount, _dealer, _cardSource);
            IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(
                context,
                target,
                modifiedAmount,
                ValueProp.Unpowered | ValueProp.Unblockable,
                _dealer,
                _cardSource,
                null);

            allDamageResults.AddRange(damageResults);
            if (!target.IsAlive) continue;

            decimal totalDamage = damageResults.Sum(result => result.TotalDamage);
            await CreatureCmd.GainBlock(target, totalDamage, ValueProp.Unpowered, null);

            TrollHistoryManager.Record(new TrollUsedEntry(
                dealer: _dealer,
                target: target,
                damage: totalDamage,
                card: _cardSource,
                roundNumber: combat.RoundNumber,
                currentSide: combat.CurrentSide,
                history: null!,
                players: combat.Players));

            await MyHook.AfterTroll(combat, context, target, totalDamage, _dealer, _cardSource);
        }

        return allDamageResults;
    }

    private IEnumerable<Creature> GetPossibleTargets()
    {
        if (_singleTarget != null)
        {
            return new[] { _singleTarget };
        }

        if (_targets != null)
        {
            return _targets;
        }

        if (_combatState != null)
        {
            if (_dealer == null)
            {
                throw new InvalidOperationException("A dealer must be set when targeting opponents.");
            }

            return _combatState.GetOpponentsOf(_dealer);
        }

        throw new InvalidOperationException("No targets set.");
    }

    private void EnsureTargetingNotSet()
    {
        if (_hasTargeting)
        {
            throw new InvalidOperationException("Targets already set.");
        }
    }
}