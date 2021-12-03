using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryDiag
{
    internal static class recurseEz
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="most"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static string RecurseVals(List<string> inputs, bool most, int index)
        {
            List<int> values = inputs.Select(x => x[index].Equals('0') ? -1 : 1).ToList();

            List<string> remaining = new List<string>();
            if (values.Sum() == 0)
                remaining = inputs.Where(x => (x[index] == '1' && most) || (x[index] == '0' && !most)).ToList();
            else
            {
                char bitValue = (values.Sum() > 0 && most) || (values.Sum() < 0 && !most) ? '1' : '0';
                remaining = inputs.Where(x => x[index].Equals(bitValue)).ToList();
            }           
            
            string foundVal = remaining.Count > 1 ?
                RecurseVals(remaining, most, index + 1) :
                remaining.First();

            return foundVal;
        }
    }
}
