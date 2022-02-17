using HarmonyLib;
using UnityEngine;

namespace UISpeedMod
{
    [HarmonyPatch(typeof(MessageControllerB), nameof(MessageControllerB.ShowMessageBox))]
    public class SeinSpeech
    {
        public static bool Prefix(MessageControllerB __instance, GameObject messageBoxPrefab)
        {
            // Just get rid of sein text, not spirit tree/control hints etc.
            if (messageBoxPrefab == __instance.StoryMessage
                || messageBoxPrefab == __instance.PickupMessage
                || messageBoxPrefab == __instance.AbilityMessage)
                return false;

            return true;
        }
    }
}
