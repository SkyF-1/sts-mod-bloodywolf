using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using StsModBloodywolf.Scripts.Pools;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Cards;

[Pool(typeof(BloodywolfCardPool))]
public sealed class ColdShoulder : CustomCardModel
{/// 冷处理
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    new List<IHoverTip>
    {
        HoverTipFactory.FromPower<CloutPower>()
    };
    public override string PortraitPath => $"res://StsModBloodywolf/images/cards/{Id.Entry.ToLowerInvariant()}.png";

	public ColdShoulder()
		: base(1, CardType.Power, CardRarity.Rare, TargetType.None)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ColdShoulderPower>(base.Owner.Creature, 1m, base.Owner.Creature, this);
    }

	protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
