using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class PullTable : BloodywolfCardModel, ISecondaryResourceHookListener
{
	private const string _increaseKey = "Increase";

	private decimal _extraDamage;

	private decimal ExtraDamage
	{
		get
		{
			return _extraDamage;
		}
		set
		{
			AssertMutable();
			_extraDamage = value;
		}
	}
    public override string PortraitPath => AssetProfile.PortraitPath!;

	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new DamageVar(5m, ValueProp.Move),
		new DynamicVar("Increase", 3m)
	};

	public PullTable()
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(choiceContext);
	}

	public Task AfterSecondaryResourceChanged(SecondaryResourceChangeContext context)
	{
		if (context.Player == base.Owner &&
			context.Delta > 0 &&
			context.Definition.Id == BloodywolfResources.CloutId)
		{
			decimal baseValue = base.DynamicVars["Increase"].BaseValue;
			base.DynamicVars.Damage.BaseValue += baseValue;
			ExtraDamage += baseValue;
		}
		return Task.CompletedTask;
	}

	protected override void OnUpgrade()
	{
        base.DynamicVars.Damage.UpgradeValueBy(2m);
        base.DynamicVars["Increase"].UpgradeValueBy(1m);
	}

	protected override void AfterDowngraded()
	{
		base.AfterDowngraded();
		base.DynamicVars.Damage.BaseValue += ExtraDamage;
	}
}
