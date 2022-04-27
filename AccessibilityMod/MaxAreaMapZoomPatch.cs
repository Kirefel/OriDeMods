using HarmonyLib;

namespace AccessibilityMod
{
    [HarmonyPatch(typeof(AreaMapNavigation), nameof(AreaMapNavigation.Awake))]
    internal class MaxAreaMapZoomPatch
    {
        private static void Postfix(AreaMapNavigation __instance)
        {
            __instance.AreaMapZoomLevel = 1;
        }
    }
}
