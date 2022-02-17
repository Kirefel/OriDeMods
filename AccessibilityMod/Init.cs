using BaseModLib;
using HarmonyLib;
using OriDeModLoader;
using UILib;
using UnityEngine;

namespace AccessibilityMod
{
    public class ModInit : IMod
    {
        public static BoolSetting cursorLock = new BoolSetting("Cursor Lock", true);
        public static FloatSetting newfloat = new FloatSetting("It's a float!", 0);

        public string Name => "Accessibility Mod";

        Harmony harmony;

        public void Init()
        {
            harmony = new Harmony("com.ori.accessibility");
            harmony.PatchAll();
            BashDeadzone.Patch(harmony);

            Cursor.lockState = cursorLock ? CursorLockMode.Confined : CursorLockMode.None;
            // Settings.CursorLock.OnChanged += value => Cursor.lockState = value ? CursorLockMode.Confined : CursorLockMode.None;

            CustomMenuManager.RegisterOptionsScreen<OptionsScreen>("Test", 100);
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
            AddToggle(ModInit.cursorLock, "Whether the cursor should be locked to the screen");
            AddSlider(ModInit.newfloat, -1f, 1f, 0.2f, "[-1, 1] step 0.2");
        }
    }
}
