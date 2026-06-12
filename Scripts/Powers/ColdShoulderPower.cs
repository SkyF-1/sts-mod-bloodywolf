using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Powers;

public sealed class ColdShoulderPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Single;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";

	public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? giver, out decimal modifiedAmount)
    {
        modifiedAmount = amount;
        if (amount < 0 && canonicalPower is CloutPower && target == base.Owner)
        {
            Flash();
            modifiedAmount = 0m;
            return true;
        }
        return false;
    }

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (card.Owner.Creature == base.Owner && card.Type == CardType.Attack)
        {
            modifiedCost = originalCost + 1m;
            return true;
        }
        modifiedCost = originalCost;
        return false;
    }
}
