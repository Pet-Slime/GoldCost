using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using GBC;

namespace LifeCost.Patchers
{
    internal class CanPlayPatch
    {
        [HarmonyPatch(typeof(PlayableCard), "CanPlay")]
        public class void_TeethPatch_CanPlay
        {
            [HarmonyPostfix]
            public static void Postfix(ref bool __result, ref PlayableCard __instance)
            {
                bool flag = __instance.Info.LifeMoneyCost() > 0;
                if (flag)
                {
                    int num = __instance.Info.LifeMoneyCost();
                    bool flag2 = SceneLoader.ActiveSceneName.StartsWith("Part1");
                    bool flag3 = flag2;
                    int currency;
                    if (flag3)
                    {
                        currency = RunState.Run.currency;
                    }
                    else
                    {
                        currency = SaveData.Data.currency;
                    }
                    int num2 = Singleton<LifeManager>.Instance.Balance + 5;
                    int num3 = currency + num2;
                    bool flag4 = num > num3;
                    if (flag4)
                    {
                        __result = false;
                    }
                }
                bool flag5 = __instance.Info.LifeCost() > 0;
                if (flag5)
                {
                    int num4 = __instance.Info.LifeCost();
                    int num5 = Singleton<LifeManager>.Instance.Balance + 5;
                    bool flag6 = num4 > num5;
                    if (flag6)
                    {
                        __result = false;
                    }
                }
                bool flag7 = __instance.Info.MoneyCost() > 0;
                if (flag7)
                {
                    int num6 = __instance.Info.MoneyCost();
                    bool flag8 = SceneLoader.ActiveSceneName.StartsWith("Part1");
                    bool flag9 = flag8;
                    int currency2;
                    if (flag9)
                    {
                        currency2 = RunState.Run.currency;
                    }
                    else
                    {
                        currency2 = SaveData.Data.currency;
                    }
                    bool flag10 = num6 > currency2;
                    if (flag10)
                    {
                        __result = false;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(HintsHandler), "OnNonplayableCardClicked")]
        public class void_TeethPatch_payCostHint
        {
            [HarmonyPrefix]
            public static bool Prefix(PlayableCard card, List<PlayableCard> cardsInHand)
            {
                bool isPart = SaveManager.SaveFile.IsPart2;
                bool flag = isPart;
                bool result;
                if (flag)
                {
                    HintsHandler.OnGBCNonPlayableCardPressed(card);
                    result = false;
                }
                else
                {
                    int num = Singleton<LifeManager>.Instance.Balance + 5;
                    int num2 = RunState.Run.currency + num;
                    bool flag2 = card.Info.LifeMoneyCost() > num2;
                    bool flag3 = flag2;
                    if (flag3)
                    {
                        Singleton<TextDisplayer>.Instance.PlayDialogueEvent("lifecost_NotEnoughLife", 0, 0, null, null);
                        result = false;
                    }
                    else
                    {
                        bool flag4 = card.Info.LifeCost() > num;
                        bool flag5 = flag4;
                        if (flag5)
                        {
                            Singleton<TextDisplayer>.Instance.PlayDialogueEvent("lifecost_NotEnoughLife", 0, 0, null, null);
                            result = false;
                        }
                        else
                        {
                            bool flag6 = card.Info.MoneyCost() > RunState.Run.currency;
                            bool flag7 = flag6;
                            if (flag7)
                            {
                                Singleton<TextDisplayer>.Instance.PlayDialogueEvent("lifecost_NotEnoughLife", 0, 0, null, null);
                                result = false;
                            }
                            else
                            {
                                result = true;
                            }
                        }
                    }
                }
                return result;
            }
        }

        public class HintsHandlerEX : HintsHandler
        {
            public static HintsHandler.Hint notEnoughLife = new HintsHandler.Hint("lifecost_NotEnoughLife", 1);

            public static HintsHandler.Hint notEnoughMoney = new HintsHandler.Hint("lifecost_NotEnoughLife", 1);

            public static HintsHandler.Hint notEnoughLifeMoney = new HintsHandler.Hint("lifecost_NotEnoughLife", 1);
        }
    }
}
