using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace StsModBloodywolf.Scripts.Compat;

public static class Sts2Compat
{
    public static AttackCommand FromCardCompat(
        this AttackCommand command,
        CardModel card,
        CardPlay cardPlay)
    {
#if STS2_API_0_107_1
        return command.FromCard(card);
#elif STS2_API_0_113_0
        return command.FromCard(card, cardPlay);
#else
#error Unsupported Sts2ApiCompat. Add an explicit AttackCommand.FromCard branch.
#endif
    }

    public static Task<IEnumerable<DamageResult>> DamageCompat(
        PlayerChoiceContext context,
        Creature target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
#if STS2_API_0_107_1
        return CreatureCmd.Damage(context, target, amount, props, dealer, cardSource);
#elif STS2_API_0_113_0
        return CreatureCmd.Damage(
            context,
            new[] { target },
            amount,
            props,
            dealer,
            cardSource,
            null,
            ModifyDamageHookType.All);
#else
#error Unsupported Sts2ApiCompat. Add an explicit CreatureCmd.Damage branch.
#endif
    }

    public static Task<IEnumerable<DamageResult>> DamageCompat(
        PlayerChoiceContext context,
        IEnumerable<Creature> targets,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
#if STS2_API_0_107_1
        return CreatureCmd.Damage(context, targets, amount, props, dealer, cardSource);
#elif STS2_API_0_113_0
        return CreatureCmd.Damage(
            context,
            targets,
            amount,
            props,
            dealer,
            cardSource,
            null,
            ModifyDamageHookType.All);
#else
#error Unsupported Sts2ApiCompat. Add an explicit CreatureCmd.Damage branch.
#endif
    }

    public static Task<IEnumerable<DamageResult>> DamageCalculatedCompat(
        PlayerChoiceContext context,
        Creature target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
#if STS2_API_0_107_1
        return CreatureCmd.Damage(context, target, amount, props, dealer, cardSource);
#elif STS2_API_0_113_0
        return CreatureCmd.Damage(
            context,
            new[] { target },
            amount,
            props,
            dealer,
            cardSource,
            null,
            ModifyDamageHookType.Cap);
#else
#error Unsupported Sts2ApiCompat. Add an explicit calculated-damage branch.
#endif
    }

    public static decimal ModifyDamagePreviewCompat(
        IRunState runState,
        ICombatState combatState,
        Creature? target,
        Creature? dealer,
        decimal amount,
        ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay,
        ModifyDamageHookType hookType,
        CardPreviewMode previewMode,
        out IEnumerable<AbstractModel> modifiers)
    {
#if STS2_API_0_107_1
        return Hook.ModifyDamage(
            runState,
            combatState,
            target,
            dealer,
            amount,
            props,
            cardSource,
            hookType,
            previewMode,
            out modifiers);
#elif STS2_API_0_113_0
        return Hook.ModifyDamage(
            runState,
            combatState,
            target,
            dealer,
            amount,
            props,
            cardSource,
            cardPlay,
            hookType,
            previewMode,
            out modifiers);
#else
#error Unsupported Sts2ApiCompat. Add an explicit Hook.ModifyDamage branch.
#endif
    }

    public static Player GetPlayerCompat(this CardPlay cardPlay)
    {
#if STS2_API_0_107_1
        return cardPlay.Card.Owner;
#elif STS2_API_0_113_0
        return cardPlay.Player;
#else
#error Unsupported Sts2ApiCompat. Add an explicit CardPlay player branch.
#endif
    }

    public static CardModel CreateDupeCompat(this CardModel card, Player owner)
    {
#if STS2_API_0_107_1
        return card.CreateDupe();
#elif STS2_API_0_113_0
        return card.CreateDupe(owner);
#else
#error Unsupported Sts2ApiCompat. Add an explicit CardModel.CreateDupe branch.
#endif
    }

    public static decimal ModifyDamageAdditiveCompat(
        this StrengthPower power,
        Creature target,
        decimal amount,
        ValueProp props,
        Creature dealer,
        CardModel? cardSource)
    {
#if STS2_API_0_107_1
        return power.ModifyDamageAdditive(target, amount, props, dealer, cardSource);
#elif STS2_API_0_113_0
        return power.ModifyDamageAdditive(target, amount, props, dealer, cardSource, null);
#else
#error Unsupported Sts2ApiCompat. Add an explicit StrengthPower damage branch.
#endif
    }

    public static decimal ModifyDamageMultiplicativeCompat(
        this VulnerablePower power,
        Creature target,
        decimal amount,
        ValueProp props,
        Creature dealer,
        CardModel? cardSource)
    {
#if STS2_API_0_107_1
        return power.ModifyDamageMultiplicative(target, amount, props, dealer, cardSource);
#elif STS2_API_0_113_0
        return power.ModifyDamageMultiplicative(target, amount, props, dealer, cardSource, null);
#else
#error Unsupported Sts2ApiCompat. Add an explicit VulnerablePower damage branch.
#endif
    }
}
