using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using InscryptionCommunityPatch.Card;
using InscryptionAPI.Helpers;

namespace LifeCost.Patchers.RenderFixes
{
    internal class RenderFix
    {
        public static void CommunityPatchHook()
        {
            Part1CardCostRender.UpdateCardCost += delegate (CardInfo card, List<Texture2D> costs)
            {
                int myCustomCost1 = card.LifeCost();
                if (myCustomCost1 > 0)
                {
                    costs.Add(TextureHelper.GetImageAsTexture($"life_pure_cost_{myCustomCost1}.png", typeof(RenderFix).Assembly));
                }
                int myCustomCost2 = card.LifeMoneyCost();
                if (myCustomCost2 > 0)
                {
                    costs.Add(TextureHelper.GetImageAsTexture($"life_cost_{myCustomCost2}.png", typeof(RenderFix).Assembly));
                }
                int myCustomCost3 = card.MoneyCost();
                if (myCustomCost3 > 0)
                {
                    costs.Add(TextureHelper.GetImageAsTexture($"money_cost_{myCustomCost3}.png", typeof(RenderFix).Assembly));
                }

            };

            Part2CardCostRender.UpdateCardCost += delegate (CardInfo card, List<Texture2D> costs)
            {
                int myCustomCost1 = card.LifeCost();
                if (myCustomCost1 > 0)
                {
                    Texture2D costTexture = Part2CardCostRender.CombineIconAndCount(myCustomCost1, TextureHelper.GetImageAsTexture("pixel_pure_life.png", typeof(RenderFix).Assembly));
                    costs.Add(costTexture);
                }

                int myCustomCost2 = card.LifeMoneyCost();
                if (myCustomCost2 > 0)
                {
                    Texture2D costTexture = Part2CardCostRender.CombineIconAndCount(myCustomCost2, TextureHelper.GetImageAsTexture("pixel_life.png", typeof(RenderFix).Assembly));
                    costs.Add(costTexture);
                }

                int myCustomCost3 = card.MoneyCost();
                if (myCustomCost3 > 0)
                {
                    Texture2D costTexture = Part2CardCostRender.CombineIconAndCount(myCustomCost3, TextureHelper.GetImageAsTexture("pixel_money.png", typeof(RenderFix).Assembly));
                    costs.Add(costTexture);
                }
            };
        }
    }
}
