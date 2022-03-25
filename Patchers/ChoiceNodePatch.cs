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

		public static ResourceType ResourceTypeMarker = (ResourceType)42;


		private static System.Random rng = new System.Random();

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}



		public static Texture2D LoadTextureFromResource(byte[] resourceFile)
		{
			var texture = new Texture2D(2, 2);
			texture.LoadImage(resourceFile);
			texture.filterMode = FilterMode.Point;
			return texture;
		}


		[HarmonyPatch(typeof(CardSingleChoicesSequencer), "GetCardbackTexture")]
		public class void_TeethPatch_textureBack
		{
			[HarmonyPostfix]
			public static void Postfix(ref Texture __result, CardChoice choice)
			{
				if (choice.resourceType == ResourceTypeMarker)
				{
					__result = TextureHelper.GetImageAsTexture("CostChoiceBack.png", typeof(ChoiceNodePatch).Assembly);
				}
			}
		}

		[HarmonyPatch(typeof(Part1CardChoiceGenerator), "GenerateCostChoices")]
		public class void_TeethPatch_choiceGenerator
		{
			[HarmonyPostfix]
			public static void Postfix(ref List<CardChoice> __result, int randomSeed)
			{
				var list = __result;
				CardChoice cardChoice = new CardChoice();
				cardChoice.resourceType = ResourceTypeMarker;
				list.Add(cardChoice);
				list.Shuffle();
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
			[HarmonyPostfix, HarmonyPatch(nameof(CardSingleChoicesSequencer.CostChoiceChosen))]
			public static IEnumerator Postfix(
			IEnumerator enumerator,
			CardSingleChoicesSequencer __instance,
			SelectableCard card
			)
			{
				if (card.ChoiceInfo.resourceType == ResourceTypeMarker)
				{
					CardInfo cardInfo = GetRandomChoosableLifeCard(SaveManager.SaveFile.GetCurrentRandomSeed());


					card.SetInfo(cardInfo);
					card.SetFaceDown(false, false);
					card.SetInteractionEnabled(false);
					yield return __instance.TutorialTextSequence(card);		
					card.SetCardbackToDefault();
					yield return __instance.WaitForCardToBeTaken(card);
					yield break;
				} else
                {
					yield return enumerator;
				}
				
			}
		}

		public static CardInfo GetRandomChoosableLifeCard(int randomSeed)
		{
			List<CardInfo> list = CardLoader.GetUnlockedCards(CardMetaCategory.ChoiceNode, CardTemple.Nature).FindAll((CardInfo x) => x.LifeMoneyCost() > 0);
			list.AddRange(CardLoader.GetUnlockedCards(CardMetaCategory.ChoiceNode, CardTemple.Nature).FindAll((CardInfo x) => x.LifeCost() > 0));
			list.AddRange(CardLoader.GetUnlockedCards(CardMetaCategory.ChoiceNode, CardTemple.Nature).FindAll((CardInfo x) => x.MoneyCost() > 0));
			bool flag = list.Count == 0;
			CardInfo result;
			if (flag)
			{
				result = CardLoader.Clone(CardLoader.GetCardByName("lifecost_Teck"));
			}
			else
			{
				result = CardLoader.Clone(list[SeededRandom.Range(0, list.Count, randomSeed)]);
			}
			return result;
		}


	}
}
