using System;
using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace LifeCost.cards
{
    public static class CardUtils
    {
        public static DialogueEvent.LineSet SetAbilityInfoDialogue(string dialogue)
        {
            return new DialogueEvent.LineSet(new List<DialogueEvent.Line>
            {
                new DialogueEvent.Line
                {
                    text = dialogue
                }
            });
        }

        public static Texture2D LoadTextureFromResource(byte[] resourceFile)
        {
            Texture2D texture2D = new Texture2D(2, 2);
            ImageConversion.LoadImage(texture2D, resourceFile);
            texture2D.filterMode = 0;
            return texture2D;
        }

        public static AbilityInfo CreateAbilityWithDefaultSettings(string rulebookName, string rulebookDescription, Type behavior, Texture2D text_a1, Sprite text_a2, string LearnDialogue, bool withDialogue = false, int powerLevel = 0, bool leshyUsable = false, bool part1Modular = true, bool stack = false)
        {
            AbilityInfo abilityInfo = AbilityManager.New("extraVoid.inscryption.LifeCost", rulebookName, rulebookDescription, behavior, text_a1);
            if (withDialogue)
            {
                abilityInfo.abilityLearnedDialogue = CardUtils.SetAbilityInfoDialogue(LearnDialogue);
            }
            abilityInfo.powerLevel = powerLevel;
            abilityInfo.activated = true;
            if (part1Modular)
            {
                abilityInfo.metaCategories = new List<AbilityMetaCategory>
                {
                    AbilityMetaCategory.Part1Modular,
                    0
                };
            }
            else
            {
                abilityInfo.metaCategories = new List<AbilityMetaCategory>
                {
                    0
                };
            }
            abilityInfo.pixelIcon = text_a2;
            abilityInfo.canStack = stack;
            return abilityInfo;
        }

        public static Sprite LoadSpriteFromResource(byte[] resourceFile)
        {
            float num = 0.5f;
            float num2 = 0.5f;
            var vector = new Vector2(num, num2);
            Texture2D texture2D = new Texture2D(2, 2);
            ImageConversion.LoadImage(texture2D, resourceFile);
            texture2D.filterMode = 0;
            return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), vector, 100f);
        }

        public static CardInfo CreateCardWithDefaultSettings(string InternalName, string DisplayName, int attack, int health, Texture2D texture_base, Texture2D texture_emission, List<CardMetaCategory> cardMetaCategories, List<Tribe> tribes, List<Trait> traits, List<Ability> abilities, Texture2D texture_pixel = null, int bloodCost = 0, int boneCost = 0, int energyCost = 0)
        {
            CardInfo cardInfo = CardManager.New("lifecost", InternalName, DisplayName, attack, health, "A puddle that errods all that touches it.");
            CardExtensions.SetPortrait(cardInfo, texture_base, texture_emission, null);
            bool flag = texture_pixel != null;
            if (flag)
            {
                CardExtensions.SetPixelPortrait(cardInfo, texture_pixel, null);
            }
            cardInfo.metaCategories = cardMetaCategories;
            cardInfo.tribes = tribes;
            cardInfo.traits = traits;
            for (int i = 0; i < abilities.Count; i++)
            {
                CardExtensions.AddAbilities(cardInfo, new Ability[]
                {
                    abilities[i]
                });
            }
            cardInfo.temple = 0;
            cardInfo.cardComplexity = CardComplexity.Intermediate;
            cardInfo.SetBloodCost(bloodCost);
            cardInfo.SetBonesCost(boneCost);
            cardInfo.SetEnergyCost(energyCost);
            return cardInfo;
        }
    }
}
