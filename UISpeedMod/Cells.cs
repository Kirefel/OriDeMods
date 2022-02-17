using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace UISpeedMod
{

    [HarmonyPatch(typeof(SeinPickupProcessor), nameof(SeinPickupProcessor.OnCollectMaxHealthContainerPickup))]
    public class HealthCell
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = instructions.ToList();

            bool skip = false;
            for (int i = 0; i < instructionList.Count; i++)
            {
                if (instructionList[i].opcode == OpCodes.Ldarg_0 && instructionList[i + 1].LoadsField(AccessTools.Field(typeof(SeinPickupProcessor), nameof(SeinPickupProcessor.HeartContainerSequence))))
                    skip = true;

                if (!skip)
                    yield return instructionList[i];

                if (instructionList[i].Calls(AccessTools.Method(typeof(ActionMethod), "Perform")))
                    skip = false;
            }
        }
    }

    [HarmonyPatch(typeof(SeinPickupProcessor), nameof(SeinPickupProcessor.OnCollectMaxEnergyContainerPickup))]
    public class EnergyCell
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = instructions.ToList();

            bool skip = false;
            for (int i = 0; i < instructionList.Count; i++)
            {
                if (instructionList[i].opcode == OpCodes.Ldarg_0 && instructionList[i + 1].LoadsField(AccessTools.Field(typeof(SeinPickupProcessor), nameof(SeinPickupProcessor.EnergyContainerSequence))))
                    skip = true;

                if (!skip)
                    yield return instructionList[i];

                if (instructionList[i].Calls(AccessTools.Method(typeof(ActionMethod), "Perform")))
                    skip = false;
            }
        }
    }

    [HarmonyPatch(typeof(SeinPickupProcessor), nameof(SeinPickupProcessor.OnCollectSkillPointPickup))]
    public class AbilityCell
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = instructions.ToList();

            bool skip = false;
            for (int i = 0; i < instructionList.Count; i++)
            {
                if (instructionList[i].opcode == OpCodes.Ldarg_0 && instructionList[i + 1].LoadsField(AccessTools.Field(typeof(SeinPickupProcessor), nameof(SeinPickupProcessor.SkillPointSequence))))
                    skip = true;

                if (!skip)
                    yield return instructionList[i];

                if (instructionList[i].Calls(AccessTools.Method(typeof(ActionMethod), "Perform")))
                    skip = false;
            }
        }
    }

    [HarmonyPatch(typeof(SeinPickupProcessor.CollectableInformation), nameof(SeinPickupProcessor.CollectableInformation.RunActionIfFirstTime))]
    public class DisableHealthEnergyPickupsPopup
    {
        public static bool Prefix() { return false; }
    }
}
