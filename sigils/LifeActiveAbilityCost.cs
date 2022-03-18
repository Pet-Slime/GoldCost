using System.Collections;
using GBC;
using DiskCardGame;
using UnityEngine;
using System.Linq;
using HarmonyLib;
using System;
using System.Collections.Generic;
using Pixelplacement;

namespace LifeCost.sigils
{
	[HarmonyPatch]
	public abstract class LifeActiveAbilityCost : ActivatedAbilityBehaviour
    {
        public virtual int LifeMoneyCost { get; }

		public virtual int LifeCost { get; }

		public virtual int MoneyCost { get; }

		public override int EnergyCost { get; }

		public override int BonesCost { get; }

		[HarmonyPostfix]
		[HarmonyPatch(typeof(ActivatedAbilityBehaviour), nameof(ActivatedAbilityBehaviour.OnActivatedAbility))]
		private static IEnumerator JankOverride(IEnumerator result, ActivatedAbilityBehaviour __instance)
		{
			if (__instance is LifeActiveAbilityCost yours)
			{
				bool baseFlag = yours.CanAfford() && yours.CanActivate();
				bool LifeFlag = yours.CanAffordLife() && yours.CanActivate();
				bool MoneyFlag = yours.CanAffordMoney() && yours.CanActivate();
				bool HybridFlag = yours.CanAffordHybrid() && yours.CanActivate();
				if (baseFlag || LifeFlag || MoneyFlag || HybridFlag)
				{
					bool energyFlag = yours.EnergyCost > 0;
					if (energyFlag)
					{
						yield return Singleton<ResourcesManager>.Instance.SpendEnergy(yours.EnergyCost);
						bool flag3 = Singleton<ConduitCircuitManager>.Instance != null;
						if (flag3)
						{
							CardSlot energyConduitSlot = Singleton<BoardManager>.Instance.GetSlots(true).Find((CardSlot x) => x.Card != null && x.Card.HasAbility(Ability.ConduitEnergy));
							bool flag4 = energyConduitSlot != null;
							if (flag4)
							{
								ConduitEnergy abilityBehaviour = energyConduitSlot.Card.GetComponent<ConduitEnergy>();
								bool flag5 = abilityBehaviour != null && abilityBehaviour.CompletesCircuit();
								if (flag5)
								{
									yield return Singleton<ResourcesManager>.Instance.AddEnergy(yours.EnergyCost);
								}
								abilityBehaviour = null;
							}
							energyConduitSlot = null;
						}
					}
					bool boneFlag = yours.BonesCost > 0;
					if (boneFlag)
					{
						yield return Singleton<ResourcesManager>.Instance.SpendBones(yours.BonesCost);
					}
					bool lifeFlag2 = yours.LifeCost > 0;
					if (lifeFlag2)
					{
						bool flag1 = SceneLoader.ActiveSceneName.StartsWith("Part1");
						if (flag1)
						{
							yield return PayCostPatch.extractCostPart1_lifeOnly(yours.LifeCost);
						}
						else
						{
							yield return PayCostPatch.extractCostPart2_lifeOnly(yours.LifeCost);
						}
					}
					bool moneyFlag2 = yours.MoneyCost > 0;
					if (moneyFlag2)
					{
						bool flag1 = SceneLoader.ActiveSceneName.StartsWith("Part1");
						if (flag1)
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
					if (hybridFlag2)
					{
						bool flag1 = SceneLoader.ActiveSceneName.StartsWith("Part1");
						if (flag1)
						{
							int currentCurrency = RunState.Run.currency;
							yield return PayCostPatch.extractCostPart1_hybrid(yours.LifeMoneyCost, currentCurrency);
						}
						else
						{
							int currentCurrency = SaveData.Data.currency;
							yield return PayCostPatch.extractCostPart2_hybrid(yours.LifeMoneyCost, currentCurrency);
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
			else
			{
				yield return result;
			}
		}


		private bool CanAffordLife()
        {
			int lifeBalance = Singleton<LifeManager>.Instance.Balance + 5;
			return lifeBalance >= LifeCost;
        }

		private bool CanAffordMoney()
		{
			int currentCurrency = 0;
			bool flag1 = SceneLoader.ActiveSceneName.StartsWith("Part1");
			if (flag1)
			{
				currentCurrency = RunState.Run.currency;
			}
			else
			{
				currentCurrency = SaveData.Data.currency;
			}
			return currentCurrency >= MoneyCost;
		}

		private bool CanAffordHybrid()
		{
			int finalCost = 0;
			int currentCurrency = 0;
			bool flag1 = SceneLoader.ActiveSceneName.StartsWith("Part1");
			if (flag1)
			{
				currentCurrency = RunState.Run.currency;
			}
			else
			{
				currentCurrency = SaveData.Data.currency;
			}
			int lifeBalance = Singleton<LifeManager>.Instance.Balance + 5;
			finalCost = currentCurrency + lifeBalance;
			return finalCost >= LifeMoneyCost;
		}
	}
}
