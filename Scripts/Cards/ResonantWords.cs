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
    public override TargetType TargetType => base.IsUpgraded ? TargetType.AllEnemies : TargetType.AnyEnemy;
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new IntVar("Count", 2m),
		new CalculationBaseVar(0m),
		new ExtraDamageVar(1m),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => card.Owner.Creature.Block)
    };
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    public ResonantWords()
        : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }
    protected override async Task OnPlayEffect(PlayerChoiceContext ctx, CardPlay play)
    {
        int count = DynamicVars["Count"].IntValue;
        for (int i = 0; i < count; i++)
        {
            var cmd = DamageCmd.Attack(base.DynamicVars.CalculatedDamage)
                .FromCard(this, play)
                .WithHitFx("vfx/vfx_attack_slash");

            if (IsUpgraded)
                cmd = cmd.TargetingAllOpponents(base.CombatState);
            else
            {
                ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
                cmd = cmd.Targeting(play.Target);
            }

            await cmd.Execute(ctx);
        }
    }
}