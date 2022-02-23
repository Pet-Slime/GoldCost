using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using LifeCost;
using System.Reflection;
using Art = LifeCostRenderFixPatcher.Resources.Artwork;
using Part2R = RenderFixMaybe.Part2CostRender_Right;
using Part2L = RenderFixMaybe.Part2CostRender_Left;
using Part1 = RenderFixMaybe.Part1CostRender;

namespace LifeCostRenderFixPatcher
{
    internal class RenderFix
    {

		public static float Yposition = 0.85f;
		public static float Xposition = 0.4f;
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

		public static Sprite MakeSpriteFromTexture2D(Texture2D texture, bool flag)
		{
			Sprite sprite;
			float Ytest = 0.85f;
			float Xtest = 0.4f;
			float ppu = 100.0f;
			Vector2 vec = new Vector2(Xtest, Ytest);

			if (flag)
			{
				Ytest = 0.85f;
				Xtest = 0.4f;
				vec = new Vector2(Xtest, Ytest);
				sprite = UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), vec, ppu);
			}
			else
			{
				Ytest = 0.0f;
				Xtest = 1.0f;
				vec = new Vector2(Xtest, Ytest);
				sprite = UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), vec, ppu);
			}
			return sprite;
		}

		public static Texture2D LoadTextureFromResource(string resourceFile)
		{

			var texture = new Texture2D(2, 2);
			var test = (byte[])Art.ResourceManager.GetObject(resourceFile);
			if (test == null)
            {
				test = Art.blood_cost_1;
			}
			texture.LoadImage(test);
			texture.filterMode = FilterMode.Point;
			return texture;
		}



		public static Texture2D CombineTextures(List<Texture2D> abilities, List<Vector2Int> patchlocations, string resource)
		{
			bool flag = abilities != null;
			Texture2D result;
			if (flag)
			{
				Texture2D texture2D2 = LoadTextureFromResource(resource);
				for (int j = 0; j < abilities.Count; j++)
				{
					int index = j;
					texture2D2.SetPixels(patchlocations[index].x, patchlocations[index].y, abilities[index].width, abilities[index].height, abilities[index].GetPixels(), 0);
				}
				texture2D2.Apply();
				result = texture2D2;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static Texture2D CombineMoxTextures(List<Texture2D> abilities, List<Vector2Int> patchlocations)
		{
			bool flag = abilities != null;
			Texture2D result;
			if (flag)
			{
				Texture2D texture2D2 = LoadTextureFromResource("mox_cost_empty");
				for (int j = 0; j < abilities.Count; j++)
				{
					int index = j;
					texture2D2.SetPixels(patchlocations[index].x, patchlocations[index].y, abilities[index].width, abilities[index].height, abilities[index].GetPixels(), 0);
				}
				texture2D2.Apply();
				result = texture2D2;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static List<Vector2Int> fourCost = new List<Vector2Int>
		{
			new Vector2Int(0, 0),
			new Vector2Int(0, 28),
			new Vector2Int(0, 56),
			new Vector2Int(0, 84)
		};

		public static List<Vector2Int> threeCost = new List<Vector2Int>
		{
			new Vector2Int(0, 28),
			new Vector2Int(0, 56),
			new Vector2Int(0, 84)
		};

		public static List<Vector2Int> twoCost = new List<Vector2Int>
		{
			new Vector2Int(0, 56),
			new Vector2Int(0, 84)
		};

		public static List<Vector2Int> oneCost = new List<Vector2Int>
		{
			new Vector2Int(0, 84)
		};


		[HarmonyPatch(typeof(RenderFixMaybe.Part1CostRender), nameof(RenderFixMaybe.Part1CostRender.Part1SpriteFinal))]
		public class lifecost_CostDisplayPatch_part1
		{
			[HarmonyPrefix]
			public static bool Prefix(ref Sprite __result, ref CardInfo card)
			{
				//Make the texture variables and set them to the default (which is 0)
				Texture2D texBloodCost = LoadTextureFromResource("blood_cost_0");
				Texture2D texBoneCcost = LoadTextureFromResource("bone_cost_0");
				Texture2D texEnergyCost = LoadTextureFromResource("bone_cost_0");
				Texture2D texLifeCost = LoadTextureFromResource("life_cost_0");
				Texture2D textGemCost = LoadTextureFromResource("mox_cost_empty");

				//A list to hold the textures (important later, to combine them all)
				List <Texture2D> list = new List<Texture2D>();

				//Get the costs of blood, bone, and energy
				int bloodCost = card.BloodCost;
				int boneCost = card.BonesCost;
				int energyCost = card.energyCost;
				int lifeCost = card.LifeCostz();
				int moxCost = card.gemsCost.Count;

				//Setting mox first
				if (moxCost > 0)
				{
					//make a new list for the mox textures
					List<Texture2D> gemCost = new List<Texture2D>();
					//load up the mox textures as "empty"
					Texture2D orange = LoadTextureFromResource("mox_cost_e");
					Texture2D blue = LoadTextureFromResource("mox_cost_e");
					Texture2D green = LoadTextureFromResource("mox_cost_e");

					List<Vector2Int> moxVector = new List<Vector2Int>
					{
						new Vector2Int(0, 0),
						new Vector2Int(21, 0),
						new Vector2Int(42, 0)
					};

					//If a card has a green mox, set the green mox
					if (card.GemsCost.Contains(GemType.Green))
					{
						green = Part1.LoadTextureFromResource(Art.mox_cost_g);
					}
					//If a card has a green mox, set the Orange mox
					if (card.GemsCost.Contains(GemType.Orange))
					{
						orange = Part1.LoadTextureFromResource(Art.mox_cost_o);
					}
					//If a card has a green mox, set the Blue mox
					if (card.GemsCost.Contains(GemType.Blue))
					{
						blue = Part1.LoadTextureFromResource(Art.mox_cost_b);
					}
					//Add all moxes to the gemcost list
					gemCost.Add(orange);
					gemCost.Add(green);
					gemCost.Add(blue);
					//Combine the textures into one
					Texture2D finalMoxTexture = Part1.CombineMoxTextures(gemCost, moxVector);
					list.Add(finalMoxTexture);
				}

				//Switch Statement to set energy texture to the right cost, and add it to the list if it exists
				if (energyCost > 0)
                {
					texEnergyCost = LoadTextureFromResource($"energy_cost_{energyCost}");
					list.Add(texEnergyCost);
				}

				//Switch statement to set the bone texture to the right cost
				if (boneCost > 0)
				{
					texBoneCcost = LoadTextureFromResource($"bone_cost_{boneCost}");
					list.Add(texBoneCcost);
				}

				//Switch statement to set the bone texture to the right cost
				if (lifeCost > 0)
				{
					texLifeCost = LoadTextureFromResource($"life_cost_{lifeCost}");
					list.Add(texLifeCost);
				}

				if (bloodCost > 0)
				{
					texBloodCost = LoadTextureFromResource($"blood_cost_{bloodCost}");
					list.Add(texBloodCost);
				}


				//Make sure to use the right vector for the amount of items.
				//So count the list and use a switch statement to pick the right one.
				//If it is 0, just add them all to the list.
				var counting = list.Count;
				var total = new List<Vector2Int>();
				switch (counting)
				{
					case 0:
						list.Add(textGemCost);
						list.Add(texEnergyCost);
						list.Add(texBoneCcost);
						list.Add(texBloodCost);
						total = Part1.fourCost;
						break;
					case 1:
						total = Part1.oneCost;
						break;
					case 2:
						total = Part1.twoCost;
						break;
					case 3:
						total = Part1.threeCost;
						break;
					case 4:
						total = Part1.fourCost;
						break;
				}

				//Combine all the textures from the list into one texture
				Texture2D finalTexture = CombineTextures(list, total, "empty_cost");

				//Convert the final texture to a sprite
				Sprite finalSprite = MakeSpriteFromTexture2D(finalTexture, true);
				__result = finalSprite;

				return false;
			}
		}



		[HarmonyPatch(typeof(RenderFixMaybe.Part2CostRender_Right), nameof(Part2R.Part2SpriteFinal))]
		public class lifecost_CostDisplayPatch_pixel_right
		{
			[HarmonyPrefix]
			public static bool Prefix(ref Sprite __result, ref CardInfo card)
			{
				__result = Part2RenderFix_right.Part2SpriteFinal(card);
				return false;
			}
		}

		[HarmonyPatch(typeof(RenderFixMaybe.Part2CostRender_Left), nameof(Part2L.Part2SpriteFinal))]
		public class lifecost_CostDisplayPatch_pixel_left
		{
			[HarmonyPrefix]
			public static bool Prefix(ref Sprite __result, ref CardInfo card)
			{
				__result = Part2RenderFix_right.Part2SpriteFinal(card);
				return false;
			}
		}

	}
}
