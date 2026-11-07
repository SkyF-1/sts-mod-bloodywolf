using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace StsModBloodywolf.Scripts.Relics;

public sealed class Reminiscence : BloodywolfRelicModel
{
    private const int NoPendingCardId = -1;
    private int _attacksPlayed;
    private int _pendingAttackCardId = NoPendingCardId;

    public override RelicRarity Rarity => RelicRarity.Rare;

    public override bool ShowCounter => CombatManager.Instance.IsInProgress;

    public override int DisplayAmount => AttacksPlayed;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar("AttackThreshold", 10)
    ];

    public override string PackedIconPath => AssetProfile.IconPath!;

    protected override string PackedIconOutlinePath => AssetProfile.IconOutlinePath!;

    protected override string BigIconPath => AssetProfile.BigIconPath!;

    [SavedProperty]
    public int AttacksPlayed
    {
        get => _attacksPlayed;
        set
        {
            AssertMutable();
            _attacksPlayed = value;
            UpdateDisplay();
        }
    }

    [SavedProperty]
    public int PendingAttackCardId
    {
        get => _pendingAttackCardId;
        set
        {
            AssertMutable();
            _pendingAttackCardId = value;
        }
    }

    public override Task BeforeCombatStart()
    {
        ResetCombatState();
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int threshold = DynamicVars["AttackThreshold"].IntValue;
        if (!CombatManager.Instance.IsInProgress
            || cardPlay.Card.Owner != Owner
            || cardPlay.Card.Type != CardType.Attack
            || AttacksPlayed >= threshold)
        {
            return;
        }

        AttacksPlayed++;
        if (AttacksPlayed < threshold)
        {
            return;
        }

        if (NetCombatCardDb.Instance.TryGetCardId(cardPlay.Card, out uint cardId))
        {
            PendingAttackCardId = checked((int)cardId);
        }
        else
        {
            PendingAttackCardId = NoPendingCardId;
        }
        Flash();
    }

    public override async Task<Task> AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner
            || PendingAttackCardId == NoPendingCardId
            || !NetCombatCardDb.Instance.TryGetCard((uint)PendingAttackCardId, out CardModel? card)
            || card == null
            || card.Owner != Owner)
        {
            return Task.CompletedTask;
        }

        PendingAttackCardId = NoPendingCardId;
        card.SetToFreeThisTurn();
        await CardPileCmd.Add(card, PileType.Hand);
        Flash();
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        ResetCombatState();
        return Task.CompletedTask;
    }

    private void ResetCombatState()
    {
        AttacksPlayed = 0;
        PendingAttackCardId = NoPendingCardId;
        Status = RelicStatus.Normal;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        int threshold = DynamicVars["AttackThreshold"].IntValue;
        Status = AttacksPlayed >= threshold - 1 ? RelicStatus.Active : RelicStatus.Normal;
        InvokeDisplayAmountChanged();
    }
}
