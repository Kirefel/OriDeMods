using BaseModLib;
using HarmonyLib;
using OriDeModLoader;

namespace DebugEnhanced
{
    public class DebugEnhanced : IMod
    {
        public string Name => "Debug Enhanced";

        public static bool ReverseDebugControlsDashCondition = true;

        private Harmony harmony;

        public void Init()
        {
            harmony = new Harmony("com.ori.debugenhanced");
            harmony.PatchAll();

            Controllers.Add<DebugEnhancedController>();
        }

        public void Unload()
        {

        }
    }
}
