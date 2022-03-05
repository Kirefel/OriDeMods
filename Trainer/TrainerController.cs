using System.Collections;
using UnityEngine;

namespace Trainer
{
    public class TrainerController : MonoBehaviour
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

        IEnumerator LateFixedUpdate()
        {
            while (TrainerActive)
            {
                yield return new WaitForFixedUpdate();

                if (suspended && resumePending)
                    Resume();
                else if (!suspended && suspensionPending)
                    SuspendAll();
            }

            coroutine = null;
        }

        private void StartLoop()
        {
            if (coroutine == null)
                coroutine = StartCoroutine(LateFixedUpdate());
        }

        public void SuspendAll()
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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Home))
            {
                if (suspended)
                    resumePending = true;
                else
                    suspensionPending = true;
            }

            // Step single frame
            if (suspended && Input.GetKeyDown(KeyCode.PageDown))
            {
                resumePending = true;
                suspensionPending = true;
            }
        }
    }
}
