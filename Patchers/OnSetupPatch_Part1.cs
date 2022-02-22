using System;
using System.Collections;
using System.Collections.Generic;
using GBC;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using Pixelplacement;

namespace LifeCost
{
    internal class OnSetupPatch_Part1
	{
		public static bool currencyBowlBattle;

		[HarmonyPatch(typeof(ResourcesManager))]
		public class void_TeethPatch_ReourceSetup
		{
			// Token: 0x06000072 RID: 114 RVA: 0x00003CAF File Offset: 0x00001EAF
			[HarmonyPostfix]
			[HarmonyPatch("Setup")]
			public static IEnumerator Postfix(IEnumerator enumerator, ResourcesManager __instance)
			{
				bool flag = __instance is Part1ResourcesManager;
				if (flag)
				{
					OnSetupPatch_Part1.currencyBowlBattle = true;
					yield return OnSetupPatch_Part1.BowlSetup();
					yield return enumerator;
				}
				yield break;
			}
		}

		[HarmonyPatch(typeof(Part1ResourcesManager))]
		public class void_TeethPatch_ReourceCleanup
		{
			[HarmonyPostfix]
			[HarmonyPatch("CleanUp")]
			public static IEnumerator Postfix(IEnumerator enumerator, Part1ResourcesManager __instance)
			{
				OnSetupPatch_Part1.currencyBowlBattle = false;
				yield return OnSetupPatch_Part1.BowlCleanup();
				yield return enumerator;
				yield break;
			}
		}

		public static IEnumerator BowlSetup()
		{
			CurrencyBowl holder = Singleton<CurrencyBowl>.Instance;
			Vector3 position = new Vector3(-2.5f, 5.4f, -0.1f);
			Vector3 rotation = new Vector3(-18f, 0f, 0f);
			Plugin.Log.LogWarning("Cost test: Setup Patch fired");
			holder.MoveIntoPlace(position, rotation, Tween.EaseLinear, true);
			yield return new WaitForSeconds(0.2f);
			foreach (Rigidbody weight in holder.activeWeights)
			{
				weight.WakeUp();
				weight.AddForce(Vector3.back * Plugin.configTeethSpeed.Value, ForceMode.Impulse);
			}
			string soundId = (holder.activeWeights.Count > 3) ? "teeth_long" : "teeth_short";
			AudioController.Instance.PlaySound3D(soundId, MixerGroup.TableObjectsSFX, holder.transform.position, 1f, 0f, null, null, new AudioParams.Randomization(true), null, false);
			yield break;
		}

		public static IEnumerator BowlCleanup()
		{
			var holder = Singleton<CurrencyBowl>.Instance;
			foreach (Rigidbody rigidbody in holder.activeWeights)
			{
				rigidbody.transform.SetParent(null);
			}
			Tween.Rotation(holder.transform, Vector3.zero, 0.2f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
			foreach (Rigidbody weight in holder.activeWeights)
			{
				weight.transform.SetParent(holder.transform);
				Tween.Position(weight.transform, holder.transform.position + Vector3.up, 0.1f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);
				yield return new WaitForSeconds(0.05f);
				holder.StartCoroutine(WeightUtil.DropWeight(weight.gameObject));
			}
			yield return new WaitForSeconds(0.2f);
			var position = new Vector3(-2.5f, 5.4f, -0.1f);
			holder.MoveAway(position);
			yield break;
		}

		[HarmonyPatch(typeof(CurrencyBowl))]
		public class void_TeethPatch_CurrencyBowlPatch
		{
			[HarmonyPostfix]
			[HarmonyPatch("ShowGain")]
			public static IEnumerator Postfix(IEnumerator enumerator, CurrencyBowl __instance, int amount, bool enterFromAbove = false, bool noTutorial = false)
			{
				bool flag3 = !OnSetupPatch_Part1.currencyBowlBattle;
				if (flag3)
				{
					__instance.MoveIntoPlace(__instance.NEAR_SCALES_POS, __instance.NEAR_SCALES_ROT, Tween.EaseInOutStrong, enterFromAbove);
				}
				yield return __instance.DropWeightsIn(amount);
				yield return new WaitForSeconds(0.75f);
				bool flag = !noTutorial;
				bool flag4 = flag;
				if (flag4)
				{
					bool flag2 = !ProgressionData.LearnedMechanic(MechanicsConcept.GainCurrency) && (StoryEventsData.EventCompleted(StoryEvent.TutorialRunCompleted) || StoryEventsData.EventCompleted(StoryEvent.ProspectorDefeated));
					bool flag5 = flag2;
					if (flag5)
					{
						ProgressionData.SetMechanicLearned(MechanicsConcept.GainCurrency);
						yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("TutorialGainCurrency", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
					}
				}
				bool flag6 = !OnSetupPatch_Part1.currencyBowlBattle;
				if (flag6)
				{
					__instance.MoveAway(__instance.NEAR_SCALES_POS);
				}
				yield break;
			}
		}
	}
}
