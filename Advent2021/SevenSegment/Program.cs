List<string> inputs = System.IO.File.ReadLines(@"c:\advent\day8\input.txt").ToList();
List<string> outputs = new List<string>();

if (inputs[0].Contains("|") && inputs[1].Contains("|"))
{
    outputs = inputs.Select(x => x.Split('|')[1]).ToList();
    inputs = inputs.Select(x => x.Split('|')[0]).ToList();
}
else
{
    outputs = inputs.Where(x => !x.Contains("|")).ToList();
    inputs = inputs.Where(x => x.Contains("|")).Select(x => x.Replace("|", "")).ToList();
}

Dictionary<int, string> keyValuePairs = new Dictionary<int, string>()
{
    { 0, "abcefg" },
    { 1, "cf" },
    { 2, "acdeg" },
    { 3, "acdfg" },
    { 4, "bcdf" },
    { 5, "abdfg" },
    { 6, "abdefg" },
    { 7, "acf" },
    { 8, "abcdefg" },
    { 9, "abcdfg" },
};

List<int> uniqueCount = new List<int>();

foreach (var pair in keyValuePairs)
{
    if (keyValuePairs
        .Select(x => x.Value.Length)
        .ToList()
        .Where(x => x == pair.Value.Length).Count() == 1)
    {
        uniqueCount.Add(pair.Value.Length);
    }
}

int count = 0;
foreach (string output in outputs)
{
    var digits = output.Trim().Split(' ');

    if (digits.Count() != 4)
        throw new Exception("To many digits in output");

    count += digits.Where(x => uniqueCount.Contains(x.Length)).Count();
}

Console.WriteLine(count);

