using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Randomiser
{
    public class RandomiserLocations : MonoBehaviour
    {
        public class Location
        {
            public enum LocationType
            {
                Keystone,
                HealthCell,
                EnergyCell,
                AbilityCell,
                Skill,
                Mapstone,
                MapstoneFragment,
                ExpLarge,
                ExpMedium,
                ExpSmall,
                Event,
                Plant,
                Cutscene
            }

            public enum WorldArea
            {
                Glades,
                Grove,
                Swamp,
                Grotto,
                Ginso,
                Valley,
                Misty,
                Forlorn,
                Sorrow,
                Horu,
                Blackroot
            }

            public readonly string name;
            public readonly Vector2 position;
            public readonly LocationType type;
            public readonly WorldArea worldArea;
            public readonly int saveIndex;
            public readonly MoonGuid guid;

            public Location(string name, Vector2 position, LocationType type, WorldArea worldArea, int saveIndex, MoonGuid guid)
            {
                this.name = name;
                this.position = position;
                this.type = type;
                this.worldArea = worldArea;
                this.saveIndex = saveIndex;
                this.guid = guid;
            }

            public bool HasBeenObtained() => false; // Randomiser.Inventory.GetBit(saveIndex)
        }

        private readonly Dictionary<string, Location> nameMap = new Dictionary<string, Location>();
        private readonly Dictionary<MoonGuid, Location> guidMap = new Dictionary<MoonGuid, Location>();

        public void Load(string file)
        {
            // TODO handle missing file

            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        continue;

                    string[] parts = line.Split(' ');

                    var location = new Location(
                        name: parts[0],
                        position: new Vector2(float.Parse(parts[1]), float.Parse(parts[2])),
                        type: (Location.LocationType)Enum.Parse(typeof(Location.LocationType), parts[3]),
                        worldArea: (Location.WorldArea)Enum.Parse(typeof(Location.WorldArea), parts[4]),
                        saveIndex: int.Parse(parts[5]),
                        guid: new MoonGuid(int.Parse(parts[5]), int.Parse(parts[6]), int.Parse(parts[7]), int.Parse(parts[8])));

                    nameMap[location.name] = location;
                    guidMap[location.guid] = location;
                }
            }
        }
    }
}
