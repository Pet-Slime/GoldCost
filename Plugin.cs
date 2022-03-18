using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using BepInEx.Configuration;
using InscryptionAPI.Card;


namespace LifeCost
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency(APIGUID, BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency(ZGUID, BepInDependency.DependencyFlags.SoftDependency)]

	public partial class Plugin : BaseUnityPlugin
	{
		public const string APIGUID = "cyantist.inscryption.api";
		public const string PluginGuid = "extraVoid.inscryption.LifeCost";
		public const string ZGUID = "community.inscryption.patch";
		private const string PluginName = "Life Scrybe";
		private const string PluginVersion = "2.0.0";

		public static string Directory;
		internal static ManualLogSource Log;


		internal static ConfigEntry<float> configTeethSpeed;


		private void Awake()
		{
			Log = base.Logger;
			Directory = this.Info.Location.Replace("LifeCost.dll", "");


			configTeethSpeed = Config.Bind("Teeth Speed", "Teeth Speed", 1f, "Configer the Speed in which golden teeth have in act 1 during the set up of the board. Used to be 25f. Higher numbers may cause issues. do not go below 1f.");

			Harmony harmony = new(PluginGuid);
			harmony.PatchAll();

			AddGreedy();
			AddVamperic();
			AddActiveStatsUpLife();
			AddActiveStatsUpMoney();
			AddActivateLifeConverter();
			AddActiveCashConverter();
			AddActivateLifeRandomStatsUp();
			addActivateEnergyGamble();
			GreedySpecialAbility.addGreedySpecialAbility();
			VampericSpecialAbility.addVampericSpecialAbility();

			cards.Teck.AddCard();
		}

		private void Start()
        {

			LifeCost.vanilla_tweaks.ChangeCardsToLifecost();
			LifeCost.Patchers.RenderFixes.RenderFix.CommunityPatchHook();
        }
	}
}