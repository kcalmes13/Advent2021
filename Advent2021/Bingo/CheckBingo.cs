using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingo
{
    internal static class CheckBingo
    {
        internal static List<int[][]> BuildBoards(List<string> inputs, int rowsPerBoard)
        {
            // build boards
            List<int[][]> boards = new List<int[][]>();

            while (inputs.Count > 0)
            {
                List<string> boardVals = inputs.Take(rowsPerBoard).ToList();
                int[][] board = new int[rowsPerBoard][];

                for (int i = 0; i < boardVals.Count; i++)
                {
                    var vals = boardVals[i].Split().ToList();
                    vals.RemoveAll(x => x.Equals(string.Empty));
                    board[i] = vals.Select(x => int.Parse(x)).ToArray();
                }

                boards.Add(board);
                inputs.RemoveRange(0, 5);
            }
            
            return boards;
        }

        internal static int TimeToBingo(int[][] board, List<int> draws)
        {
            List<List<int>> boardCombos = new List<List<int>>();

            for (int i = 0; i < board.Length; i++)
            {
                boardCombos.Add(board[i].ToList());
                List<int> row = new List<int>();

                for (int j = 0; j < board[i].Length; j++)
                {
                    row.Add(board[j][i]);
                }

                boardCombos.Add(row);
            }

            List<int> drawn = draws.Take(board.Length).ToList();
            int count = board.Length;
            while (!boardCombos.Any(x => x.All(z => drawn.Contains(z))) || count > draws.Count)
            {
                count++;
                drawn.Add(draws[count - 1]);
            }

            return count;
        }

        internal static int CalculateScore(int[][] board, List<int> draws)
        {
            int total = 0;

            foreach (var row in board)
            {
                total += row.Where(x => !draws.Contains(x)).Sum();
            }

            total = total * draws.Last();

            return total;
        }
    }    
}
