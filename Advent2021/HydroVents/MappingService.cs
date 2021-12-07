using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroVents
{
    internal static class MappingService
    {
        internal static Dictionary<string, int> MapValues(List<Position> positions)
        {
            Dictionary<string, int> mapping = new Dictionary<string, int>();

            Parallel.ForEach(positions, position =>
            {
                bool calculate = (position.StartX == position.EndX || position.StartY == position.EndY);

                if (!calculate)
                    return;

                List<string> keys = new List<string>();

                if (position.StartX == position.EndX)
                {
                    int low = position.EndY > position.StartY ? position.StartY : position.EndY;

                    for (int i = 0; i <= Math.Abs(position.EndY - position.StartY); i++)
                    {
                        keys.Add($"{position.StartX}, {low + i}");
                    }
                }

                if (position.StartY == position.EndY)
                {
                    int low = position.EndX > position.StartX ? position.StartX : position.EndX;

                    for (int i = 0; i <= Math.Abs(position.EndX - position.StartX); i++)
                    {
                        keys.Add($"{low + i}, {position.StartY}");
                    }
                }

                foreach(string key in keys)
                {
                    lock(mapping)
                    {
                        if (mapping.ContainsKey(key))
                            mapping[key]++;
                        else
                            mapping.Add(key, 1);
                    }
                }
            });

            return mapping;
        }

        internal static Dictionary<string, int> MapValuesDiag(List<Position> positions)
        {
            Dictionary<string, int> mapping = new Dictionary<string, int>();

            Parallel.ForEach(positions, position =>
            {
                List<string> keys = new List<string>();

                bool equal = false;
                int count = 0;

                while(!equal)
                {
                    var x = position.StartX > position.EndX ? position.StartX - count : position.StartX + count;
                    x = position.StartX == position.EndX ? position.StartX : x;
                    var y = position.StartY > position.EndY ? position.StartY - count : position.StartY + count;
                    y = position.StartY == position.EndY ? position.StartY : y;

                    keys.Add($"{x}, {y}");
                    count++;

                    if (x == position.EndX && y == position.EndY)
                    {
                        equal = true;
                    }
                }

                foreach (string key in keys)
                {
                    lock (mapping)
                    {
                        if (mapping.ContainsKey(key))
                            mapping[key]++;
                        else
                            mapping.Add(key, 1);
                    }
                }
            });

            return mapping;
        }
    }
    
}

