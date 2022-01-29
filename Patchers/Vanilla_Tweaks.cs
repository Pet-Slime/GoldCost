using DiskCardGame;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LifeCost
{
    public static class CostDictionaries
    {
        public static Dictionary<CardInfo, int> LifeCost = new Dictionary<CardInfo, int>();
    }

    public static class ext
    {


        public static int LifeCostz(this CardInfo infoz, int whattoset = -1)
        {
            var info = CardLoader.AllData.Find(cardInfo => cardInfo.name == infoz.name);
            if (whattoset != -1)
            {
                CostDictionaries.LifeCost.Add(info, whattoset + 1);
            }

            int cost;
            if (CostDictionaries.LifeCost.TryGetValue(info, out cost))
            {
 ///               Plugin.Log.LogInfo("the cost of " + info + " is equal to " + cost);
                return cost - 1;
            }
            else
            {
  ///              Plugin.Log.LogInfo("the cost of " + info + " was not found");
                return -1;
            }

        }
    }


    public static class vanilla_tweaks
    { 
        public static void AddCard()
        {
            var cards = ScriptableObjectLoader<CardInfo>.AllData;


            for (int index = 0; index < cards.Count; index++)
            {
                CardInfo info = cards[index];
                if (info.energyCost < 0)
                {
                    info.LifeCostz(info.PowerLevel-1);
                    info.energyCost = 0;
                }
                if (info.bonesCost > 0)
                {
                    info.LifeCostz(info.bonesCost);
                    info.bonesCost = 0;
                }
            }
        }
    }
}
