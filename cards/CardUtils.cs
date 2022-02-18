using System.Collections.Generic;
using System.IO;
using APIPlugin;
using BepInEx;
using DiskCardGame;
using UnityEngine;
using static System.IO.File;

namespace LifeCost.cards
{
    public static class CardUtils
    {
		public static Texture2D GetTextureFromPath(string path)
		{
			byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, path));
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(imgBytes);
			return tex;
		}

		public static AbilityInfo CreateInfoWithDefaultSettings(
			string rulebookName, string rulebookDescription, string LearnDialogue, bool withDialogue = false, int powerLevel = 0, bool leshyUsable = false
		)
		{
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = powerLevel;
			info.rulebookName = rulebookName;
			info.rulebookDescription = rulebookDescription;
			info.metaCategories = new List<AbilityMetaCategory>()
			{
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
			};
			info.opponentUsable = leshyUsable;
			if (withDialogue)
			{
				info.abilityLearnedDialogue = SetAbilityInfoDialogue(LearnDialogue);
			}

			return info;
		}

		public static DialogueEvent.LineSet SetAbilityInfoDialogue(string dialogue)
		{
			return new DialogueEvent.LineSet(
				new List<DialogueEvent.Line>()
				{
					new DialogueEvent.Line()
					{
						text = dialogue
					}
				}
			);
		}

		public static Texture2D LoadTextureFromResource(byte[] resourceFile)
		{
			var texture = new Texture2D(2, 2);
			texture.LoadImage(resourceFile);
			texture.filterMode = FilterMode.Point;
			return texture;
		}

		public static Sprite LoadSpriteFromResource(byte[] resourceFile)
		{
			var Yposition = 0.5f;
			var Xposition = 0.5f;
			var vector = new Vector2(Xposition, Yposition);

			var texture = new Texture2D(2, 2);
			texture.LoadImage(resourceFile);
			texture.filterMode = FilterMode.Point;
			var sprite = UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), vector, 100.0f);
			return sprite;
		}


		public static AbilityIdentifier GetAbilityId(string rulebookName)
		{

#pragma warning disable CS0618 // Type or member is obsolete
            return AbilityIdentifier.GetAbilityIdentifier(LifeCost.Plugin.PluginGuid, rulebookName);
#pragma warning restore CS0618 // Type or member is obsolete

        }
	}
}
