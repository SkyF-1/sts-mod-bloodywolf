using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class Unrivaled : BloodywolfCardModel
{/// 独步天下
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<UnrivaledPower>(1m),
		new HotTakeVar(9m)
	];
	public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Eternal };
	public override string PortraitPath => AssetProfile.PortraitPath!;

	public Unrivaled()
		: base(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<UnrivaledPower>(choiceContext, base.Owner.Creature, base.DynamicVars["UnrivaledPower"].BaseValue, base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}
