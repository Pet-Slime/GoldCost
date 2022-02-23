using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using UnityEngine;
using Art = LifeCost.Resources.Cost;

namespace LifeCost.Patchers.RenderFixes
{
    internal class RenderFix
    {
		[HarmonyPatch(typeof(CardDisplayer), nameof(CardDisplayer.GetCostSpriteForCard))]
		public class void_TeethPatch_CostRenderVanilla
		{
			[HarmonyPostfix]
			public static void Postfix(ref Sprite __result, ref CardInfo card)
			{
				if (!Plugin.RenderFixActive && card.LifeCostz() > 0)
				{
					//Make sure we are in Leshy's Cabin
					string flag = SceneLoader.ActiveSceneName;

					switch (flag)
					{
						///Leshy's Cabin main gameplay area
						case "Part1_Cabin":
							__result = GetAct1Sprite(card);
							break;
						///Leshy when making a death card
						case "Part1_Sanctum":
							__result = GetAct1Sprite(card);
							break;
						///Grimora in the final
						case "finale_grimora":
							break;
						///Magnificus in the final
						case "finale_magnificus":
							break;
						///I dont know what this scene is but I dont want to edit cards here
						case "finale_redacted":
							break;
						///All of P03's factory
						case "Part3_Cabin":
							break;
						///If it's none of the above areas, then it's somewhere in act 2
						default:
							__result = GetAct2Sprite(card);
						break;
					}
				}
			}
        }

		public static float Yposition = 0.5f;
		public static float Xposition = 0.5f;
		public static float pixelPerUnity = 100.0f;
		public static Vector2 vector = new Vector2(Xposition, Yposition);


		public static Sprite LoadSpriteFromResource(string resourceFile)
		{
			var texture = new Texture2D(2, 2);
			var test = (byte[])Art.ResourceManager.GetObject(resourceFile);
			if (test == null)
			{
				test = Art.cost_blank;
			}
			texture.LoadImage(test);
			texture.filterMode = FilterMode.Point;
			var sprite = UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), vector, pixelPerUnity);
			return sprite;
		}

		private static Sprite GetAct1Sprite(CardInfo card)
		{
			int lifeCost = card.LifeCostz();
			Sprite sprite = LoadSpriteFromResource("cost_blank");
			if (lifeCost > 0)
            {
				if (card.HasAbility(lifecost_vamperic.ability) || card.specialAbilities.Contains(VampericSpecialAbility.specialAbility))
				{
					sprite = LoadSpriteFromResource($"life_cost_{lifeCost}");
				}
				else if (card.HasAbility(lifecost_Greedy.ability) || card.specialAbilities.Contains(GreedySpecialAbility.specialAbility))
				{
					sprite = LoadSpriteFromResource($"life_cost_{lifeCost}");
				}
				else
				{
					sprite = LoadSpriteFromResource($"life_cost_{lifeCost}");
				}
			}
			return sprite;
		}

		private static Sprite GetAct2Sprite(CardInfo card)
		{
			int lifeCost = card.LifeCostz();
			Sprite sprite = LoadSpriteFromResource("cost_blank");
			if (lifeCost > 0)
			{
				if (card.HasAbility(lifecost_vamperic.ability) || card.specialAbilities.Contains(VampericSpecialAbility.specialAbility))
				{
					sprite = LoadSpriteFromResource($"pixel_B_{lifeCost}");
				}
				else if (card.HasAbility(lifecost_Greedy.ability) || card.specialAbilities.Contains(GreedySpecialAbility.specialAbility))
				{
					sprite = LoadSpriteFromResource($"pixel_M_{lifeCost}");
				}
				else
				{
					sprite = LoadSpriteFromResource($"pixel_L_{lifeCost}");
				}
			}
			return sprite;
		}

	}
}
