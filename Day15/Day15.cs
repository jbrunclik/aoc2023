using System.Text.RegularExpressions;

namespace Aoc2023.Day15;

partial class Day15 : Solution
{
    List<string> ParseInput() => Input[0].Split(",").ToList();

    int ComputeHash(string step)
    {
        var hash = 0;
        foreach (var ch in step)
        {
            hash += ch;
            hash *= 17;
            hash %= 256;
        }
        return hash;
    }

    Dictionary<int, List<Lens>> AssignLenses(List<string> steps)
    {
        var boxes = Enumerable.Range(0, 256).ToDictionary(key => key, value => new List<Lens>());
        foreach (var step in steps)
        {
            var match = StepRegex().Match(step);
            var label = match.Groups["label"].Value;
            var box = ComputeHash(label);
            var lensIndex = boxes[box].FindIndex(l => l.Label == label);
            var op = match.Groups["op"].Value;
            switch (op)
            {
                case "-":
                    if (lensIndex > -1)
                        boxes[box].RemoveAt(lensIndex);
                    break;
                case "=":
                    var lens = new Lens(label, int.Parse(match.Groups["lens"].Value));
                    if (lensIndex == -1)
                        boxes[box].Add(lens);
                    else
                        boxes[box][lensIndex] = lens;
                    break;
                default:
                    throw new ApplicationException($"Unknown operation {op}");
            }
        }
        return boxes;
    }

    int CountFocusingPower(Dictionary<int, List<Lens>> boxes) =>
        boxes
            .SelectMany(
                box => box.Value.Select((lens, j) => (box.Key + 1) * (j + 1) * lens.FocalLength)
            )
            .Sum();

    public override string Part1() => ParseInput().Select(ComputeHash).Sum().ToString();

    public override string Part2() => CountFocusingPower(AssignLenses(ParseInput())).ToString();

    record Lens(string Label, int FocalLength);

    [GeneratedRegex(@"(?<label>[a-z]+)(?<op>[-=])(?<lens>\d)?")]
    private static partial Regex StepRegex();
}
