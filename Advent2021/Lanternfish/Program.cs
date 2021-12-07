
using Lanternfish;

List<string> inputs = System.IO.File.ReadLines(@"c:\advent\day6\input.txt").ToList();

List<int> starting = inputs.First().Split(',').Select(x => int.Parse(x)).ToList();

double count = FishService.CaclulateFishV2(80, starting);

Console.WriteLine(count);

count = FishService.CaclulateFishV2(256, starting);

Console.WriteLine(count);