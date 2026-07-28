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
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class ColdShoulder : BloodywolfCardModel
{/// 冷处�?
	protected override bool HasEnergyCostX => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>{ CardKeyword.Ethereal };
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public ColdShoulder()
		: base(0, CardType.Power, CardRarity.Rare, TargetType.None)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ColdShoulderPower>(choiceContext, base.Owner.Creature, ResolveEnergyXValue(), base.Owner.Creature, this);
    }

	protected override void OnUpgrade()
    {
        base.RemoveKeyword(CardKeyword.Ethereal);
    }
}
