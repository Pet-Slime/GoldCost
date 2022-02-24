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
		private NewAbility AddActiveCashConverter()
		{
			// setup ability
			const string rulebookName = "Cash Converter";
			const string rulebookDescription = "Pay 4 foils to put 1 damage on your opponent's side of the scale";
			const string LearnDialogue = "Money for Blood";
			// const string TextureFile = "Artwork/void_pathetic.png";

			AbilityInfo info = LifeCost.cards.CardUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 3, false);
			info.canStack = false;
			info.activated = true;
			info.pixelIcon = LifeCost.cards.CardUtils.LoadSpriteFromResource(Art.lifecost_CashConverter_a2);

			Texture2D tex = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_CashConverter);

			var abIds = LifeCost.cards.CardUtils.GetAbilityId(info.rulebookName);
			
			NewAbility newAbility = new NewAbility(info, typeof(lifecost_ActivateCashConverter), tex, abIds);


			// set ability to behaviour class
			lifecost_ActivateCashConverter.ability = newAbility.ability;

			return newAbility;

		}
	}

	public class lifecost_ActivateCashConverter : LifeCost.sigils.LifeActiveAbilityCost
	{
		public override Ability Ability => ability;

		public static Ability ability;

		protected override int MoneyCost
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