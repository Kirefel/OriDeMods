using HarmonyLib;
using System;
using UnityEngine;

namespace MapTrail
{
    [HarmonyPatch(typeof(GameController), nameof(GameController.Awake))]
    public class InitialiseController
    {
        public static void Postfix(GameController __instance)
        {
            var controller = new GameObject("maptrail").AddComponent<MapTrailController>();
            controller.MoonGuid = new MoonGuid(new Guid("653e23f4-f975-48a0-8bfd-13ede6641438"));
            controller.RegisterToSaveSceneManager(__instance.GetComponent<SaveSceneManager>());
            controller.transform.SetParent(__instance.transform);
        }
    }
}
