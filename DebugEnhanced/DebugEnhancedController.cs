using Game;
using HarmonyLib;
using UnityEngine;

namespace DebugEnhanced
{
    public class DebugEnhancedController : MonoBehaviour
    {
        void Awake()
        {
            // Enable debug controls on game launch
            DebugMenuB.MakeDebugMenuExist();
            DebugMenuB.DebugControlsEnabled = true;
        }

        void FixedUpdate()
        {
            RightClickMapTeleport();
            RightStickDebugNavigation(); // like wotw debug controls
        }

        void OnGUI()
        {
            if (DebugMenuB.DebugControlsEnabled)
            {
                // TODO test this looks ok. May need label instead of box content.
                GUI.Box(new Rect(Screen.width - 200, 0, 200, 40), "DEBUG");
            }
        }

        private static void RightClickMapTeleport()
        {
            if (!GameMapUI.Instance || !GameMapUI.Instance.IsVisible || !DebugMenuB.DebugControlsEnabled)
                return;

            if (Core.Input.RightClick.OnPressed)
            {
                Vector2 cursorPosition = Core.Input.CursorPositionUI;
                Vector2 worldPosition = AreaMapUI.Instance.Navigation.MapToWorldPosition(cursorPosition);
                if (Characters.Sein != null)
                {
                    Characters.Sein.Position = worldPosition;
                    Characters.Sein.Position = worldPosition + new Vector2(0f, 0.5f);
                    UI.Cameras.Current.MoveCameraToTargetInstantly(true);
                    UI.Menu.HideMenuScreen(true);
                    return;
                }
            }
        }

        private static void RightStickDebugNavigation()
        {
            // TODO disable whenever regular debug controls are disabled
            // TODO update regular debug controls to not teleport if RT is held
            // TODO also ignore dash/grenade inputs while debug controls are enabled
            // TODO rebindable teleport/speed up controls?

            var movement = Core.Input.AnalogAxisRight;
            if (movement.sqrMagnitude < 0.3f)
            {
                // TODO teleport if needed
                return;
            }

            // move cursor...
        }
    }

    [HarmonyPatch(typeof(SeinLogicCycle), nameof(SeinLogicCycle.AllowDash))]
    class PatchSeinLogicCycleDashDebugControls
    {
        static bool Prefix(out bool __result)
        {
            __result = false;
            if (DebugEnhanced.ReverseDebugControlsDashCondition && DebugMenuB.DebugControlsEnabled)
                return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(SeinLogicCycle), nameof(SeinLogicCycle.AllowGrenade))]
    class PatchSeinLogicCycleGrenadeDebugControls
    {
        static bool Prefix(out bool __result)
        {
            __result = false;
            if (DebugEnhanced.ReverseDebugControlsDashCondition && DebugMenuB.DebugControlsEnabled)
                return false;
            return true;
        }
    }
}
