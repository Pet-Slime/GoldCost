using DiskCardGame;
using System.Collections.Generic;
using HarmonyLib;

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
            if (info == null)
            {
                return -1;
            }
            if (whattoset != -1)
            {
                CostDictionaries.LifeCost.Add(info, whattoset + 1);
            }

            int cost;
            if (CostDictionaries.LifeCost.TryGetValue(info, out cost))
            {
                return cost - 1;
            }
            else
            {
                return -1;
            }

        }
    }


    public static class vanilla_tweaks
    {

        [HarmonyPatch(typeof(LoadingScreenManager), "LoadGameData")]
        public class LoadingScreenManager_LoadGameData
        {
            public static void Postfix()
            {
                var cards = ScriptableObjectLoader<CardInfo>.AllData;


                for (int index = 0; index < cards.Count; index++)
                {
                    CardInfo info = cards[index];
                    if (info.energyCost < 0)
                    {
                        int cost = info.energyCost * -1;
                        info.LifeCostz(cost);
                        info.energyCost = 0;
                    }

                    if (info.name == "Zombie")
                    {
                        info.LifeCostz(info.bonesCost);
                        info.bonesCost = 0;
                    }

                    if (info.name == "FrankNStein")
                    {
                        info.LifeCostz(info.bonesCost);
                        info.bonesCost = 0;
                        info.specialAbilities.Add(VampericSpecialAbility.specialAbility);
                    }

                    if (info.name == "Gravedigger")
                    {
                        info.LifeCostz(info.bonesCost);
                        info.bonesCost = 0;
                        info.specialAbilities.Add(GreedySpecialAbility.specialAbility);
                    }
                }
            }
        }
    }
}
