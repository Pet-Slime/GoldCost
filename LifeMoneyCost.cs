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
            int num = card.Info.LifeMoneyCost();
            bool flag2 = SceneLoader.ActiveSceneName.StartsWith("Part1");
            bool flag3 = flag2;
            int currency;
            if (flag3)
            {
                currency = RunState.Run.currency;
            }
            else
            {
                currency = SaveData.Data.currency;
            }
            int num2 = Singleton<LifeManager>.Instance.Balance + 5;
            int num3 = currency + num2;
            bool flag4 = num > num3;
            if (flag4)
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
                int currentCurrency2 = RunState.Run.currency;
                yield return PayCost.extractCostPart1_hybrid(cardCost, currentCurrency2);
            }
            else
            {

                int currentCurrency3 = SaveData.Data.currency;
                yield return PayCost.extractCostPart2_hybrid(cardCost, currentCurrency3);
            }
        }
    }
}
