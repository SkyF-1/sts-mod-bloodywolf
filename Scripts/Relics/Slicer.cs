using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Relics;

[Pool(typeof(BloodywolfRelicPool))]
public sealed class Slicer : CustomRelicModel
{
    private const decimal MaxGold = 30m;

    public override RelicRarity Rarity => RelicRarity.Common;

    public override string PackedIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";

    protected override string PackedIconOutlinePath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";

    protected override string BigIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";

    public override async Task AfterCombatEnd(CombatRoom _)
    {
        decimal num = Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0m;
        decimal gold = Math.Min(num, MaxGold);
        if (gold <= 0m)
        {
            return;
        }

        Flash();
        await PlayerCmd.GainGold(gold, Owner);
    }
}
