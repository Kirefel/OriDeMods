using HarmonyLib;
using OriDeModLoader;

namespace SeinLib
{
    public class SeinLib : IMod
    {
        public string Name => "SeinLib";

        Harmony harmony;

        public void Init()
        {
            harmony = new Harmony("com.ori.seinlib");
            harmony.PatchAll();
        }

        public void Unload()
        {
            harmony.UnpatchAll("com.ori.seinlib");
        }
    }
}
