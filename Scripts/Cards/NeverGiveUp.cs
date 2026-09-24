using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class NeverGiveUp : BloodywolfCardModel
{
    private const int _baseBlock = 11;

    private int _currentBlock = _baseBlock;
    private int _increasedBlock;

    public override bool GainsBlock => true;

    [SavedProperty]
    public int CurrentBlock
    {
        get => _currentBlock;
        set
        {
            AssertMutable();
            _currentBlock = value;
            base.DynamicVars.Block.BaseValue = _currentBlock;
        }
    }

    [SavedProperty]
    public int IncreasedBlock
    {
        get => _increasedBlock;
        set
        {
            AssertMutable();
            _increasedBlock = value;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(CurrentBlock, ValueProp.Move),
        new DynamicVar("Increase", 2m)
    ];

    public NeverGiveUp()
        : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        if (base.Pile?.Type == PileType.Hand)
        {
            int increase = base.DynamicVars["Increase"].IntValue;
            BuffFromCombat(increase);
            (base.DeckVersion as NeverGiveUp)?.BuffFromCombat(increase);
        }

        return Task.CompletedTask;
    }

    private void BuffFromCombat(int extraBlock)
    {
        IncreasedBlock += extraBlock;
        UpdateBlock();
    }

    private void UpdateBlock()
    {
        CurrentBlock = _baseBlock + IncreasedBlock;
    }

    protected override void AfterDowngraded()
    {
        UpdateBlock();
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Increase"].UpgradeValueBy(1m);
    }
}