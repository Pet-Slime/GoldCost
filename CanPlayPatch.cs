using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using System.Linq;

namespace LifeCost
{
    internal class CanPlayPatch
    {
		[HarmonyPatch(typeof(PlayableCard), "CanPlay")]
		public class void_TeethPatch_CanPlay
		{
			[HarmonyPostfix]
			public static void Postfix(ref bool __result, ref PlayableCard __instance)
			{
				if (__instance.Info.LifeCostz() > 0) {

					int costToPay = __instance.Info.LifeCostz();
					int currentCurrency = RunState.Run.currency; 
					int lifeBalance = Singleton<LifeManager>.Instance.Balance + 5;
					int finalCurrency = currentCurrency + lifeBalance +1;

					if (costToPay > finalCurrency)
                    {
						__result = false;
                    }
				}
			}
		}
	}
}
