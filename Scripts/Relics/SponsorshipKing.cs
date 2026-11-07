using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Relics;

public sealed class SponsorshipKing : BloodywolfRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Shop;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        new PowerVar<DexterityPower>(1m)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<CloutPower>(),
        HoverTipFactory.FromPower<DexterityPower>()
    ];

    public override string PackedIconPath => AssetProfile.IconPath!;

    protected override string PackedIconOutlinePath => AssetProfile.IconOutlinePath!;

    protected override string BigIconPath => AssetProfile.BigIconPath!;

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (applier == Owner.Creature && amount > 0m && power is CloutPower)
        {
            Flash();
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
    }

    public override async Task<Task> AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner)
        {
            Flash();
            await PowerCmd.Apply<DexterityPower>(
                choiceContext,
                new[] { Owner.Creature },
                -DynamicVars.Dexterity.BaseValue,
                Owner.Creature,
                null);
        }
        return Task.CompletedTask;
    }
}
