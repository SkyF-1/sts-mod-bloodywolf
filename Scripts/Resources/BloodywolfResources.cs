using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Char;
using STS2RitsuLib;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Resources;

public static class BloodywolfResources
{
    public const string CloutLocalId = "clout";
        
    private static bool _registered;

    public static SecondaryResourceDefinition CloutDefinition { get; private set; } = null!;
    public static string CloutId => CloutDefinition.Id;

    public static void Register()
    {
        if (_registered)
            return;

        var registry = RitsuLibFramework.GetSecondaryResourceRegistry(Entry.ModId);
        CloutDefinition = registry.Register(
            CloutLocalId,
            new SecondaryResourceDefinition(
                defaultAmount: 0,
                baseMaxAmount: null,
                turnStartPolicy: SecondaryResourceTurnStartPolicy.None,
                persistencePolicy: SecondaryResourcePersistencePolicy.Combat,
                locTable: SecondaryResourceDefinition.DefaultLocTable,
                smallIconPath: "res://StsModBloodywolf/images/resources/clout_small.png",
                largeIconPath: "res://StsModBloodywolf/images/resources/clout_large.png"));

        registry.AlwaysShowInCombatUiForCharacter<Bloodywolf>(CloutLocalId);

        registry.RegisterCombatUi(
            "clout_combat_counter",
            parent =>
            {
                var counter = NSecondaryResourceCounter.Create(
                    CloutDefinition,
                    new SecondaryResourceCounterStyle
                    {
                        CounterSize = new Vector2(74, 74),
                        FontSize = 32,
                        PositiveColor = new Color("#ffffff"),
                        FormatAmount = static (amount, _) => amount.ToString(),
                        IconStyle = SecondaryResourceIconStyle.Default with
                        {
                            Size = new Vector2(74, 74),
                            HoverTip = SecondaryResourceHoverTipStyle.Default,
                        },
                    });
                var energyCounter = parent.GetNodeOrNull<Control>("%EnergyCounterContainer");
                if (energyCounter != null)
                    counter.Position = energyCounter.Position + new Vector2(50, 40);
                return counter;
            },
            context => context.Node.Bind(context.Player));

        registry.RegisterCardUi(
            "clout_card_cost",
            parent =>
            {
                var ui = NSecondaryResourceCardCostUi.Create(
                    CloutId,
                    new SecondaryResourceCardCostUiStyle
                    {
                        IconSize = new Vector2(48, 48),
                        FontSize = 24,
                    });
                var energyIcon = parent.GetNodeOrNull<TextureRect>("%EnergyIcon");
                if (energyIcon != null)
                    ui.Position = energyIcon.Position + new Vector2(-8, 48);
                return ui;
            },
            context => context.Node.Refresh(context));

        _registered = true;
    }

    public static int GetClout(Player player)
    {
        return SecondaryResourceCmd.Get(player, CloutId);
    }

    public static int GetClout(Creature? creature)
    {
        return creature?.Player is { } player ? GetClout(player) : 0;
    }

    public static Task<int> GainClout(Player player, int amount, AbstractModel? source = null)
    {
        return SecondaryResourceCmd.Gain(player, CloutId, amount, source);
    }

    public static Task<int> LoseClout(Player player, int amount, AbstractModel? source = null)
    {
        return SecondaryResourceCmd.Lose(player, CloutId, amount, source);
    }

    public static Task<bool> SpendClout(Player player, int amount, CardModel? card = null, AbstractModel? source = null)
    {
        return SecondaryResourceCmd.Spend(player, CloutId, amount, card, source);
    }

    public static HoverTip CreateCloutHoverTip(int? amount = null)
    {
        return ModSecondaryResourceRegistry.CreateHoverTip(CloutId, amount ?? 0);
    }
}
