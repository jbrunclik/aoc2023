using System.Text.RegularExpressions;

namespace Aoc2023.Day8;

partial class Day8 : Solution
{
    private (char[], Dictionary<string, (string, string)>) ParseInput()
    {
        var instructions = Input[0].ToCharArray();
        var mapNodes = new Dictionary<string, (string, string)>();

        foreach (var line in Input.Skip(2))
        {
            var match = MapRegex().Match(line);
            mapNodes[match.Groups["node"].Value] = (
                match.Groups["left"].Value,
                match.Groups["right"].Value
            );
        }

        return (instructions, mapNodes);
    }

    long StepsToLastNode(
        char[] instructions,
        Dictionary<string, (string, string)> mapNodes,
        string startNode,
        IsLastNode isLastNode
    )
    {
        long steps = 0;
        var offset = 0;
        var node = startNode;

        while (!isLastNode(node))
        {
            var instruction = instructions[offset];
            var (left, right) = mapNodes[node];
            node = instruction == 'L' ? left : right;
            offset = (offset + 1) % instructions.Length;
            steps++;
        }

        return steps;
    }

    delegate bool IsLastNode(string nodeName);

    public override string Part1()
    {
        var (instructions, mapNodes) = ParseInput();
        return StepsToLastNode(instructions, mapNodes, "AAA", n => n == "ZZZ").ToString();
    }

    static long GCD(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long LCM(long a, long b)
    {
        return (a / GCD(a, b)) * b;
    }

    public override string Part2()
    {
        var (instructions, mapNodes) = ParseInput();
        var stepsToLastNodes = mapNodes
            .Keys
            .Where(n => n.EndsWith("A"))
            .Select(n => StepsToLastNode(instructions, mapNodes, n, n => n.EndsWith("Z")));
        return stepsToLastNodes.Aggregate(1L, (a, b) => LCM(a, b)).ToString();
    }

    [GeneratedRegex(@"(?<node>[A-Z]+) = \((?<left>[A-Z]+), (?<right>[A-Z]+)\)")]
    private static partial Regex MapRegex();
}
