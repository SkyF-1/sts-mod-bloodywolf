using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.Cards;

public sealed class Verdict : BloodywolfCardModel, ISecondaryResourceHookListener
{/// 定论
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new DamageVar(32m, ValueProp.Move) };
    public override string PortraitPath => AssetProfile.PortraitPath!;

	public Verdict()
		: base(7, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}

	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3").WithHitVfxSpawnedAtBase()
			.Execute(choiceContext);
	}
	public Task AfterSecondaryResourceChanged(SecondaryResourceChangeContext context)
	{
		if (context.Player == base.Owner &&
			context.Delta > 0 &&
			context.Definition.Id == BloodywolfResources.CloutId)
		{
			base.EnergyCost.AddThisCombat(-1);
		}
		return Task.CompletedTask;
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(10m);
	}

}
