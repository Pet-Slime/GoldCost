using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using Art = LifeCost.Resources.Artwork;
using Part2R = RenderFixMaybe.Part2CostRender_Right;
using Part2L = RenderFixMaybe.Part2CostRender_Left;
using Part1 = RenderFixMaybe.Part1CostRender;

namespace LifeCost
{
    internal class RenderFix
    {

		[HarmonyPatch(typeof(RenderFixMaybe.Part1CostRender), nameof(RenderFixMaybe.Part1CostRender.Part1SpriteFinal))]
		public class lifecost_CostDisplayPatch_part1
		{
			[HarmonyPrefix]
			public static bool Prefix(ref Sprite __result, ref CardInfo card)
			{
				//Make the texture variables and set them to the default (which is 0)
				Texture2D texBloodCost = Part1.LoadTextureFromResource(Art.blood_cost_0);
				Texture2D texBoneCcost = Part1.LoadTextureFromResource(Art.bone_cost_0);
				Texture2D texEnergyCost = Part1.LoadTextureFromResource(Art.energy_cost_0);
				Texture2D texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_0);
				Texture2D textGemCost = Part1.LoadTextureFromResource(Art.mox_cost_empty);

				//A list to hold the textures (important later, to combine them all)
				List<Texture2D> list = new List<Texture2D>();

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
					Texture2D orange = Part1.LoadTextureFromResource(Art.mox_cost_e);
					Texture2D blue = Part1.LoadTextureFromResource(Art.mox_cost_e);
					Texture2D green = Part1.LoadTextureFromResource(Art.mox_cost_e);

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
				switch (energyCost)
				{
					case 1:
						texEnergyCost = Part1.LoadTextureFromResource(Art.energy_cost_1);
						list.Add(texEnergyCost);
						break;
					case 2:
						texEnergyCost = Part1.LoadTextureFromResource(Art.energy_cost_2);
						list.Add(texEnergyCost);
						break;
					case 3:
						texEnergyCost = Part1.LoadTextureFromResource(Art.energy_cost_3);
						list.Add(texEnergyCost);
						break;
					case 4:
						texEnergyCost = Part1.LoadTextureFromResource(Art.energy_cost_4);
						list.Add(texEnergyCost);
						break;
					case 5:
						texEnergyCost = Part1.LoadTextureFromResource(Art.energy_cost_5);
						list.Add(texEnergyCost);
						break;
					case 6:
						texEnergyCost = Part1.LoadTextureFromResource(Art.energy_cost_6);
						list.Add(texEnergyCost);
						break;
				}

				//Switch statement to set the bone texture to the right cost
				switch (boneCost)
				{
					case 1:
						texBoneCcost = Part1.LoadTextureFromResource(Art.bone_cost_1);
						list.Add(texBoneCcost);
						break;
					case 2:
						texBoneCcost = Part1.LoadTextureFromResource(Art.bone_cost_2);
						list.Add(texBoneCcost);
						break;
					case 3:
						texBoneCcost = Part1.LoadTextureFromResource(Art.bone_cost_3);
						list.Add(texBoneCcost);
						break;
					case 4:
						texBoneCcost = Part1.LoadTextureFromResource(Art.bone_cost_4);
						list.Add(texBoneCcost);
						break;
					case 5:
						texBoneCcost = Part1.LoadTextureFromResource(Art.bone_cost_5);
						list.Add(texBoneCcost);
						break;
					case 6:
						texBoneCcost = Part1.LoadTextureFromResource(Art.bone_cost_6);
						list.Add(texBoneCcost);
						break;
					case 7:
						texBoneCcost = Part1.LoadTextureFromResource(Art.bone_cost_7);
						list.Add(texBoneCcost);
						break;
					case 8:
						texBoneCcost = Part1.LoadTextureFromResource(Art.bone_cost_8);
						list.Add(texBoneCcost);
						break;
					case 9:
						texBoneCcost = Part1.LoadTextureFromResource(Art.bone_cost_9);
						list.Add(texBoneCcost);
						break;
					case 10:
						texBoneCcost = Part1.LoadTextureFromResource(Art.bone_cost_10);
						list.Add(texBoneCcost);
						break;
				}

				//Switch statement to set the bone texture to the right cost
				switch (lifeCost)
				{
					case 1:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_1);
						list.Add(texLifeCost);
						break;
					case 2:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_2);
						list.Add(texLifeCost);
						break;
					case 3:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_3);
						list.Add(texLifeCost);
						break;
					case 4:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_4);
						list.Add(texLifeCost);
						break;
					case 5:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_5);
						list.Add(texLifeCost);
						break;
					case 6:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_6);
						list.Add(texLifeCost);
						break;
					case 7:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_7);
						list.Add(texLifeCost);
						break;
					case 8:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_8);
						list.Add(texLifeCost);
						break;
					case 9:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_9);
						list.Add(texLifeCost);
						break;
					case 10:
						texBoneCcost = Part1.LoadTextureFromResource(Art.life_cost_10);
						list.Add(texLifeCost);
						break;
					case 11:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_11);
						list.Add(texLifeCost);
						break;
					case 12:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_12);
						list.Add(texLifeCost);
						break;
					case 13:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_13);
						list.Add(texLifeCost);
						break;
					case 14:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_14);
						list.Add(texBoneCcost);
						break;
					case 15:
						texLifeCost = Part1.LoadTextureFromResource(Art.life_cost_15);
						list.Add(texBoneCcost);
						break;
				}

				switch (bloodCost)
				{
					case 1:
						texBloodCost = Part1.LoadTextureFromResource(Art.blood_cost_1);
						list.Add(texBloodCost);
						break;
					case 2:
						texBloodCost = Part1.LoadTextureFromResource(Art.blood_cost_2);
						list.Add(texBloodCost);
						break;
					case 3:
						texBloodCost = Part1.LoadTextureFromResource(Art.blood_cost_3);
						list.Add(texBloodCost);
						break;
					case 4:
						texBloodCost = Part1.LoadTextureFromResource(Art.blood_cost_4);
						list.Add(texBloodCost);
						break;
					case 5:
						texBloodCost = Part1.LoadTextureFromResource(Art.blood_cost_5);
						list.Add(texBloodCost);
						break;
					case 6:
						texBloodCost = Part1.LoadTextureFromResource(Art.blood_cost_6);
						list.Add(texBloodCost);
						break;
					case 7:
						texBloodCost = Part1.LoadTextureFromResource(Art.blood_cost_7);
						list.Add(texBloodCost);
						break;
					case 8:
						texBloodCost = Part1.LoadTextureFromResource(Art.blood_cost_8);
						list.Add(texBloodCost);
						break;
					case 9:
						texBloodCost = Part1.LoadTextureFromResource(Art.blood_cost_9);
						list.Add(texBloodCost);
						break;
					case 10:
						texBloodCost = Part1.LoadTextureFromResource(Art.blood_cost_10);
						list.Add(texBloodCost);
						break;
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
				Texture2D finalTexture = Part1.CombineTextures(list, total, Art.empty_cost);

				//Convert the final texture to a sprite
				Sprite finalSprite = Part1.MakeSpriteFromTexture2D(finalTexture, true);
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
