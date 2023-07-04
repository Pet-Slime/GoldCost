using System.Collections;
using GBC;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using Pixelplacement;

namespace LifeCost.Patchers
{
    internal class OnSetupPatch_Part2
    {
        public static int PlayerFoils { get; protected set; }

        public static IEnumerator foilSetup()
        {
            Plugin.Log.LogWarning("Life cost set up");
            Plugin.Log.LogWarning(SaveData.Data.currency);
            yield break;
        }

        public static IEnumerator foilCleanUp()
        {
            Plugin.Log.LogWarning("Life cost clean up");
            Plugin.Log.LogWarning(SaveData.Data.currency);
            yield break;
        }

        public static IEnumerator foilSpend(int amount)
        {
            Plugin.Log.LogWarning("Spending foils: " + SaveData.Data.currency.ToString());
            yield return SaveData.Data.currency = SaveData.Data.currency - amount;
            Plugin.Log.LogWarning("current foils: " + SaveData.Data.currency.ToString());
            yield break;
        }

        public static IEnumerator foilToZero()
        {
            yield return SaveData.Data.currency = 0;
            yield break;
        }

        [HarmonyPatch(typeof(PixelResourcesManager), "Setup")]
        public class void_TeethPatch_ReourceSetup2
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                Singleton<PixelResourcesManager>.Instance.StartCoroutine(OnSetupPatch_Part2.foilSetup());
            }
        }

        [HarmonyPatch(typeof(PixelBoardManager), "CleanUp")]
        public class void_TeethPatch_ReourceCleanUp2
        {
            [HarmonyPrefix]
            public static void Prefix()
            {
                Singleton<PixelResourcesManager>.Instance.StartCoroutine(OnSetupPatch_Part2.foilCleanUp());
            }
        }
    }
}
