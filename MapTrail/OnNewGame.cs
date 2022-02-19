using HarmonyLib;

namespace MapTrail
{
    [HarmonyPatch(typeof(GameController), nameof(GameController.SetupGameplay))]
    public class OnNewGame
    {
        public static void Postfix()
        {
            MapTrailController.Instance.Reset();
        }
    }
}
