using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using System.Linq;
using GBC;

namespace LifeCost
{

	internal class TumpTablePatch
	{


		[HarmonyPatch(typeof(TableVisualEffectsManager))]
		public class void_TeethPatch_thumptable
		{
			[HarmonyPostfix, HarmonyPatch(nameof(TableVisualEffectsManager.ThumpTable))]
			public static void ThumpTable(float intensity, ref TableVisualEffectsManager __instance)
			{
				List<Transform> list = new List<Transform>();
				if (Singleton<CurrencyBowl>.Instance != null)
				{
					list.Add(Singleton<CurrencyBowl>.Instance.bowl.transform);
					foreach (Rigidbody itemSlot in Singleton<CurrencyBowl>.Instance.ActiveWeights)
					{
						list.Add(itemSlot.transform);
					}

				}
				List<Transform> list3 = new List<Transform>();
				list3.AddRange(list);
				float num = 0.02f;
				float num2 = 0.04f;
				foreach (Transform transform in list3)
				{
					Tween.LocalPosition(transform, new Vector3(transform.localPosition.x, -0.5f * intensity, transform.localPosition.z), num, 0f, null, Tween.LoopType.None, null, null, true);
				}
				foreach (Transform transform3 in list)
				{
					Tween.LocalPosition(transform3, new Vector3(transform3.localPosition.x, 0.75f * intensity, transform3.localPosition.z), num2, num2 + num, null, Tween.LoopType.None, null, null, true);
					Tween.LocalPosition(transform3, new Vector3(transform3.localPosition.x, 0f, transform3.localPosition.z), num2, num2 * 2f + num, null, Tween.LoopType.None, null, null, true);
				}

			}
		}
	}
}
