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
		private void AddActiveCashConverter()
		{
			// setup ability
			const string rulebookName = "Cash Converter";
			const string rulebookDescription = "Pay 4 foils to put 1 damage on your opponent's side of the scale";
			const string LearnDialogue = "Money for Blood";
			Texture2D tex_a1 = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_CashConverter);
			Sprite tex_a2 = LifeCost.cards.CardUtils.LoadSpriteFromResource(Art.lifecost_CashConverter_a2);
			int powerlevel = 5;
			bool LeshyUsable = true;
			bool part1Shops = false;
			bool canStack = false;

			// set ability to behaviour class
			lifecost_ActivateCashConverter.ability = cards.CardUtils.CreateAbilityWithDefaultSettings(rulebookName, rulebookDescription, typeof(lifecost_ActivateCashConverter), tex_a1, tex_a2, LearnDialogue,
																					true, powerlevel, LeshyUsable, part1Shops, canStack).ability;

		}
	}

	public class lifecost_ActivateCashConverter : LifeCost.sigils.LifeActiveAbilityCost
	{
		public override Ability Ability => ability;

		public static Ability ability;

		public override int MoneyCost
		{
			get
			{
				return 4;
			}
		}

		public override IEnumerator Activate()
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return new WaitForSeconds(0.15f);
			bool flag1 = !SaveManager.SaveFile.IsPart2;
			if (flag1)
			{
				var waitTime = 0.1F;
				Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
				yield return new WaitForSeconds(waitTime);
				yield return PayCostPatch.ShowDamageSequence(1, 1, false);
				yield return new WaitForSeconds(waitTime);
				Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
				Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

			}
			else
			{
				var waitTime = 0.5F;
				yield return new WaitForSeconds(waitTime);
				yield return PayCostPatch.ShowDamageSequence(1, 1, false);
				yield return new WaitForSeconds(waitTime);
			}
			yield return base.LearnAbility(0.25f);
			yield return new WaitForSeconds(0.1f);
			yield break;
		}

	}
}