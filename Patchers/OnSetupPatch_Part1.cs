using System;
using System.Collections;
using System.Collections.Generic;
using GBC;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using Pixelplacement;
using Object = UnityEngine.Object;

namespace LifeCost.Patchers
{
    internal class OnSetupPatch_Part1
    {

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
            List<Rigidbody>.Enumerator enumerator = default(List<Rigidbody>.Enumerator);
            string soundId = (holder.activeWeights.Count > 3) ? "teeth_long" : "teeth_short";
            AudioController.Instance.PlaySound3D(soundId, MixerGroup.TableObjectsSFX, holder.transform.position, 1f, 0f, null, null, new AudioParams.Randomization(true), null, false);
            yield break;
        }


        public static IEnumerator BowlCleanup()
        {
            CurrencyBowl holder = Singleton<CurrencyBowl>.Instance;
            foreach (Rigidbody rigidbody in holder.activeWeights)
            {
                rigidbody.transform.SetParent(null);
            }
            List<Rigidbody>.Enumerator enumerator = default(List<Rigidbody>.Enumerator);
            Tween.Rotation(holder.transform, Vector3.zero, 0.2f, 0f, Tween.EaseIn, 0, null, null, true);
            foreach (Rigidbody weight in holder.activeWeights)
            {
                weight.transform.SetParent(holder.transform);
                Tween.Position(weight.transform, holder.transform.position + Vector3.up, 0.1f, 0f, Tween.EaseOut, 0, null, null, true);
                yield return new WaitForSeconds(0.05f);
                holder.StartCoroutine(WeightUtil.DropWeight(weight.gameObject));
            }
            List<Rigidbody>.Enumerator enumerator2 = default(List<Rigidbody>.Enumerator);
            yield return new WaitForSeconds(0.2f);
            Vector3 position = new Vector3(-2.5f, 5.4f, -0.1f);
            yield return new WaitForSeconds(0.2f);
            holder.MoveAway(position);
            yield return new WaitForSeconds(0.2f);
            yield break;
            yield break;
        }

        // Token: 0x04000001 RID: 1
        public static bool currencyBowlBattle;

        // Token: 0x0200001D RID: 29
        [HarmonyPatch(typeof(ResourcesManager))]
        public class void_TeethPatch_ReourceSetup
        {
            // Token: 0x0600007C RID: 124 RVA: 0x00003920 File Offset: 0x00001B20
            [HarmonyPostfix]
            [HarmonyPatch("Setup")]
            public static IEnumerator Postfix(IEnumerator enumerator, ResourcesManager __instance)
            {
                bool flag = __instance is Part1ResourcesManager;
                OnSetupPatch_Part1.currencyBowlBattle = true;
                bool flag2 = flag && Singleton<CurrencyBowl>.Instance != null;
                if (flag2)
                {
                    ///                    yield return OnSetupPatch_Part1.BowlSetup();
                    yield return new WaitForSeconds(0.2f);
                }
                yield return enumerator;
                yield break;
            }
        }

        // Token: 0x0200001E RID: 30
        [HarmonyPatch(typeof(Part1ResourcesManager))]
        public class void_TeethPatch_ReourceCleanup
        {
            // Token: 0x0600007E RID: 126 RVA: 0x0000393F File Offset: 0x00001B3F
            [HarmonyPostfix]
            [HarmonyPatch("CleanUp")]
            public static IEnumerator Postfix(IEnumerator enumerator, Part1ResourcesManager __instance)
            {
                OnSetupPatch_Part1.currencyBowlBattle = false;
                bool flag = Singleton<CurrencyBowl>.Instance != null;
                if (flag)
                {
                    yield return new WaitForSeconds(0.2f);
                    ///                    yield return OnSetupPatch_Part1.BowlCleanup();
                    yield return new WaitForSeconds(0.2f);
                }
                yield return enumerator;
                yield break;
            }
        }

