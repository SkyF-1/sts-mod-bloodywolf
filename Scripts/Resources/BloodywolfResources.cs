using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Char;
using StsModBloodywolf.Scripts.UI;
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
                smallIconPath: "res://StsModBloodywolf/images/resources/clout/clout_small.png",
                largeIconPath: "res://StsModBloodywolf/images/resources/clout/clout_large.png"));

        registry.AlwaysShowInCombatUiForCharacter<Bloodywolf>(CloutLocalId);

        registry.RegisterCombatUi(
            "clout_combat_counter",
            _ => InstantiateUiScene<CloutCombatCounterUi>(
                "res://StsModBloodywolf/scenes/secondary_resources/clout_combat_counter.tscn"),
            context => context.Node.Refresh(context));

        registry.RegisterCardUi(
            "clout_card_cost",
            _ => InstantiateUiScene<CloutCardCostUi>(
                "res://StsModBloodywolf/scenes/secondary_resources/clout_card_cost.tscn"),
            context => context.Node.Refresh(context));

        _registered = true;
    }

    private static T InstantiateUiScene<T>(string path) where T : Node
    {
        return PreloadManager.Cache.GetScene(path)
            .Instantiate<T>(PackedScene.GenEditState.Disabled);
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
