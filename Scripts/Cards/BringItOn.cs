using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class BringItOn : CustomCardModel
{/// 要打就来
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<StrengthPower>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new PowerVar<StrengthPower>(2m)
    };
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public BringItOn()
		: base(1, CardType.Power, CardRarity.Uncommon, TargetType.AllAllies)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<Player> enumerable = base.CombatState.Players.Where((Player p) => p.Creature.IsAlive);
        foreach (Player item in enumerable)
		{
            await PowerCmd.Apply<StrengthPower>(item.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, this);
		}
        await PowerCmd.Apply<StrengthPower>(base.CombatState.HittableEnemies, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, this);
    }

	protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
