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

		//On Play, spend life
		[HarmonyPatch(typeof(PlayableCard), "OnPlayed")]
		public class void_TeethPatch_payCost
		{
			[HarmonyPostfix]
			public static void Postfix(ref PlayableCard __instance)
			{
				Plugin.Log.LogWarning("Cost test: PayCost Patch fired");
				if (__instance.Info.LifeCostz() > 0 && __instance.slot.IsPlayerSlot) {

					int costToPay = __instance.Info.LifeCostz();

					Plugin.Log.LogWarning("Cost test: costToPay- " + costToPay);

					bool flag1 = SceneLoader.ActiveSceneName == "Part1_Cabin" || SceneLoader.ActiveSceneName == "Part1_Sanctum";
					if (flag1)
					{
						int currentCurrency = RunState.Run.currency;
						Plugin.Log.LogWarning("Cost test: currentCurrency- " + currentCurrency);
						__instance.StartCoroutine(extractCostPart1(costToPay, currentCurrency));
					} else
					{
						int currentCurrency = OnSetupPatch_Part2.PlayerFoils;
						Plugin.Log.LogWarning("Cost test: currentCurrency- " + currentCurrency);
						__instance.StartCoroutine(extractCostPart2(costToPay, currentCurrency));
					}
					Plugin.Log.LogWarning("Cost test: run currency - " + RunState.Run.currency);
					Plugin.Log.LogWarning("Cost test: player life - " + Singleton<LifeManager>.Instance.PlayerDamage);
				}
			}
		}

		//Do the calculations here
		public static IEnumerator extractCostPart1(int costToPay, int currentCurrency)
		{
			var waitTime = 0.5F;
			if (costToPay > currentCurrency)
			{
				Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, false);
				costToPay = costToPay - currentCurrency;
				Plugin.Log.LogWarning("Cost test: costToPay after - currentCurrency - " + costToPay);
				Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, false);
				yield return new WaitForSeconds(waitTime);
				List<Rigidbody> list = Singleton<CurrencyBowl>.Instance.TakeWeights(RunState.Run.currency);
				foreach (Rigidbody rigidbody in list)
				{
					float num3 = (float)list.IndexOf(rigidbody) * 0.05f;
					Tween.Position(rigidbody.transform, rigidbody.transform.position + Vector3.up * 0.5f, 0.075f, num3, Tween.EaseIn, Tween.LoopType.None, null, null, true);
					Tween.Position(rigidbody.transform, new Vector3(0f, 5.5f, 4f), 0.3f, 0.125f + num3, Tween.EaseOut, Tween.LoopType.None, null, null, true);
					UnityEngine.Object.Destroy(rigidbody.gameObject, 0.5f);
				}
				RunState.Run.currency = 0;
				yield return new WaitForSeconds(waitTime);
				yield return ShowDamageSequence(costToPay, costToPay, true); 
				Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);

			}
			else
			{
				Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, false);
				yield return new WaitForSeconds(waitTime);
				List<Rigidbody> list = Singleton<CurrencyBowl>.Instance.TakeWeights(costToPay);
				foreach (Rigidbody rigidbody in list)
				{
					float num3 = (float)list.IndexOf(rigidbody) * 0.05f;
					Tween.Position(rigidbody.transform, rigidbody.transform.position + Vector3.up * 0.5f, 0.075f, num3, Tween.EaseIn, Tween.LoopType.None, null, null, true);
					Tween.Position(rigidbody.transform, new Vector3(0f, 5.5f, 4f), 0.3f, 0.125f + num3, Tween.EaseOut, Tween.LoopType.None, null, null, true);
					UnityEngine.Object.Destroy(rigidbody.gameObject, 0.5f);
				}
				yield return new WaitForSeconds(waitTime);
				RunState.Run.currency = currentCurrency - costToPay;
				Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
			}
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


		public static IEnumerator extractCostPart2(int costToPay, int currentCurrency)
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




		//Adjust the hint for Life
		[HarmonyPatch(typeof(HintsHandler), "OnNonplayableCardClicked")]
		public class void_TeethPatch_payCostHint
		{ 

			[HarmonyPrefix]
			public static bool Prefix(PlayableCard card, List<PlayableCard> cardsInHand)
			{
				bool isPart = SaveManager.SaveFile.IsPart2;
				if (isPart)
				{
					HintsHandler.OnGBCNonPlayableCardPressed(card);
				}
				else
				{
					bool flag = card.EnergyCost > Singleton<ResourcesManager>.Instance.PlayerEnergy;
					if (flag)
					{
						HintsHandler.notEnoughEnergyHint.TryPlayDialogue(new string[]
						{
						card.Info.DisplayedNameLocalized,
						card.EnergyCost.ToString(),
						Singleton<ResourcesManager>.Instance.PlayerEnergy.ToString()
						});
					}
					else
					{
						bool flag2 = card.Info.GemsCost.Exists((GemType x) => !Singleton<ResourcesManager>.Instance.HasGem(x));
						if (flag2)
						{
							GemType gem = card.Info.GemsCost.Find((GemType x) => !Singleton<ResourcesManager>.Instance.HasGem(x));
							HintsHandler.notEnoughGemsHint.TryPlayDialogue(new string[]
							{
							card.Info.DisplayedNameLocalized,
							HintsHandler.GetColorCodeForGem(gem) + Localization.Translate(gem.ToString()) + "</color>"
							});
						}
						else
						{
							bool flag3 = card.Info.BonesCost > Singleton<ResourcesManager>.Instance.PlayerBones;
							if (flag3)
							{
								HintsHandler.notEnoughBonesHint.TryPlayDialogue(new string[]
								{
								card.Info.DisplayedNameLocalized,
								card.Info.BonesCost.ToString()
								});
							}
							else
							{
								bool flag4 = !Singleton<BoardManager>.Instance.SacrificesCreateRoomForCard(card, Singleton<BoardManager>.Instance.GetSlots(true));
								if (flag4)
								{
									HintsHandler.slotsFullHint.TryPlayDialogue(null);
								}
								else
								{
									bool flag5;
									if (cardsInHand != null)
									{
										if (cardsInHand.Exists((PlayableCard x) => x.Info.name == "Squirrel") && Singleton<BoardManager>.Instance.AvailableSacrificeValue == card.Info.BloodCost - 1)
										{
											flag5 = Singleton<BoardManager>.Instance.GetSlots(true).Exists((CardSlot x) => x.Card == null);
											goto IL_231;
										}
									}
									flag5 = false;
								IL_231:
									bool flag6 = flag5;
									if (flag6)
									{
										HintsHandler.notEnoughBloodButSquirrelHint.TryPlayDialogue(new string[]
										{
										card.Info.DisplayedNameLocalized
										});
									}
									else
									{
										PlayableCard playableCard = Singleton<BoardManager>.Instance.CardsOnBoard.Find((PlayableCard x) => !x.OpponentCard && !x.CanBeSacrificed);
										bool flag7 = playableCard != null;
										if (flag7)
										{
											HintsHandler.notEnoughBloodTerrainHint.TryPlayDialogue(new string[]
											{
											playableCard.Info.DisplayedNameLocalized
											});
										}
										 else if (card.Info.BloodCost < 0)
										{
											
										///	var message = "You do not have enough Life to play that. Gain Life by damaging me.";
										///	CustomCoroutine.Instance.StartCoroutine(Singleton<TextDisplayer>.Instance.ShowMessage(message, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null));
										} else
										{
											HintsHandler.notEnoughBloodHint.TryPlayDialogue(new string[]
											{
											card.Info.DisplayedNameLocalized,
											card.Info.BloodCost.ToString()
											});
										}
									}
								}
							}
						}
					}
				}
				return false;
			}
		}
	}
}
