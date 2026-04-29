using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;


namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Chat : CustomCardModel
{/// 杂谈
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CloutPower>()
    };
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
        new EnergyVar(1),
        new CardsVar(1),
        new RateVar(1m),
		new CalculationBaseVar(0m),
		new CalculationExtraVar(1m),
		new CalculatedVar("times").WithMultiplier((CardModel card, Creature? _) =>
        {
            if (card?.CombatState?.HittableEnemies == null) return 0m;
            int count = card.CombatState.HittableEnemies
                .Count(c => c.GetPowerAmount<CupLossPower>() > 0);
            return count;
        })
	};
    protected override bool ShouldGlowGoldInternal => base.CombatState != null && base.CombatState.HittableEnemies.Any(c => c.GetPowerAmount<CupLossPower>() > 0);
	public Chat()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        var timesVar = base.DynamicVars["times"] as CalculatedVar;
        int times = timesVar != null ? (int)timesVar.Calculate(cardPlay.Target) : 0;
        for(int i = 0; i < times; i++)
        {
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
            await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
            await PowerCmd.Apply<CloutPower>(
            base.Owner.Creature, 
            base.DynamicVars[RateVar.Key].BaseValue, 
            base.Owner.Creature, 
            this);
        }
	}

	protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}

