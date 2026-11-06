using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Char;
using STS2RitsuLib.Interop.AutoRegistration;

namespace StsModBloodywolf.Scripts.Cards;

[RegisterCharacterStarterCard(typeof(Bloodywolf), 4)]
public sealed class DefendWolf : BloodywolfCardModel
{/// 防御
	public override bool GainsBlock => true;

	protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Defend };

	protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5m, ValueProp.Move)];
    public override string PortraitPath => AssetProfile.PortraitPath!;

	public DefendWolf()
		: base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Block.UpgradeValueBy(3m);
	}
}
