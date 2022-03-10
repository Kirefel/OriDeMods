using BaseModLib;
using Game;
using HarmonyLib;
using OriDeModLoader;
using UnityEngine;

namespace Randomiser
{
    public class RandomiserMod : IMod
    {
        public string Name => "Randomiser";

        Harmony harmony;

        public void Init()
        {
            harmony = new Harmony("com.ori.randomiser");
            harmony.PatchAll();
        }

        public void Unload()
        {

        }
    }

    public class Randomiser
    {
        static BasicMessageProvider messageProvider;

        public static void Grant(MoonGuid guid)
        {
            Message(guid.ToString());
            GameWorld.Instance.CurrentArea.DirtyCompletionAmount();
        }

        private static void Message(string message)
        {
            if (messageProvider == null)
                messageProvider = ScriptableObject.CreateInstance<BasicMessageProvider>();

            messageProvider.SetMessage(message);
            UI.Hints.Show(messageProvider, HintLayer.Gameplay);
        }
    }
}
