using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using GBC;

namespace LifeCost.Patchers
{
    internal class CostTierPatch
    {
		[HarmonyPatch(typeof(CardInfo), nameof(CardInfo.CostTier), MethodType.Getter)]
		public class CardInfo_CostTier_Patch
		{
			[HarmonyPostfix]
			public static void Postfix(ref int __result, ref CardInfo __instance)
			{
				__result += Mathf.RoundToInt((float)__instance.LifeCostz() / 2f);
			}
		}

		[HarmonyPatch(typeof(Deck), nameof(Deck.CardCanBePlayedByTurn2WithHand), MethodType.Normal)]
		public class Deck_CardCanBePlayedByTurn2WithHand
		{
			[HarmonyPostfix]
			public static void Postfix(ref CardInfo card, List<CardInfo> hand, ref bool __result, ref Deck __instance)
			{
				bool flag9 = card.LifeCostz() <= 3;
				__result = (__result && flag9);
			}
		}
	}
}