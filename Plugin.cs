using BepInEx;
using BepInEx.Logging;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx.Configuration;


namespace LifeCost
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency(APIGUID, BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency(ZGUID, BepInDependency.DependencyFlags.SoftDependency)]

	public partial class Plugin : BaseUnityPlugin
	{
		public const string APIGUID = "cyantist.inscryption.api";
		public const string PluginGuid = "extraVoid.inscryption.LifeCost";
		public const string ZGUID = "extraVoid.inscryption.renderPatcher";
		public static bool RenderFixActive = false;
		private const string PluginName = "Life Scrybe";
		private const string PluginVersion = "1.4.0";

		public static string Directory;
		internal static ManualLogSource Log;


		internal static ConfigEntry<float> configTeethSpeed;


		private void Awake()
		{
			Log = base.Logger;
			Directory = this.Info.Location.Replace("LifeCost.dll", "");


			configTeethSpeed = Config.Bind("Teeth Speed", "Teeth Speed", 1f, "Configer the Speed in which golden teeth have in act 1 during the set up of the board. Used to be 25f. Higher numbers may cause issues. do not go below 1f.");

			if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ZGUID))
			{
				RenderFixActive = true;
			}

			Harmony harmony = new(PluginGuid);
			harmony.PatchAll();



///			APIPlugin.NewDialogue.Add("lifecost_NotEnoughLife", new DialogueEvent()
///			{
///				id = "NotEnoughLife",
///				speakers = new List<DialogueEvent.Speaker>() { DialogueEvent.Speaker.Leshy },
///				mainLines = new DialogueEvent.LineSet()
///				{
///					lines = new List<DialogueEvent.Line>()
///					{
///						new DialogueEvent.Line { text = "You dont have enough life to gamble away for that card." }
///					}
///				}
///			});

			AddGreedy();
			AddVamperic();
			AddActiveStatsUpLife();
			AddActiveStatsUpMoney();
			GreedySpecialAbility.addGreedySpecialAbility();
			VampericSpecialAbility.addVampericSpecialAbility();

			cards.Teck.AddCard();
			///			cards.card_1.AddCard();
			///			cards.card_2.AddCard();
			///			cards.card_3.AddCard();
			///			cards.card_4.AddCard();
			///			cards.card_5.AddCard();
			///			cards.card_6.AddCard();
			///			cards.card_7.AddCard();
			///			cards.card_8.AddCard();
			///			cards.card_9.AddCard();
		}
	}
}