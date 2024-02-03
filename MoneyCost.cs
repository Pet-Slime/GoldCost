using DiskCardGame;
using GBC;
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
    internal class MoneyCost : CustomCardCost
    {
        public override string CostName => "LifeCost";

        public override bool CostSatisfied(int cardCost, PlayableCard card)
        {
            // if the player has enough energy to pay the cost
            // takes the vanilla energy cost into account
            int num6 = card.Info.MoneyCost();
            bool flag8 = SceneLoader.ActiveSceneName.StartsWith("Part1");
            bool flag9 = flag8;
            int currency2;
            if (flag9)
            {
                currency2 = RunState.Run.currency;
            }
            else
            {
                currency2 = SaveData.Data.currency;
            }
            bool flag10 = num6 > currency2;
            if (flag10)
            {
                return false;
            }
            return true;
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
