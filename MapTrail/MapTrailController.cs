using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MapTrail
{
    public class MapTrailController : SaveSerialize, ICustomSave
    {
        public static MapTrailController Instance { get; private set; }

        const int Capacity = 20;
        public Queue<Vector2> recentPositions = new Queue<Vector2>(Capacity);


        List<CustomWorldMapIcon> icons = new List<CustomWorldMapIcon>();

        const float Interval = 0.1f;
        float timeRemaining = Interval;

        public void Start()
        {
            Instance = this;

            var baseGuid = new MoonGuid(new Guid("89bc4d8f-9bb2-42c8-ba5f-8f95c68f46c0"));
            
            for (int i = 0; i < Capacity; i++)
            {
                var g = new MoonGuid(baseGuid);
                g.D += i;
                CustomWorldMapIcon newIcon = new CustomWorldMapIcon(WorldMapIconType.HealthUpgrade, Vector2.zero, g);
                icons.Add(newIcon);
                CustomWorldMapIconManager.Register(newIcon);
            }
        }

        public void Update()
        {
            var sein = Game.Characters.Sein;
            if (!sein || sein.IsSuspended || sein.Controller.InputLocked)
                return;

            timeRemaining -= Time.deltaTime;
            while (timeRemaining <= 0)
            {
                AddPosition(sein.Position);
                timeRemaining += Interval;
            }
        }

        public void AddPosition(Vector2 vec)
        {
            const float distance = 8 * 8;

            // Ignore changes if not moving
            if (recentPositions.Count == 0 || (vec - recentPositions.Last()).sqrMagnitude < distance)
                return;

            while (recentPositions.Count >= Capacity)
                recentPositions.Dequeue();

            recentPositions.Enqueue(vec);

            UpdateMapIcons();
        }

        public override void Serialize(Archive ar)
        {
            ar.Serialize(recentPositions);

            if (ar.Reading)
                UpdateMapIcons();
        }

        public void Reset()
        {
            recentPositions.Clear();
            UpdateMapIcons();
        }

        void UpdateMapIcons()
        {
            int i = 0;
            foreach (var pos in recentPositions)
                icons[i++].Position = pos + Vector2.up * 1.5f;

            for (; i < Capacity; i++)
                icons[i].Position = new Vector3(-1000, -1000, 0);
        }
    }
}
