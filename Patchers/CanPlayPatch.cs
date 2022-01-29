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
					int currentCurrency = 0; 
					bool flag1 = SceneLoader.ActiveSceneName == "Part1_Cabin" || SceneLoader.ActiveSceneName == "Part1_Sanctum";
					if (flag1)
					{
						currentCurrency = RunState.Run.currency;
					}
					else
					{
						currentCurrency = OnSetupPatch_Part2.PlayerFoils;
					}
					int lifeBalance = Singleton<LifeManager>.Instance.Balance + 5;
					int finalCurrency = currentCurrency + lifeBalance + 1;

					if (costToPay > finalCurrency)
                    {
						__result = false;
                    }
				}
			}
		}
	}
}
