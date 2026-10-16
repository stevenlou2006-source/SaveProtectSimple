using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace cn_xc.SaveProtectSimple
{
	[BepInPlugin("cn_xc.SaveProtectSimple", "Save Protect Simple", "1.1.0")]
	public class SaveProtectSimplePlugin : BaseUnityPlugin
	{
		internal static ManualLogSource Log;

		public static ConfigEntry<bool> PreventSaveDelete;
		public static ConfigEntry<bool> AllowDelete;
		public static ConfigEntry<bool> RemoveSaveLimit;
		public static ConfigEntry<int> MaxSaveSlots;
		public static ConfigEntry<bool> EnableAutoBackup;

		private readonly Harmony harmony = new Harmony("cn_xc.SaveProtectSimple");

		private void Awake()
		{
			Log = Logger;

			PreventSaveDelete = Config.Bind("General", "PreventSaveDelete", true, "死亡时不删除存档。");
			AllowDelete = Config.Bind("General", "AllowDelete", false, "存档清理模式。");
			RemoveSaveLimit = Config.Bind("SaveLimit", "RemoveSaveLimit", true, "移除存档数量上限。");
			MaxSaveSlots = Config.Bind("SaveLimit", "MaxSaveSlots", 999, "最大存档数量。");
			EnableAutoBackup = Config.Bind("AutoBackup", "EnableAutoBackup", true, "退出房间时自动创建存档。");

			var t1 = AccessTools.Method(typeof(StatsManager), "SaveFileDelete");
			if (t1 != null) harmony.Patch(t1, prefix: new HarmonyMethod(typeof(SaveProtectSimplePlugin), nameof(SaveFileDeletePrefix)));

			var t2 = AccessTools.Method(typeof(MenuPageSaves), "OnNewGame");
			if (t2 != null) harmony.Patch(t2, prefix: new HarmonyMethod(typeof(SaveProtectSimplePlugin), nameof(OnNewGamePrefix)));

			var t3 = AccessTools.Method(typeof(RunManager), "LeaveToMainMenu");
			if (t3 != null) harmony.Patch(t3, postfix: new HarmonyMethod(typeof(SaveProtectSimplePlugin), nameof(LeaveToMainMenuPostfix)));

			Log.LogInfo("Save Protect Simple v1.1.0 已加载！");
		}

		public static bool SaveFileDeletePrefix(string saveFileName)
		{
			if (AllowDelete.Value) return true;
			if (!PreventSaveDelete.Value) return true;
			Log.LogInfo($"🛡️ 保留: {saveFileName}");
			return false;
		}

		public static void OnNewGamePrefix(MenuPageSaves __instance)
		{
			if (!RemoveSaveLimit.Value) return;
			var field = AccessTools.Field(typeof(MenuPageSaves), "maxSaveFiles");
			if (field != null) field.SetValue(__instance, MaxSaveSlots.Value);
		}

		public static void LeaveToMainMenuPostfix()
		{
			if (!EnableAutoBackup.Value) return;
			var stats = StatsManager.instance;
			if (stats == null) return;

			var readyField = AccessTools.Field(typeof(StatsManager), "saveFileReady");
			if (readyField == null) return;
			if (!(bool)readyField.GetValue(stats)) return;

			bool isMultiplayer = SemiFunc.IsMultiplayer();
			bool isMaster = false;

			if (isMultiplayer)
			{
				// 反射获取 PhotonNetwork.IsMasterClient，无需添加 Photon DLL 引用
				var pnType = AccessTools.TypeByName("PhotonNetwork");
				if (pnType != null)
				{
					var masterProp = AccessTools.Property(pnType, "IsMasterClient");
					if (masterProp != null)
						isMaster = (bool)masterProp.GetValue(null);
				}
			}

			if (!isMultiplayer || isMaster)
			{
				string name = "REPO_SAVE_AUTO_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
				try
				{
					stats.SaveFileCreate(name, false);
					stats.SaveGame(name);
					Log.LogInfo($"💾 自动存档: {name}");
				}
				catch (Exception ex)
				{
					Log.LogError($"自动存档失败: {ex.Message}");
				}
			}
		}
	}
}