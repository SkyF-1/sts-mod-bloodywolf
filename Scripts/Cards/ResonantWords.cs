using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class ResonantWords : BloodywolfCardModel
{
    /// 掷地有声
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
		new CalculationBaseVar(0m),
		new ExtraDamageVar(1m),
        new DynamicVar("Count", 2m),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => card.Owner.Creature.Block)
    };
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    public ResonantWords()
        : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for(int i = 0; i < DynamicVars["Count"].IntValue; i++)
        await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this)
            .TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Count"].UpgradeValueBy(1m);
    }
}