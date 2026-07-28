using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Stomp : BloodywolfCardModel
{
    /// 踩头
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new BlockVar(5m, ValueProp.Move),
        new PowerVar<CupLossPower>(5m)
    };
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CupLossPower>()
    };
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    public Stomp()
        : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<CupLossPower>(choiceContext, cardPlay.Target, DynamicVars[CupLossPower.Key].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3m);
    }
}