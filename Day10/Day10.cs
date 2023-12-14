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

    static readonly char[] LeftPipes = "-LF".ToCharArray();
    static readonly char[] RightPipes = "-J7".ToCharArray();
    static readonly char[] UpPipes = "|7F".ToCharArray();
    static readonly char[] DownPipes = "|LJ".ToCharArray();
    static readonly char[] NoPipes = [];

    char DetermineStartPipe(char[,] map, (int, int) startCoords)
    {
        var row = startCoords.Item1;
        var col = startCoords.Item2;

        // 6 possibilities
        //     |     |   |
        // -S- S S-  S- -S -S
        //     | |          |
        if (
            col > 0
            && col < map.GetLength(1) - 1
            && LeftPipes.Contains(map[row, col - 1])
            && RightPipes.Contains(map[row, col + 1])
        )
            return '-';
        else if (
            row > 0
            && row < map.GetLength(0) - 1
            && UpPipes.Contains(map[row - 1, col])
            && DownPipes.Contains(map[row + 1, col])
        )
            return '|';
        else if (
            row < map.GetLength(0) - 1
            && col < map.GetLength(1) - 1
            && RightPipes.Contains(map[row, col + 1])
            && DownPipes.Contains(map[row + 1, col])
        )
            return 'F';
        else if (
            row > 0
            && col < map.GetLength(1) - 1
            && RightPipes.Contains(map[row, col + 1])
            && UpPipes.Contains(map[row - 1, col])
        )
            return 'L';
        else if (
            row > 0
            && col > 0
            && LeftPipes.Contains(map[row, col - 1])
            && UpPipes.Contains(map[row - 1, col])
        )
            return 'J';
        else if (
            row < map.GetLength(0) - 1
            && col > 0
            && LeftPipes.Contains(map[row, col - 1])
            && DownPipes.Contains(map[row + 1, col])
        )
            return '7';

        throw new ApplicationException("Unable to determine type of the start pipe");
    }

    static readonly Dictionary<(int, int), Dictionary<char, char[]>> Candidates = new Dictionary<
        (int, int),
        Dictionary<char, char[]>
    >
    {
        {
            (-1, 0), // Up
            new Dictionary<char, char[]>
            {
                { '|', UpPipes },
                { '-', NoPipes },
                { 'L', UpPipes },
                { 'J', UpPipes },
                { '7', NoPipes },
                { 'F', NoPipes },
            }
        },
        {
            (0, 1), // Right
            new Dictionary<char, char[]>
            {
                { '|', NoPipes },
                { '-', RightPipes },
                { 'L', RightPipes },
                { 'J', NoPipes },
                { '7', NoPipes },
                { 'F', RightPipes },
            }
        },
        {
            (1, 0), // Down
            new Dictionary<char, char[]>
            {
                { '|', DownPipes },
                { '-', NoPipes },
                { 'L', NoPipes },
                { 'J', NoPipes },
                { '7', DownPipes },
                { 'F', DownPipes },
            }
        },
        {
            (0, -1), // Left
            new Dictionary<char, char[]>
            {
                { '|', NoPipes },
                { '-', LeftPipes },
                { 'L', NoPipes },
                { 'J', LeftPipes },
                { '7', LeftPipes },
                { 'F', NoPipes },
            }
        },
    };

    Dictionary<(int, int), int> FindLoop(char[,] map, (int, int) startCoords)
    {
        var distances = new Dictionary<(int, int), int>();
        var queue = new Queue<((int, int), int)>();

        queue.Enqueue((startCoords, 0));

        while (queue.Count > 0)
        {
            var (currentCoords, distance) = queue.Dequeue();
            var currentPipe = map[currentCoords.Item1, currentCoords.Item2];
            distances[currentCoords] = distance;

            foreach (var candidate in Candidates)
            {
                {
                    var row = currentCoords.Item1 + candidate.Key.Item1;
                    var col = currentCoords.Item2 + candidate.Key.Item2;
                    var validNeigbors = candidate.Value[currentPipe];

                    if (row < 0 || row >= map.GetLength(0) || col < 0 || col >= map.GetLength(1))
                        continue;

                    if (!distances.ContainsKey((row, col)) && validNeigbors.Contains(map[row, col]))
                        queue.Enqueue(((row, col), distance + 1));
                }
            }
        }

        return distances;
    }

    int CountInnerPoints(char[,] map, List<(int, int)> loopCoords)
    {
        // Use ray tracing to figure out if a point is inside the polygon
        // https://www.eecs.umich.edu/courses/eecs380/HANDOUTS/PROJ2/InsidePoly.html
        var sum = 0;
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (loopCoords.Contains((i, j)))
                    continue;

                var crossed = 0;
                for (var k = 0; k < j; k++)
                {
                    if (DownPipes.Contains(map[i, k]) && loopCoords.Contains((i, k)))
                        crossed++;
                }

                if (crossed % 2 == 1)
                    sum += 1;
            }
        }
        return sum;
    }

    public override string Part1()
    {
        var map = ParseInput();
        var startCoords = GetStartCoords(map);
        var startPipe = DetermineStartPipe(map, startCoords);
        map[startCoords.Item1, startCoords.Item2] = startPipe;
        var loop = FindLoop(map, startCoords);
        return loop.Values.Max().ToString();
    }

    public override string Part2()
    {
        var map = ParseInput();
        var startCoords = GetStartCoords(map);
        var startPipe = DetermineStartPipe(map, startCoords);
        map[startCoords.Item1, startCoords.Item2] = startPipe;
        var loop = FindLoop(map, startCoords);
        return CountInnerPoints(map, loop.Keys.ToList()).ToString();
    }
}
