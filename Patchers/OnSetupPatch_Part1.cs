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
				OnSetupPatch_Part1.currencyBowlBattle = true;
				if (flag && Singleton<CurrencyBowl>.Instance != null)
				{
					yield return OnSetupPatch_Part1.BowlSetup();
					yield return new WaitForSeconds(0.2f);
				}
				yield return enumerator;
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
				if (Singleton<CurrencyBowl>.Instance != null)
				{
					yield return new WaitForSeconds(0.2f);
					yield return OnSetupPatch_Part1.BowlCleanup();
					yield return new WaitForSeconds(0.2f);
				}
				yield return enumerator;
				yield break;
			}
		}

		[HarmonyPatch(typeof(TurnManager))]
		public class void_TeethPatch_SetupPhase
		{
			[HarmonyPostfix, HarmonyPatch(nameof(TurnManager.SetupPhase))]
			public static IEnumerator Postfix(IEnumerator enumerator, EncounterData encounterData, TurnManager __instance)
			{
				__instance.IsSetupPhase = true;
				Singleton<PlayerHand>.Instance.PlayingLocked = true;
				if (__instance.SpecialSequencer != null)
				{
					yield return __instance.SpecialSequencer.PreBoardSetup();
				}
				yield return new WaitForSeconds(0.15f);
				yield return Singleton<LifeManager>.Instance.Initialize(__instance.SpecialSequencer == null || __instance.SpecialSequencer.ShowScalesOnStart);
				if (ProgressionData.LearnedMechanic(MechanicsConcept.Rulebook) && Singleton<TableRuleBook>.Instance != null)
				{
					Singleton<TableRuleBook>.Instance.SetOnBoard(true);
				}
				__instance.StartCoroutine(Singleton<BoardManager>.Instance.Initialize());
				yield return new WaitForSeconds(0.2f);
				__instance.StartCoroutine(Singleton<ResourcesManager>.Instance.Setup());
				yield return new WaitForSeconds(1.0f);
				yield return __instance.opponent.IntroSequence(encounterData);
				yield return new WaitForSeconds(0.2f);
				__instance.StartCoroutine(__instance.PlacePreSetCards(encounterData));
				yield return new WaitForSeconds(0.2f);
				if (Singleton<BoonsHandler>.Instance != null)
				{
					yield return Singleton<BoonsHandler>.Instance.ActivatePreCombatBoons();
				}
				if (__instance.SpecialSequencer != null)
				{
					yield return __instance.SpecialSequencer.PreDeckSetup();
				}
				Singleton<PlayerHand>.Instance.Initialize();
				yield return Singleton<CardDrawPiles>.Instance.Initialize();
				if (__instance.SpecialSequencer != null)
				{
					yield return __instance.SpecialSequencer.PreHandDraw();
				}
				yield return Singleton<CardDrawPiles>.Instance.DrawOpeningHand(__instance.GetFixedHand());
				if (__instance.opponent.QueueFirstCardBeforePlayer)
				{
					yield return __instance.opponent.QueueNewCards(true, false);
				}
				if (AscensionSaveData.Data.ChallengeIsActive(AscensionChallenge.StartingDamage))
				{
					ChallengeActivationUI.TryShowActivation(AscensionChallenge.StartingDamage);
					yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, true, 0.125f, null, 0f, false);
				}
				__instance.IsSetupPhase = false;
				yield break;
			}
		}


		[HarmonyPatch(typeof(TurnManager))]
		public class void_TeethPatch_CleanupPhase
		{
			[HarmonyPostfix, HarmonyPatch(nameof(TurnManager.CleanupPhase))]
			public static IEnumerator Postfix(IEnumerator enumerator, TurnManager __instance)
			{
				__instance.PlayerWon = __instance.PlayerIsWinner();
				__instance.GameEnding = true;
				__instance.UpdateMisplaysStat();
				if (!__instance.PlayerWon && __instance.opponent != null && __instance.opponent.Blueprint != null)
				{
					AnalyticsManager.SendFailedEncounterEvent(__instance.opponent.Blueprint, __instance.opponent.Difficulty, __instance.TurnNumber);
				}
				if (__instance.SpecialSequencer != null)
				{
					yield return __instance.SpecialSequencer.PreCleanUp();
				}
				Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
				if (__instance.PlayerWon && __instance.PostBattleSpecialNode == null)
				{
					Singleton<ViewManager>.Instance.SwitchToView((Singleton<GameFlowManager>.Instance == null) ? View.MapDefault : View.Default, false, false);
				}
				else
				{
					Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
				}
				yield return new WaitForSeconds(0.1f);
				__instance.StartCoroutine(Singleton<ResourcesManager>.Instance.CleanUp());
				yield return new WaitForSeconds(0.2f);
				__instance.StartCoroutine(Singleton<PlayerHand>.Instance.CleanUp());
				yield return new WaitForSeconds(0.2f);
				__instance.StartCoroutine(Singleton<CardDrawPiles>.Instance.CleanUp());
				yield return new WaitForSeconds(0.2f);
				yield return __instance.opponent.CleanUp();
				yield return new WaitForSeconds(0.2f);
				yield return __instance.opponent.OutroSequence(__instance.PlayerWon);
				yield return new WaitForSeconds(0.2f);
				__instance.StartCoroutine(Singleton<BoardManager>.Instance.CleanUp());
				yield return new WaitForSeconds(0.2f);
				if (Singleton<TableRuleBook>.Instance != null)
				{
					Singleton<TableRuleBook>.Instance.SetOnBoard(false);
				}
				yield return Singleton<LifeManager>.Instance.CleanUp();
				if (__instance.SpecialSequencer != null)
				{
					yield return __instance.SpecialSequencer.GameEnd(__instance.PlayerWon);
				}
				if (!__instance.PlayerWon && Singleton<GameFlowManager>.Instance != null)
				{
					yield return Singleton<GameFlowManager>.Instance.PlayerLostBattleSequence(__instance.opponent);
				}
				if (__instance.PlayerWon && SaveManager.SaveFile.IsPart3)
				{
					Part3SaveData.Data.IncreaseBounty(10);
				}
				UnityEngine.Object.Destroy(__instance.opponent.gameObject);
				if (Singleton<GameFlowManager>.Instance != null)
				{
					yield return __instance.TransitionToNextGameState();
				}
				Singleton<PlayerHand>.Instance.SetShown(false, false);
				UnityEngine.Object.Destroy(__instance.SpecialSequencer);
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
			yield return new WaitForSeconds(0.2f);
			holder.MoveAway(position);
			yield return new WaitForSeconds(0.2f);
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
