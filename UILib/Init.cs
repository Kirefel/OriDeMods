using HarmonyLib;
using OriDeModLoader;

namespace UISpeedMod
{
    public class ModInit : IMod
    {
        public string Name => "UI Lib";

        Harmony harmony;

        public void Init()
        {
            harmony = new Harmony("com.ori.uilib");
            harmony.PatchAll();
        }

        public void Unload()
        {
            harmony.UnpatchAll("com.ori.uilib");
        }
    }
}
