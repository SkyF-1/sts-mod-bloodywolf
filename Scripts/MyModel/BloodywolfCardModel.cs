using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Services;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace StsModBloodywolf.Scripts.Cards;

[RegisterCard(typeof(BloodywolfCardPool), Inherit = true)]
public abstract class BloodywolfCardModel : ModCardTemplate
{
	public override bool GainsBlock =>
		DynamicVars.Any(static dynamicVar => dynamicVar.Value is BlockVar or CalculatedBlockVar);

	public override CardAssetProfile AssetProfile => new(
		PortraitPath: $"res://StsModBloodywolf/images/cards/stsmodbloodywolf-{Id.Entry.Replace("STS_MOD_BLOODYWOLF_CARD_", "").ToLowerInvariant()}.png");

	public override string PortraitPath => AssetProfile.PortraitPath!;

	protected abstract Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay);
	protected sealed override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		BloodywolfAudioService.PlayCard(this);
		await OnPlayEffect(choiceContext, cardPlay);
	}

    public BloodywolfCardModel(int baseCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true) : base(baseCost, type, rarity, target, showInCardLibrary)
    {
    }

}
