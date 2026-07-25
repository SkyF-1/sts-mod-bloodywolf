using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Factories;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Services;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.ValueProps;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public abstract class BloodywolfCardModel : CustomCardModel
{
	public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
	protected abstract Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay);
	protected sealed override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		BloodywolfAudioService.PlayCard(GetType().Name.ToLowerInvariant());
		await OnPlayEffect(choiceContext, cardPlay);
	}
	public static async Task<decimal> GiveBlock(Creature creature, decimal amount, CardPlay? cardPlay, bool fast = false)
    {
        return await CreatureCmd.GainBlock(creature, amount, ValueProp.Unpowered, cardPlay, fast);
    }

    public BloodywolfCardModel(int baseCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true, bool autoAdd = true) : base(baseCost, type, rarity, target, showInCardLibrary, autoAdd)
    {
    }
}
