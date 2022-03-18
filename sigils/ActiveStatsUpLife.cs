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
		private void AddActiveStatsUpLife()
		{
			// setup ability
			const string rulebookName = "Vamperic Strength";
			const string rulebookDescription = "Pay 3 life to increase the power and health of this card by 1";
			const string LearnDialogue = "Hurting oneself can lead to an increase in strength.";
			Texture2D tex_a1 = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_ActivateStatsUpLife);
			Sprite tex_a2 = LifeCost.cards.CardUtils.LoadSpriteFromResource(Art.lifecost_ActivateStatsUpLife_a2);
			int powerlevel = 3;
			bool LeshyUsable = true;
			bool part1Shops = true;
			bool canStack = false;

			var test = cards.CardUtils.CreateAbilityWithDefaultSettings(rulebookName, rulebookDescription, typeof(lifecost_ActivateStatsUpLife), tex_a1, tex_a2, LearnDialogue,
																					true, powerlevel, LeshyUsable, part1Shops, canStack);

			test.activated = true;

			// set ability to behaviour class
			lifecost_ActivateStatsUpLife.ability = test.ability;

		}
	}

	public class lifecost_ActivateStatsUpLife : LifeCost.sigils.LifeActiveAbilityCost
	{
		public override Ability Ability => ability;

		public static Ability ability;

		public override int LifeCost
		{
			get
			{
				return 3;
			}
		}

		public override IEnumerator Activate()
		{
			bool flag1 = !SaveManager.SaveFile.IsPart2;
			if (flag1)
			{
				Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
			}
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
			yield return new WaitForSeconds(0.25f);
			if (flag1)
			{
				Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
			}
			yield break;
		}


		private const string MOD_ID = "statsUp";

	}
}