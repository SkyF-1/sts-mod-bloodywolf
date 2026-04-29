using BaseLibToRitsu.Generated;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;

namespace StsModBloodywolf.Scripts.Powers;

public class LandminePower : CustomPowerModel
{
    public CardModel? SourceCard { get; set; }


    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if (side == base.Owner.Side)
        {
            ArgumentNullException.ThrowIfNull(SourceCard, "SourceCard");
            // 使用存储的可变卡牌实例
            await DamageCmd.Attack(base.Amount)
                .FromCard(SourceCard)
                .Unpowered()
                .Targeting(base.Owner)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
            await PowerCmd.Remove(this);
        }
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			List<IHoverTip> list = new List<IHoverTip>();
            if(SourceCard is null)return list;
			List<IHoverTip> list2 = list;
			AbstractModel originModel = SourceCard;
			IEnumerable<IHoverTip> collection;
			if (!(originModel is CardModel card))
			{
				if (!(originModel is PotionModel model))
				{
					if (!(originModel is RelicModel relic))
					{
						throw new InvalidOperationException();
					}
					collection = HoverTipFactory.FromRelic(relic);
				}
				else
				{
					collection = new List<IHoverTip>{HoverTipFactory.FromPotion(model)};
				}
			}
			else
			{
				collection = new List<IHoverTip>{HoverTipFactory.FromCard(card)};
			}
			list2.AddRange(collection);
			return new List<IHoverTip>(list);
		}
	}

}