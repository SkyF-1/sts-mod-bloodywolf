using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using StsModBloodywolf.Scripts.Char;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;

namespace StsModBloodywolf.Scripts.Resources;

[RegisterSingleton]
public sealed class CloutDecaySingleton : HookedSingletonModel
{
    public CloutDecaySingleton() : base(HookType.Combat)
    {
    }

    public override async Task AfterSideTurnEndLate(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        foreach (var participant in participants)
        {
            if (participant.Player?.Character is not Bloodywolf)
                continue;

            await BloodywolfResources.LoseClout(participant.Player, 1, this);
        }
    }
}
