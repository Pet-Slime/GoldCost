using DiskCardGame;
using InscryptionAPI.Card;


namespace LifeCost
{
    public class GreedySpecialAbility : SpecialCardBehaviour
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility;

        public static void addGreedySpecialAbility()
        {
            GreedySpecialAbility.specialAbility = SpecialTriggeredAbilityManager.Add(Plugin.PluginGuid, "Greedy (Special ability)", typeof(GreedySpecialAbility)).Id;
        }
    }
}