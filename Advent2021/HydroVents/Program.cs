using HydroVents;

List<string> inputs = System.IO.File.ReadLines(@"c:\advent\day5\input.txt").ToList();
inputs = inputs.Select(x => x.Replace(" -> ", ";")).ToList();

List<Position> positions = new List<Position>();

foreach (var input in inputs)
{
    List<string> values = input.Split(";").ToList();

    var position = new Position()
    {
        StartX = int.Parse(values[0].Split(',')[0]),
        StartY = int.Parse(values[0].Split(',')[1]),
        EndX = int.Parse(values[1].Split(',')[0]),
        EndY = int.Parse(values[1].Split(',')[1])
    };

    positions.Add(position);
}

Dictionary<string, int> mapping = MappingService.MapValues(positions);

int overlapp = mapping.Where(x => x.Value >= 2).Count();

Console.WriteLine(overlapp);

Dictionary<string, int> mapping2 = MappingService.MapValuesDiag(positions);

overlapp = mapping2.Where(x => x.Value >= 2).Count();

Console.WriteLine(overlapp);