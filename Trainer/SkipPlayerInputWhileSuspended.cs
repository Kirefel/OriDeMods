using HarmonyLib;

namespace Trainer
{
    [HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.FixedUpdate))]
    internal class SkipPlayerInputWhileSuspended
    {
        private static bool Prefix()
        {
            // Returning false causes the original method to be skipped
            return !TrainerFrameStep.ShouldSkipInput;
        }
    }
}
