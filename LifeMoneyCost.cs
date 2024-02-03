using DiskCardGame;
using InscryptionAPI.CardCosts;
using System.Collections;
using LifeCost.Patchers;
using GBC;

namespace LifeCost
{
    internal class LifeMoneyCost : CustomCardCost
    {
        public override string CostName => "LifeMoneyCost";

        public override bool CostSatisfied(int cardCost, PlayableCard card)
        {

            return cardCost <= (Singleton<ResourcesManager>.Instance.PlayerEnergy - card.EnergyCost);
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

            yield return Singleton<ResourcesManager>.Instance.SpendEnergy(cardCost);
        }
    }
}
