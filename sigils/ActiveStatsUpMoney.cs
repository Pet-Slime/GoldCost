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
		private NewAbility AddActiveStatsUpMoney()
		{
			// setup ability
			const string rulebookName = "Greedy Strength";
			const string rulebookDescription = "Activate: Pay 5 currency to increase the power and health of this card by 1";
			const string LearnDialogue = "One can be hired to do many tasks";
			// const string TextureFile = "Artwork/void_pathetic.png";

			AbilityInfo info = LifeCost.cards.CardUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 3, false);
			info.canStack = false;
			info.activated = true;
			info.pixelIcon = LifeCost.cards.CardUtils.LoadSpriteFromResource(Art.lifecost_ActivateStatsUpMoney_a2);

			Texture2D tex = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_ActivateStatsUpMoney);

			var abIds = LifeCost.cards.CardUtils.GetAbilityId(info.rulebookName);
			
			NewAbility newAbility = new NewAbility(info, typeof(lifecost_ActiveStatsUpMoney), tex, abIds);


			// set ability to behaviour class
			lifecost_ActiveStatsUpMoney.ability = newAbility.ability;

			return newAbility;

		}
	}

	public class lifecost_ActiveStatsUpMoney : LifeCost.sigils.LifeActiveAbilityCost
	{
		public override Ability Ability => ability;

		public static Ability ability;

		protected override int MoneyCost
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
			yield return new WaitForSeconds(0.25f);
			yield break;
		}


		private const string MOD_ID = "statsUp";

	}
}