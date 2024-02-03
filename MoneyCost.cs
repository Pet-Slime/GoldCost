using DiskCardGame;
using GBC;
using InscryptionAPI.Card;
using InscryptionAPI.CardCosts;
using LifeCost.Patchers;
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
        public override string CostName => "MoneyCost";

        public override bool CostSatisfied(int cardCost, PlayableCard card)
        {
            int num6 = card.Info.MoneyCost();
            bool flagScene = SceneLoader.ActiveSceneName.StartsWith("Part1");
            int currency2;
            if (flagScene)
            {
                currency2 = RunState.Run.currency;
            }
            else
            {
                currency2 = SaveData.Data.currency;
            }
            bool enoughMoney = num6 > currency2;
            if (enoughMoney)
            {
                return false;
            }
            return true;
        }

        // the dialogue that's played when you try to play a card with this cost, and CostSatisfied is false
        public override string CostUnsatisfiedHint(int cardCost, PlayableCard card)
        {
            return $"You do not have enough Foils to play {card.Info.DisplayedNameLocalized}.";
        }

        // this is called after a card with this cost resolves on the board
        // if your cost spends a resource, this is where you'd put that logic
        public override IEnumerator OnPlayed(int cardCost, PlayableCard card)
        {
            bool flag = SceneLoader.ActiveSceneName.StartsWith("Part1");
            if (flag)
            {
                yield return PayCost.extractCostPart1_MoneyOnly(cardCost, RunState.Run.currency);
            }
            else
            {
                yield return PayCost.extractCostPart2_MoneyOnly(cardCost);
            }
        }
    }
}
