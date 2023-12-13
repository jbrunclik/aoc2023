namespace Aoc2023.Day12;

partial class Day12 : Solution
{
    List<(string, List<int>)> ParseInput(bool unfold)
    {
        var springs = new List<(string, List<int>)>();
        foreach (var line in Input)
        {
            var lineParts = line.Split(" ", 2);
            var records = lineParts[0];
            var groups = lineParts[1];

            if (unfold)
            {
                records = string.Join('?', Enumerable.Repeat(records, 5));
                groups = string.Join(',', Enumerable.Repeat(groups, 5));
            }

            springs.Add((records, groups.Split(",").Select(int.Parse).ToList()));
        }
        return springs;
    }

    long ComputeValidArrangements(
        string records,
        List<int> groups,
        int recordIndex,
        int groupIndex,
        int groupLength,
        ref Dictionary<(int, int, int), long> cache
    )
    {
        var key = (recordIndex, groupIndex, groupLength);
        if (cache.ContainsKey(key))
            return cache[key];

        if (recordIndex == records.Length)
        {
            if (groupIndex == groups.Count && groupLength == 0)
                return 1;
            else if (groupIndex == groups.Count - 1 && groups[groupIndex] == groupLength)
                return 1;
            else
                return 0;
        }

        var arrangements = 0L;
        foreach (var variant in new char[] { '.', '#' })
        {
            if (records[recordIndex] == variant || records[recordIndex] == '?')
            {
                if (variant == '.' && groupLength == 0)
                    arrangements += ComputeValidArrangements(
                        records,
                        groups,
                        recordIndex + 1,
                        groupIndex,
                        0,
                        ref cache
                    );
                else if (
                    variant == '.'
                    && groupLength > 0
                    && groupIndex < groups.Count
                    && groups[groupIndex] == groupLength
                )
                    arrangements += ComputeValidArrangements(
                        records,
                        groups,
                        recordIndex + 1,
                        groupIndex + 1,
                        0,
                        ref cache
                    );
                else if (variant == '#')
                    arrangements += ComputeValidArrangements(
                        records,
                        groups,
                        recordIndex + 1,
                        groupIndex,
                        groupLength + 1,
                        ref cache
                    );
            }
        }

        cache[key] = arrangements;
        return arrangements;
    }

    public override string Part1()
    {
        var springs = ParseInput(false);
        var sum = 0L;
        foreach (var (records, groups) in springs)
        {
            var cache = new Dictionary<(int, int, int), long>();
            sum += ComputeValidArrangements(records, groups, 0, 0, 0, ref cache);
        }
        return sum.ToString();
    }

    public override string Part2()
    {
        var springs = ParseInput(true);
        var sum = 0L;
        foreach (var (records, groups) in springs)
        {
            var cache = new Dictionary<(int, int, int), long>();
            sum += ComputeValidArrangements(records, groups, 0, 0, 0, ref cache);
        }
        return sum.ToString();
    }
}
