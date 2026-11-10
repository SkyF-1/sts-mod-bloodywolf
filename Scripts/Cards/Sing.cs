using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class Sing : BloodywolfCardModel
{/// 歌唱
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<StrengthPower>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new HotTakeVar(8m),
        new CloutLossVar(2m)
    };

	public Sing()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
	{
	}
    protected override bool ShouldGlowRedInternal => BloodywolfResources.GetClout(base.Owner) >= base.DynamicVars[HotTakeVar.Key].BaseValue;
    public override string PortraitPath => AssetProfile.PortraitPath!;
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");
        int clout = BloodywolfResources.GetClout(base.Owner);
        await PowerCmd.Apply<FuckingTerriblePower>(choiceContext,
            base.CombatState.Enemies,
            clout,
            base.Owner.Creature,
            null);
        //言论条�?
        if (clout >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        {
            await BloodywolfResources.LoseClout(
                base.Owner,
                (int)base.DynamicVars[CloutLossVar.Key].BaseValue,
                this);
        }
    }

	protected override void OnUpgrade()
	{
		base.DynamicVars[HotTakeVar.Key].UpgradeValueBy(2m);
	}
}
