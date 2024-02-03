using DiskCardGame;
using InscryptionAPI.CardCosts;
using System.Collections;
using LifeCost.Patchers;

namespace LifeCost
{
    internal class LifeCost : CustomCardCost
    {
        public override string CostName => "LifeCost";

        public override bool CostSatisfied(int cardCost, PlayableCard card)
        {
            int costLife = card.Info.LifeCost();
            int balanceLife = Singleton<LifeManager>.Instance.Balance + 5;
            if (costLife > balanceLife)
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
