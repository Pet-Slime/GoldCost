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
		private void AddGreedy()
		{
			// setup ability
			const string rulebookName = "Greedy";
			const string rulebookDescription = "[creature] will only accept Teeth for it's cost instead of Life and Teeth, if it has a Finance cost.";
			const string LearnDialogue = "Gooooooooldddddd! *cough* sorry about that. Couldn't resist.";
			Texture2D tex_a1 = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_greedy);
			byte[] tex_a2 = Art.lifecost_greedy_a2;
			int powerlevel = 0;
			bool LeshyUsable = false;
			bool part1Shops = false;
			bool canStack = false;

			// set ability to behaviour class
			lifecost_Greedy.ability = cards.CardUtils.CreateAbilityWithDefaultSettings(rulebookName, rulebookDescription, typeof(lifecost_Greedy), tex_a1, tex_a2, LearnDialogue,
																					true, powerlevel, LeshyUsable, part1Shops, canStack).ability;

		}
	}

	public class lifecost_Greedy : AbilityBehaviour
	{
		public override Ability Ability => ability;

		public static Ability ability;

	}
}