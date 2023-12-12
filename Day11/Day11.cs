using System.Text.RegularExpressions;

namespace Aoc2023.Day11;

partial class Day11 : Solution
{
    char[,] ParseInput()
    {
        int rows = Input.Length;
        int cols = Input[0].Length;

        var map = new char[rows, cols];
        Input
            .Select(
                (line, rowIndex) =>
                    line.Select((ch, colIndex) => map[rowIndex, colIndex] = ch).ToArray()
            )
            .ToArray();
        return map;
    }

    List<(int, int)> GetGalaxyCoords(char[,] map)
    {
        var galaxyCoords = new List<(int, int)>();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == '#')
                {
                    galaxyCoords.Add((i, j));
                }
            }
        }
        return galaxyCoords;
    }

    (List<int>, List<int>) GetEmpty(char[,] map)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        var emptyRows = Enumerable
            .Range(0, rows)
            .Where(i => Enumerable.Range(0, cols).All(j => map[i, j] == '.'))
            .ToList();
        var emptyCols = Enumerable
            .Range(0, cols)
            .Where(j => Enumerable.Range(0, rows).All(i => map[i, j] == '.'))
            .ToList();

        return (emptyRows, emptyCols);
    }

    public static List<int> CreateRange(int start, int end)
    {
        int min = Math.Min(start, end);
        int max = Math.Max(start, end);

        return Enumerable.Range(min, max - min + 1).ToList();
    }

    List<long> CalculateDistances(
        char[,] map,
        List<(int, int)> galaxyCoords,
        List<int> emptyRows,
        List<int> emptyCols,
        int expansion
    )
    {
        var distances = new List<long>();
        for (int i = 0; i < galaxyCoords.Count; i++)
        {
            for (int j = i + 1; j < galaxyCoords.Count; j++)
            {
                var distance =
                    Math.Abs(galaxyCoords[i].Item1 - galaxyCoords[j].Item1)
                    + Math.Abs(galaxyCoords[i].Item2 - galaxyCoords[j].Item2);

                distance +=
                    CreateRange(galaxyCoords[i].Item1, galaxyCoords[j].Item1)
                        .Where(r => emptyRows.Contains(r))
                        .Count() * expansion;
                distance +=
                    CreateRange(galaxyCoords[i].Item2, galaxyCoords[j].Item2)
                        .Where(r => emptyCols.Contains(r))
                        .Count() * expansion;

                distances.Add(distance);
            }
        }
        return distances;
    }

    public override string Part1()
    {
        var map = ParseInput();
        var galaxyCoords = GetGalaxyCoords(map);
        var (emptyRows, emptyCols) = GetEmpty(map);
        var distances = CalculateDistances(map, galaxyCoords, emptyRows, emptyCols, 1);
        return distances.Sum().ToString();
    }

    public override string Part2()
    {
        var map = ParseInput();
        var galaxyCoords = GetGalaxyCoords(map);
        var (emptyRows, emptyCols) = GetEmpty(map);
        var distances = CalculateDistances(map, galaxyCoords, emptyRows, emptyCols, 999999);
        return distances.Sum().ToString();
    }
}
