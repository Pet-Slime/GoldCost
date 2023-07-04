using DiskCardGame;
using System.Collections.Generic;
using InscryptionAPI.Card;
using System.Collections;
using System;
using System.Linq;

namespace LifeCost.Patchers
{
    public static class ext
    {
        public static int LifeCost(this CardInfo info)
        {
            int? extendedPropertyAsInt = CardExtensions.GetExtendedPropertyAsInt(info, "LifeCost");
            return (extendedPropertyAsInt != null) ? extendedPropertyAsInt.Value : 0;
        }

        public static int MoneyCost(this CardInfo info)
        {
            int? extendedPropertyAsInt = CardExtensions.GetExtendedPropertyAsInt(info, "MoneyCost");
            return (extendedPropertyAsInt != null) ? extendedPropertyAsInt.Value : 0;
        }

        public static int LifeMoneyCost(this CardInfo info)
        {
            int? extendedPropertyAsInt = CardExtensions.GetExtendedPropertyAsInt(info, "LifeMoneyCost");
            return (extendedPropertyAsInt != null) ? extendedPropertyAsInt.Value : 0;
        }
    }










	// Token: 0x02000008 RID: 8
	public static class vanilla_tweaks
    {
        // Token: 0x06000018 RID: 24 RVA: 0x00002209 File Offset: 0x00000409
        public static void ChangeCardsToLifecost()
        {
            CardManager.ModifyCardList += delegate (List<CardInfo> cards)
            {
                foreach (CardInfo cardInfo in from c in cards
                                              where c.EnergyCost < 0
                                              select c)
                {
                    int num = cardInfo.EnergyCost * -1;
                    CardExtensions.SetExtendedProperty(cardInfo, "LifeMoneyCost", num);
                    cardInfo.SetEnergyCost(0);
                }
                return cards;
            };
        }
    }
}