        // Token: 0x0200001F RID: 31
        ///       [HarmonyPatch(typeof(TurnManager))]
        ///       public class void_TeethPatch_CleanupPhase
        ///       {
        ///           // Token: 0x06000080 RID: 128 RVA: 0x0000395E File Offset: 0x00001B5E
        ///           [HarmonyPostfix]
        ///           [HarmonyPatch("CleanupPhase")]
        ///           public static IEnumerator Postfix(IEnumerator enumerator, TurnManager __instance)
        ///           {
        ///               __instance.PlayerWon = __instance.PlayerIsWinner();
        ///               __instance.GameEnding = true;
        ///               __instance.UpdateMisplaysStat();
        ///               bool flag = !__instance.PlayerWon && __instance.Opponent != null && __instance.Opponent.Blueprint != null;
        ///               if (flag)
        ///               {
        ///                   AnalyticsManager.SendFailedEncounterEvent(__instance.Opponent.Blueprint, __instance.Opponent.Difficulty, __instance.TurnNumber);
        ///               }
        ///               bool flag2 = __instance.SpecialSequencer != null;
        ///               if (flag2)
        ///               {
        ///                   yield return __instance.SpecialSequencer.PreCleanUp();
        ///               }
        ///               Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
        ///               bool flag3 = __instance.PlayerWon && __instance.PostBattleSpecialNode == null;
        ///               if (flag3)
        ///               {
        ///                   Singleton<ViewManager>.Instance.SwitchToView((Singleton<GameFlowManager>.Instance == null) ? View.MapDefault : View.Default, false, false);
        ///               }
        ///               else
        ///               {
        ///                   Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
        ///               }
        ///               yield return new WaitForSeconds(0.1f);
        ///               __instance.StartCoroutine(Singleton<ResourcesManager>.Instance.CleanUp());
        ///               yield return new WaitForSeconds(0.2f);
        ///               __instance.StartCoroutine(Singleton<PlayerHand>.Instance.CleanUp());
        ///               yield return new WaitForSeconds(0.2f);
        ///               __instance.StartCoroutine(Singleton<CardDrawPiles>.Instance.CleanUp());
        ///               yield return new WaitForSeconds(0.2f);
        ///               yield return __instance.opponent.CleanUp();
        ///               yield return new WaitForSeconds(0.2f);
        ///               yield return __instance.opponent.OutroSequence(__instance.PlayerWon);
        ///               yield return new WaitForSeconds(0.2f);
        ///               __instance.StartCoroutine(Singleton<BoardManager>.Instance.CleanUp());
        ///               yield return new WaitForSeconds(0.2f);
        ///               bool flag4 = Singleton<TableRuleBook>.Instance != null;
        ///               if (flag4)
        ///               {
        ///                   Singleton<TableRuleBook>.Instance.SetOnBoard(false);
        ///               }
        ///               yield return Singleton<LifeManager>.Instance.CleanUp();
        ///               bool flag5 = __instance.SpecialSequencer != null;
        ///               if (flag5)
        ///               {
        ///                   yield return __instance.SpecialSequencer.GameEnd(__instance.PlayerWon);
        ///               }
        ///               bool flag6 = !__instance.PlayerWon && Singleton<GameFlowManager>.Instance != null;
        ///               if (flag6)
        ///               {
        ///                   yield return Singleton<GameFlowManager>.Instance.PlayerLostBattleSequence(__instance.opponent);
        ///               }
        ///               bool flag7 = __instance.PlayerWon && SaveManager.SaveFile.IsPart3;
        ///               if (flag7)
        ///               {
        ///                   Part3SaveData.Data.IncreaseBounty(10);
        ///               }
        ///               Object.Destroy(__instance.opponent.gameObject);
        ///               bool flag8 = Singleton<GameFlowManager>.Instance != null;
        ///               if (flag8)
        ///               {
        ///                   yield return __instance.TransitionToNextGameState();
        ///               }
        ///               Singleton<PlayerHand>.Instance.SetShown(false, false);
        ///               Object.Destroy(__instance.SpecialSequencer);
        ///               yield break;
        ///           }
        ///       }


        ///        [HarmonyPatch(typeof(CurrencyBowl))]
        ///        public class void_TeethPatch_CurrencyBowlPatch
        ///        {
        ///            // Token: 0x06000082 RID: 130 RVA: 0x0000397D File Offset: 0x00001B7D
        ///            [HarmonyPostfix]
        ///            [HarmonyPatch("ShowGain")]
        ///            public static IEnumerator Postfix(IEnumerator enumerator, CurrencyBowl __instance, int amount, bool enterFromAbove = false, bool noTutorial = false)
        ///            {
        ///                bool flag3 = !OnSetupPatch_Part1.currencyBowlBattle;
        ///                bool flag9 = flag3;
        ///                if (flag9)
        ///                {
        ///                    __instance.MoveIntoPlace(__instance.NEAR_SCALES_POS, __instance.NEAR_SCALES_ROT, Tween.EaseInOutStrong, enterFromAbove);
        ///                }
        ///                yield return __instance.DropWeightsIn(amount);
        ///                yield return new WaitForSeconds(0.75f);
        ///                bool flag4 = !noTutorial;
        ///                bool flag5 = flag4;
        ///                bool flag10 = flag5;
        ///                if (flag10)
        ///                {
        ///                    bool flag6 = !ProgressionData.LearnedMechanic(MechanicsConcept.GainCurrency) && (StoryEventsData.EventCompleted(StoryEvent.TutorialRunCompleted) || StoryEventsData.EventCompleted(StoryEvent.ProspectorDefeated));
        ///                    bool flag7 = flag6;
        ///                    bool flag11 = flag7;
        ///                    if (flag11)
        ///                    {
        ///                        ProgressionData.SetMechanicLearned(MechanicsConcept.GainCurrency);
        ///                        yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("TutorialGainCurrency", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
        ///                    }
        ///                }
        ///                bool flag8 = !OnSetupPatch_Part1.currencyBowlBattle;
        ///                bool flag12 = flag8;
        ///                if (flag12)
        ///                {
        ///                    __instance.MoveAway(__instance.NEAR_SCALES_POS);
        ///                }
        ///                yield break;
        ///            }
        ///        }
        ///    }
    }
}
