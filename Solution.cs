namespace Aoc2023;

abstract class Solution
{
    public string[] Input { get; set; } = [];

    public void ParseInput(string inputPath)
    {
        Console.WriteLine($"Parsing input file \"{inputPath}\"...");
        Input = File.ReadAllLines(inputPath);
    }

    public abstract string Part1();
    public abstract string Part2();
}
