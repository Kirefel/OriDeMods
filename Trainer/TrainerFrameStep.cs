using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trainer
{
    public class TrainerFrameStep : MonoBehaviour
    {
        private Coroutine coroutine;

        private bool trainerActive;
        public bool TrainerActive
        {
            get => trainerActive;
            private set
            {
                trainerActive = value;
                if (trainerActive)
                    StartLoop();
            }
        }

        private bool suspensionPending = false;
        private bool resumePending = false;

        private bool suspended = false;

        public static bool ShouldSkipInput { get; private set; } = false;

        private void StartLoop()
        {
            if (coroutine == null)
                coroutine = StartCoroutine(Coroutine());
        }

        IEnumerator Coroutine()
        {
            while (TrainerActive)
            {
                yield return new WaitForFixedUpdate();

                LateFixedUpdate();
            }

            coroutine = null;
        }

        private void Suspend()
        {
            SuspensionManager.SuspendAll();
            suspensionPending = false;
            suspended = true;
            ShouldSkipInput = true;
        }

        private void Resume()
        {
            SuspensionManager.ResumeAll();
            suspended = false;
            resumePending = false;
            ShouldSkipInput = false;
        }

        void Start()
        {
            TrainerActive = true;
        }


        private string lastInput;

        void Update()
        {
            if (Input.GetKeyDown(Controls.StepPause))
            {
                if (suspended)
                    resumePending = true;
                else
                    suspensionPending = true;
            }

            // Step single frame
            if (suspended && Input.GetKeyDown(Controls.Step))
            {
                resumePending = true;
                suspensionPending = true;
            }
        }

        private void LateFixedUpdate()
        {
            if (suspended && resumePending)
            {
                Resume();
            }
            else if (!suspended && suspensionPending)
            {
                CollectInputsFromFrame();
                Suspend();
            }
        }

        void OnGUI()
        {
            if (lastInput != null)
            {
                GUI.Box(new Rect(10, 10, 400, 20), "");
                GUI.Label(new Rect(10, 10, 400, 20), lastInput);
            }
        }

        List<string> inputs = new List<string>();
        private void CollectInputsFromFrame()
        {
            inputs.Clear();

            GetButton(Core.Input.Up, "Up");
            GetButton(Core.Input.Down, "Down");
            GetButton(Core.Input.Left, "Left");
            GetButton(Core.Input.Right, "Right");

            GetButton(Core.Input.Jump, "Jump");
            GetButton(Core.Input.SoulFlame, "SoulFlame");
            GetButton(Core.Input.SpiritFlame, "SpiritFlame");
            GetButton(Core.Input.Bash, "Bash");

            GetButton(Core.Input.Glide, "Glide");
            GetButton(Core.Input.ChargeJump, "ChargeJump");
            GetButton(Core.Input.LeftShoulder, "Grenade");
            GetButton(Core.Input.RightShoulder, "Dash");

            lastInput = string.Join(" ", inputs.ToArray());
        }

        private void GetButton(Core.Input.InputButtonProcessor button, string name)
        {
            if (button.OnPressed) inputs.Add(name + "*");
            else if (button.IsPressed) inputs.Add(name);
        }
    }
}
