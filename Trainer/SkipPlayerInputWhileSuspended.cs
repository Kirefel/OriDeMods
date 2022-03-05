using HarmonyLib;

namespace Trainer
{
    [HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.FixedUpdate))]
    class SkipPlayerInputWhileSuspended
    {
        static bool Prefix()
        {
            // Returning false causes the original method to be skipped
            return !TrainerController.ShouldSkipInput;
        }
    }
}
