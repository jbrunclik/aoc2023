namespace Aoc2023.Day14;

partial class Day14 : Solution
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

    char[,] Tilt(char[,] platform)
    {
        var tiltedPlatform = new char[platform.GetLength(0), platform.GetLength(1)];

        // Copy all cube-shaped rocks and empty space
        for (var row = 0; row < platform.GetLength(0); row++)
        {
            for (var col = 0; col < platform.GetLength(1); col++)
            {
                if (platform[row, col] == '#' || platform[row, col] == '.')
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
                    tiltedPlatform[row, col] = '.'; // Replace with empty space
                    tiltedPlatform[newRow, col] = 'O'; // Move to the new place
                }
            }
        }

        return tiltedPlatform;
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

    public override string Part1()
    {
        var platform = ParseInput();
        var tiltedPlatform = Tilt(platform);
        var load = ComputeLoad(tiltedPlatform);
        return load.ToString();
    }

    public override string Part2() => "";
}
