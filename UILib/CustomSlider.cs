﻿using UnityEngine;
using BaseModLib;
using HarmonyLib;

namespace UILib
{
    public class CustomSlider : MonoBehaviour
    {
        public FloatSetting Setting;
    }

    namespace Harmony
    {
        [HarmonyPatch(typeof(MusicVolumeSlider), "get_Value")]
        internal class MusicVolumeSlider_GetValue
        {
            private static bool Prefix(MusicVolumeSlider __instance, out float __result)
            {
                var customSlider = __instance.GetComponent<CustomSlider>();
                if (customSlider != null)
                {
                    __result = customSlider.Setting.Value;
                    return false;
                }

                __result = 0;
                return true; // This will allow the original to return music volume (not really hardcoded to 0)
            }
        }

        [HarmonyPatch(typeof(MusicVolumeSlider), "set_Value")]
        internal class MusicVolumeSlider_SetValue
        {
            private static bool Prefix(MusicVolumeSlider __instance, float value)
            {
                var customSlider = __instance.GetComponent<CustomSlider>();
                if (customSlider != null)
                {
                    customSlider.Setting.Value = value;
                    // Settings.SetDirty()
                    return false;
                }

                return true;
            }
        }
    }
}
