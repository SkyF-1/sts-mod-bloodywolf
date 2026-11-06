using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class Unranked : BloodywolfCardModel
{
    /// 榜上无名
    public override string PortraitPath => AssetProfile.PortraitPath!;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<StrengthPower>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new PowerVar<StrengthPower>(1m)
    };
    public Unranked()
        : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<UnrankedPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}