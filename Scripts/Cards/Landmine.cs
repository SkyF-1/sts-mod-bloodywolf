using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class Landmine : BloodywolfCardModel
{/// 爆了！
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new RateVar(1m),
    };
	public Landmine()
    : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}
    public override string PortraitPath => AssetProfile.PortraitPath!;
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
    	await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        int clout = BloodywolfResources.GetClout(base.Owner);
        if (clout > 0)
            await BloodywolfResources.SpendClout(base.Owner, clout, this, this);

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
