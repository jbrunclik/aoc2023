using System.Text.RegularExpressions;

namespace Aoc2023.Day9;

partial class Day9 : Solution
{
    List<List<int>> ParseInput() =>
        Input.Select(l => l.Split(" ").Select(int.Parse).ToList()).ToList();

    List<int> GenerateDiffs(List<int> line) =>
        line.Select((value, index) => new { value, index })
            .Skip(1)
            .Select(x => x.value - line[x.index - 1])
            .ToList();

    bool IsAllZeroes(List<int> line) => line.All(x => x == 0);

    List<List<int>> GeneratePyramid(List<int> record)
    {
        var pyramid = new List<List<int>>();
        var currentLine = record;
        pyramid.Add(currentLine);
        while (!IsAllZeroes(currentLine))
        {
            currentLine = GenerateDiffs(currentLine);
            pyramid.Add(currentLine);
        }
        pyramid.Reverse();
        return pyramid;
    }

    public override string Part1()
    {
        var reports = ParseInput();
        var sum = 0;
        foreach (var report in reports)
        {
            var pyramid = GeneratePyramid(report);
            for (var i = 0; i < pyramid.Count; i++)
                pyramid[i].Add(i == 0 ? 0 : pyramid[i].Last() + pyramid[i - 1].Last());
            sum += pyramid.Last().Last();
        }
        return sum.ToString();
    }

    public override string Part2()
    {
        var reports = ParseInput();
        var sum = 0;
        foreach (var report in reports)
        {
            var pyramid = GeneratePyramid(report);
            for (var i = 0; i < pyramid.Count; i++)
                pyramid[i].Insert(0, i == 0 ? 0 : pyramid[i].First() - pyramid[i - 1].First());
            sum += pyramid.Last().First();
        }
        return sum.ToString();
    }
}
