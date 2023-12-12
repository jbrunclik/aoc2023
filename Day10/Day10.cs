namespace Aoc2023.Day10;

partial class Day10 : Solution
{
    char[,] ParseInput()
    {
        var rows = Input.Length;
        var cols = Input[0].Length;

        var map = new char[rows, cols];
        Input
            .Select(
                (line, rowIndex) =>
                    line.Select((ch, colIndex) => map[rowIndex, colIndex] = ch).ToArray()
            )
            .ToArray();
        return map;
    }

    (int, int) GetStartCoords(char[,] map)
    {
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 'S')
                    return (i, j);
            }
        }
        throw new ApplicationException("Unable to find start coords");
    }

    Dictionary<(int, int), int> FindLoop(char[,] map, (int, int) startCoords)
    {
        var distances = new Dictionary<(int, int), int>();
        var queue = new Queue<((int, int), int)>();

        queue.Enqueue((startCoords, 0));

        while (queue.Count > 0)
        {
            var (currentCoords, distance) = queue.Dequeue();
            distances[currentCoords] = distance;

            foreach (
                var candidate in new Dictionary<(int, int), char[]>
                {
                    { (-1, 0), "|F7S".ToCharArray() },
                    { (0, -1), "-LFS".ToCharArray() },
                    { (0, 1), "-J7S".ToCharArray() },
                    { (1, 0), "|LJS".ToCharArray() }
                }
            )
            {
                {
                    var row = currentCoords.Item1 + candidate.Key.Item1;
                    var col = currentCoords.Item2 + candidate.Key.Item2;

                    if (row < 0 || row >= map.GetLength(0) || col < 0 || col >= map.GetLength(1))
                        continue;

                    if (
                        !distances.ContainsKey((row, col))
                        && candidate.Value.Contains(map[row, col])
                    )
                        queue.Enqueue(((row, col), distance + 1));
                }
            }
        }

        return distances;
    }

    static bool IsInsideLoop(
        int row,
        int col,
        char[,] map,
        List<(int, int)> loopCoordinates,
        int rows,
        int cols
    )
    {
        for (int j = col; j < cols; j++)
        {
            if (loopCoordinates.Contains((row, j)))
            {
                return true;
            }
        }
        return false;
    }

    public override string Part1()
    {
        var map = ParseInput();
        var startCoords = GetStartCoords(map);
        var loop = FindLoop(map, startCoords);
        return loop.Values.Max().ToString();
    }

    public override string Part2() => "";
}
