using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class TestServer : BloodywolfCardModel
{/// 测试�?
	protected override bool HasEnergyCostX => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<DataPower>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DynamicVar("Data", 4m),
    };
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public TestServer()
		: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<DataPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Data"].BaseValue * ResolveEnergyXValue(), base.Owner.Creature, this);
        await PowerCmd.Apply<TestServerPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
    }

	protected override void OnUpgrade()
    {
        base.DynamicVars["Data"].UpgradeValueBy(2m);
    }
}
