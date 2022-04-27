using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OriDeModLoader.UIExtensions;
using UnityEngine;

namespace MapTrail
{
    public class MapTrailController : SaveSerialize, ICustomSave
    {
        public static MapTrailController Instance { get; private set; }

        private const int Capacity = 20;
        public Queue<Vector2> recentPositions = new Queue<Vector2>(Capacity);
        private readonly List<CustomWorldMapIcon> icons = new List<CustomWorldMapIcon>();
        private const float Interval = 0.1f;
        private float timeRemaining = Interval;
        private Mesh squareMesh;
        private Texture2D circleTex;

        public void Start()
        {
            Instance = this;

            var baseGuid = new MoonGuid(new Guid("89bc4d8f-9bb2-42c8-ba5f-8f95c68f46c0"));

            PreloadResources();

            for (int i = 0; i < Capacity; i++)
            {
                var g = new MoonGuid(baseGuid);
                g.D += i;
                CustomWorldMapIcon newIcon = new CustomWorldMapIcon(CustomWorldMapIconType.WaterVein, Vector2.zero, g);
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
            if (recentPositions.Count > 0 && (vec - recentPositions.Last()).sqrMagnitude < distance)
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

        private void UpdateMapIcons()
        {
            int i = 0;
            foreach (var pos in recentPositions)
                icons[i++].Position = pos + Vector2.up * 1.5f;

            for (; i < Capacity; i++)
                icons[i].Position = new Vector3(-1000, -1000, 0);
        }

        private void PreloadResources()
        {
            var width = 1f;
            var height = 1f;

            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[4]
            {
                    new Vector3(0, 0, 0),
                    new Vector3(width, 0, 0),
                    new Vector3(0, height, 0),
                    new Vector3(width, height, 0)
            };
            mesh.vertices = vertices;

            int[] tris = new int[6]
            {
                    // lower left triangle
                    0, 2, 1,
                    // upper right triangle
                    2, 3, 1
            };
            mesh.triangles = tris;

            Vector3[] normals = new Vector3[4]
            {
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward
            };
            mesh.normals = normals;

            Vector2[] uv = new Vector2[4]
            {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1)
            };
            mesh.uv = uv;

            squareMesh = mesh;
            circleTex = LoadTextureFromFile("Mods/assets/MapTrail/circle.png", 64, 64);
        }

        private GameObject IconFunc()
        {
            OriDeModLoader.Loader.Log("Creating new icon");

            try
            {
                var iconGO = new GameObject("icon");
                var mf = iconGO.AddComponent<MeshFilter>();

                mf.mesh = squareMesh;

                var mr = iconGO.AddComponent<MeshRenderer>();
                mr.material = new Material(Shader.Find("Hidden/UberShader/CCD82372EF7AEC0E4A9C618731D4E911"));

                mr.material.mainTexture = Texture2D.whiteTexture;// circleTex; 
                //Shader.Find("UberShader")

                UberShaderAPI.SetMainTexture(mr, Texture2D.whiteTexture, false);


                Debug.Log("Successfully created new icon!");

                return iconGO;
            }
            catch (Exception ex)
            {
                OriDeModLoader.Loader.Log(ex.ToString());
                throw;
            }
        }

        private Texture2D LoadTextureFromFile(string path, int width, int height)
        {
            //if (!File.Exists(path))
            //    throw new FileNotFoundException("Failed to load texture (file not found)", path);

            var bytes = File.ReadAllBytes(path);
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false, true);

            tex.LoadImage(bytes);
            return tex;
        }
    }
}
