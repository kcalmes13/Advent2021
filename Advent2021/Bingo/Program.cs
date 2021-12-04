using Bingo;

List<string> inputs = System.IO.File.ReadLines(@"c:\advent\day4\input.txt").ToList();

// build your draws
List<int> draws = inputs.First().Split(',').Select(x => int.Parse(x)).ToList();
inputs.RemoveRange(0, 2);

// get index of first empty (can use directly thank to 1 row being index 0)
int rowCount =  inputs.IndexOf(string.Empty);
inputs.RemoveAll(x => x.Equals(string.Empty));

// build boards
List<int[][]> boards = CheckBingo.BuildBoards(inputs, rowCount);

int count = -1;
int[][] winningBoard = new int[5][];

foreach(int[][] board in boards)
{
    var lowestBingo = CheckBingo.TimeToBingo(board, draws);


    if (count == -1 || lowestBingo < count)
    {
        count = lowestBingo;
        winningBoard = board;
    }
}

List<int> neededDraws = draws.Take(count).ToList();
int total = CheckBingo.CalculateScore(winningBoard, neededDraws);

Console.WriteLine(total);

count = -1;
int[][] losingBoard = new int[5][];

foreach (int[][] board in boards)
{
    var highestBingo = CheckBingo.TimeToBingo(board, draws);

    if (count == -1 || highestBingo > count)
    {
        count = highestBingo;
        losingBoard = board;
    }
}

neededDraws = draws.Take(count).ToList();
total = CheckBingo.CalculateScore(losingBoard, neededDraws);

Console.WriteLine(total);