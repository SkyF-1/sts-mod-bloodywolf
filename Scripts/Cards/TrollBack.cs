using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Commands;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class TrollBack : BloodywolfCardModel
{
    /// 反串
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new TrollVar(1m)
    };

    public TrollBack()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await PowerCmd.Apply<TrollBackPower>(choiceContext, cardPlay.Target, DynamicVars[TrollVar.Key].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}