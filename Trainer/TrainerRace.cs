using BaseModLib;
using Game;
using System.Collections;
using UnityEngine;

namespace Trainer
{
    public class TrainerRace : MonoBehaviour, ISuspendable
    {
        public Vector3 start;
        public Vector3 end;
        public Vector3 endDirection;

        public static bool RaceActive = false; // used to pause saves
        private bool raceRunning = false;
        private bool countingDown = false;

        float timeTaken;

        BasicMessageProvider messageProvider;

        public bool IsSuspended { get; set; }

        void Awake()
        {
            messageProvider = ScriptableObject.CreateInstance<BasicMessageProvider>();
            SuspensionManager.Register(this);
        }

        void Update()
        {
            if (TrainerFrameStep.ShouldSkipInput || !Characters.Sein)
                return;

            if (Input.GetKeyDown(Controls.RaceStart))
            {
                if (!RaceActive && start == Vector3.zero)
                {
                    start = Characters.Sein.Position;
                    GameController.Instance.CreateCheckpoint();
                    GameController.Instance.SaveGameController.PerformSave();
                    Message("Start set, go to end");
                    RaceActive = true;
                }
                else if (RaceActive && end == Vector3.zero)
                {
                    end = Characters.Sein.Position;
                    endDirection = end - start;
                    StartRace();
                }
            }

            if (RaceActive && !countingDown && Input.GetKeyDown(Controls.RaceRestart))
                RestartRace();

            if (RaceActive && Input.GetKeyDown(Controls.RaceEnd))
                CancelRace();

            if (RaceActive && raceRunning)
            {
                if (!IsSuspended)
                    timeTaken += Time.deltaTime;

                if (Vector3.Dot(endDirection, Characters.Sein.Position - end) >= 0)
                    EndRace();
            }
        }

        IEnumerator StartRaceCoroutine()
        {
            raceRunning = false;
            countingDown = true;
            timeTaken = 0;

            Message("3");
            SuspensionManager.SuspendAll();

            yield return new WaitForSeconds(1);
            Message("2");
            yield return new WaitForSeconds(1);
            Message("1");
            yield return new WaitForSeconds(1);
            Message("Go!");

            SuspensionManager.ResumeAll();
            countingDown = false;
            if (RaceActive)
                raceRunning = true;
        }

        void Message(string message, float duration = 1f)
        {
            messageProvider.SetMessage(message);
            UI.Hints.Show(messageProvider, HintLayer.Gameplay, duration);
        }

        void StartRace()
        {
            RaceActive = true;
            RestartRace();
        }

        void EndRace()
        {
            Message("Time taken: " + timeTaken, 5f);
            raceRunning = false;
        }

        void CancelRace()
        {
            raceRunning = false;
            RaceActive = false;
            SuspensionManager.ResumeAll();
            end = Vector3.zero;
            start = Vector3.zero;
        }

        void RestartRace()
        {
            GameController.Instance.SaveGameController.PerformLoad();
            StartCoroutine(StartRaceCoroutine());
        }

        void OnGUI()
        {
            if (RaceActive)
            {
                GUI.Label(new Rect(10, 10, 200, 40), "Race active, saves disabled");
            }
        }
    }
}
