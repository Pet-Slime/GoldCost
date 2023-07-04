using System.Collections;
using GBC;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using LifeCost.Patchers;

namespace LifeCost.sigils
{
    [HarmonyPatch]
    public abstract class LifeActiveAbilityCost : ActivatedAbilityBehaviour
    {
        // Token: 0x1700000E RID: 14
        // (get) Token: 0x0600003B RID: 59 RVA: 0x000026F3 File Offset: 0x000008F3
        public virtual int LifeMoneyCost { get; }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x0600003C RID: 60 RVA: 0x000026FB File Offset: 0x000008FB
        public virtual int LifeCost { get; }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x0600003D RID: 61 RVA: 0x00002703 File Offset: 0x00000903
        public virtual int MoneyCost { get; }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x0600003E RID: 62 RVA: 0x0000270B File Offset: 0x0000090B
        public override int EnergyCost { get; }

        // Token: 0x17000012 RID: 18
        // (get) Token: 0x0600003F RID: 63 RVA: 0x00002713 File Offset: 0x00000913
        public override int BonesCost { get; }

        // Token: 0x06000040 RID: 64 RVA: 0x0000271B File Offset: 0x0000091B
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ActivatedAbilityBehaviour), "OnActivatedAbility")]
        private static IEnumerator JankOverride(IEnumerator result, ActivatedAbilityBehaviour __instance)
        {
            Plugin.Log.LogMessage("Lifecost active ability patch firing 1");
            LifeActiveAbilityCost yours = __instance as LifeActiveAbilityCost;
            bool flag9 = yours != null;
            if (flag9)
            {
                Plugin.Log.LogMessage("Lifecost active ability patch firing 2");
                bool baseFlag = yours.CanAfford() && yours.CanActivate();
                bool LifeFlag = yours.CanAffordLife() && yours.CanActivate();
                bool MoneyFlag = yours.CanAffordMoney() && yours.CanActivate();
                bool HybridFlag = yours.CanAffordHybrid() && yours.CanActivate();
                bool flag10 = baseFlag && LifeFlag && MoneyFlag && HybridFlag;
                if (flag10)
                {
                    bool energyFlag = yours.EnergyCost > 0;
                    bool flag11 = energyFlag;
                    if (flag11)
                    {
                        yield return Singleton<ResourcesManager>.Instance.SpendEnergy(yours.EnergyCost);
                        bool flag3 = Singleton<ConduitCircuitManager>.Instance != null;
                        bool flag12 = flag3;
                        if (flag12)
                        {
                            CardSlot energyConduitSlot = Singleton<BoardManager>.Instance.GetSlots(true).Find((CardSlot x) => x.Card != null && x.Card.HasAbility(Ability.ConduitEnergy));
                            bool flag4 = energyConduitSlot != null;
                            bool flag13 = flag4;
                            if (flag13)
                            {
                                ConduitEnergy abilityBehaviour = energyConduitSlot.Card.GetComponent<ConduitEnergy>();
                                bool flag5 = abilityBehaviour != null && abilityBehaviour.CompletesCircuit();
                                bool flag14 = flag5;
                                if (flag14)
                                {
                                    yield return Singleton<ResourcesManager>.Instance.AddEnergy(yours.EnergyCost);
                                }
                                abilityBehaviour = null;
                                abilityBehaviour = null;
                            }
                            energyConduitSlot = null;
                            energyConduitSlot = null;
                        }
                    }
                    bool boneFlag = yours.BonesCost > 0;
                    bool flag15 = boneFlag;
                    if (flag15)
                    {
                        yield return Singleton<ResourcesManager>.Instance.SpendBones(yours.BonesCost);
                    }
                    bool lifeFlag2 = yours.LifeCost > 0;
                    bool flag16 = lifeFlag2;
                    if (flag16)
                    {
                        bool flag6 = SceneLoader.ActiveSceneName.StartsWith("Part1");
                        bool flag17 = flag6;
                        if (flag17)
                        {
                            yield return PayCostPatch.extractCostPart1_lifeOnly(yours.LifeCost);
                        }
                        else
                        {
                            yield return PayCostPatch.extractCostPart2_lifeOnly(yours.LifeCost);
                        }
                    }
                    bool moneyFlag2 = yours.MoneyCost > 0;
                    bool flag18 = moneyFlag2;
                    if (flag18)
                    {
                        bool flag7 = SceneLoader.ActiveSceneName.StartsWith("Part1");
                        bool flag19 = flag7;
                        if (flag19)
                        {
                            int currentCurrency = RunState.Run.currency;
                            yield return PayCostPatch.extractCostPart1_MoneyOnly(yours.MoneyCost, currentCurrency);
                        }
                        else
                        {
                            yield return PayCostPatch.extractCostPart2_MoneyOnly(yours.MoneyCost);
                        }
                    }
                    bool hybridFlag2 = yours.LifeMoneyCost > 0;
                    bool flag20 = hybridFlag2;
                    if (flag20)
                    {
                        bool flag8 = SceneLoader.ActiveSceneName.StartsWith("Part1");
                        bool flag21 = flag8;
                        if (flag21)
                        {
                            int currentCurrency2 = RunState.Run.currency;
                            yield return PayCostPatch.extractCostPart1_hybrid(yours.LifeMoneyCost, currentCurrency2);
                        }
                        else
                        {
                            int currentCurrency3 = SaveData.Data.currency;
                            yield return PayCostPatch.extractCostPart2_hybrid(yours.LifeMoneyCost, currentCurrency3);
                        }
                    }
                    yield return new WaitForSeconds(0.1f);
                    yield return yours.PreSuccessfulTriggerSequence();
                    yield return yours.Activate();
                    ProgressionData.SetMechanicLearned(MechanicsConcept.GBCActivatedAbilities);
                }
                else
                {
                    AudioController.Instance.PlaySound2D("toneless_negate", MixerGroup.GBCSFX, 0.2f, 0f, null, null, null, null, false);
                    yield return new WaitForSeconds(0.25f);
                }
                yield break;
            }
            yield return result;
            yield break;
        }

        // Token: 0x06000041 RID: 65 RVA: 0x00002734 File Offset: 0x00000934
        private bool CanAffordLife()
        {
            int num = Singleton<LifeManager>.Instance.Balance + 5;
            return num >= this.LifeCost;
        }

        // Token: 0x06000042 RID: 66 RVA: 0x00002760 File Offset: 0x00000960
        private bool CanAffordMoney()
        {
            bool flag = SceneLoader.ActiveSceneName.StartsWith("Part1");
            bool flag2 = flag;
            int currency;
            if (flag2)
            {
                currency = RunState.Run.currency;
            }
            else
            {
                currency = SaveData.Data.currency;
            }
            return currency >= this.MoneyCost;
        }

        // Token: 0x06000043 RID: 67 RVA: 0x000027B4 File Offset: 0x000009B4
        private bool CanAffordHybrid()
        {
            Plugin.Log.LogMessage("Lifecost active ability patch firing 3");
            bool flag = SceneLoader.ActiveSceneName.StartsWith("Part1");
            bool flag2 = flag;
            int currency;
            if (flag2)
            {
                currency = RunState.Run.currency;
            }
            else
            {
                currency = SaveData.Data.currency;
            }
            int num = Singleton<LifeManager>.Instance.Balance + 5;
            int num2 = currency + num;
            return num2 >= this.LifeMoneyCost;
        }
    }
}