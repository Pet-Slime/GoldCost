using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LifeCost.Patchers
{

    internal class PayCostPatch
    {
        public static IEnumerator extractCostPart1_hybrid(int costToPay, int currentCurrency)
        {
            float waitTime = 0.1f;
            bool flag = costToPay > currentCurrency;
            if (flag)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
                costToPay -= currentCurrency;
                yield return new WaitForSeconds(waitTime);
                List<Rigidbody> list = Singleton<CurrencyBowl>.Instance.TakeWeights(RunState.Run.currency);
                foreach (Rigidbody rigidbody in list)
                {
                    yield return new WaitForSeconds(waitTime);
                    float num3 = (float)list.IndexOf(rigidbody) * 0.05f;
                    Tween.Position(rigidbody.transform, rigidbody.transform.position + Vector3.up * 0.5f, 0.075f, num3, Tween.EaseIn, 0, null, null, true);
                    Tween.Position(rigidbody.transform, new Vector3(0f, 5.5f, 4f), 0.3f, 0.125f + num3, Tween.EaseOut, 0, null, null, true);
                    Object.Destroy(rigidbody.gameObject, 0.5f);
                }
                List<Rigidbody>.Enumerator enumerator = default(List<Rigidbody>.Enumerator);
                RunState.Run.currency = 0;
                yield return new WaitForSeconds(waitTime);
                yield return PayCostPatch.ShowDamageSequence(costToPay, costToPay, true, 0.125f, null, 0f, false);
                Singleton<ViewManager>.Instance.SwitchToView(0, false, true);
                Singleton<ViewManager>.Instance.Controller.LockState = 0;
                list = null;
            }
            else
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
                yield return new WaitForSeconds(waitTime);
                List<Rigidbody> list2 = Singleton<CurrencyBowl>.Instance.TakeWeights(costToPay);
                foreach (Rigidbody rigidbody2 in list2)
                {
                    yield return new WaitForSeconds(waitTime);
                    float num4 = (float)list2.IndexOf(rigidbody2) * 0.05f;
                    Tween.Position(rigidbody2.transform, rigidbody2.transform.position + Vector3.up * 0.5f, 0.075f, num4, Tween.EaseIn, 0, null, null, true);
                    Tween.Position(rigidbody2.transform, new Vector3(0f, 5.5f, 4f), 0.3f, 0.125f + num4, Tween.EaseOut, 0, null, null, true);
                    Object.Destroy(rigidbody2.gameObject, 0.5f);
                }
                yield return new WaitForSeconds(waitTime);
                RunState.Run.currency = currentCurrency - costToPay;
                Singleton<ViewManager>.Instance.SwitchToView(0, false, true);
                Singleton<ViewManager>.Instance.Controller.LockState = 0;
                list2 = null;
            }
            yield break;
        }


        public static IEnumerator extractCostPart1_lifeOnly(int costToPay)
        {
            float waitTime = 0.1f;
            Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
            yield return new WaitForSeconds(waitTime);
            yield return PayCostPatch.ShowDamageSequence(costToPay, costToPay, true, 0.125f, null, 0f, false);
            yield return new WaitForSeconds(waitTime);
            Singleton<ViewManager>.Instance.SwitchToView(0, false, true);
            Singleton<ViewManager>.Instance.Controller.LockState = 0;
            yield break;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000020D6 File Offset: 0x000002D6
        public static IEnumerator extractCostPart1_MoneyOnly(int costToPay, int currentCurrency)
        {
            float waitTime = 0.1f;
            Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
            yield return new WaitForSeconds(waitTime);
            List<Rigidbody> list = Singleton<CurrencyBowl>.Instance.TakeWeights(costToPay);
            foreach (Rigidbody rigidbody in list)
            {
                yield return new WaitForSeconds(waitTime);
                float num3 = (float)list.IndexOf(rigidbody) * 0.05f;
                Tween.Position(rigidbody.transform, rigidbody.transform.position + Vector3.up * 0.5f, 0.075f, num3, Tween.EaseIn, 0, null, null, true);
                Tween.Position(rigidbody.transform, new Vector3(0f, 5.5f, 4f), 0.3f, 0.125f + num3, Tween.EaseOut, 0, null, null, true);
                Object.Destroy(rigidbody.gameObject, 0.5f);
            }
            List<Rigidbody>.Enumerator enumerator = default(List<Rigidbody>.Enumerator);
            yield return new WaitForSeconds(waitTime);
            RunState.Run.currency = currentCurrency - costToPay;
            Singleton<ViewManager>.Instance.SwitchToView(0, false, true);
            Singleton<ViewManager>.Instance.Controller.LockState = 0;
            yield break;
        }

        // Token: 0x0600000F RID: 15 RVA: 0x000020EC File Offset: 0x000002EC
        public static IEnumerator ShowDamageSequence(int damage, int numWeights, bool toPlayer, float waitAfter = 0.125f, GameObject alternateWeightPrefab = null, float waitBeforeCalcDamage = 0f, bool changeView = false)
        {
            bool flag = damage > 1 && Singleton<OpponentAnimationController>.Instance != null;
            bool flag8 = flag;
            if (flag8)
            {
                bool flag2 = P03AnimationController.Instance != null && P03AnimationController.Instance.CurrentFace == 0;
                bool flag9 = flag2;
                if (flag9)
                {
                    P03AnimationController.Instance.SwitchToFace(toPlayer ? P03AnimationController.Face.Happy : P03AnimationController.Face.Angry, false, true);
                }
                else
                {
                    bool flag3 = Singleton<LifeManager>.Instance.scales != null;
                    bool flag10 = flag3;
                    if (flag10)
                    {
                        Singleton<OpponentAnimationController>.Instance.SetLookTarget(Singleton<LifeManager>.Instance.Scales.transform, Vector3.up * 2f);
                    }
                }
            }
            bool flag4 = Singleton<LifeManager>.Instance.Scales != null;
            bool flag11 = flag4;
            if (flag11)
            {
                if (changeView)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, false);
                    yield return new WaitForSeconds(0.1f);
                }
                yield return Singleton<LifeManager>.Instance.Scales.AddDamage(damage, numWeights, toPlayer, alternateWeightPrefab);
                bool flag5 = waitBeforeCalcDamage > 0f;
                bool flag12 = flag5;
                if (flag12)
                {
                    yield return new WaitForSeconds(waitBeforeCalcDamage);
                }
                if (toPlayer)
                {
                    Singleton<LifeManager>.Instance.PlayerDamage += damage;
                }
                else
                {
                    Singleton<LifeManager>.Instance.OpponentDamage += damage;
                }
                yield return new WaitForSeconds(waitAfter);
            }
            bool flag6 = Singleton<OpponentAnimationController>.Instance != null;
            bool flag13 = flag6;
            if (flag13)
            {
                bool flag7 = P03AnimationController.Instance != null && (P03AnimationController.Instance.CurrentFace == P03AnimationController.Face.Angry || P03AnimationController.Instance.CurrentFace == P03AnimationController.Face.Happy);
                bool flag14 = flag7;
                if (flag14)
                {
                    P03AnimationController.Instance.PlayFaceStatic();
                    P03AnimationController.Instance.SwitchToFace(0, false, false);
                }
                else
                {
                    Singleton<OpponentAnimationController>.Instance.ClearLookTarget();
                }
            }
            yield break;
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002128 File Offset: 0x00000328
        public static IEnumerator extractCostPart2_hybrid(int costToPay, int currentCurrency)
        {
            float waitTime = 0.5f;
            bool flag = costToPay > currentCurrency;
            if (flag)
            {
                costToPay -= currentCurrency;
                yield return new WaitForSeconds(waitTime);
                AudioController.Instance.PlaySound2D("chipDelay_2", 0, 1f, 0f, null, null, null, null, false);
                yield return OnSetupPatch_Part2.foilToZero();
                yield return new WaitForSeconds(waitTime);
                yield return PayCostPatch.ShowDamageSequence(costToPay, costToPay, true, 0.125f, null, 0f, false);
            }
            else
            {
                AudioController.Instance.PlaySound2D("chipDelay_2", 0, 1f, 0f, null, null, null, null, false);
                yield return OnSetupPatch_Part2.foilSpend(costToPay);
            }
            yield break;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x0000213E File Offset: 0x0000033E
        public static IEnumerator extractCostPart2_lifeOnly(int costToPay)
        {
            float waitTime = 0.5f;
            yield return new WaitForSeconds(waitTime);
            yield return PayCostPatch.ShowDamageSequence(costToPay, costToPay, true, 0.125f, null, 0f, false);
            yield return new WaitForSeconds(waitTime);
            yield break;
        }

        // Token: 0x06000012 RID: 18 RVA: 0x0000214D File Offset: 0x0000034D
        public static IEnumerator extractCostPart2_MoneyOnly(int costToPay)
        {
            float waitTime = 0.5f;
            yield return new WaitForSeconds(waitTime);
            AudioController.Instance.PlaySound2D("chipDelay_2", 0, 1f, 0f, null, null, null, null, false);
            yield return OnSetupPatch_Part2.foilSpend(costToPay);
            yield return new WaitForSeconds(waitTime);
            yield break;
        }

        // Token: 0x02000029 RID: 41
        [HarmonyPatch(typeof(PlayerHand))]
        public class void_TeethPatch_payCost
        {
            // Token: 0x060000AD RID: 173 RVA: 0x00004172 File Offset: 0x00002372
            [HarmonyPostfix]
            [HarmonyPatch("SelectSlotForCard")]
            public static IEnumerator Postfix(IEnumerator enumerator, PlayerHand __instance, PlayableCard card)
            {
                bool flag3 = card.Info.LifeMoneyCost() > 0 || card.Info.LifeCost() > 0 || card.Info.MoneyCost() > 0;
                if (flag3)
                {
                    __instance.CardsInHand.ForEach(delegate (PlayableCard x)
                    {
                        x.SetEnabled(false);
                    });
                    yield return new WaitWhile(() => __instance.ChoosingSlot);
                    __instance.OnSelectSlotStartedForCard(card);
                    bool flag4 = Singleton<RuleBookController>.Instance != null;
                    if (flag4)
                    {
                        Singleton<RuleBookController>.Instance.SetShown(false, true);
                    }
                    Singleton<BoardManager>.Instance.CancelledSacrifice = false;
                    __instance.choosingSlotCard = card;
                    bool flag5 = card != null && card.Anim != null;
                    if (flag5)
                    {
                        card.Anim.SetSelectedToPlay(true);
                    }
                    Singleton<BoardManager>.Instance.ShowCardNearBoard(card, true);
                    bool flag6 = Singleton<TurnManager>.Instance.SpecialSequencer != null;
                    if (flag6)
                    {
                        yield return Singleton<TurnManager>.Instance.SpecialSequencer.CardSelectedFromHand(card);
                    }
                    bool cardWasPlayed = false;
                    bool requiresSacrifices = card.Info.BloodCost > 0;
                    bool flag7 = requiresSacrifices;
                    if (flag7)
                    {
                        List<CardSlot> validSlots = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll((CardSlot x) => x.Card != null);
                        yield return Singleton<BoardManager>.Instance.ChooseSacrificesForCard(validSlots, card);
                        validSlots = null;
                    }
                    bool flag8 = !Singleton<BoardManager>.Instance.CancelledSacrifice;
                    if (flag8)
                    {
                        List<CardSlot> validSlots2 = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll((CardSlot x) => x.Card == null);
                        yield return Singleton<BoardManager>.Instance.ChooseSlot(validSlots2, !requiresSacrifices);
                        CardSlot lastSelectedSlot = Singleton<BoardManager>.Instance.LastSelectedSlot;
                        bool flag9 = lastSelectedSlot != null;
                        if (flag9)
                        {
                            cardWasPlayed = true;
                            card.Anim.SetSelectedToPlay(false);
                            yield return __instance.PlayCardOnSlot(card, lastSelectedSlot);
                            bool flag10 = card.Info.BonesCost > 0;
                            if (flag10)
                            {
                                yield return Singleton<ResourcesManager>.Instance.SpendBones(card.Info.BonesCost);
                            }
                            bool flag11 = card.EnergyCost > 0;
                            if (flag11)
                            {
                                yield return Singleton<ResourcesManager>.Instance.SpendEnergy(card.EnergyCost);
                            }
                            bool flag12 = card.Info.LifeMoneyCost() > 0 || card.Info.LifeCost() > 0 || card.Info.MoneyCost() > 0;
                            if (flag12)
                            {
                                bool flag2 = !SaveManager.SaveFile.IsPart2;
                                bool flag13 = flag2;
                                if (flag13)
                                {
                                    bool flag14 = card.Info.LifeCost() > 0;
                                    if (flag14)
                                    {
                                        int costToPay = card.Info.LifeCost();
                                        yield return PayCostPatch.extractCostPart1_lifeOnly(costToPay);
                                    }
                                    bool flag15 = card.Info.MoneyCost() > 0;
                                    if (flag15)
                                    {
                                        int costToPay2 = card.Info.MoneyCost();
                                        int currentCurrency = RunState.Run.currency;
                                        yield return PayCostPatch.extractCostPart1_MoneyOnly(costToPay2, currentCurrency);
                                    }
                                    bool flag16 = card.Info.LifeMoneyCost() > 0;
                                    if (flag16)
                                    {
                                        int costToPay3 = card.Info.LifeMoneyCost();
                                        int currentCurrency2 = RunState.Run.currency;
                                        yield return PayCostPatch.extractCostPart1_hybrid(costToPay3, currentCurrency2);
                                    }
                                }
                                else
                                {
                                    bool flag17 = card.Info.LifeCost() > 0;
                                    if (flag17)
                                    {
                                        int costToPay4 = card.Info.LifeCost();
                                        yield return PayCostPatch.extractCostPart2_lifeOnly(costToPay4);
                                    }
                                    bool flag18 = card.Info.MoneyCost() > 0;
                                    if (flag18)
                                    {
                                        int costToPay5 = card.Info.MoneyCost();
                                        yield return PayCostPatch.extractCostPart2_MoneyOnly(costToPay5);
                                    }
                                    bool flag19 = card.Info.LifeMoneyCost() > 0;
                                    if (flag19)
                                    {
                                        int costToPay6 = card.Info.LifeMoneyCost();
                                        int currentCurrency3 = OnSetupPatch_Part2.PlayerFoils;
                                        yield return PayCostPatch.extractCostPart2_hybrid(costToPay6, currentCurrency3);
                                    }
                                }
                            }
                        }
                        validSlots2 = null;
                        lastSelectedSlot = null;
                    }
                    bool flag20 = !cardWasPlayed;
                    if (flag20)
                    {
                        Singleton<BoardManager>.Instance.ShowCardNearBoard(card, false);
                    }
                    __instance.choosingSlotCard = null;
                    bool flag21 = card != null && card.Anim != null;
                    if (flag21)
                    {
                        card.Anim.SetSelectedToPlay(false);
                    }
                    __instance.CardsInHand.ForEach(delegate (PlayableCard x)
                    {
                        x.SetEnabled(true);
                    });
                    yield break;
                }
                yield return enumerator;
                yield break;
            }
        }
    }
}
