using System.Collections.Generic;

namespace SeinLib
{
    public static class CustomSeinAbilityManager
    {
        private static List<CustomSeinAbility> customAbilities = new List<CustomSeinAbility>();

        public static void Add(CustomSeinAbility ability)
        {
            customAbilities.Add(ability);
        }

        internal static void UpdateStateActive(SeinLogicCycle logicCycle)
        {
            foreach (var a in customAbilities)
                a.SetStateActive(a.AllowAbility(logicCycle));
        }

        internal static void UpdateCharacterState()
        {
            foreach (var a in customAbilities)
                CharacterState.UpdateCharacterState(a);
        }
    }
}
