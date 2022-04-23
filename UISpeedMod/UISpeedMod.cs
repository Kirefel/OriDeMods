﻿using HarmonyLib;
using OriDeModLoader;

namespace UISpeedMod
{
    public class UISpeedMod : IMod
    {
        public string Name => "UI Speed Mod";

        Harmony harmony;

        public void Init()
        {
            harmony = new Harmony("com.ori.uispeed");
            harmony.PatchAll();
        }

        public void Unload()
        {
            harmony.UnpatchAll("com.ori.uispeed");
        }
    }
}