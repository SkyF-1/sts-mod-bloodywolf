using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.Commands;
using StsModBloodywolf.Scripts.DynamicVars;


namespace StsModBloodywolf.Scripts.Powers;

public sealed class TrollBackPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new TrollVar(1m)
    };
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
	public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (creature == base.Applier && cardSource != null)
        {
            Flash();
            for(int i = 0; i < Amount; i++)
                await MyCmd.Troll(new BlockingPlayerChoiceContext(), base.Owner, amount, null);
        }
    }
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		await PowerCmd.Remove(this);
	}
}
