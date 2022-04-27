using BaseModLib;
using HarmonyLib;
using OriDeModLoader;
using OriDeModLoader.UIExtensions;
using UnityEngine;

namespace AccessibilityMod
{
    public class AccessibilityMod : IMod
    {
        // TODO this is the mod where free gjump, invert climb, screen shake reduction etc. would go

        public static BoolSetting cursorLock = new BoolSetting("Cursor Lock", true);
        public static BoolSetting runInBackground = new BoolSetting("Run In Background", true);
        public static FloatSetting newfloat = new FloatSetting("It's a float!", 0);

        public string Name => "Accessibility Mod";

        private Harmony harmony;

        public void Init()
        {
            harmony = new Harmony("com.ori.accessibility");
            harmony.PatchAll();
            BashDeadzone.Patch(harmony);
            MoreSaveSlots.Patch(harmony);

            Cursor.lockState = cursorLock ? CursorLockMode.Confined : CursorLockMode.None;
            // Settings.CursorLock.OnChanged += value => Cursor.lockState = value ? CursorLockMode.Confined : CursorLockMode.None;

            CustomMenuManager.RegisterOptionsScreen<OptionsScreen>("Accessibility", 100);
        }

        public void Unload()
        {
            harmony.UnpatchAll("com.ori.accessibility");
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public class OptionsScreen : CustomOptionsScreen
    {
        public override void InitScreen()
        {
            AddToggle(AccessibilityMod.cursorLock, "Whether the cursor should be locked to the screen");
            AddToggle(AccessibilityMod.runInBackground, "Whether the game should continue to run when the window is not selected");
            AddSlider(AccessibilityMod.newfloat, -1f, 1f, 0.2f, "[-1, 1] step 0.2");
        }
    }
}
