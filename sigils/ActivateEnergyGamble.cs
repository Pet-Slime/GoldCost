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
		private void addActivateEnergyGamble()
		{
			// setup ability
			const string rulebookName = "Max Energy Gamble";
			const string rulebookDescription = "Pay 6 energy to put 0 to 3 damage on someone's side of the scale";
			const string LearnDialogue = "Money for Blood";
			Texture2D tex_a1 = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_ActivateEnergyGamble);
			byte[] tex_a2 = Art.lifecost_ActivateEnergyGamble_a2;
			int powerlevel = 3;
			bool LeshyUsable = true;
			bool part1Shops = false;
			bool canStack = false;

			// set ability to behaviour class
			lifecost_ActivateLifeRandomStatsUp.ability = cards.CardUtils.CreateAbilityWithDefaultSettings(rulebookName, rulebookDescription, typeof(lifecost_ActivateLifeRandomStatsUp), tex_a1, tex_a2, LearnDialogue,
																					true, powerlevel, LeshyUsable, part1Shops, canStack).ability;

		}
	}

	public class lifecost_ActivateEnergyGamble : LifeCost.sigils.LifeActiveAbilityCost
	{
		public override Ability Ability => ability;

		public static Ability ability;

		public override int EnergyCost
		{
			get
			{
				return 6;
			}
		}

		public override IEnumerator Activate()
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return new WaitForSeconds(0.15f);
			bool flag1 = !SaveManager.SaveFile.IsPart2;
			var amount = Random.Range(0, 3);
			if (amount == 0)
			{
				yield return base.LearnAbility(0.25f);
				yield return new WaitForSeconds(0.1f);
				yield break;
			}
			System.Random rnd = new System.Random();
			bool whoGetsit = (rnd.Next(2) == 0);

			if (flag1)
			{
				var waitTime = 0.1F;
				Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
				yield return new WaitForSeconds(waitTime);
				base.Card.Anim.LightNegationEffect();
				yield return PayCostPatch.ShowDamageSequence(amount, amount, whoGetsit);
				yield return new WaitForSeconds(waitTime);
				Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
				Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

			}
			else
			{
				var waitTime = 0.5F;
				yield return new WaitForSeconds(waitTime);
				base.Card.Anim.LightNegationEffect();
				yield return PayCostPatch.ShowDamageSequence(amount, amount, whoGetsit);
				yield return new WaitForSeconds(waitTime);
			}
			yield return base.LearnAbility(0.25f);
			yield return new WaitForSeconds(0.1f);
			yield break;
		}

	}
}