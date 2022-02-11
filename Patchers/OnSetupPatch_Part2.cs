using System.Collections;
using GBC;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using Pixelplacement;

namespace LifeCost
{
    internal class OnSetupPatch_Part2
    {

		public static int PlayerFoils { get; protected set; }

		[HarmonyPatch(typeof(PixelResourcesManager), nameof(PixelResourcesManager.Setup))]
		public class void_TeethPatch_ReourceSetup2
		{
			[HarmonyPostfix]
			public static void Postfix()
			{
				Singleton<PixelResourcesManager>.Instance.StartCoroutine(foilSetup());
			}
		}

		[HarmonyPatch(typeof(PixelBoardManager), nameof(PixelBoardManager.CleanUp))]
		public class void_TeethPatch_ReourceCleanUp2
		{
			[HarmonyPrefix]
			public static void Prefix()
			{
				Singleton<PixelResourcesManager>.Instance.StartCoroutine(foilCleanUp());
			}
		}

		public static IEnumerator foilSetup()
		{
			Plugin.Log.LogWarning("Life cost set up");
			yield return PlayerFoils = SaveData.Data.currency;
			yield return SaveData.Data.currency = 0;
			Plugin.Log.LogWarning(PlayerFoils);
			yield break;
		}

		public static IEnumerator foilCleanUp()
		{
			Plugin.Log.LogWarning("Life cost clean up");
			yield return SaveData.Data.currency = PlayerFoils;
			yield return PlayerFoils = 0;
			yield break;
		}

		public static IEnumerator foilSpend(int amount)
		{
			Plugin.Log.LogWarning("Spending foils: " + PlayerFoils);
			yield return PlayerFoils -= amount;
			Plugin.Log.LogWarning("current foils: " + PlayerFoils);
			yield break;
		}

		public static IEnumerator foilToZero()
		{
			yield return PlayerFoils = 0;
			yield break;
		}
	}
}
