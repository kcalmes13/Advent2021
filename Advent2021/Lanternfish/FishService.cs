using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lanternfish
{
    internal static class FishService
    {
        internal static int CaclulateFish(int days, List<int> startingFish)
        {
            List<int> currentFish = startingFish;

            while (days > 0)
            {
                int startCount = currentFish.Count;
                for (int i = 0; i < startCount; i++)
                {
                    switch(currentFish[i])
                    {
                        case 0:
                            currentFish[i] = 6;
                            currentFish.Add(8);
                            break;
                        default:
                            currentFish[i]--;
                            break;
                    }
                }

                days--;

                if (days == 100)
                {
                    var c = 0;
                }
            }

            return currentFish.Count();
        }

        internal static double CaclulateFishV2(int days, List<int> startingFish)
        {
            List<int> currentFish = startingFish;
            double[] fishAmounts = new double[9];

            foreach (int fish in currentFish)
            {
                fishAmounts[fish]++;
            }

            while (days > 0)
            {
                double[] newFish = new double[9];
                for (int i = 8; i >= 0; i--)
                {
                    switch(i)
                    {
                        case 0:
                            newFish[8] = fishAmounts[0];
                            newFish[6] += fishAmounts[0];
                            break;
                        default:
                            newFish[i-1] = fishAmounts[i];
                            break;
                    }
                }

                fishAmounts = newFish;
                days--;
            }

            return fishAmounts.Sum();
        }
    }
}
