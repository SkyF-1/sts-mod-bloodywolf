using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace StsModBloodywolf.Scripts.Commands;

public static class MyCmd
{
    public static async Task<decimal> GiveBlock(Creature creature, decimal amount, CardPlay? cardPlay, bool fast = false)
    {
        return await CreatureCmd.GainBlock(creature, amount, ValueProp.Unpowered, cardPlay, fast);
    }

	public static async Task<IEnumerable<DamageResult>> Troll(PlayerChoiceContext choiceContext, Creature target, decimal amount, CardModel? dealer)
	{
        if(dealer is null)
        {
            var damageResult = await CreatureCmd.Damage(choiceContext, target, amount, ValueProp.Unpowered | ValueProp.Unblockable, null, null);
            if(!target.IsAlive)return damageResult;
            await CreatureCmd.GainBlock(target, damageResult.First().TotalDamage, ValueProp.Unpowered, null);
            return damageResult;
        }
        else
        {
            var damageResult = await CreatureCmd.Damage(choiceContext, target, amount, ValueProp.Unpowered | ValueProp.Unblockable, dealer.Owner.Creature, dealer);
            if(!target.IsAlive)return damageResult;
            await CreatureCmd.GainBlock(target, damageResult.First().TotalDamage, ValueProp.Unpowered, null);
            return damageResult;
        }
	}
}
