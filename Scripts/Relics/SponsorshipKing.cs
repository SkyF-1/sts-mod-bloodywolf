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
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Relics;

public sealed class SponsorshipKing : BloodywolfRelicModel, ISecondaryResourceHookListener
{
    public override RelicRarity Rarity => RelicRarity.Shop;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        new PowerVar<DexterityPower>(1m)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        BloodywolfResources.CreateCloutHoverTip(),
        HoverTipFactory.FromPower<DexterityPower>()
    ];

    public override string PackedIconPath => AssetProfile.IconPath!;

    protected override string PackedIconOutlinePath => AssetProfile.IconOutlinePath!;

    protected override string BigIconPath => AssetProfile.BigIconPath!;

    public async Task AfterSecondaryResourceChanged(SecondaryResourceChangeContext context)
    {
        if (context.Player == Owner &&
            context.Delta > 0 &&
            context.Definition.Id == BloodywolfResources.CloutId)
        {
            Flash();
            await CardPileCmd.Draw(new BlockingPlayerChoiceContext(), DynamicVars.Cards.BaseValue, Owner);
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
