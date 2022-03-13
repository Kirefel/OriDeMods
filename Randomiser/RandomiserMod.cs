using BaseModLib;
using Game;
using HarmonyLib;
using OriDeModLoader;
using System;
using System.ComponentModel;
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

            Controllers.Add<RandomiserInventory>("b9d5727e-43ff-4a6c-a9d1-d51489b3733d", "Randomiser", mb => Randomiser.Inventory = mb as RandomiserInventory);

            SceneBootstrap.RegisterHandler(RandomiserBootstrap.SetupBootstrap, "Randomiser");
            RandomiserIcons.Initialise();
        }

        public void Unload()
        {

        }
    }


    public class RandomiserAction
    {
        public string action;
        public string parameter;
    }

    public class Randomiser
    {
        public static RandomiserInventory Inventory { get; internal set; }

        static BasicMessageProvider messageProvider;

        public static void Grant(MoonGuid guid)
        {
            Message(guid.ToString());
            GameWorld.Instance.CurrentArea.DirtyCompletionAmount();
        }

        public static bool Has(MoonGuid guid) => false;

        private static void Message(string message)
        {
            if (messageProvider == null)
                messageProvider = ScriptableObject.CreateInstance<BasicMessageProvider>();

            messageProvider.SetMessage(message);
            UI.Hints.Show(messageProvider, HintLayer.Gameplay);
        }
    }

    public class RandomiserInventory : SaveSerialize
    {
        public bool finishedEscape;

        public override void Serialize(Archive ar)
        {
            ar.Serialize(ref finishedEscape);
        }
    }
}
