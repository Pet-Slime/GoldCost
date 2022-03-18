using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using DiskCardGame;
using UnityEngine;
using InscryptionAPI.Card;
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

		public static AbilityInfo CreateAbilityWithDefaultSettings(
				string rulebookName, string rulebookDescription, Type behavior, Texture2D text_a1, byte[] text_a2,
				string LearnDialogue, bool withDialogue = false, int powerLevel = 0, bool leshyUsable = false, bool part1Modular = true, bool stack = false
		)
		{
			AbilityInfo createdAbilityInfo = AbilityManager.New(
				LifeCost.Plugin.PluginGuid,
				rulebookName,
				rulebookDescription,
				behavior,
				text_a1
			)
			;
			// This sets up the learned Dialog event
			if (withDialogue)
			{
				createdAbilityInfo.abilityLearnedDialogue = SetAbilityInfoDialogue(LearnDialogue);
			}
			// How powerful the ability is
			createdAbilityInfo.powerLevel = powerLevel;
			// Can it show up on totems for leshy?
			createdAbilityInfo.activated = leshyUsable;
			// If true, allows in shops and in totems. If false, just the rule book
			if (part1Modular)
			{
				createdAbilityInfo.metaCategories = new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook };
			}
			else
			{
				createdAbilityInfo.metaCategories = new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook };
			}
///			createdAbilityInfo.pixelIcon = LoadSpriteFromResource(text_a2);
			// Does the ability stack?
			createdAbilityInfo.canStack = stack;
			return createdAbilityInfo;
		}

		public static CardInfo CreateCardWithDefaultSettings(
			string InternalName, string DisplayName, int attack, int health, Texture2D texture_base, Texture2D texture_emission,
			List<CardMetaCategory> cardMetaCategories, List<Tribe> tribes, List<Trait> traits, List<Ability> abilities, Texture2D texture_pixel = null, int bloodCost = 0, int boneCost = 0, int energyCost = 0
		)
		{
			CardInfo cardinfo = CardManager.New(
			modPrefix: "lifecost",
			InternalName,
			DisplayName,
			attack,
			health,
			description: "A puddle that errods all that touches it."
			);
			cardinfo.SetPortrait(texture_base, texture_emission);
			if (texture_pixel != null)
			{
				cardinfo.SetPixelPortrait(texture_pixel);
			}
			cardinfo.metaCategories = cardMetaCategories;
			cardinfo.tribes = tribes;
			cardinfo.traits = traits;
			for (int index = 0; index < abilities.Count; index++)
			{
				cardinfo.AddAbilities(abilities[index]);
			}
			cardinfo.temple = CardTemple.Nature;
			cardinfo.cardComplexity = CardComplexity.Intermediate;
			cardinfo.cost = bloodCost;
			cardinfo.bonesCost = boneCost;
			cardinfo.energyCost = energyCost;
			return cardinfo;
		}


	}
}
