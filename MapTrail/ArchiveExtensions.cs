using System.Collections.Generic;
using UnityEngine;

namespace MapTrail
{
    public static class ArchiveExtensions
    {
        public static void Serialize(this Archive ar, Queue<Vector2> vectors)
        {
            if (ar.Writing)
            {
                ar.Serialize(vectors.Count);
                foreach (var vec in vectors)
                    ar.Serialize(vec);
            }
            else
            {
                vectors.Clear();

                int count = ar.Serialize(0);
                for (int i = 0; i < count; i++)
                    vectors.Enqueue(ar.Serialize(Vector2.zero));
            }
        }
    }
}
