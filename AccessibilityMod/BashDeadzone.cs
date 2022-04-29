using System.Collections.Generic;
using HarmonyLib;

namespace AccessibilityMod
{
    internal class BashDeadzone
    {
        internal static void Patch(Harmony harmony)
        {
            // Required to patch internal class
            harmony.Patch(AccessTools.Method("BashAttackGame:FixedUpdate"), transpiler: new HarmonyMethod(typeof(BashDeadzone), nameof(BashDeadzone.Transpiler)));
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
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
