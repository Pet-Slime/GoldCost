using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.CardCosts;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LifeCost
{
    internal class LifeMoneyCost : CustomCardCost
    {
        public override string CostName => "LifeMoneyCost";

        public override bool CostSatisfied(int cardCost, PlayableCard card)
        {
            // if the player has enough energy to pay the cost
            // takes the vanilla energy cost into account

            int PlayerLife = Singleton<LifeManager>.Instance.Balance + 5;
            int? Cost = card.Info.GetExtendedPropertyAsInt("LifeCost");
            return cardCost <= (PlayerLife - Cost);
        }

        // the dialogue that's played when you try to play a card with this cost, and CostSatisfied is false
        public override string CostUnsatisfiedHint(int cardCost, PlayableCard card)
        {
            return $"You do not have enouggh life to play {card.Info.DisplayedNameLocalized}.";
        }

        // this is called after a card with this cost resolves on the board
        // if your cost spends a resource, this is where you'd put that logic
        public override IEnumerator OnPlayed(int cardCost, PlayableCard card)
        {
            bool flag = SceneLoader.ActiveSceneName.StartsWith("Part1");
            if (flag)
            {
                yield return PayCost.extractCostPart1_lifeOnly(cardCost);
            }
            else
            {
                yield return PayCost.extractCostPart2_lifeOnly(cardCost);
            }
        }
    }
}
