using BaseModLib;
using HarmonyLib;
using OriDeModLoader;

namespace MapTrail
{
    public class ModInit : IMod
    {
        public string Name => "Map Trail";

        private Harmony harmony;

        public void Init()
        {
            harmony = new Harmony("com.ori.maptrail");
            harmony.PatchAll();

            Hooks.OnStartNewGame += () =>
            {
                MapTrailController.Instance.Reset();
            };

            Controllers.Add<MapTrailController>("653e23f4-f975-48a0-8bfd-13ede6641438");
        }

        public void Unload()
        {
            harmony.UnpatchAll("com.ori.maptrail");
        }
    }
}
