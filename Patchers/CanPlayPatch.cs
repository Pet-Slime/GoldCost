using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using GBC;

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

					int finalCurrency = 0;

					if (__instance.HasAbility(lifecost_vamperic.ability))
                    {
						finalCurrency =  lifeBalance;
					} else if (__instance.HasAbility(lifecost_Greedy.ability))
					{
						finalCurrency = currentCurrency;
					} else
                    {
						finalCurrency = currentCurrency + lifeBalance;
					}

					if (costToPay > finalCurrency)
                    {
						__result = false;
					}
				}
			}
		}


		//Adjust the hint for Life
		[HarmonyPatch(typeof(HintsHandler), "OnNonplayableCardClicked")]
		public class void_TeethPatch_payCostHint
		{

			[HarmonyPrefix]
			public static bool Prefix(PlayableCard card, List<PlayableCard> cardsInHand)
			{
				bool isPart = SaveManager.SaveFile.IsPart2;
				if (isPart)
				{
					HintsHandler.OnGBCNonPlayableCardPressed(card);
					return false;
				}
				else
				{
					int lifeBalance = Singleton<LifeManager>.Instance.Balance + 5;
					int finalCurrency = RunState.Run.currency + lifeBalance;
					bool flag = card.Info.LifeCostz() > finalCurrency;
					if (flag)
					{
//						var cost = card.Info.LifeCostz();
//						HintsHandlerEX.notEnoughLife.TryPlayDialogue(null);
						return false;
					}
				}
				return true;
				
			}
		}

		public class HintsHandlerEX : HintsHandler
		{

			public static HintsHandler.Hint notEnoughLife = new HintsHandler.Hint("lifecost_NotEnoughLife", 1);

		}
	}
}
