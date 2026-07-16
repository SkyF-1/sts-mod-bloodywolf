using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Flourishing : BloodywolfCardModel
{
    /// 风生水起
	public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>{CardKeyword.Exhaust};
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    public Flourishing()
        : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new PowerVar<FlourishingPower>(1m),
	};
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FlourishingPower>(choiceContext, base.Owner.Creature, base.DynamicVars["FlourishingPower"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}