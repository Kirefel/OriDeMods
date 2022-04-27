using HarmonyLib;

namespace AccessibilityMod
{
    [HarmonyPatch(typeof(GameController), "OnApplicationFocus")]
    internal class AlwaysRunInBackground
    {
        private static bool Prefix()
        {
            if (AccessibilityMod.runInBackground)
            {
                GameController.IsFocused = true;
                return false;
            }

            return true;
        }
    }
}
