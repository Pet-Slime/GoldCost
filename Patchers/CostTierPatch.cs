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
        [HarmonyPatch(typeof(CardInfo), "CostTier", MethodType.Getter)]
        public class CardInfo_CostTier_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref int __result, ref CardInfo __instance)
            {
                __result += Mathf.RoundToInt((float)__instance.LifeMoneyCost() / 2f) + Mathf.RoundToInt((float)__instance.LifeCost()) + Mathf.RoundToInt((float)__instance.MoneyCost() / 4f);
            }
        }

        [HarmonyPatch(typeof(Deck), "CardCanBePlayedByTurn2WithHand", 0)]
        public class Deck_CardCanBePlayedByTurn2WithHand
        {
            [HarmonyPostfix]
            public static void Postfix(ref CardInfo card, List<CardInfo> hand, ref bool __result, ref Deck __instance)
            {
                bool flag = card.LifeCost() <= 4 || card.LifeMoneyCost() <= 4;
                __result = (__result && flag);
            }
        }
    }
}
