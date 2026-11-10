// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MegaCrit.Sts2.Core.HoverTips;
// using StsModBloodywolf.Scripts.Pools;
// using StsModBloodywolf.Scripts.DynamicVars;
// using StsModBloodywolf.Scripts.Powers;

// namespace StsModBloodywolf.Scripts.Cards;

// [RegisterCard(typeof(BloodywolfCardPool))]
// public sealed class TourDeForce : BloodywolfCardModel
// {/// 绝活
//     public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
// 	protected override IEnumerable<DynamicVar> CanonicalVars => [new RateVar(8m)];
//     public override string PortraitPath => AssetProfile.PortraitPath!;

// 	public TourDeForce()
// 		: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
// 	{
// 	}

// 	protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//     {
//         await BloodywolfResources.GainClout(
//         base.Owner,
//         base.DynamicVars[RateVar.Key].IntValue,
//         this);
//     }

// 	protected override void OnUpgrade()
//     {
//         base.DynamicVars[RateVar.Key].UpgradeValueBy(2m);
//     }
// }
