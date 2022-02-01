using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using UnityEngine;
using Art = LifeCost.Resources.Artwork;

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


		public static Sprite LoadSpriteFromResource(byte[] resourceFile)
		{
			var texture = new Texture2D(2, 2);
			texture.LoadImage(resourceFile);
			texture.filterMode = FilterMode.Point;
			var sprite = UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), vector, pixelPerUnity);
			return sprite;
		}

		private static Sprite GetAct1Sprite(CardInfo card)
		{
			int lifeCost = card.LifeCostz();
			Sprite sprite = LoadSpriteFromResource(Art.cost_blank);
			switch (lifeCost)
			{
				case 1:
					sprite = LoadSpriteFromResource(Art.life_cost_1);
					break;
				case 2:
					sprite = LoadSpriteFromResource(Art.life_cost_2);
					break;
				case 3:
					sprite = LoadSpriteFromResource(Art.life_cost_3);
					break;
				case 4:
					sprite = LoadSpriteFromResource(Art.life_cost_4);
					break;
				case 5:
					sprite = LoadSpriteFromResource(Art.life_cost_5);
					break;
				case 6:
					sprite = LoadSpriteFromResource(Art.life_cost_6);
					break;
				case 7:
					sprite = LoadSpriteFromResource(Art.life_cost_7);
					break;
				case 8:
					sprite = LoadSpriteFromResource(Art.life_cost_8);
					break;
				case 9:
					sprite = LoadSpriteFromResource(Art.life_cost_9);
					break;
				case 10:
					sprite = LoadSpriteFromResource(Art.life_cost_10);
					break;
				case 11:
					sprite = LoadSpriteFromResource(Art.life_cost_11);
					break;
				case 12:
					sprite = LoadSpriteFromResource(Art.life_cost_12);
					break;
				case 13:
					sprite = LoadSpriteFromResource(Art.life_cost_13);
					break;
				case 14:
					sprite = LoadSpriteFromResource(Art.life_cost_14);
					break;
				case 15:
					sprite = LoadSpriteFromResource(Art.life_cost_15);
					break;
			}
			return sprite;
		}

		private static Sprite GetAct2Sprite(CardInfo card)
		{
			int lifeCost = card.LifeCostz();
			Sprite sprite = LoadSpriteFromResource(Art.cost_blank);
			switch (lifeCost)
			{
				case 1:
					sprite = LoadSpriteFromResource(Art.pixel_L_1);
					break;
				case 2:
					sprite = LoadSpriteFromResource(Art.pixel_L_2);
					break;
				case 3:
					sprite = LoadSpriteFromResource(Art.pixel_L_3);
					break;
				case 4:
					sprite = LoadSpriteFromResource(Art.pixel_L_4);
					break;
				case 5:
					sprite = LoadSpriteFromResource(Art.pixel_L_5);
					break;
				case 6:
					sprite = LoadSpriteFromResource(Art.pixel_L_6);
					break;
				case 7:
					sprite = LoadSpriteFromResource(Art.pixel_L_7);
					break;
				case 8:
					sprite = LoadSpriteFromResource(Art.pixel_L_8);
					break;
				case 9:
					sprite = LoadSpriteFromResource(Art.pixel_L_9);
					break;
				case 10:
					sprite = LoadSpriteFromResource(Art.pixel_L_10);
					break;
				case 11:
					sprite = LoadSpriteFromResource(Art.pixel_L_11);
					break;
				case 12:
					sprite = LoadSpriteFromResource(Art.pixel_L_12);
					break;
				case 13:
					sprite = LoadSpriteFromResource(Art.pixel_L_13);
					break;
				case 14:
					sprite = LoadSpriteFromResource(Art.pixel_L_14);
					break;
				case 15:
					sprite = LoadSpriteFromResource(Art.pixel_L_15);
					break;
			}
			return sprite;
		}

	}
}
