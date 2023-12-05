using System.Text.RegularExpressions;

namespace Aoc2023.Day5;

class Day5 : Solution
{
    private record Translation(long SourceStart, long DestinationStart, long RangeLength);

    private (List<long>, Dictionary<string, List<Translation>>) ParseInput()
    {
        var seedsToPlant = new List<long> { };
        var translations = new Dictionary<string, List<Translation>>();

        var mapType = "";
        foreach (string line in Input)
        {
            if (line == "")
            {
                continue;
            }
            else if (line.StartsWith("seeds:"))
            {
                foreach (
                    var s in line.Split(":")
                        .ElementAt(1)
                        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                )
                {
                    seedsToPlant.Add(long.Parse(s));
                }
            }
            else if (line.EndsWith("map:"))
            {
                mapType = line.Split(" ").ElementAt(0);
                translations[mapType] = new List<Translation>();
            }
            else
            {
                var lineSplit = line.Split(" ");
                var destinationStart = long.Parse(lineSplit.ElementAt(0));
                var sourceStart = long.Parse(lineSplit.ElementAt(1));
                var rangeLength = long.Parse(lineSplit.ElementAt(2));

                translations[mapType].Add(
                    new Translation(sourceStart, destinationStart, rangeLength)
                );
            }
        }

        return (seedsToPlant, translations);
    }

    public override string Part1()
    {
        (var seedsToPlant, var translations) = ParseInput();
        var steps = new List<string>
        {
            "seed-to-soil",
            "soil-to-fertilizer",
            "fertilizer-to-water",
            "water-to-light",
            "light-to-temperature",
            "temperature-to-humidity",
            "humidity-to-location",
        };

        var destinations = new List<long>();
        foreach (var seed in seedsToPlant)
        {
            long curVal = seed;
            foreach (var step in steps)
            {
                foreach (var translation in translations[step])
                {
                    if (
                        curVal >= translation.SourceStart
                        && curVal < translation.SourceStart + translation.RangeLength
                    )
                    {
                        long newVal =
                            (curVal - translation.SourceStart) + translation.DestinationStart;
                        curVal = newVal;
                        break;
                    }
                }
            }
            destinations.Add(curVal);
        }

        return destinations.Min().ToString();
    }

    private record SeedRange(long RangeStart, long RangeLength);

    public override string Part2()
    {
        (var seedsToPlant, var translations) = ParseInput();
        var steps = new List<string>
        {
            "humidity-to-location",
            "temperature-to-humidity",
            "light-to-temperature",
            "water-to-light",
            "fertilizer-to-water",
            "soil-to-fertilizer",
            "seed-to-soil",
        };

        var seedRanges = new List<SeedRange>();
        for (var i = 0; i < seedsToPlant.Count; i += 2)
        {
            var seedRange = new SeedRange(seedsToPlant[i], seedsToPlant[i + 1]);
            seedRanges.Add(seedRange);
        }

        for (var i = 1; true; i++)
        {
            long curVal = i;
            foreach (var step in steps)
            {
                foreach (var translation in translations[step])
                {
                    if (
                        curVal >= translation.DestinationStart
                        && curVal < translation.DestinationStart + translation.RangeLength
                    )
                    {
                        long newVal =
                            (curVal - translation.DestinationStart) + translation.SourceStart;
                        curVal = newVal;
                        break;
                    }
                }
            }

            foreach (var seedRange in seedRanges)
            {
                if (
                    curVal >= seedRange.RangeStart
                    && curVal < seedRange.RangeStart + seedRange.RangeLength
                )
                {
                    return i.ToString();
                }
            }
        }
    }
}
