using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Commands;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class StrictlyBetter : BloodywolfCardModel
{
    /// 上位替代
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new TrollVar(6m),
    };

    public StrictlyBetter()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
    }
    
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        base.EnergyCost.AddThisCombat(1);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        if(base.Pile == null)
        {
            return;
        }
		if(base.Pile.Type != PileType.Hand)
        {
            return;
        }
        if (cardPlay.Card.EnergyCost.CostsX)
        {
            return;
        }
        if(cardPlay.Card.EnergyCost.GetWithModifiers(CostModifiers.All) >= base.EnergyCost.GetWithModifiers(CostModifiers.All))
        {
            return;
        }
        IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
        if (hittableEnemies.Count != 0)
		{
            Creature target = base.Owner.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
            await MyCmd.Troll(choiceContext, target, base.DynamicVars[TrollVar.Key].BaseValue, base.Owner.Creature, this);
		}
	}

    protected override void OnUpgrade()
    {
        base.DynamicVars[TrollVar.Key].UpgradeValueBy(2m);
    }
}