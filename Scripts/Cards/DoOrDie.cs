// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.HoverTips;
// using StsModBloodywolf.Scripts.Pools;
// using StsModBloodywolf.Scripts.Powers;

// namespace StsModBloodywolf.Scripts.Cards;

// [RegisterCard(typeof(BloodywolfCardPool))]
// public sealed class DoOrDie : BloodywolfCardModel
// {
//     /// 背水一�?
//     public DoOrDie()
//         : base(2, CardType.Power, CardRarity.Ancient, TargetType.Self)
//     {
//     }

//     public override string PortraitPath => AssetProfile.PortraitPath!;

//     protected override async Task OnPlayEffect(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//     {
//         // Clear all Clout
//         int cloutAmount = BloodywolfResources.GetClout(base.Owner);
//         if (cloutAmount > 0)
//         {
//             await BloodywolfResources.SpendClout(base.Owner, cloutAmount, this, this);
//         }

//         // Exhaust draw pile and discard pile
//         var drawPile = PileType.Draw.GetPile(base.Owner).Cards;
//         var discardPile = PileType.Discard.GetPile(base.Owner).Cards;
//         if(drawPile.Count == 0 && discardPile.Count == 0)
//         {
//             return;
//         }
//         foreach(CardModel card in drawPile.ToList())
//         {
//             await CardCmd.Exhaust(choiceContext, card, false, true);
//         }
//         foreach(CardModel card in discardPile.ToList())
//         {
//             await CardCmd.Exhaust(choiceContext, card, false, true);
//         }
//     }

//     protected override void OnUpgrade()
//     {
//         base.AddKeyword(CardKeyword.Retain);
//     }
// }
