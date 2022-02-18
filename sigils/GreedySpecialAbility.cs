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
    public class GreedySpecialAbility : SpecialCardBehaviour
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility;

        public static void addGreedySpecialAbility()
        {
            StatIconInfo iconInfo = new StatIconInfo();
            iconInfo.rulebookName = "Greedy (Special ability)";
            iconInfo.rulebookDescription = "A greedy creature with the Finace cost will only accept currency for thier pay cost.";
            iconInfo.iconType = SpecialStatIcon.None;
            iconInfo.iconGraphic = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_greedy);
            iconInfo.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook };

            SpecialAbilityIdentifier identifier = SpecialAbilityIdentifier.GetID(Plugin.PluginGuid, "GreedySpecialAbility");

            NewSpecialAbility newSpecialAbility = new NewSpecialAbility(typeof(GreedySpecialAbility), identifier, iconInfo);
            specialAbility = newSpecialAbility.specialTriggeredAbility;
        }
    }
}