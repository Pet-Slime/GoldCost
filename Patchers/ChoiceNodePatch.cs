using System.Collections;
using System.Collections.Generic;
using InscryptionAPI.Helpers;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;

namespace LifeCost.Patchers
{
    internal static class ChoiceNodePatch
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int i = list.Count;
            while (i > 1)
            {
                i--;
                int index = ChoiceNodePatch.rng.Next(i + 1);
                T value = list[index];
                list[index] = list[i];
                list[i] = value;
            }
        }

        public static Texture2D LoadTextureFromResource(byte[] resourceFile)
        {
            Texture2D texture2D = new Texture2D(2, 2);
            ImageConversion.LoadImage(texture2D, resourceFile);
            texture2D.filterMode = 0;
            return texture2D;
        }

        public static CardInfo GetRandomChoosableLifeCard(int randomSeed)
        {
            List<CardInfo> list = CardLoader.GetUnlockedCards(0, 0).FindAll((CardInfo x) => x.LifeMoneyCost() > 0);
            list.AddRange(CardLoader.GetUnlockedCards(0, 0).FindAll((CardInfo x) => x.LifeCost() > 0));
            list.AddRange(CardLoader.GetUnlockedCards(0, 0).FindAll((CardInfo x) => x.MoneyCost() > 0));
            bool flag = list.Count == 0;
            bool flag2 = flag;
            CardInfo result;
            if (flag2)
            {
                result = CardLoader.Clone(CardLoader.GetCardByName("lifecost_Teck"));
            }
            else
            {
                result = CardLoader.Clone(list[SeededRandom.Range(0, list.Count, randomSeed)]);
            }
            return result;
        }

        public static ResourceType ResourceTypeMarker = (ResourceType)42;

        private static System.Random rng = new System.Random();

        [HarmonyPatch(typeof(CardSingleChoicesSequencer), "GetCardbackTexture")]
        public class void_TeethPatch_textureBack
        {
            [HarmonyPostfix]
            public static void Postfix(ref Texture __result, CardChoice choice)
            {
                bool flag = choice.resourceType == ChoiceNodePatch.ResourceTypeMarker;
                if (flag)
                {
                    __result = TextureHelper.GetImageAsTexture("CostChoiceBack.png", typeof(ChoiceNodePatch).Assembly, 0);
                }
            }
        }

        [HarmonyPatch(typeof(Part1CardChoiceGenerator), "GenerateCostChoices")]
        public class void_TeethPatch_choiceGenerator
        {
            [HarmonyPostfix]
            public static void Postfix(ref List<CardChoice> __result, int randomSeed)
            {
                List<CardChoice> list = __result;
                list.Add(new CardChoice
                {
                    resourceType = ChoiceNodePatch.ResourceTypeMarker
                });
                list.Shuffle<CardChoice>();
                while (list.Count > 3)
                {
                    list.RemoveAt(SeededRandom.Range(0, list.Count, randomSeed++));
                }
                __result = list;
            }
        }

        [HarmonyPatch(typeof(CardSingleChoicesSequencer))]
        public class void_TeethPatch_CostChoiceSequencer
        {
            [HarmonyPostfix]
            [HarmonyPatch("CostChoiceChosen")]
            public static IEnumerator Postfix(IEnumerator enumerator, CardSingleChoicesSequencer __instance, SelectableCard card)
            {
                bool flag = card.ChoiceInfo.resourceType == ChoiceNodePatch.ResourceTypeMarker;
                if (flag)
                {
                    CardInfo cardInfo = ChoiceNodePatch.GetRandomChoosableLifeCard(SaveManager.SaveFile.GetCurrentRandomSeed());
                    card.SetInfo(cardInfo);
                    card.SetFaceDown(false, false);
                    card.SetInteractionEnabled(false);
                    yield return __instance.TutorialTextSequence(card);
                    card.SetCardbackToDefault();
                    yield return __instance.WaitForCardToBeTaken(card);
                    yield break;
                }
                yield return enumerator;
                yield break;
            }
        }
    }
}
