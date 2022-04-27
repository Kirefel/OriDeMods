using BaseModLib;
using HarmonyLib;
using OriDeModLoader;

namespace Trainer
{
    public class Trainer : IMod
    {
        public string Name => "Trainer";

        private Harmony harmony;

        public void Init()
        {
            Controllers.Add<TrainerFrameStep>(group: "Trainer");

            harmony = new Harmony("com.ori.trainer");
            harmony.PatchAll();
        }

        public void Unload()
        {

        }
    }
}
