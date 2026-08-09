using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class JellyLegs : BloodywolfCardModel
{
    /// 软脚
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<DexterityPower>()
    };
    public JellyLegs()
        : base(0, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
        new EnergyVar(2),
        new CardsVar(2),
		new PowerVar<JellyLegsPower>(1m)
	};
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
		await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        await PowerCmd.Apply<JellyLegsPower>(choiceContext, base.Owner.Creature, base.DynamicVars["JellyLegsPower"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Energy.UpgradeValueBy(1);
        base.DynamicVars.Cards.UpgradeValueBy(1);
    }
}