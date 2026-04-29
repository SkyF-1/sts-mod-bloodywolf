using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.DynamicVars;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class BackStomp : CustomCardModel
{
    /// 回踩
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(10m, ValueProp.Move),
        new HotTakeVar(3m),
        new CardsVar(1)
    };
    protected override bool ShouldGlowGoldInternal => base.Owner.Creature.GetPower<CloutPower>()?.Amount >= base.DynamicVars[HotTakeVar.Key].BaseValue;

    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    public BackStomp()
        : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        decimal cloutAmount = base.Owner.Creature.GetPower<CloutPower>()?.Amount ?? 0;
        if (cloutAmount >= base.DynamicVars[HotTakeVar.Key].BaseValue)
        {
            CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, base.DynamicVars.Cards.IntValue);
            CardPile pile = PileType.Discard.GetPile(base.Owner);
            IEnumerable<CardModel> selectedCards = await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, base.Owner, prefs);
            foreach (CardModel card in selectedCards)
            {
                await CardPileCmd.Add(card, PileType.Hand);
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(1m);
        base.DynamicVars.Cards.UpgradeValueBy(1);
    }
}