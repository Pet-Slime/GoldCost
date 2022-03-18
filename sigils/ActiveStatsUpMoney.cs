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
		private void AddActiveStatsUpMoney()
		{
			// setup ability
			const string rulebookName = "Greedy Strength";
			const string rulebookDescription = "Pay 5 currency to increase the power and health of this card by 1";
			const string LearnDialogue = "One can be hired to do many tasks";
			Texture2D tex_a1 = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_ActivateStatsUpMoney);
			byte[] tex_a2 = Art.lifecost_ActivateStatsUpMoney_a2;
			int powerlevel = 3;
			bool LeshyUsable = true;
			bool part1Shops = false;
			bool canStack = false;

			// set ability to behaviour class
			lifecost_ActiveStatsUpMoney.ability = cards.CardUtils.CreateAbilityWithDefaultSettings(rulebookName, rulebookDescription, typeof(lifecost_ActiveStatsUpMoney), tex_a1, tex_a2, LearnDialogue,
																					true, powerlevel, LeshyUsable, part1Shops, canStack).ability;

		}
	}

	public class lifecost_ActiveStatsUpMoney : LifeCost.sigils.LifeActiveAbilityCost
	{
		public override Ability Ability => ability;

		public static Ability ability;

		public override int MoneyCost
		{
			get
			{
				return 5;
			}
		}

		public override IEnumerator Activate()
		{
			CardModificationInfo mod = base.Card.TemporaryMods.Find((CardModificationInfo x) => x.singletonId == MOD_ID);
			bool flag = mod == null;
			if (flag)
			{
				mod = new CardModificationInfo();
				mod.singletonId = MOD_ID;
				base.Card.AddTemporaryMod(mod);
			}
			mod.attackAdjustment++;
			mod.healthAdjustment++;
			base.Card.OnStatsChanged();
			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
			yield return new WaitForSeconds(0.25f);
			Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
			yield break;
		}


		private const string MOD_ID = "statsUp";

	}
}