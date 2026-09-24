using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Landmine : BloodywolfCardModel
{/// 爆了！
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CloutPower>()
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new RateVar(1m),
    };
	public Landmine()
    : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
    	await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        var cloutPower = base.Owner.Creature.GetPower<CloutPower>();
        if (cloutPower != null)
        {
            await PowerCmd.Remove(cloutPower);
        }

        foreach (CardModel card in PileType.Hand.GetPile(base.Owner).Cards)
        {
            if (!card.EnergyCost.CostsX)
            {
                card.SetToFreeThisTurn();
            }
        }

        await PowerCmd.Apply<LandminePower>(choiceContext, base.Owner.Creature, base.DynamicVars[RateVar.Key].BaseValue, base.Owner.Creature, this);
    }

	protected override void OnUpgrade()
	{
        base.EnergyCost.UpgradeBy(-1);
	}
}
