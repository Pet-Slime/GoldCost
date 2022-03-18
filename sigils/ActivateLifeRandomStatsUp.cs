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
		private void AddActivateLifeRandomStatsUp()
		{
			// setup ability
			const string rulebookName = "Die Roll";
			const string rulebookDescription = "Pay 5 Life/Foils to gain between 0 and 6 increase in stats, distributed randomly";
			const string LearnDialogue = "Sing it once, Sing it twice, take a chance and roll the dice!";
			Texture2D tex_a1 = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_ActivateLifeRandomStatsUp);
			Sprite tex_a2 = LifeCost.cards.CardUtils.LoadSpriteFromResource(Art.lifecost_ActivateLifeRandomStatsUp_a2);
			int powerlevel = 3;
			bool LeshyUsable = true;
			bool part1Shops = false;
			bool canStack = false;

			var test = cards.CardUtils.CreateAbilityWithDefaultSettings(rulebookName, rulebookDescription, typeof(lifecost_ActivateLifeRandomStatsUp), tex_a1, tex_a2, LearnDialogue,
																					true, powerlevel, LeshyUsable, part1Shops, canStack);

			test.activated = true;

			// set ability to behaviour class
			lifecost_ActivateLifeRandomStatsUp.ability = test.ability;

		}
	}

	public class lifecost_ActivateLifeRandomStatsUp : LifeCost.sigils.LifeActiveAbilityCost
	{
		public override Ability Ability => ability;

		public static Ability ability;

		public override int BonesCost
		{
			get
			{
				return 1;
			}
		}

		public override IEnumerator Activate()
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return new WaitForSeconds(0.15f);
			CardModificationInfo mod = base.Card.TemporaryMods.Find((CardModificationInfo x) => x.singletonId == MOD_ID);
			bool isPart1 = !SaveManager.SaveFile.IsPart2;
			bool flag = mod == null;
			if (flag)
			{
				mod = new CardModificationInfo();
				mod.singletonId = MOD_ID;
				base.Card.AddTemporaryMod(mod);
			}
			yield return new WaitForSeconds(0.5f);
			var StatsUp = Random.Range(0, 6);
			if (isPart1)
			{
				Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
				for (var index = 0; index < StatsUp; index++)
				{
					var HealthorAttack = Random.Range(1, 100);
					if (HealthorAttack > 50)
					{
						yield return new WaitForSeconds(0.2f);
						mod.healthAdjustment++;
						base.Card.OnStatsChanged();
						base.Card.Anim.StrongNegationEffect();

					}
					else
					{
						yield return new WaitForSeconds(0.2f);
						mod.attackAdjustment++;
						base.Card.OnStatsChanged();
						base.Card.Anim.StrongNegationEffect();
					}
				}
				yield return new WaitForSeconds(0.1f);
				Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
			}
			else
			{
				for (var index = 0; index < StatsUp; index++)
				{
					var HealthorAttack = Random.Range(1, 100);
					if (HealthorAttack > 50)
					{
						yield return new WaitForSeconds(0.2f);
						mod.healthAdjustment++;
						base.Card.OnStatsChanged();
						base.Card.Anim.StrongNegationEffect();

					}
					else
					{
						yield return new WaitForSeconds(0.2f);
						mod.attackAdjustment++;
						base.Card.OnStatsChanged();
						base.Card.Anim.StrongNegationEffect();
					}
				}
			}
			yield return base.LearnAbility(0.25f);
			yield return new WaitForSeconds(0.1f);
			yield break;
		}
		private const string MOD_ID = "LifeGambleStatsUp";

	}
}