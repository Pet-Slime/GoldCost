using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using LifeCost.cards;
using LifeCost.lib;
using LifeCost.Patchers.RenderFixes;
using LifeCost.sigils;
using LifeCost.Resources;
using UnityEngine;
using JetBrains.Annotations;
using InscryptionAPI.CardCosts;
using InscryptionAPI.Helpers;

namespace LifeCost
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency(APIGUID, BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency(ZGUID, BepInDependency.DependencyFlags.SoftDependency)]

    public partial class Plugin : BaseUnityPlugin
    {
        // Token: 0x06000019 RID: 25 RVA: 0x00002234 File Offset: 0x00000434
        private void Awake()
        {
            Plugin.Log = base.Logger;
            Plugin.Directory = base.Info.Location.Replace("LifeCost.dll", "");
            Plugin.configTeethSpeed = base.Config.Bind<float>("Teeth Speed", "Teeth Speed", 1f, "Configer the Speed in which golden teeth have in act 1 during the set up of the board. Used to be 25f. Higher numbers may cause issues. do not go below 1f.");
            Harmony harmony = new Harmony("extraVoid.inscryption.LifeCost");
            harmony.PatchAll();
            CardCostManager.Register(PluginGuid, "LifeCost", typeof(LifeCost), );
            this.AddActiveStatsUpLife();
            this.AddActiveStatsUpMoney();
            this.AddActivateLifeConverter();
            this.AddActiveCashConverter();
            this.AddActivateLifeRandomStatsUp();
            this.addActivateEnergyGamble();
            Teck.AddCard();
        }

        public static Texture2D TextureMethod(int cardCost, CardInfo info, PlayableCard card)
        {
            return TextureHelper.GetImageAsTexture($"myCost_{cardCost}");
        }

        // Token: 0x0600001A RID: 26 RVA: 0x000022D2 File Offset: 0x000004D2
        private void Start()
        {
            Plugin.Log.LogMessage("Lifecost start event fired");
            global::LifeCost.Patchers.vanilla_tweaks.ChangeCardsToLifecost();
        }

        // Token: 0x0600001B RID: 27 RVA: 0x000022F4 File Offset: 0x000004F4
        [HarmonyPatch(typeof(DialogueDataUtil), "ReadDialogueData")]
        [HarmonyPostfix]
        public static void BossDialogue()
        {
            DialogueHelper.AddOrModifySimpleDialogEvent("lifecost_NotEnoughLife", new string[]
            {
                "You do not have enough life to play that card."
            }, new string[][]
            {
                new string[]
                {
                    "You are too hurt to play that card."
                },
                new string[]
                {
                    "That card demands more life."
                }
            }, null, null, "NewRunDealtDeckDefault");
        }

        // Token: 0x0600001C RID: 28 RVA: 0x0000235C File Offset: 0x0000055C
        private void addActivateEnergyGamble()
        {
            Texture2D text_a = CardUtils.LoadTextureFromResource(Sigils.lifecost_ActivateEnergyGamble);
            Sprite text_a2 = CardUtils.LoadSpriteFromResource(Sigils.lifecost_ActivateEnergyGamble_a2);
            int powerLevel = 0;
            bool leshyUsable = true;
            bool part1Modular = false;
            bool stack = false;
            lifecost_ActivateEnergyGamble.ability = CardUtils.CreateAbilityWithDefaultSettings("Max Energy Gamble", "Pay 6 energy to put 0 to 3 damage on someone's side of the scale", typeof(lifecost_ActivateEnergyGamble), text_a, text_a2, "Money for Blood", true, powerLevel, leshyUsable, part1Modular, stack).ability;
        }

        // Token: 0x0600001D RID: 29 RVA: 0x000023BC File Offset: 0x000005BC
        private void AddActivateLifeRandomStatsUp()
        {
            Texture2D text_a = CardUtils.LoadTextureFromResource(Sigils.lifecost_ActivateLifeRandomStatsUp);
            Sprite text_a2 = CardUtils.LoadSpriteFromResource(Sigils.lifecost_ActivateLifeRandomStatsUp_a2);
            int powerLevel = 2;
            bool leshyUsable = true;
            bool part1Modular = false;
            bool stack = false;
            AbilityInfo abilityInfo = CardUtils.CreateAbilityWithDefaultSettings("Die Roll", "Pay 5 Life/Foils to gain between 0 and 6 increase in stats, distributed randomly", typeof(lifecost_ActivateLifeRandomStatsUp), text_a, text_a2, "Sing it once, Sing it twice, take a chance and roll the dice!", true, powerLevel, leshyUsable, part1Modular, stack);
            abilityInfo.activated = true;
            lifecost_ActivateLifeRandomStatsUp.ability = abilityInfo.ability;
        }

        // Token: 0x0600001E RID: 30 RVA: 0x00002428 File Offset: 0x00000628
        private void AddActiveCashConverter()
        {
            Texture2D text_a = CardUtils.LoadTextureFromResource(Sigils.lifecost_CashConverter);
            Sprite text_a2 = CardUtils.LoadSpriteFromResource(Sigils.lifecost_CashConverter_a2);
            int powerLevel = 2;
            bool leshyUsable = true;
            bool part1Modular = false;
            bool stack = false;
            lifecost_ActivateCashConverter.ability = CardUtils.CreateAbilityWithDefaultSettings("Cash Converter", "Pay 4 foils to put 1 damage on your opponent's side of the scale", typeof(lifecost_ActivateCashConverter), text_a, text_a2, "Money for Blood", true, powerLevel, leshyUsable, part1Modular, stack).ability;
        }

        // Token: 0x0600001F RID: 31 RVA: 0x00002488 File Offset: 0x00000688
        private void AddActivateLifeConverter()
        {
            Texture2D text_a = CardUtils.LoadTextureFromResource(Sigils.lifecost_LifeConverter);
            Sprite text_a2 = CardUtils.LoadSpriteFromResource(Sigils.lifecost_LifeConverter_a2);
            int powerLevel = 3;
            bool leshyUsable = true;
            bool part1Modular = false;
            bool stack = false;
            lifecost_ActivateLifeConverter.ability = CardUtils.CreateAbilityWithDefaultSettings("Life Converter", "Pay 2 life to gain 1 foils", typeof(lifecost_ActivateLifeConverter), text_a, text_a2, "Blood for money", true, powerLevel, leshyUsable, part1Modular, stack).ability;
        }

        // Token: 0x06000020 RID: 32 RVA: 0x000024E8 File Offset: 0x000006E8
        private void AddActiveStatsUpLife()
        {
            Texture2D text_a = CardUtils.LoadTextureFromResource(Sigils.lifecost_ActivateStatsUpLife);
            Sprite text_a2 = CardUtils.LoadSpriteFromResource(Sigils.lifecost_ActivateStatsUpLife_a2);
            int powerLevel = 3;
            bool leshyUsable = true;
            bool part1Modular = true;
            bool stack = false;
            AbilityInfo abilityInfo = CardUtils.CreateAbilityWithDefaultSettings("Vamperic Strength", "Pay 3 life to increase the power and health of this card by 1", typeof(lifecost_ActivateStatsUpLife), text_a, text_a2, "Hurting oneself can lead to an increase in strength.", true, powerLevel, leshyUsable, part1Modular, stack);
            abilityInfo.activated = true;
            lifecost_ActivateStatsUpLife.ability = abilityInfo.ability;
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00002554 File Offset: 0x00000754
        private void AddActiveStatsUpMoney()
        {
            Texture2D text_a = CardUtils.LoadTextureFromResource(Sigils.lifecost_ActivateStatsUpMoney);
            Sprite text_a2 = CardUtils.LoadSpriteFromResource(Sigils.lifecost_ActivateStatsUpMoney_a2);
            int powerLevel = 3;
            bool leshyUsable = true;
            bool part1Modular = false;
            bool stack = false;
            lifecost_ActiveStatsUpMoney.ability = CardUtils.CreateAbilityWithDefaultSettings("Greedy Strength", "Pay 5 currency to increase the power and health of this card by 1", typeof(lifecost_ActiveStatsUpMoney), text_a, text_a2, "One can be hired to do many tasks", true, powerLevel, leshyUsable, part1Modular, stack).ability;
        }

        // Token: 0x04000003 RID: 3
        public const string APIGUID = "cyantist.inscryption.api";

        // Token: 0x04000004 RID: 4
        public const string PluginGuid = "extraVoid.inscryption.LifeCost";

        // Token: 0x04000005 RID: 5
        public const string ZGUID = "community.inscryption.patch";

        // Token: 0x04000006 RID: 6
        private const string PluginName = "Life Scrybe";

        // Token: 0x04000007 RID: 7
        private const string PluginVersion = "2.0.0";

        // Token: 0x04000008 RID: 8
        public static string Directory;

        // Token: 0x04000009 RID: 9
        internal static ManualLogSource Log;

        // Token: 0x0400000A RID: 10
        internal static ConfigEntry<float> configTeethSpeed;
    }
}
