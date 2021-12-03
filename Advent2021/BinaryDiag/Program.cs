using BinaryDiag;

List<string> inputs = System.IO.File.ReadLines(@"c:\advent\day3\input.txt").ToList();
string gamma = string.Empty;

for (int i =0; i < inputs.First().Length; i++)
{
    List<int> values = inputs.Select(x => x[i].Equals('0') ? -1 : 1).ToList();

    gamma += values.Sum() > 0 ? "1" : "0";
}

var gammaVal = Convert.ToInt64(gamma, 2);
string eps = new string(gamma.Select(x => x.Equals('0') ? '1' : '0').ToArray());
var epsVal = Convert.ToInt64(eps, 2);
var total = gammaVal * epsVal;

Console.WriteLine(total);

string oxy = recurseEz.RecurseVals(inputs, true, 0);
string carb = recurseEz.RecurseVals(inputs, false, 0);

total = Convert.ToInt64(oxy, 2) * Convert.ToInt64(carb, 2);

Console.WriteLine(total);
