using System.Collections;
using GBC;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Art = LifeCost.Resources.Sigils;
using System.Linq;

namespace LifeCost
{
	public partial class Plugin
	{
		//Request by blind
		private void AddActivateLifeConverter()
		{
			// setup ability
			const string rulebookName = "Life Converter";
			const string rulebookDescription = "Pay 2 life to gain 2 foils";
			const string LearnDialogue = "Blood for money";
			Texture2D tex_a1 = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_LifeConverter);
			Sprite tex_a2 = LifeCost.cards.CardUtils.LoadSpriteFromResource(Art.lifecost_LifeConverter_a2);
			int powerlevel = 3;
			bool LeshyUsable = true;
			bool part1Shops = false;
			bool canStack = false;

			// set ability to behaviour class
			lifecost_ActivateLifeConverter.ability = cards.CardUtils.CreateAbilityWithDefaultSettings(rulebookName, rulebookDescription, typeof(lifecost_ActivateLifeConverter), tex_a1, tex_a2, LearnDialogue,
																					true, powerlevel, LeshyUsable, part1Shops, canStack).ability;

		}
	}

	public class lifecost_ActivateLifeConverter : LifeCost.sigils.LifeActiveAbilityCost
	{
		public override Ability Ability => ability;

		public static Ability ability;

		public override int LifeCost
		{
			get
			{
				return 2;
			}
		}

		public override IEnumerator Activate()
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return new WaitForSeconds(0.15f);
			bool flag1 = !SaveManager.SaveFile.IsPart2;
			if (flag1)
			{
				if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("extraVoid.inscryption.LifeCost"))
				{
					Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
					yield return new WaitForSeconds(0.25f); RunState.Run.currency += (2);
					yield return Singleton<CurrencyBowl>.Instance.DropWeightsIn(2);
					yield return new WaitForSeconds(0.75f);
					Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
					Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
				}
				else
				{
					Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
					yield return new WaitForSeconds(0.25f); RunState.Run.currency += (2);
					yield return Singleton<CurrencyBowl>.Instance.ShowGain(2, true, false);
					yield return new WaitForSeconds(0.25f);
					Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
					Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
				}
			}
			else
			{
				SaveData.Data.currency += 2;
				base.Card.Anim.LightNegationEffect();
			}
			yield return base.LearnAbility(0.25f);
			yield return new WaitForSeconds(0.1f);
			yield break;
		}

	}
}