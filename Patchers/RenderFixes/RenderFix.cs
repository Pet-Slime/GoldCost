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
                int num = card.LifeCost();
                bool flag = num > 0;
                if (flag)
                {
                    costs.Add(TextureHelper.GetImageAsTexture(string.Format("life_pure_cost_{0}.png", num), typeof(RenderFix).Assembly, 0));
                }
                int num2 = card.LifeMoneyCost();
                bool flag2 = num2 > 0;
                if (flag2)
                {
                    costs.Add(TextureHelper.GetImageAsTexture(string.Format("life_cost_{0}.png", num2), typeof(RenderFix).Assembly, 0));
                }
                int num3 = card.MoneyCost();
                bool flag3 = num3 > 0;
                if (flag3)
                {
                    costs.Add(TextureHelper.GetImageAsTexture(string.Format("money_cost_{0}.png", num3), typeof(RenderFix).Assembly, 0));
                }
            };
            Part2CardCostRender.UpdateCardCost += delegate (CardInfo card, List<Texture2D> costs)
            {
                int num = card.LifeCost();
                bool flag = num > 0;
                if (flag)
                {
                    Texture2D item = Part2CardCostRender.CombineIconAndCount(num, TextureHelper.GetImageAsTexture("pixel_pure_life.png", typeof(RenderFix).Assembly, 0));
                    costs.Add(item);
                }
                int num2 = card.LifeMoneyCost();
                bool flag2 = num2 > 0;
                if (flag2)
                {
                    Texture2D item2 = Part2CardCostRender.CombineIconAndCount(num2, TextureHelper.GetImageAsTexture("pixel_life.png", typeof(RenderFix).Assembly, 0));
                    costs.Add(item2);
                }
                int num3 = card.MoneyCost();
                bool flag3 = num3 > 0;
                if (flag3)
                {
                    Texture2D item3 = Part2CardCostRender.CombineIconAndCount(num3, TextureHelper.GetImageAsTexture("pixel_money.png", typeof(RenderFix).Assembly, 0));
                    costs.Add(item3);
                }
            };
        }
    }
}
