using StsModBloodywolf.Scripts.Cards;
using StsModBloodywolf.Scripts.Powers;
using MegaCrit.Sts2.Core.Localization;
using STS2RitsuLib.Interop.AutoRegistration;

namespace StsModBloodywolf.Scripts.Powers;

[RegisterPower]
public class FuckingTerriblePower : CustomTemporaryStrengthPower<Sing>
{
    public override LocString Title => new("powers", "STS_MOD_BLOODYWOLF_POWER_FUCKING_TERRIBLE_POWER.title");
	protected override bool IsPositive => false;
}
