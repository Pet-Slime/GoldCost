using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using System.Linq;

namespace SpaceCost
{

	internal class PayCostPatch
	{
		[HarmonyPatch(typeof(PlayableCard), "OnPlayed")]
		public class void_TeethPatch_payCost
		{


			[HarmonyPostfix]
			public static void Postfix(ref PlayableCard __instance)
			{
				Plugin.Log.LogWarning("Cost test: PayCost Patch fired");
				if (__instance.Info.BloodCost < 0) {

					int costToPay = __instance.Info.BloodCost * -1;
					int currentCurrency = RunState.Run.currency;
					int lifeBalance = Singleton<LifeManager>.Instance.Balance * -1;
					int finalCurrency = currentCurrency + lifeBalance;


					Plugin.Log.LogWarning("Cost test: costToPay- " + costToPay);
					Plugin.Log.LogWarning("Cost test: currentCurrency- " + currentCurrency);

					if (costToPay > currentCurrency)
                    {
						RunState.Run.currency = 0;
						costToPay = costToPay - currentCurrency;
						Plugin.Log.LogWarning("Cost test: costToPay after - currentCurrency - " + costToPay);
						var damage = costToPay;
						var numWeights = 1;
						var toPlayer = true;
						var alternatePrefab = ResourceBank.Get<GameObject>("Prefabs/Environment/ScaleWeights/Weight");
						alternatePrefab.transform.localScale.Set(3 * costToPay, 3 * costToPay, 3 * costToPay);
						Singleton<LifeManager>.Instance.Scales3D.highlightedInteractable.SetEnabled(false);
						Singleton<LifeManager>.Instance.Scales3D.highlightedInteractable.ShowState(HighlightedInteractable.State.Highlighted, false, 0.1f);
						int num = 0;
						for (int i = 0; i < numWeights; i = num + 1)
						{
							Vector3 dropPos = toPlayer ? Singleton<LifeManager>.Instance.Scales3D.PlayerSide : Singleton<LifeManager>.Instance.Scales3D.OpponentSide;
							GameObject weight = WeightUtil.SpawnWeight(Singleton<LifeManager>.Instance.Scales3D.transform, dropPos, true, alternatePrefab);
							WeightUtil.DropWeight(weight);
							

							if (toPlayer)
							{
								Singleton<LifeManager>.Instance.Scales3D.playerWeight += damage / numWeights;
								Singleton<LifeManager>.Instance.Scales3D.playerWeights.Add(weight);
								Singleton<LifeManager>.Instance.PlayerDamage += damage;
							}
							else
							{
								Singleton<LifeManager>.Instance.Scales3D.opponentWeight += damage / numWeights;
								Singleton<LifeManager>.Instance.Scales3D.opponentWeights.Add(weight);
								Singleton<LifeManager>.Instance.OpponentDamage += damage;
							}

							Singleton<LifeManager>.Instance.Scales3D.SetBalanceBasedOnWeights(false);
							dropPos = default(Vector3);
							weight = null;
							num = i;
						}
						Singleton<LifeManager>.Instance.Scales3D.highlightedInteractable.ShowState(HighlightedInteractable.State.NonInteractable, false, 0.2f);
						Singleton<LifeManager>.Instance.Scales3D.highlightedInteractable.SetEnabled(true);

					} else {
						Singleton<CurrencyBowl>.Instance.MoveIntoPlace(Singleton<CurrencyBowl>.Instance.NEAR_SCALES_POS, Singleton<CurrencyBowl>.Instance.NEAR_SCALES_ROT, Tween.EaseInOutStrong, false);
						Singleton<CurrencyBowl>.Instance.TakeWeights(currentCurrency);
						Singleton<CurrencyBowl>.Instance.MoveAway(Singleton<CurrencyBowl>.Instance.NEAR_SCALES_POS);
						RunState.Run.currency = currentCurrency - costToPay;
					}


					Plugin.Log.LogWarning("Cost test: run currency - " + RunState.Run.currency);
					Plugin.Log.LogWarning("Cost test: player life - " + Singleton<LifeManager>.Instance.PlayerDamage);
				}
			}
		}
	}
}
