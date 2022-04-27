using System.Collections.Generic;
using HarmonyLib;

namespace AccessibilityMod
{
    public class BashDeadzone
    {
        public static void Patch(Harmony harmony)
        {
            // Required to patch internal class
            harmony.Patch(AccessTools.Method("BashAttackGame:FixedUpdate"), transpiler: new HarmonyMethod(typeof(BashDeadzone), nameof(BashDeadzone.Transpiler)));
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var i in instructions)
            {
                if (i.LoadsConstant(0.0400000028f))
                    yield return CodeInstruction.LoadField(typeof(Settings), "BashDeadzone");
                else
                    yield return i;
            }
        }
    }
}
