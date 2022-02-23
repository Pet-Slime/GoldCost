using DiskCardGame;
using UnityEngine;
using LifeCost;
using System.Collections.Generic;
using Art = LifeCostRenderFixPatcher.Resources.Artwork;

namespace LifeCostRenderFixPatcher
{
    internal class Part2RenderFix_left
	{
		public static float Yposition = 0.85f;
		public static float Xposition = 0.4f;
		public static float pixelPerUnity = 100.0f;
		public static Vector2 vector = new Vector2(Xposition, Yposition);



		public static Sprite MakeSpriteFromTexture2D(Texture2D texture)
		{
			Sprite sprite;
			float Ytest = 0.75f;
			float Xtest = 0.85f;
			float ppu = 100.0f;
			Vector2 vec = new Vector2(Xtest, Ytest);
			sprite = UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), vec, ppu);
			return sprite;
		}

		public static Texture2D LoadTextureFromResource(byte[] resourceFile)
		{
			var texture = new Texture2D(2, 2);
			texture.LoadImage(resourceFile);
			texture.filterMode = FilterMode.Point;
			return texture;
		}



		public static Texture2D CombineTextures(List<Texture2D> abilities, List<Vector2Int> patchlocations, byte[] resource)
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

		public static Texture2D GetFinalTexture(int cardCost, byte[] resource)
		{
			List<Vector2Int> pixelCost = new List<Vector2Int>
			{
				new Vector2Int(1, 0),
				new Vector2Int(8, 0)
			};
			Texture2D textCost = LoadTextureFromResource(Art.pixel_blank);
			List<Texture2D> list = new List<Texture2D>();
			Texture2D artCost = LoadTextureFromResource(resource);
			list.Add(artCost);
			switch (cardCost)
			{
				case 1:
					textCost = LoadTextureFromResource(Art.pixel_mox_empty);
					list.Add(textCost);
					break;
				case 2:
					textCost = LoadTextureFromResource(Art.pixel_L_2);
					list.Add(textCost);
					break;
				case 3:
					textCost = LoadTextureFromResource(Art.pixel_L_3);
					list.Add(textCost);
					break;
				case 4:
					textCost = LoadTextureFromResource(Art.pixel_L_4);
					list.Add(textCost);
					break;
				case 5:
					textCost = LoadTextureFromResource(Art.pixel_L_5);
					list.Add(textCost);
					break;
				case 6:
					textCost = LoadTextureFromResource(Art.pixel_L_6);
					list.Add(textCost);
					break;
				case 7:
					textCost = LoadTextureFromResource(Art.pixel_L_7);
					list.Add(textCost);
					break;
				case 8:
					textCost = LoadTextureFromResource(Art.pixel_L_8);
					list.Add(textCost);
					break;
				case 9:
					textCost = LoadTextureFromResource(Art.pixel_L_9);
					list.Add(textCost);
					break;
				case 10:
					textCost = LoadTextureFromResource(Art.pixel_L_10);
					list.Add(textCost);
					break;
				case 11:
					textCost = LoadTextureFromResource(Art.pixel_L_11);
					list.Add(textCost);
					break;
				case 12:
					textCost = LoadTextureFromResource(Art.pixel_L_12);
					list.Add(textCost);
					break;
				case 13:
					textCost = LoadTextureFromResource(Art.pixel_L_13);
					list.Add(textCost);
					break;
				case 14:
					textCost = LoadTextureFromResource(Art.pixel_L_14);
					list.Add(textCost);
					break;
				case 15:
					textCost = LoadTextureFromResource(Art.pixel_L_15);
					list.Add(textCost);
					break;
			}
			Texture2D finalBloodTexture = CombineTextures(list, pixelCost, Art.pixel_blank);
			return finalBloodTexture;
		}



		public static Sprite Part2SpriteFinal(CardInfo card)
		{
			;

			//A list to hold the textures (important later, to combine them all)
			List<Texture2D> masterList = new List<Texture2D>();

			int bloodCost = card.BloodCost;
			int boneCost = card.BonesCost;
			int energyCost = card.energyCost;
			int lifeCost = card.LifeCostz();
			int moxCost = card.gemsCost.Count;

			if (bloodCost > 0)
			{
				Texture2D texBloodCost = GetFinalTexture(bloodCost, Art.pixel_blood);
				masterList.Add(texBloodCost);

			}
			if (boneCost > 0)
			{
				Texture2D texBoneCcost = GetFinalTexture(boneCost, Art.pixel_bone);
				masterList.Add(texBoneCcost);


			}
			if (lifeCost > 0)
			{

				if (card.HasAbility(lifecost_vamperic.ability) || card.specialAbilities.Contains(VampericSpecialAbility.specialAbility))
				{
					Texture2D texLifeCost = GetFinalTexture(lifeCost, Art.pixel_pure_life);
					masterList.Add(texLifeCost);
				}
				else if (card.HasAbility(lifecost_Greedy.ability) || card.specialAbilities.Contains(GreedySpecialAbility.specialAbility))
				{
					Texture2D texLifeCost = GetFinalTexture(lifeCost, Art.pixel_money);
					masterList.Add(texLifeCost);
				}
				else
				{
					Texture2D texLifeCost = GetFinalTexture(lifeCost, Art.pixel_life);
					masterList.Add(texLifeCost);
				}

			}
			if (energyCost > 0)
			{
				Texture2D texEnergyCost = GetFinalTexture(energyCost, Art.pixel_energy);
				masterList.Add(texEnergyCost);

			}
			if (moxCost > 0)
			{
				List<Texture2D> gemCost = new List<Texture2D>();

				//If a card has a green mox, set the green mox
				if (card.GemsCost.Contains(GemType.Green))
				{
					Texture2D green = LoadTextureFromResource(Art.pixel_mox_green);
					gemCost.Add(green);
				}
				//If a card has a green mox, set the Orange mox
				if (card.GemsCost.Contains(GemType.Orange))
				{
					Texture2D orange = LoadTextureFromResource(Art.pixel_mox_red);
					gemCost.Add(orange);
				}
				//If a card has a green mox, set the Blue mox
				if (card.GemsCost.Contains(GemType.Blue))
				{
					Texture2D blue = LoadTextureFromResource(Art.pixel_mox_blue);
					gemCost.Add(blue);
				}
				var gemCounting = gemCost.Count;
				var gemTotal = new List<Vector2Int>();
				switch (gemCounting)
				{
					case 1:
						gemTotal = new List<Vector2Int>
						{
							new Vector2Int(1, 0)
						};
						break;
					case 2:
						gemTotal = new List<Vector2Int>
						{
							new Vector2Int(1, 0),
							new Vector2Int(9, 0)
						};
						break;
					case 3:
						gemTotal = new List<Vector2Int>
						{
							new Vector2Int(1, 0),
							new Vector2Int(9, 0),
							new Vector2Int(17, 0)
						};
						break;
				}

				Texture2D finalMoxTexture = CombineTextures(gemCost, gemTotal, Art.pixel_blank);

				masterList.Add(finalMoxTexture);
			}

			var counting = masterList.Count;
			var total = new List<Vector2Int>();
			switch (counting)
			{
				case 0:
					Texture2D blankCost = LoadTextureFromResource(Art.pixel_blank);
					masterList.Add(blankCost);
					total = new List<Vector2Int>
					{
						new Vector2Int(0, 0)
					};
					break;
				case 1:
					total = new List<Vector2Int>
					{
						new Vector2Int(0, 24)
					};
					break;
				case 2:
					total = new List<Vector2Int>
					{
						new Vector2Int(0, 16),
						new Vector2Int(0, 24)
					};
					break;
				case 3:
					total = new List<Vector2Int>
					{
						new Vector2Int(0, 8),
						new Vector2Int(0, 16),
						new Vector2Int(0, 24)
					};
					break;
				case 4:
					total = new List<Vector2Int>
					{
						new Vector2Int(0, 0),
						new Vector2Int(0, 8),
						new Vector2Int(0, 16),
						new Vector2Int(0, 24)
					};
					break;
			}

			//Combine all the textures from the list into one texture
			Texture2D finalTexture = CombineTextures(masterList, total, Art.pixel_base);

			//Convert the final texture to a sprite
			Sprite finalSprite = MakeSpriteFromTexture2D(finalTexture);
			return finalSprite;
		}
	}
}
