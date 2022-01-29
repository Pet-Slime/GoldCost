using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using APIPlugin;

namespace LifeCost.cards
{
    public static class card_4
    {
		public static void AddCard()
		{

			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.GBCPlayable);
			metaCategories.Add(CardMetaCategory.GBCPack);

			List<Tribe> Tribes = new List<Tribe>();

			List<Ability> Abilities = new List<Ability>();

			List<Trait> Traits = new List<Trait>();

			List<AbilityIdentifier> customAbilities = new List<AbilityIdentifier>();
			customAbilities.Add(AbilityIdentifier.GetID("extraVoid.inscryption.voidSigils", "Opportunist"));

			List<SpecialTriggeredAbility> specialAbilities = new List<SpecialTriggeredAbility>();

			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

			Texture2D DefaultTexture = CardUtils.GetTextureFromPath("Artwork/place_holder.png");
			Texture2D pixelTexture = CardUtils.GetTextureFromPath("Artwork/pixelportrait_placeholder.png");

			Texture2D eTexture = CardUtils.GetTextureFromPath("Artwork/place_holder.png");

			IceCubeIdentifier iceCubeId = null;
			EvolveIdentifier evolveId = null;
			TailIdentifier tail = null;

			NewCard.Add(name: "lifescrybe_luckyPatreon",
				displayedName: "Lucky Patreon",
				baseAttack: 0,
				baseHealth: 2,
				metaCategories,
				cardComplexity: CardComplexity.Advanced,
				temple: CardTemple.Undead,
				description: "The lucky one",
				hideAttackAndHealth: false,
				bloodCost: 0,
				bonesCost: 0,
				energyCost: -1,
				gemsCost: null,
				specialStatIcon: SpecialStatIcon.None,
				Tribes,
				Traits,
				specialAbilities,
				Abilities,
				customAbilities,
				specialAbilitiesIdsParam: null,
				evolveParams: null,
				defaultEvolutionName: null,
				tailParams: null,
				iceCubeParams: null,
				flipPortraitForStrafe: false,
				onePerDeck: false,
				appearanceBehaviour,
				DefaultTexture,
				altTex: null,
				titleGraphic: null,
				pixelTexture,
				eTexture,
				animatedPortrait: null,
				decals: null,
				evolveId,
				iceCubeId,
				tail);

			
		}
	}
}
