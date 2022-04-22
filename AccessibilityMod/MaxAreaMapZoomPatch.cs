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

    //public class MyAbility : CustomSeinAbility
    //{
    //    public override bool AllowAbility(SeinLogicCycle logicCycle)
    //    {
    //        return !logicCycle.Sein.Abilities.Swimming.IsSwimming;
    //    }

    //    public override void UpdateCharacterState()
    //    {
    //        if (Input.GetKey(KeyCode.K))
    //        {
    //            Debug.Log("You are holding K");
    //        }
    //    }
    //}
}
