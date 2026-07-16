using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Localization;
using StsModBloodywolf.Scripts.Cards;
using StsModBloodywolf.Scripts.Powers;

namespace StsModBloodywolf.Scripts.Powers;

public class RideWavePower : CustomTemporaryStrengthPower
{
	public override AbstractModel OriginModel => ModelDb.Card<RideWave>();
    public override LocString Title => new LocString("powers", "STSMODBLOODYWOLF-RIDE_WAVE_POWER.title");
	protected override bool IsPositive => true;
	public override string? CustomPackedIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";
    public override string? CustomBigIconPath => $"res://StsModBloodywolf/images/powers/{Id.Entry.ToLowerInvariant()}.png";

}
