namespace Aoc2023.Day13;

partial class Day13 : Solution
{
    List<char[,]> ParseInput() =>
        string.Join("\n", Input)
            .Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(section =>
            {
                var rows = section.Split('\n');
                var rowCount = rows.Length;
                var colCount = rows[0].Length;
                var pattern = new char[rowCount, colCount];

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        pattern[i, j] = rows[i][j];
                    }
                }
                return pattern;
            })
            .ToList();

    static int CountDifferentChars(string a, string b) =>
        a.Zip(b, (c1, c2) => c1 != c2).Count(diff => diff);

    int FindReflection(char[,] pattern, int desiredDifferences, bool isHorizontal)
    {
        var lines = Enumerable
            .Range(0, isHorizontal ? pattern.GetLength(0) : pattern.GetLength(1))
            .Select(
                index =>
                    new string(
                        Enumerable
                            .Range(0, isHorizontal ? pattern.GetLength(1) : pattern.GetLength(0))
                            .Select(i => isHorizontal ? pattern[index, i] : pattern[i, index])
                            .ToArray()
                    )
            )
            .ToList();

        for (var lineNum = 0; lineNum < lines.Count - 1; lineNum++)
        {
            var differentCharacters = 0;
            for (var offset = 0; offset <= lineNum; offset++)
            {
                if (lineNum - offset < 0 || lineNum + offset + 1 >= lines.Count)
                    continue;

                differentCharacters += CountDifferentChars(
                    lines[lineNum - offset],
                    lines[lineNum + offset + 1]
                );
            }
            if (differentCharacters == desiredDifferences)
                return lineNum + 1;
        }

        return 0;
    }

    public override string Part1()
    {
        var patterns = ParseInput();
        var sum = 0;
        foreach (var pattern in patterns)
        {
            sum += FindReflection(pattern, 0, true) * 100;
            sum += FindReflection(pattern, 0, false);
        }
        return sum.ToString();
    }

    public override string Part2()
    {
        var patterns = ParseInput();
        var sum = 0;
        foreach (var pattern in patterns)
        {
            sum += FindReflection(pattern, 1, true) * 100;
            sum += FindReflection(pattern, 1, false);
        }
        return sum.ToString();
    }
}
