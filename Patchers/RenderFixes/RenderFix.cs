using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using UnityEngine;
using Art = LifeCost.Resources.Cost;
using InscryptionCommunityPatch.Card;
using InscryptionAPI.Helpers;

namespace LifeCost.Patchers.RenderFixes
{
    internal class RenderFix
    {

        public static Texture2D LoadTextureFromResource(string resourceFile)
        {
            var texture = new Texture2D(2, 2);
            var test = (byte[])Art.ResourceManager.GetObject(resourceFile);
            if (test == null)
            {
                test = Art.cost_blank;
            }
            texture.LoadImage(test);
            texture.filterMode = FilterMode.Point;
            return texture;
        }


        public static void CommunityPatchHook()
        {
            Part1CardCostRender.UpdateCardCost += delegate (CardInfo card, List<Texture2D> costs)
            {
                int myCustomCost = card.LifeCostz();
                if (myCustomCost > 0)
                    costs.Add(LoadTextureFromResource($"life_cost_{myCustomCost}"));
            };
        }
    }
}
