using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using StsModBloodywolf.Scripts.Commands;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class Slander : BloodywolfCardModel
{/// 诋毁
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> {
        new TrollVar(9m)
        // new RateVar(2m)
    };
    protected override bool ShouldGlowGoldInternal => base.CombatState.Enemies.Any((Creature c)=> c.GetPower<CupLossPower>() != null);

	public Slander()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";
    protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		int count = 1;
        if(cardPlay.Target.GetPower<CupLossPower>() != null)count = 2;
        for(int i = 0; i < count; i++)
        {
            await MyCmd.Troll(choiceContext, cardPlay.Target, base.DynamicVars[TrollVar.Key].BaseValue, this);
        }

        // await PowerCmd.Apply<CloutPower>(choiceContext, 
        //     base.Owner.Creature, 
        //     base.DynamicVars[RateVar.Key].BaseValue, 
        //     base.Owner.Creature, 
        //     this);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars[TrollVar.Key].UpgradeValueBy(3m);
	}
}
