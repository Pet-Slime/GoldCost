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
        public static int LifeCost(this CardInfo info)
        {
            int? lifecost = info.GetExtendedPropertyAsInt("LifeCost");
            return lifecost.HasValue ? lifecost.Value : 0;
        }

        public static int MoneyCost(this CardInfo info)
        {
            int? lifecost = info.GetExtendedPropertyAsInt("MoneyCost");
            return lifecost.HasValue ? lifecost.Value : 0;
        }

        public static int LifeMoneyCost(this CardInfo info)
        {
            int? lifecost = info.GetExtendedPropertyAsInt("LifeMoneyCost");
            return lifecost.HasValue ? lifecost.Value : 0;
        }
    }


    public static class vanilla_tweaks
    {
        public static void ChangeCardsToLifecost()
        {

            CardManager.ModifyCardList += delegate (List<CardInfo> cards)
            {
                foreach (CardInfo card in cards.Where(c => c.energyCost < 0))
                {
                    int cost = card.energyCost * -1;
                    card.SetExtendedProperty("LifeMoneyCost", cost);
                    card.energyCost = 0;
                }

                return cards;
            };


            CardInfo card = CardManager.BaseGameCards.CardByName("Zombie");
            card.AddAbilities(lifecost_ActivateStatsUpLife.ability);
        }
    }
}
