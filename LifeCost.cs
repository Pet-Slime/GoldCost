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
    internal class LifeCost : CustomCardCost
    {
        public override string CostName => "LifeCost";

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
            float waitTime = 0.1f;
            Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
            yield return new WaitForSeconds(waitTime);
            yield return Singleton<LifeManager>.Instance.ShowDamageSequence(cardCost, cardCost, true, 0.125f, null, 0f, false);
            yield return new WaitForSeconds(waitTime);
            Singleton<ViewManager>.Instance.SwitchToView(0, false, true);
            Singleton<ViewManager>.Instance.Controller.LockState = 0;
            yield break;
        }
    }
}
