using System.Collections.Generic;
using System.Collections;
using GBC;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Art = LifeCost.Resources.Artwork;
using System.Linq;


namespace LifeCost
{
    public class VampericSpecialAbility : SpecialCardBehaviour
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility;

        public static void addVampericSpecialAbility()
        {
            StatIconInfo iconInfo = new StatIconInfo();
            iconInfo.rulebookName = "Vamperic (Special ability)";
            iconInfo.rulebookDescription = "A vamperic creature with the Finace cost will only accept life for thier pay cost.";
            iconInfo.iconType = SpecialStatIcon.None;
            iconInfo.iconGraphic = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_vamperic);
            iconInfo.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook };

            SpecialAbilityIdentifier identifier = SpecialAbilityIdentifier.GetID(Plugin.PluginGuid, "VampericSpecialAbility");

            NewSpecialAbility newSpecialAbility = new NewSpecialAbility(typeof(VampericSpecialAbility), identifier, iconInfo);
            specialAbility = newSpecialAbility.specialTriggeredAbility;
        }
    }
}