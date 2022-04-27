using HarmonyLib;

namespace AccessibilityMod
{
    [HarmonyPatch(typeof(AreaMapNavigation), nameof(AreaMapNavigation.Awake))]
    class MaxAreaMapZoomPatch
    {
        static void Postfix(AreaMapNavigation __instance)
        {
            __instance.AreaMapZoomLevel = 1;
        }
    }
}
