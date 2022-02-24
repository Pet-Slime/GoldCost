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
		private NewAbility AddActiveStatsUpLife()
		{
			// setup ability
			const string rulebookName = "Vamperic Strength";
			const string rulebookDescription = "Pay 3 life to increase the power and health of this card by 1";
			const string LearnDialogue = "Hurting oneself can lead to an increase in strength.";
			// const string TextureFile = "Artwork/void_pathetic.png";

			AbilityInfo info = LifeCost.cards.CardUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 3, false);
			info.canStack = false;
			info.activated = true;
			info.pixelIcon = LifeCost.cards.CardUtils.LoadSpriteFromResource(Art.lifecost_ActivateStatsUpLife_a2);

			Texture2D tex = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_ActivateStatsUpLife);

			var abIds = LifeCost.cards.CardUtils.GetAbilityId(info.rulebookName);
			
			NewAbility newAbility = new NewAbility(info, typeof(lifecost_ActivateStatsUpLife), tex, abIds);


			// set ability to behaviour class
			lifecost_ActivateStatsUpLife.ability = newAbility.ability;

			return newAbility;

		}
	}

	public class lifecost_ActivateStatsUpLife : LifeCost.sigils.LifeActiveAbilityCost
	{
		public override Ability Ability => ability;

		public static Ability ability;

		protected override int LifeCost
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