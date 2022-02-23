using BepInEx;
using BepInEx.Logging;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx.Configuration;


namespace LifeCostRenderFixPatcher
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency(APIGUID, BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency(LGUID, BepInDependency.DependencyFlags.SoftDependency)]
	[BepInDependency(ZGUID, BepInDependency.DependencyFlags.SoftDependency)]

	public partial class Plugin : BaseUnityPlugin
	{
		public const string APIGUID = "cyantist.inscryption.api";
		public const string PluginGuid = "extraVoid.inscryption.LifeCostRenderPatcher";
		public const string ZGUID = "extraVoid.inscryption.renderPatcher";
		public const string LGUID = "extraVoid.inscryption.LifeCost";
		private const string PluginName = "Life Scrybe Render Patcher";
		private const string PluginVersion = "1.0.0";

		public static string Directory;
		internal static ManualLogSource Log;


		private void Awake()
		{
			Log = base.Logger;

			if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ZGUID))
			{
				Harmony harmony = new(PluginGuid);
				harmony.PatchAll();
			}
		}
	}
}