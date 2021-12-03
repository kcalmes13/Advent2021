using PilotSub;

List<string> inputs = System.IO.File.ReadLines(@"c:\advent\day2\input.txt").ToList();
List<ShipCommand> commands = inputs.Select(x => new ShipCommand()
{
    Direction = x.Split(' ')[0],
    Amount = double.Parse(x.Split(' ')[1])
}).ToList();

var up = commands.Where(x => x.Direction.Equals("up", StringComparison.OrdinalIgnoreCase)).Select(x => x.Amount).Sum();
var down = commands.Where(x => x.Direction.Equals("down", StringComparison.OrdinalIgnoreCase)).Select(x => x.Amount).Sum();
var forward = commands.Where(x => x.Direction.Equals("forward", StringComparison.OrdinalIgnoreCase)).Select(x => x.Amount).Sum();

var total = (down - up) * forward;

Console.WriteLine(total);

Position position = new Position()
{
    Aim = 0,
    Depth = 0,
    Horizontal = 0
};

foreach(var command in commands)
{  
    if (command.Direction.Equals("up", StringComparison.OrdinalIgnoreCase))
        position.Aim -= command.Amount;

    if (command.Direction.Equals("down", StringComparison.OrdinalIgnoreCase))
        position.Aim += command.Amount;

    if (command.Direction.Equals("forward", StringComparison.OrdinalIgnoreCase))
    {
        position.Horizontal += command.Amount;
        position.Depth += (position.Aim * command.Amount);
    }        
}

total = position.Horizontal * position.Depth;

Console.WriteLine(total);