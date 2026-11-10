using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Char;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Interop.AutoRegistration;

namespace StsModBloodywolf.Scripts.Cards;

[RegisterCharacterStarterCard(typeof(Bloodywolf))]
public sealed class Review : BloodywolfCardModel
{/// 测评
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new RateVar(2m) };
    public override string PortraitPath => AssetProfile.PortraitPath!;

	public Review()
		: base(0, CardType.Skill, CardRarity.Basic, TargetType.Self)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await BloodywolfResources.GainClout(
            base.Owner,
            (int)base.DynamicVars[RateVar.Key].BaseValue,
            this);
    }

	protected override void OnUpgrade()
    {
        base.DynamicVars[RateVar.Key].UpgradeValueBy(1m);
    }
}
