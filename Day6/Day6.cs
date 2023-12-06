using System.Text.RegularExpressions;

namespace Aoc2023.Day6;

class Day6 : Solution
{
    private static List<long> ParseLine1(string line)
    {
        var parsedLine = new List<long>();
        var lineSplit = line.Split(":");
        foreach (var n in lineSplit.ElementAt(1).Split(" ", StringSplitOptions.RemoveEmptyEntries))
        {
            parsedLine.Add(long.Parse(n));
        }
        return parsedLine;
    }

    private static List<long> ParseLine2(string line)
    {
        var parsedLine = new List<string>();
        var lineSplit = line.Split(":");
        foreach (var n in lineSplit.ElementAt(1).Split(" ", StringSplitOptions.RemoveEmptyEntries))
        {
            parsedLine.Add(n);
        }
        return new List<long> { long.Parse(string.Join("", parsedLine)) };
    }

    private long Solve(List<long> times, List<long> distances)
    {
        var validSolutions = new List<long>();
        for (var race = 0; race < times.Count; race++)
        {
            var time = times[race];
            var maxDistance = distances[race];

            var valid = 0;
            for (var i = 0; i <= time; i++)
            {
                var heldMs = i;
                var racedMs = time - i;
                var distance = racedMs * heldMs;

                if (distance > maxDistance)
                {
                    valid += 1;
                }
            }
            validSolutions.Add(valid);
        }

        return validSolutions.Aggregate((x, y) => x * y);
    }

    public override string Part1()
    {
        var times = ParseLine1(Input.ElementAt(0));
        var distances = ParseLine1(Input.ElementAt(1));
        return Solve(times, distances).ToString();
    }

    public override string Part2()
    {
        var times = ParseLine2(Input.ElementAt(0));
        var distances = ParseLine2(Input.ElementAt(1));
        return Solve(times, distances).ToString();
    }
}
