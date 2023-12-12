namespace Aoc2023.Day12;

partial class Day12 : Solution
{
    List<SpringCondition> ParseInput() =>
        Input
            .Select(line =>
            {
                var lineSplit = line.Split(" ", 2);
                var records = lineSplit[0];
                var damagedGroups = lineSplit[1].Split(",").Select(int.Parse).ToList();
                return new SpringCondition(records, damagedGroups);
            })
            .ToList();

    void GenerateCombinations(string records, int index, ref List<String> variants)
    {
        var unknownIndex = records.IndexOf('?', index);
        if (unknownIndex == -1)
        {
            variants.Add(records);
            return;
        }

        var withDot =
            records.Substring(0, unknownIndex) + "." + records.Substring(unknownIndex + 1);
        var withHash =
            records.Substring(0, unknownIndex) + "#" + records.Substring(unknownIndex + 1);

        GenerateCombinations(withDot, unknownIndex + 1, ref variants);
        GenerateCombinations(withHash, unknownIndex + 1, ref variants);
    }

    List<string> GenerateVariants(string records)
    {
        var variants = new List<string>();
        GenerateCombinations(records, 0, ref variants);
        return variants;
    }

    List<int> CountHahshes(string records) =>
        records
            .Select((c, index) => new { Char = c, Index = index })
            .GroupBy(
                x =>
                    new { x.Char, IndexDiff = x.Index - records.Take(x.Index).Count(y => y == '#') }
            )
            .Where(g => g.Key.Char == '#')
            .Select(g => g.Count())
            .ToList();

    bool IsValidVariant(string records, List<int> damagedGroups) =>
        CountHahshes(records).SequenceEqual(damagedGroups);

    public override string Part1()
    {
        var springConditions = ParseInput();
        return springConditions
            .SelectMany(
                s => GenerateVariants(s.Records).Where(v => IsValidVariant(v, s.DamagedGroups))
            )
            .Count()
            .ToString();
    }

    public override string Part2() => "";

    record SpringCondition(string Records, List<int> DamagedGroups);
}
