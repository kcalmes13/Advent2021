List<string> inputs = System.IO.File.ReadLines(@"c:\advent\day1\input.txt").ToList();
int increase = 0;

for (int i = 0; i < inputs.Count - 1; i++)
{
    if (double.Parse(inputs[i]) < double.Parse(inputs[i + 1]))
    {
        increase++;
    }
}

Console.WriteLine(increase);

increase = 0;
for (int i = 0; i < inputs.Count - 3; i++)
{
    if (double.Parse(inputs[i]) < double.Parse(inputs[i + 3]))
    {
        increase++;
    }
}

Console.WriteLine(increase);