using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Rally : CustomCardModel
{
    /// 号召
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CloutPower>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new RateVar(2m),
        new HotTakeVar(9m)
    };
    protected override bool ShouldGlowGoldInternal => base.Owner.Creature.GetPower<CloutPower>()?.Amount + base.DynamicVars[RateVar.Key].BaseValue >= base.DynamicVars[HotTakeVar.Key].BaseValue;

    public Rally()
        : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<CloutPower>(base.Owner.Creature, base.DynamicVars[RateVar.Key].BaseValue, base.Owner.Creature, this);

        decimal cloutAmount = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        if (cloutAmount >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        {
            int num = 10 - base.Owner.PlayerCombatState.Hand.Cards.Count;
		    await CardPileCmd.Draw(choiceContext, num, base.Owner);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars[HotTakeVar.Key].UpgradeValueBy(-1m);
    }
}