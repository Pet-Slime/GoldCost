using System;
using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;


namespace LifeCost.Patchers
{

    internal class TumpTablePatch
    {
        [HarmonyPatch(typeof(TableVisualEffectsManager))]
        public class void_TeethPatch_thumptable
        {
            [HarmonyPostfix]
            [HarmonyPatch("ThumpTable")]
            public static void ThumpTable(float intensity, ref TableVisualEffectsManager __instance)
            {
                List<Transform> list = new List<Transform>();
                bool flag = Singleton<CurrencyBowl>.Instance != null;
                if (flag)
                {
                    list.Add(Singleton<CurrencyBowl>.Instance.bowl.transform);
                    foreach (Rigidbody rigidbody in Singleton<CurrencyBowl>.Instance.ActiveWeights)
                    {
                        list.Add(rigidbody.transform);
                    }
                }
                List<Transform> list2 = new List<Transform>();
                list2.AddRange(list);
                float num = 0.02f;
                float num2 = 0.04f;
                foreach (Transform transform in list2)
                {
                    Tween.LocalPosition(transform, new Vector3(transform.localPosition.x, -0.5f * intensity, transform.localPosition.z), num, 0f, null, 0, null, null, true);
                }
                foreach (Transform transform2 in list)
                {
                    Tween.LocalPosition(transform2, new Vector3(transform2.localPosition.x, 0.75f * intensity, transform2.localPosition.z), num2, num2 + num, null, 0, null, null, true);
                    Tween.LocalPosition(transform2, new Vector3(transform2.localPosition.x, 0f, transform2.localPosition.z), num2, num2 * 2f + num, null, 0, null, null, true);
                }
            }
        }
    }
}
