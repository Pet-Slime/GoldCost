using DiskCardGame;
using InscryptionAPI.Card;


namespace LifeCost
{
    public class VampericSpecialAbility : SpecialCardBehaviour
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility;

        public static void addVampericSpecialAbility()
        {
            VampericSpecialAbility.specialAbility = SpecialTriggeredAbilityManager.Add(Plugin.PluginGuid, "Vamperic (Special Ability)", typeof(VampericSpecialAbility)).Id;
        }
    }
}