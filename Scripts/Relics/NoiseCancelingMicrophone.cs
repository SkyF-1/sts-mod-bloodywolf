// using BaseLib.Abstracts;
// using BaseLib.Utils;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.Entities.Relics;
// using StsModBloodywolf.Scripts.Pools;
// using StsModBloodywolf.Scripts.Powers;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.CardSelection;
// using StsModBloodywolf.Scripts.Cards;
// using MegaCrit.Sts2.Core.HoverTips;
// using MegaCrit.Sts2.Core.Entities.Players;
// using MegaCrit.Sts2.Core.Rooms;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;

// namespace StsModBloodywolf.Scripts.Relics;

// [Pool(typeof(BloodywolfRelicPool))]
// public class NoiseCancelingMicrophone : CustomRelicModel
// {
// 	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>{
// 		new EnergyVar("EnergyThreshold", 2),
// 		new EnergyVar(1)
// 	};
// 	private bool _usedThisCombat;
// 	private bool UsedThisCombat
// 	{
// 		get
// 		{
// 			return _usedThisCombat;
// 		}
// 		set
// 		{
// 			AssertMutable();
// 			_usedThisCombat = value;
// 		}
// 	}
//     // 稀有度
//     public override RelicRarity Rarity => RelicRarity.Rare;
//     // 小图标
//     public override string PackedIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
//     // 轮廓图标
//     protected override string PackedIconOutlinePath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
//     // 大图标
//     protected override string BigIconPath => $"res://StsModBloodywolf/images/relics/{Id.Entry.ToLowerInvariant()}.png";
// 	public override Task BeforeCombatStart()
// 	{
// 		UsedThisCombat = false;
// 		base.Status = RelicStatus.Active;
// 		return Task.CompletedTask;
// 	}
// 	public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
// 	{
// 		modifiedCost = originalCost;
// 		if (card.Owner != base.Owner)
// 		{
// 			return false;
// 		}
// 		if (card.EnergyCost.GetWithModifiers(CostModifiers.None | CostModifiers.Local) < 2m)
// 		{
// 			return false;
// 		}
// 		if (card.EnergyCost.CostsX)
// 		{
// 			return false;
// 		}
// 		if (UsedThisCombat)
// 		{
// 			return false;
// 		}
// 		bool flag;
// 		switch (card.Pile?.Type)
// 		{
// 		case PileType.Hand:
// 		case PileType.Play:
// 			flag = true;
// 			break;
// 		default:
// 			flag = false;
// 			break;
// 		}
// 		if (!flag)
// 		{
// 			return false;
// 		}
// 		modifiedCost = default(decimal);
// 		return true;
// 	}
// 	public override async Task BeforeCardPlayed(CardPlay cardPlay)
// 	{
// 		if (cardPlay.Card.Owner == base.Owner && cardPlay.Card.EnergyCost.GetWithModifiers(CostModifiers.None | CostModifiers.Local) >= 2m && !cardPlay.Card.EnergyCost.CostsX && !UsedThisCombat)
// 		{
// 			bool flag;
// 			switch (cardPlay.Card.Pile?.Type)
// 			{
// 			case PileType.Hand:
// 			case PileType.Play:
// 				flag = true;
// 				break;
// 			default:
// 				flag = false;
// 				break;
// 			}
// 			if (flag)
// 			{
// 				UsedThisCombat = true;
// 				base.Status = RelicStatus.Normal;
// 			}
// 		}
// 	}

//     public override Task AfterCombatEnd(CombatRoom _)
// 	{
// 		UsedThisCombat = false;
// 		base.Status = RelicStatus.Normal;
// 		return Task.CompletedTask;
// 	}
// }