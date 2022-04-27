using HarmonyLib;

namespace AccessibilityMod
{
    // Fix bug by waiting until the user releases up and down before allowing them to make selections
    [HarmonyPatch(typeof(CleverMenuItemSelectionManager), nameof(CleverMenuItemSelectionManager.Start))]
    internal class PauseMenuQTMBugfix
    {
        // Ideally this is associated with the selection manager
        // However only one can be open for selection at a time, so we can get away with the static field
        public static bool delayNavigation;

        private static void Prefix()
        {
            delayNavigation = Core.Input.MenuDown.IsPressed || Core.Input.MenuUp.IsPressed;
        }
    }

    [HarmonyPatch(typeof(CleverMenuItemSelectionManager), nameof(CleverMenuItemSelectionManager.FixedUpdate))]
    internal class PauseMenuQTMBugfixFixedUpdate
    {
        private static void Prefix(CleverMenuItemSelectionManager __instance)
        {
            if (__instance.IsActive && PauseMenuQTMBugfix.delayNavigation && !Core.Input.MenuDown.IsPressed && !Core.Input.MenuUp.IsPressed)
                PauseMenuQTMBugfix.delayNavigation = false;
        }
    }

    [HarmonyPatch(typeof(CleverMenuItemSelectionManager), nameof(CleverMenuItemSelectionManager.MoveSelection))]
    internal class PauseMenuQTMBugfixMoveSelection
    {
        private static bool Prefix() => !PauseMenuQTMBugfix.delayNavigation; // i.e. allow method to run if delay navigation is false
    }
}
