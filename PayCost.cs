using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using GBC;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LifeCost
{
    internal class PayCost
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
                yield return PayCost.ShowDamageSequence(costToPay, costToPay, true, 0.125f, null, 0f, false);
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
            yield return PayCost.ShowDamageSequence(costToPay, costToPay, true, 0.125f, null, 0f, false);
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
                    bool flag3 = Singleton<LifeManager>.Instance.Scales != null;
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
                yield return foilToZero();
                yield return new WaitForSeconds(waitTime);
                yield return PayCost.ShowDamageSequence(costToPay, costToPay, true, 0.125f, null, 0f, false);
            }
            else
            {
                AudioController.Instance.PlaySound2D("chipDelay_2", 0, 1f, 0f, null, null, null, null, false);
                yield return foilSpend(costToPay);
            }
            yield break;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x0000213E File Offset: 0x0000033E
        public static IEnumerator extractCostPart2_lifeOnly(int costToPay)
        {
            float waitTime = 0.5f;
            yield return new WaitForSeconds(waitTime);
            yield return PayCost.ShowDamageSequence(costToPay, costToPay, true, 0.125f, null, 0f, false);
            yield return new WaitForSeconds(waitTime);
            yield break;
        }


        public static IEnumerator extractCostPart2_MoneyOnly(int costToPay)
        {
            float waitTime = 0.5f;
            yield return new WaitForSeconds(waitTime);
            AudioController.Instance.PlaySound2D("chipDelay_2", 0, 1f, 0f, null, null, null, null, false);
            yield return foilSpend(costToPay);
            yield return new WaitForSeconds(waitTime);
            yield break;
        }

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



    }
}
