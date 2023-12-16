namespace Aoc2023.Day14;

partial class Day14 : Solution
{
    char[,] ParseInput()
    {
        var rows = Input.Length;
        var cols = Input[0].Length;

        var map = new char[rows, cols];
        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for (int colIndex = 0; colIndex < cols; colIndex++)
            {
                char ch = Input[rowIndex][colIndex];
                if (ch == '#' || ch == 'O')
                    map[rowIndex, colIndex] = ch;
            }
        }

        return map;
    }

    char[,] Tilt(char[,] platform)
    {
        var tiltedPlatform = new char[platform.GetLength(0), platform.GetLength(1)];

        // Copy all cube-shaped rocks
        for (var row = 0; row < platform.GetLength(0); row++)
        {
            for (var col = 0; col < platform.GetLength(1); col++)
            {
                if (platform[row, col] == '#')
                {
                    tiltedPlatform[row, col] = platform[row, col];
                }
            }
        }

        // Place all rounded rocks
        for (var row = 0; row < platform.GetLength(0); row++)
        {
            for (var col = 0; col < platform.GetLength(1); col++)
            {
                if (platform[row, col] == 'O')
                {
                    int newRow = row;
                    while (newRow > 0)
                    {
                        if (
                            tiltedPlatform[newRow - 1, col] == '#'
                            || tiltedPlatform[newRow - 1, col] == 'O'
                        )
                            break;

                        newRow--;
                    }
                    tiltedPlatform[newRow, col] = 'O';
                }
            }
        }

        return tiltedPlatform;
    }

    char[,] Rotate(char[,] platform)
    {
        int rows = platform.GetLength(0);
        int cols = platform.GetLength(1);

        char[,] rotated = new char[cols, rows];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                rotated[j, rows - 1 - i] = platform[i, j];
            }
        }

        return rotated;
    }

    int ComputeLoad(char[,] platform)
    {
        var load = 0;
        for (var row = 0; row < platform.GetLength(0); row++)
        {
            var rowLoad = platform.GetLength(0) - row;
            for (var col = 0; col < platform.GetLength(1); col++)
            {
                if (platform[row, col] == 'O')
                    load += rowLoad;
            }
        }
        return load;
    }

    static string PlatformToString(char[,] platform) =>
        string.Join(
            "",
            Enumerable
                .Range(0, platform.GetLength(0))
                .Select(
                    row =>
                        new string(
                            Enumerable
                                .Range(0, platform.GetLength(1))
                                .Select(col => platform[row, col])
                                .ToArray()
                        )
                )
        );

    public override string Part1()
    {
        var platform = ParseInput();
        var tiltedPlatform = Tilt(platform);
        var load = ComputeLoad(tiltedPlatform);
        return load.ToString();
    }

    public override string Part2()
    {
        var platform = ParseInput();
        var seen = new Dictionary<string, int>();

        var totalIterations = 1_000_000_000;
        for (var i = 0; i < totalIterations; i++)
        {
            var k = PlatformToString(platform);
            if (!seen.TryGetValue(k, out int firstOccurence))
                seen[k] = i;
            else
            {
                var cycleLength = i - firstOccurence;
                var repeatCycle = (totalIterations - i) / cycleLength;
                i += cycleLength * repeatCycle;
            }

            for (var j = 0; j < 4; j++)
                platform = Rotate(Tilt(platform));
        }

        var load = ComputeLoad(platform);
        return load.ToString();
    }
}
