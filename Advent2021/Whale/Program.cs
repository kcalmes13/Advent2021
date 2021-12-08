List<string> inputs = System.IO.File.ReadLines(@"c:\advent\day7\input.txt").ToList();

List<int> positions = inputs.First().Split(',').Select(x => int.Parse(x)).ToList();

long cost = long.MaxValue;
int postion = 0;

Parallel.For(positions.Min(), positions.Max(), index =>
{
    long fuelSum = positions.Select(x => x > index ? x- index : index - x).Sum();

    if (cost > fuelSum)
    {
        Interlocked.Exchange(ref cost,fuelSum);
        Interlocked.Exchange(ref postion, index);
    }    
});

Console.WriteLine(postion);
Console.WriteLine(cost);

cost = long.MaxValue;
postion = 0;

Parallel.For(positions.Min(), positions.Max(), index =>
{
    long fuelSum = positions.Select(x => x > index ? Enumerable.Range(1, x - index).Sum() : Enumerable.Range(1, index - x).Sum()).Sum();

    if (cost > fuelSum)
    {
        Interlocked.Exchange(ref cost, fuelSum);
        Interlocked.Exchange(ref postion, index);
    }
});

Console.WriteLine(postion);
Console.WriteLine(cost);