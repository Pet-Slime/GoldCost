using BepInEx;
using BepInEx.Logging;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx.Configuration;


namespace SpaceCost
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency(APIGUID, BepInDependency.DependencyFlags.HardDependency)]

	public partial class Plugin : BaseUnityPlugin
	{
		public const string APIGUID = "cyantist.inscryption.api";
		public const string PluginGuid = "extraVoid.inscryption.SpaceCost";
		private const string PluginName = "Dream Scrybe";
		private const string PluginVersion = "1.0.0";

		public static string Directory;
		internal static ManualLogSource Log;


		private void Awake()
		{
			Log = base.Logger;
			Directory = this.Info.Location.Replace("SpaceCost.dll", "");


			Harmony harmony = new(PluginGuid);
			harmony.PatchAll();
			//Abilities
		}

		private void Start()
		{
		}
	}


}