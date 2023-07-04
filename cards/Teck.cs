using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using InscryptionAPI.Card;
using LifeCost.Resources;
using LifeCost.sigils;

namespace LifeCost.cards
{
    public static class Teck
    {
        public static void AddCard()
        {
            string text = "lifecost_Teck";
            string text2 = "Teck";
            string description = "The Lost Beast, in the shape of a tooth, showing up only in error.";
            int num = 1;
            int num2 = 1;
            int bloodCost = 0;
            int boneCost = 0;
            int energyCost = 0;
            List<CardMetaCategory> cardMetaCategories = new List<CardMetaCategory>();
            List<Tribe> tribes = new List<Tribe>();
            List<Ability> list = new List<Ability>();
            list.Add(lifecost_ActivateStatsUpLife.ability);
            List<Trait> traits = new List<Trait>();
            Texture2D texture2D = CardUtils.LoadTextureFromResource(Cards.teck);
            Texture2D texture2D2 = CardUtils.LoadTextureFromResource(Cards.pixel_teck);
            Texture2D texture2D3 = CardUtils.LoadTextureFromResource(Cards.teck_e);
            string internalName = text;
            string displayName = text2;
            int attack = num;
            int health = num2;
            Texture2D texture_base = texture2D;
            Texture2D texture_emission = texture2D3;
            Texture2D texture_pixel = texture2D2;
            CardInfo cardInfo = CardUtils.CreateCardWithDefaultSettings(internalName, displayName, attack, health, texture_base, texture_emission, cardMetaCategories, tribes, traits, list, texture_pixel, bloodCost, boneCost, energyCost);
            cardInfo.description = description;
            CardExtensions.SetExtendedProperty(cardInfo, "LifeMoneyCost", 2);
            CardManager.Add("lifecost", cardInfo);
        }
    }
}
