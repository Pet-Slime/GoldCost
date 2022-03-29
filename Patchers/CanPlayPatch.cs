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

				//Hybrid Cost first
				if (__instance.Info.LifeMoneyCost() > 0) {

					int costToPay = __instance.Info.LifeMoneyCost();
					int currentCurrency = 0; 
					bool flag1 = SceneLoader.ActiveSceneName.StartsWith("Part1");
					if (flag1)
					{
						currentCurrency = RunState.Run.currency;
					}
					else
					{
						currentCurrency = SaveData.Data.currency;
					}
					int lifeBalance = Singleton<LifeManager>.Instance.Balance + 5;
					int finalCurrency = currentCurrency + lifeBalance;
					if (costToPay > finalCurrency)
                    {
						__result = false;
					}
				}
				if (__instance.Info.LifeCost() > 0)
				{

					int costToPay = __instance.Info.LifeCost();
					int lifeBalance = Singleton<LifeManager>.Instance.Balance + 5;
					if (costToPay > lifeBalance)
					{
						__result = false;
					}
				}

				if (__instance.Info.MoneyCost() > 0)
				{
					int costToPay = __instance.Info.MoneyCost();
					int currentCurrency = 0;
					bool flag1 = SceneLoader.ActiveSceneName.StartsWith("Part1");
					if (flag1)
					{
						currentCurrency = RunState.Run.currency;
					}
					else
					{
						currentCurrency = SaveData.Data.currency;
					}
					if (costToPay > currentCurrency)
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
					bool flag = card.Info.LifeMoneyCost() > finalCurrency;
					if (flag)
					{
						TextDisplayer.Instance.PlayDialogueEvent("lifecost_NotEnoughLife", TextDisplayer.MessageAdvanceMode.Auto, TextDisplayer.EventIntersectMode.Wait, null, null);
						return false;
					}
					bool flag1 = card.Info.LifeCost() > lifeBalance;
					if (flag1)
					{
						TextDisplayer.Instance.PlayDialogueEvent("lifecost_NotEnoughLife", TextDisplayer.MessageAdvanceMode.Auto, TextDisplayer.EventIntersectMode.Wait, null, null);
						return false;
					}
					bool flag2 = card.Info.MoneyCost() > RunState.Run.currency;
					if (flag2)
					{
						TextDisplayer.Instance.PlayDialogueEvent("lifecost_NotEnoughLife", TextDisplayer.MessageAdvanceMode.Auto, TextDisplayer.EventIntersectMode.Wait, null, null);
						return false;
					}
				}
				return true;
				
			}
		}

		public class HintsHandlerEX : HintsHandler
		{

			public static HintsHandler.Hint notEnoughLife = new HintsHandler.Hint("lifecost_NotEnoughLife", 1);

			public static HintsHandler.Hint notEnoughMoney = new HintsHandler.Hint("lifecost_NotEnoughLife", 1);

			public static HintsHandler.Hint notEnoughLifeMoney = new HintsHandler.Hint("lifecost_NotEnoughLife", 1);


		}
	}
}
