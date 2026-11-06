using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using StsModBloodywolf.Scripts.Config;
using StsModBloodywolf.Scripts.Services;
using StsModBloodywolf.Scripts.Audio;
using MegaCrit.Sts2.Core.Combat;
using StsModBloodywolf.Scripts.CombatHistory;
using STS2RitsuLib;
using STS2RitsuLib.Interop;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace StsModBloodywolf.Scripts;

// 必须要加的属性，用于注册Mod。字符串和初始化函数命名一致。
[ModInitializer("Init")]
public class Entry
{
	public const string ModId = "StsModBloodywolf";
	public static readonly Logger Logger = RitsuLibFramework.CreateLogger(ModId);

	// 初始化函数
	public static void Init()
	{
		// 打patch（即修改游戏代码的功能）用
		// 传入参数随意，只要不和其他人撞车即可
		var harmony = new Harmony("sts2.SkyF.Bloodywolf");
		harmony.PatchAll();
		var assembly = typeof(Entry).Assembly;
		RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);
		ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);
		CardAudioSettings.Initialize();
		Logger.Info("Mod initialized!");
		BloodywolfAudioService.Initialize();
		CombatManager.Instance.CombatSetUp += _ =>
		{
			TrollHistoryManager.Clear();
			HotTakeHistory.Clear();
		};
	}
}
