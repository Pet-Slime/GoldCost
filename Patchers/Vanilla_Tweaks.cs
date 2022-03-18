using DiskCardGame;
using System.Collections.Generic;
using InscryptionAPI.Card;
using System.Collections;
using System;
using System.Linq;

namespace LifeCost
{
    public static class ext
    {
        public static int LifeCostz(this CardInfo info)
        {
            int? lifecost = info.GetExtendedPropertyAsInt("LifeCost");
            return lifecost.HasValue ? lifecost.Value : 0;
        }
    }


    public static class vanilla_tweaks
    {
        public static void ChangeCardsToLifecost()
        {
            ///           var cards = ScriptableObjectLoader<CardInfo>.AllData;
            ///
            ///
            ///           for (int index = 0; index < cards.Count; index++)
            ///           {
            ///               CardInfo info = cards[index];
            ///               if (info.energyCost < 0)
            ///               {
            ///                   int cost = info.energyCost * -1;
            ///                   info.SetExtendedProperty("LifeCost", cost);
            ///                   info.energyCost = 0;
            ///               }
            ///           }

            CardManager.ModifyCardList += delegate (List<CardInfo> cards)
            {
                foreach (CardInfo card in cards.Where(c => c.energyCost < 0))
                {
                    int cost = card.energyCost * -1;
                    card.SetExtendedProperty("LifeCost", cost);
                    card.energyCost = 0;
                }

                return cards;
            };

        }
    }
}
