using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using InscryptionAPI.Card;

namespace LifeCost.cards
{
    public static class Teck
	{
		public static readonly Ability CustomAbility = InscryptionAPI.Guid.GuidManager.GetEnumValue<Ability>("extraVoid.inscryption.LifeCost", "Vamperic Strength");
		public static void AddCard()
		{
			string name = "lifecost_Teck";
			string displayName = "Teck";
			string description = "The Lost Beast, in the shape of a tooth, showing up only in error.";
			int baseAttack = 1;
			int baseHealth = 1;
			int bloodCost = 0;
			int boneCost = 0;
			int energyCost = 0;

			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

			List<Tribe> Tribes = new List<Tribe>();

			List<Ability> Abilities = new List<Ability>();
			Abilities.Add(Ability.ActivatedDrawSkeleton);

			List<Trait> Traits = new List<Trait>();

			Texture2D DefaultTexture = CardUtils.LoadTextureFromResource(LifeCost.Resources.Cards.teck);
			Texture2D pTexture = CardUtils.LoadTextureFromResource(LifeCost.Resources.Cards.pixel_teck);
			Texture2D eTexture = CardUtils.LoadTextureFromResource(LifeCost.Resources.Cards.teck_e);

			CardInfo newCard = CardUtils.CreateCardWithDefaultSettings(
				InternalName: name,
				DisplayName: displayName,
				attack: baseAttack,
				health: baseHealth,
				texture_base: DefaultTexture,
				texture_emission: eTexture,
				texture_pixel: pTexture,
				cardMetaCategories: metaCategories,
				tribes: Tribes,
				traits: Traits,
				abilities: Abilities,
				bloodCost: bloodCost,
				boneCost: boneCost,
				energyCost: energyCost
				);
			newCard.description = description;
			newCard.SetExtendedProperty("LifeCost", 2);
			CardManager.Add("lifecost", newCard);

		}
	}
}
