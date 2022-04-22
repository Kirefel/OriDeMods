using BaseModLib;
using HarmonyLib;
using OriDeModLoader;
using OriDeModLoader.UIExtensions;
using UnityEngine;

namespace AccessibilityMod
{
    public class AccessibilityMod : IMod
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

            CustomMenuManager.RegisterOptionsScreen<OptionsScreen>("Accessibility", 100);
            //CustomSeinAbilityManager.Add<MyAbility>("3962d401-67ee-48c0-86fa-7ab430ea2ddd");

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
            AddSlider(AccessibilityMod.newfloat, -1f, 1f, 0.2f, "[-1, 1] step 0.2");
        }
    }

    //public class MyAbility : CustomSeinAbility
    //{
    //    public override bool AllowAbility(SeinLogicCycle logicCycle)
    //    {
    //        return !logicCycle.Sein.Abilities.Swimming.IsSwimming;
    //    }

    //    public override void UpdateCharacterState()
    //    {
    //        if (Input.GetKey(KeyCode.K))
    //        {
    //            Debug.Log("You are holding K");
    //        }
    //    }
    //}
}
