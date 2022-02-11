using System.Collections;
using GBC;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using Pixelplacement;

namespace LifeCost
{
    internal class OnSetupPatch_Part1
    {

		[HarmonyPatch(typeof(ResourcesManager))]
		public class void_TeethPatch_ReourceSetup
		{
			[HarmonyPostfix, HarmonyPatch(nameof(ResourcesManager.Setup))]
			public static IEnumerator Postfix(
			IEnumerator enumerator,
			ResourcesManager __instance
			)
			{
				if (__instance is Part1ResourcesManager)
				{
					yield return BowlSetup();
					yield return enumerator;
				}

			}
		}

		[HarmonyPatch(typeof(Part1ResourcesManager))]
		public class void_TeethPatch_ReourceCleanup
		{
			[HarmonyPostfix, HarmonyPatch(nameof(Part1ResourcesManager.CleanUp))]
			public static IEnumerator Postfix(
			IEnumerator enumerator,
			Part1ResourcesManager __instance
			)
			{
				yield return BowlCleanup();
				yield return enumerator;

			}
		}

		public static IEnumerator BowlSetup()
		{
			var holder = Singleton<CurrencyBowl>.Instance;
			var position = new Vector3(-2.5f, 5.4f, -0.1f);
			var rotation = new Vector3(-18f, 0f, 0f);
			Plugin.Log.LogWarning("Cost test: Setup Patch fired");
			holder.MoveIntoPlace(position, rotation, Tween.EaseLinear, true);
			yield return new WaitForSeconds(0.2f);
			foreach (Rigidbody weight in holder.activeWeights)
			{
				weight.WakeUp();
				weight.AddForce(Vector3.back * 25f, ForceMode.Impulse);
			}
			string soundId = (holder.activeWeights.Count > 3) ? "teeth_long" : "teeth_short";
			AudioController.Instance.PlaySound3D(soundId, MixerGroup.TableObjectsSFX, holder.transform.position, 1f, 0f, null, null, new AudioParams.Randomization(true), null, false);
			yield break;
		}

		public static IEnumerator BowlCleanup()
		{
			Singleton<CurrencyBowl>.Instance.Hide();
			yield break;
		}
	}
}
