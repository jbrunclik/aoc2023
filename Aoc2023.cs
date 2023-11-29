using System.CommandLine;

namespace Aoc2023;

class Aoc2023
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Advent of Code 2023");

        var dayArgument = new Argument<uint>(name: "day", description: "Day of the month");
        rootCommand.AddArgument(dayArgument);

        rootCommand.SetHandler(
            (day) =>
            {
                Solve(day);
            },
            dayArgument
        );

        return await rootCommand.InvokeAsync(args);
    }

    static void Solve(uint day)
    {
        Console.WriteLine($"Solving day {day}...");

        Type? dayType = Type.GetType($"Aoc2023.Day{day}.Day{day}");
        if (dayType == null)
        {
            throw new NotImplementedException($"Solution for Day {day} not implemented");
        }

        Solution solution = (Solution?)Activator.CreateInstance(dayType)!;

        // Parse input data.
        string inputPath = Path.Combine(
            System.Environment.CurrentDirectory,
            $"day{day}",
            "input.txt"
        );
        solution.ParseInput(inputPath);

        Console.WriteLine($"Part 1 solution: {solution.Part1()}");
        Console.WriteLine($"Part 2 solution: {solution.Part2()}");
    }
}
