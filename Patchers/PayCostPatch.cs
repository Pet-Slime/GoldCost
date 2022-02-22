using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using HarmonyLib;
using System.Linq;
using GBC;

namespace LifeCost
{

	internal class PayCostPatch
	{


	[HarmonyPatch(typeof(PlayerHand))]
	public class void_TeethPatch_payCost
	{
		[HarmonyPostfix, HarmonyPatch(nameof(PlayerHand.SelectSlotForCard))]
		public static IEnumerator Postfix(
		IEnumerator enumerator,
		PlayerHand __instance,
		PlayableCard card
		)
		{
///			Plugin.Log.LogWarning("Cost test: PayCost Patch fired");
			if (card.Info.LifeCostz() > 0)
			{
	
				__instance.CardsInHand.ForEach(delegate (PlayableCard x)
				{
					x.SetEnabled(false);
				});
				yield return new WaitWhile(() => __instance.ChoosingSlot);
				__instance.OnSelectSlotStartedForCard(card);
				if (Singleton<RuleBookController>.Instance != null)
				{
					Singleton<RuleBookController>.Instance.SetShown(false, true);
				}
				Singleton<BoardManager>.Instance.CancelledSacrifice = false;
				__instance.choosingSlotCard = card;
				if (card != null && card.Anim != null)
				{
					card.Anim.SetSelectedToPlay(true);
				}
				Singleton<BoardManager>.Instance.ShowCardNearBoard(card, true);
				if (Singleton<TurnManager>.Instance.SpecialSequencer != null)
				{
					yield return Singleton<TurnManager>.Instance.SpecialSequencer.CardSelectedFromHand(card);
				}
				bool cardWasPlayed = false;
				bool requiresSacrifices = card.Info.BloodCost > 0;
				if (requiresSacrifices)
				{
					List<CardSlot> validSlots = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll((CardSlot x) => x.Card != null);
					yield return Singleton<BoardManager>.Instance.ChooseSacrificesForCard(validSlots, card);
				}
				if (!Singleton<BoardManager>.Instance.CancelledSacrifice)
				{
					List<CardSlot> validSlots2 = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll((CardSlot x) => x.Card == null);
					yield return Singleton<BoardManager>.Instance.ChooseSlot(validSlots2, !requiresSacrifices);
					CardSlot lastSelectedSlot = Singleton<BoardManager>.Instance.LastSelectedSlot;
					if (lastSelectedSlot != null)
					{
						cardWasPlayed = true;
						card.Anim.SetSelectedToPlay(false);
						yield return __instance.PlayCardOnSlot(card, lastSelectedSlot);
						if (card.Info.BonesCost > 0)
						{
							yield return Singleton<ResourcesManager>.Instance.SpendBones(card.Info.BonesCost);
						}
						if (card.EnergyCost > 0)
						{
							yield return Singleton<ResourcesManager>.Instance.SpendEnergy(card.EnergyCost);
						}
						if (card.Info.LifeCostz() > 0)
						{
							int costToPay = card.Info.LifeCostz();
							bool flag2 = !SaveManager.SaveFile.IsPart2;
							if (flag2)
								{
								if (card.HasAbility(lifecost_vamperic.ability) || card.Info.specialAbilities.Contains(VampericSpecialAbility.specialAbility))
									{
									yield return extractCostPart1_lifeOnly(costToPay);
								}
								else if (card.HasAbility(lifecost_Greedy.ability) || card.Info.specialAbilities.Contains(GreedySpecialAbility.specialAbility))
									{
									int currentCurrency = RunState.Run.currency;
									yield return extractCostPart1_MoneyOnly(costToPay, currentCurrency);
								}
								else
								{
									int currentCurrency = RunState.Run.currency;
									yield return extractCostPart1_hybrid(costToPay, currentCurrency);
								}
							}
							else
							{
								if (card.HasAbility(lifecost_vamperic.ability) || card.Info.specialAbilities.Contains(VampericSpecialAbility.specialAbility))
									{
									yield return extractCostPart2_lifeOnly(costToPay);
								}
								else if (card.HasAbility(lifecost_Greedy.ability) || card.Info.specialAbilities.Contains(GreedySpecialAbility.specialAbility))
									{
									yield return extractCostPart2_MoneyOnly(costToPay);
								}
								else
								{
									int currentCurrency = OnSetupPatch_Part2.PlayerFoils;
									yield return extractCostPart2_hybrid(costToPay, currentCurrency);
								}
							}
						}
					}
				}
				if (!cardWasPlayed)
				{
					Singleton<BoardManager>.Instance.ShowCardNearBoard(card, false);
				}
				__instance.choosingSlotCard = null;
				if (card != null && card.Anim != null)
				{
					card.Anim.SetSelectedToPlay(false);
				}
				__instance.CardsInHand.ForEach(delegate (PlayableCard x)
				{
					x.SetEnabled(true);
				});
				yield break;
	
	
	
			}
			else
			{
				yield return enumerator;
			}
	
		}
	}
	

