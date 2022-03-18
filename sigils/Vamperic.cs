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
		private void AddVamperic()
		{
			// setup ability
			const string rulebookName = "Vamperic";
			const string rulebookDescription = "[creature] will only accept Life for it's cost instead of Life and Teeth, if it has a Finance cost.";
			const string LearnDialogue = "Gooooooooldddddd! *cough* sorry about that. Couldn't resist.";
			Texture2D tex_a1 = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_vamperic);
			byte[] tex_a2 = Art.lifecost_vamperic_a2;
			int powerlevel = 0;
			bool LeshyUsable = false;
			bool part1Shops = false;
			bool canStack = false;

			// set ability to behaviour class
			lifecost_vamperic.ability = cards.CardUtils.CreateAbilityWithDefaultSettings(rulebookName, rulebookDescription, typeof(lifecost_vamperic), tex_a1, tex_a2, LearnDialogue,
																					true, powerlevel, LeshyUsable, part1Shops, canStack).ability;

		}
	}

	public class lifecost_vamperic : AbilityBehaviour
	{
		public override Ability Ability => ability;

		public static Ability ability;

	}
}