using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Randomiser
{
    public enum GoalMode
    {
        None,
        ForceTrees,
        ForceMaps,
        WarmthFrags,
        WorldTour
    }

    public enum KeyMode
    {
        None,
        Clues,
        Shards,
        LimitKeys
    }

    [Flags]
    public enum RandomiserFlags
    {
        None = 0,
        OpenWorld = 1,
        ClosedDungeons = 2,
        OHKO = 4,
        [Description("0XP")]
        ZeroXP = 8,
        SkipFinalEscape = 16,
        StompTriggers = 32
    }

    public class RandomiserSeed : SaveSerialize
    {
        // Seed consists of flags, goals and item placements
        public GoalMode GoalMode { get; private set; }
        public KeyMode KeyMode { get; private set; }
        public RandomiserFlags Flags { get; private set; }
        Dictionary<MoonGuid, RandomiserAction> map;

        // The random seed used to generate the... seed. But nobody means this when they say "seed".
        string seed;

        public bool HasFlag(RandomiserFlags flag) => (Flags & flag) != 0;

        public override void Serialize(Archive ar)
        {
            GoalMode = (GoalMode)ar.Serialize((int)GoalMode);
            KeyMode = (KeyMode)ar.Serialize((int)KeyMode);
            Flags = (RandomiserFlags)ar.Serialize((int)Flags);
            ar.Serialize(ref seed);

            // TODO serialise map
        }

        public void LoadSeed(string filepath)
        {
            // TODO handle missing file

            Reset();

            using (var reader = new StreamReader(filepath))
            {
                string[] meta = reader.ReadLine().Split('|');
                ParseMeta(meta);

                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    if (line.Length == 0)
                        continue;

                    map[new MoonGuid(new Guid(line[0]))] = new RandomiserAction { action = line[1], parameter = line[2] };
                }
            }
        }

        private void ParseMeta(string[] meta)
        {
            seed = meta[1];

            string[] flagsAndOtherThings = meta[0].Split(',');
            foreach (string str in flagsAndOtherThings)
            {
                if (TryParse(str, out GoalMode goalMode))
                    GoalMode = goalMode;
                if (TryParse(str, out KeyMode keyMode))
                    KeyMode = keyMode;
                if (TryParse(str, out RandomiserFlags flag))
                    Flags |= flag;
            }
        }

        bool TryParse<T>(string value, out T result)
        {
            object x = Enum.Parse(typeof(T), value);
            if (x != null)
            {
                result = (T)x;
                return true;
            }

            result = default(T);
            return false;
        }

        private void Reset()
        {
            GoalMode = 0;
            KeyMode = 0;
            Flags = 0;
            map.Clear();
            seed = "";
        }
    }
}
