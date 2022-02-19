using HarmonyLib;
using OriDeModLoader;

namespace MapTrail
{
    public class ModInit : IMod
    {
        public string Name => "Map Trail";

        Harmony harmony;

        public void Init()
        {
            harmony = new Harmony("com.ori.maptrail");
            harmony.PatchAll();
        }

        public void Unload()
        {
            harmony.UnpatchAll("com.ori.maptrail");
        }
    }
}
