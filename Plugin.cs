using BepInEx;
using BepInEx.Logging;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx.Configuration;


namespace LifeCost
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency(APIGUID, BepInDependency.DependencyFlags.HardDependency)]

	public partial class Plugin : BaseUnityPlugin
	{
		public const string APIGUID = "cyantist.inscryption.api";
		public const string PluginGuid = "extraVoid.inscryption.LifeCost";
		private const string PluginName = "Life Scrybe";
		private const string PluginVersion = "1.0.0";

		public static string Directory;
		internal static ManualLogSource Log;


		private void Awake()
		{
			Log = base.Logger;
			Directory = this.Info.Location.Replace("LifeCost.dll", "");


			Harmony harmony = new(PluginGuid);
			harmony.PatchAll();
			//Abilities
		}

		private void Start()
		{
			vanilla_tweaks.AddCard();
		}
	}
}