		//Do the calculations here
		public static IEnumerator extractCostPart1_hybrid(int costToPay, int currentCurrency)
		{
			var waitTime = 0.1F;
			if (costToPay > currentCurrency)
			{
				Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
				costToPay = costToPay - currentCurrency;
///				Plugin.Log.LogWarning("Cost test: costToPay after - currentCurrency - " + costToPay);
				yield return new WaitForSeconds(waitTime);
				List<Rigidbody> list = Singleton<CurrencyBowl>.Instance.TakeWeights(RunState.Run.currency);
				foreach (Rigidbody rigidbody in list)
				{
					yield return new WaitForSeconds(waitTime);
					float num3 = (float)list.IndexOf(rigidbody) * 0.05f;
					Tween.Position(rigidbody.transform, rigidbody.transform.position + Vector3.up * 0.5f, 0.075f, num3, Tween.EaseIn, Tween.LoopType.None, null, null, true);
					Tween.Position(rigidbody.transform, new Vector3(0f, 5.5f, 4f), 0.3f, 0.125f + num3, Tween.EaseOut, Tween.LoopType.None, null, null, true);
					UnityEngine.Object.Destroy(rigidbody.gameObject, 0.5f);
				}
				RunState.Run.currency = 0;
				yield return new WaitForSeconds(waitTime);
				yield return ShowDamageSequence(costToPay, costToPay, true); 
				Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, true);
				Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

			}
			else
			{
				Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
				yield return new WaitForSeconds(waitTime);
				List<Rigidbody> list = Singleton<CurrencyBowl>.Instance.TakeWeights(costToPay);
				foreach (Rigidbody rigidbody in list)
				{
					yield return new WaitForSeconds(waitTime);
					float num3 = (float)list.IndexOf(rigidbody) * 0.05f;
					Tween.Position(rigidbody.transform, rigidbody.transform.position + Vector3.up * 0.5f, 0.075f, num3, Tween.EaseIn, Tween.LoopType.None, null, null, true);
					Tween.Position(rigidbody.transform, new Vector3(0f, 5.5f, 4f), 0.3f, 0.125f + num3, Tween.EaseOut, Tween.LoopType.None, null, null, true);
					UnityEngine.Object.Destroy(rigidbody.gameObject, 0.5f);
				}
				yield return new WaitForSeconds(waitTime);
				RunState.Run.currency = currentCurrency - costToPay;
				Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, true);
				Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
			}
			yield break;
		}

		public static IEnumerator extractCostPart1_lifeOnly(int costToPay)
		{
			var waitTime = 0.1F;
			Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
			yield return new WaitForSeconds(waitTime);
			yield return ShowDamageSequence(costToPay, costToPay, true);
			yield return new WaitForSeconds(waitTime);
			Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, true);
			Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
			yield break;
		}

		public static IEnumerator extractCostPart1_MoneyOnly(int costToPay, int currentCurrency)
		{
			var waitTime = 0.1F;
			Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
			yield return new WaitForSeconds(waitTime);
			List<Rigidbody> list = Singleton<CurrencyBowl>.Instance.TakeWeights(costToPay);
			foreach (Rigidbody rigidbody in list)
			{
				yield return new WaitForSeconds(waitTime);
				float num3 = (float)list.IndexOf(rigidbody) * 0.05f;
				Tween.Position(rigidbody.transform, rigidbody.transform.position + Vector3.up * 0.5f, 0.075f, num3, Tween.EaseIn, Tween.LoopType.None, null, null, true);
				Tween.Position(rigidbody.transform, new Vector3(0f, 5.5f, 4f), 0.3f, 0.125f + num3, Tween.EaseOut, Tween.LoopType.None, null, null, true);
				UnityEngine.Object.Destroy(rigidbody.gameObject, 0.5f);
			}
			yield return new WaitForSeconds(waitTime);
			RunState.Run.currency = currentCurrency - costToPay;
			Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, true);
			Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
			yield break;
		}

		//Ported Damage sequence from KCM to make mod work in both non-KCM version and the KCM version
		public static IEnumerator ShowDamageSequence(int damage, int numWeights, bool toPlayer, float waitAfter = 0.125f, GameObject alternateWeightPrefab = null, float waitBeforeCalcDamage = 0f, bool changeView = false)
		{
			bool flag = damage > 1 && Singleton<OpponentAnimationController>.Instance != null;
			if (flag)
			{
				bool flag2 = P03AnimationController.Instance != null && P03AnimationController.Instance.CurrentFace == P03AnimationController.Face.Default;
				if (flag2)
				{
					P03AnimationController.Instance.SwitchToFace(toPlayer ? P03AnimationController.Face.Happy : P03AnimationController.Face.Angry, false, true);
				}
				else
				{
					bool flag3 = Singleton<LifeManager>.Instance.scales != null;
					if (flag3)
					{
						Singleton<OpponentAnimationController>.Instance.SetLookTarget(Singleton<LifeManager>.Instance.scales.transform, Vector3.up * 2f);
					}
				}
			}
			bool flag4 = Singleton<LifeManager>.Instance.scales != null;
			if (flag4)
			{
				if (changeView)
				{
					Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, false);
					yield return new WaitForSeconds(0.1f);
				}
				yield return Singleton<LifeManager>.Instance.scales.AddDamage(damage, numWeights, toPlayer, alternateWeightPrefab);
				bool flag5 = waitBeforeCalcDamage > 0f;
				if (flag5)
				{
					yield return new WaitForSeconds(waitBeforeCalcDamage);
				}
				if (toPlayer)
				{
					Singleton<LifeManager>.Instance.PlayerDamage += damage;
				}
				else
				{
					Singleton<LifeManager>.Instance.OpponentDamage += damage;
				}
				yield return new WaitForSeconds(waitAfter);
			}
			bool flag6 = Singleton<OpponentAnimationController>.Instance != null;
			if (flag6)
			{
				bool flag7 = P03AnimationController.Instance != null && (P03AnimationController.Instance.CurrentFace == P03AnimationController.Face.Angry || P03AnimationController.Instance.CurrentFace == P03AnimationController.Face.Happy);
				if (flag7)
				{
					P03AnimationController.Instance.PlayFaceStatic();
					P03AnimationController.Instance.SwitchToFace(P03AnimationController.Face.Default, false, false);
				}
				else
				{
					Singleton<OpponentAnimationController>.Instance.ClearLookTarget();
				}
			}
			yield break;
		}


		public static IEnumerator extractCostPart2_hybrid(int costToPay, int currentCurrency)
		{
			var waitTime = 0.5F;
			if (costToPay > currentCurrency)
			{
				costToPay = costToPay - currentCurrency;
				yield return new WaitForSeconds(waitTime);
				AudioController.Instance.PlaySound2D("chipDelay_2", MixerGroup.None, 1f, 0f, null, null, null, null, false);
				yield return OnSetupPatch_Part2.foilToZero();
				yield return new WaitForSeconds(waitTime); 
				yield return ShowDamageSequence(costToPay, costToPay, true);

			}
			else
			{
				AudioController.Instance.PlaySound2D("chipDelay_2", MixerGroup.None, 1f, 0f, null, null, null, null, false);
				yield return OnSetupPatch_Part2.foilSpend(costToPay);
			}
			yield break;
		}

		public static IEnumerator extractCostPart2_lifeOnly(int costToPay)
		{
			var waitTime = 0.5F;
			yield return new WaitForSeconds(waitTime);
			yield return ShowDamageSequence(costToPay, costToPay, true);
			yield return new WaitForSeconds(waitTime);
			yield break;
		}

		public static IEnumerator extractCostPart2_MoneyOnly(int costToPay)
		{
			var waitTime = 0.5F;
			yield return new WaitForSeconds(waitTime);
			AudioController.Instance.PlaySound2D("chipDelay_2", MixerGroup.None, 1f, 0f, null, null, null, null, false);
			yield return OnSetupPatch_Part2.foilSpend(costToPay);
			yield return new WaitForSeconds(waitTime);
			yield break;
		}

	}
}
