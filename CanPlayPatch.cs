using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using System.Linq;

namespace SpaceCost
{
    internal class CanPlayPatch
    {
		[HarmonyPatch(typeof(PlayableCard), "CanPlay")]
		public class void_TeethPatch_CanPlay
		{
			[HarmonyPostfix]
			public static void Postfix(ref bool __result, ref PlayableCard __instance)
			{
				Plugin.Log.LogWarning("Cost test: CanPlay Patch fired");
				Plugin.Log.LogWarning("Cost test: blood cost info- " + __instance.Info.BloodCost);
				Plugin.Log.LogWarning("Cost test: blood cost info- " + (__instance.Info.BloodCost < 0));
				if (__instance.Info.BloodCost < 0) {

					int costToPay = __instance.Info.BloodCost * -1;
					int currentCurrency = RunState.Run.currency; 
					int lifeBalance = Singleton<LifeManager>.Instance.Balance + 5;
					int finalCurrency = currentCurrency + lifeBalance;

					if (costToPay > finalCurrency)
                    {
						__result = false;
                    }
				}
			}
		}
	}
}
