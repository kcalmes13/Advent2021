using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenSegment
{
    internal static class Decoder
    {
        internal static int Decode(string input, string output, Dictionary<int, string> originalValues)
        {
            int sum = 0;

            List<string> digits = input.Trim().Split(' ').ToList();
            digits.AddRange(output.Trim().Split(' '));

            Dictionary<int, string> codeValues = new Dictionary<int, string>();
            Dictionary<char, char> index = new Dictionary<char, char>();

            // get our know values for decoding
            foreach (var pair in originalValues)
            {
                if (originalValues
                    .Select(x => x.Value.Length)
                    .ToList()
                    .Where(x => x == pair.Value.Length).Count() == 1 &&
                    digits.Any(x => x.Length == pair.Value.Length))
                {
                    codeValues.Add(pair.Key, digits.First(x => x.Length == pair.Value.Length));
                }
            }

            while (codeValues.Count != originalValues.Count)
            {
                string digit = digits.First(x => codeValues.Values.FirstOrDefault(y => y.Equals(x)) != null && codeValues.Values.Any(y => y.Contains(x) && y.Replace(x, "").Length == 1));
                var codePair1 = codeValues.First(x => x.Value.Equals(digit));
                var codePair2 = codeValues.First(y => y.Value.Contains(digit) && y.Value.Replace(digit, "").Length == 1);
                var originalPair1 = originalValues[codePair1.Key];
                var originalPair2 = originalValues[codePair2.Key];

                char coded = codePair2.Value.Replace(codePair1.Value, "").First();
                char original = originalPair2.Replace(originalPair1, "").First();

                index.Add(coded, original);


            }

            return sum;
        }


    }
}
