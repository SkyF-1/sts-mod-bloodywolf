using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Pools;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class WhipYou : CustomCardModel
{/// 鞭子抽你
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new DamageVar(8m, ValueProp.Move),
        new HotTakeVar(5m),
        new CardsVar(2)
	};

    protected override bool ShouldGlowGoldInternal => base.Owner.Creature.GetPower<CloutPower>()?.Amount >= base.DynamicVars[HotTakeVar.Key].IntValue;
	public WhipYou()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NThinSliceVfx.Create(cardPlay.Target));
		float num = base.Owner.Character.AttackAnimDelay;
		if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Normal)
		{
			num += 0.2f;
		}
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.WithAttackerAnim("Attack", num)
			.Execute(choiceContext);
        decimal CloutValue = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        //言论条件
        if(CloutValue >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        {
            await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        }
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(3m);
	}
}
