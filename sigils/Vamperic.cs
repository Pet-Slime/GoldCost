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
		private NewAbility AddVamperic()
		{
			// setup ability
			const string rulebookName = "Vamperic";
			const string rulebookDescription = "[creature] will only accept Life for it's cost instead of Life and Teeth, if it has a Finance cost.";
			const string LearnDialogue = "Gooooooooldddddd! *cough* sorry about that. Couldn't resist.";
			// const string TextureFile = "Artwork/void_pathetic.png";

			AbilityInfo info = LifeCost.cards.CardUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 3, false);
			info.canStack = true;
			info.pixelIcon = LifeCost.cards.CardUtils.LoadSpriteFromResource(Art.lifecost_vamperic_a2);

			Texture2D tex = LifeCost.cards.CardUtils.LoadTextureFromResource(Art.lifecost_vamperic);

			var abIds = LifeCost.cards.CardUtils.GetAbilityId(info.rulebookName);
			
			NewAbility newAbility = new NewAbility(info, typeof(lifecost_vamperic), tex, abIds);


			// set ability to behaviour class
			lifecost_vamperic.ability = newAbility.ability;

			return newAbility;

		}
	}

	public class lifecost_vamperic : AbilityBehaviour
	{
		public override Ability Ability => ability;

		public static Ability ability;

	}
}