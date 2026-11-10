using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Relics;

public sealed class Slicer : BloodywolfRelicModel
{
    private const decimal MaxGold = 30m;

    public override RelicRarity Rarity => RelicRarity.Common;

    public override string PackedIconPath => AssetProfile.IconPath!;

    protected override string PackedIconOutlinePath => AssetProfile.IconOutlinePath!;

    protected override string BigIconPath => AssetProfile.BigIconPath!;

    public override async Task AfterCombatEnd(CombatRoom _)
    {
        decimal num = BloodywolfResources.GetClout(Owner);
        decimal gold = Math.Min(num, MaxGold);
        if (gold <= 0m)
        {
            return;
        }

        Flash();
        await PlayerCmd.GainGold(gold, Owner);
    }
}
