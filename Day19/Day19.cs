using System.Text.RegularExpressions;

namespace Aoc2023.Day19;

record Condition(string Target, string? Param = null, string? Op = null, int? Value = null);

record Range(int First, int Last);

partial class Day19 : Solution
{
    (Dictionary<string, List<Condition>>, List<Dictionary<string, int>>) ParseInput()
    {
        var workflows = new Dictionary<string, List<Condition>>();
        var parts = new List<Dictionary<string, int>>();

        var isWorkflow = true;
        foreach (var line in Input)
        {
            if (line == "")
            {
                isWorkflow = false;
                continue;
            }

            if (isWorkflow)
            {
                var match = WorkflowRegex().Match(line);
                var conditions = new List<Condition>();

                var name = match.Groups["name"].Value;
                var numConditions = match.Groups["target"].Captures.Count;

                for (var i = 0; i < numConditions; i++)
                {
                    Condition condition;
                    if (i == numConditions - 1)
                    {
                        var target = match.Groups["target"].Captures[i].Value;
                        condition = new Condition(Target: target);
                    }
                    else
                    {
                        var param = match.Groups["param"].Captures[i].Value;
                        var op = match.Groups["op"].Captures[i].Value;
                        var value = int.Parse(match.Groups["value"].Captures[i].Value);
                        var target = match.Groups["target"].Captures[i].Value;
                        condition = new Condition(
                            Target: target,
                            Param: param,
                            Op: op,
                            Value: value
                        );
                    }

                    conditions.Add(condition);
                }

                workflows[name] = conditions;
            }
            else
            {
                var part = new Dictionary<string, int>();
                var match = PartRegex().Match(line);
                var numValues = match.Groups["value"].Captures.Count;
                for (var i = 0; i < numValues; i++)
                {
                    var param = match.Groups["param"].Captures[i].Value;
                    var value = int.Parse(match.Groups["value"].Captures[i].Value);
                    part[param] = value;
                }
                parts.Add(part);
            }
        }

        return (workflows, parts);
    }

    string RunWorkflow(List<Condition> conditions, Dictionary<string, int> part)
    {
        foreach (var condition in conditions)
        {
            if (condition.Op == null)
                return condition.Target;
            else
            {
                var partValue = part[condition.Param!];
                if (condition.Op == "<" && partValue < condition.Value)
                    return condition.Target;
                else if (condition.Op == ">" && partValue > condition.Value)
                    return condition.Target;
            }
        }
        throw new ApplicationException("Unknown state");
    }

    bool IsAccepted(Dictionary<string, List<Condition>> workflows, Dictionary<string, int> part)
    {
        var currentWorkflow = workflows["in"];
        while (true)
        {
            var nextWorkflow = RunWorkflow(currentWorkflow, part);
            if (nextWorkflow == "A")
                return true;
            else if (nextWorkflow == "R")
                return false;
            else
                currentWorkflow = workflows[nextWorkflow];
        }
    }

    int SumRatings(Dictionary<string, int> part) => part.Values.Sum();

    public override string Part1()
    {
        var (workflows, parts) = ParseInput();
        return parts.Where(p => IsAccepted(workflows, p)).Select(SumRatings).Sum().ToString();
    }

    (Range, Range) SplitRange(Range range, string op, int value)
    {
        if (op == "<")
            return (new Range(range.First, value - 1), new Range(value, range.Last));
        else if (op == ">")
            return (new Range(range.First, value), new Range(value + 1, range.Last));
        throw new ApplicationException($"Uknown operation {op}");
    }

    List<Dictionary<string, Range>> FindValidRanges(Dictionary<string, List<Condition>> workflows)
    {
        var validRanges = new List<Dictionary<string, Range>>();

        var queue = new Queue<(Dictionary<string, Range>, string)>();
        queue.Enqueue(
            (
                new Dictionary<string, Range>
                {
                    { "x", new Range(1, 4000) },
                    { "m", new Range(1, 4000) },
                    { "a", new Range(1, 4000) },
                    { "s", new Range(1, 4000) },
                },
                "in"
            )
        );

        while (queue.Count > 0)
        {
            var (ranges, workflow) = queue.Dequeue();

            if (workflow == "A")
            {
                validRanges.Add(ranges);
                continue;
            }
            else if (workflow == "R")
                continue;

            foreach (var condition in workflows[workflow])
            {
                if (condition.Op == null)
                    queue.Enqueue((ranges, condition.Target));
                else
                {
                    var (left, right) = SplitRange(
                        ranges[condition.Param!],
                        condition.Op,
                        (int)condition.Value!
                    );

                    ranges.Remove(condition.Param!);
                    var newRanges = new Dictionary<string, Range>(ranges);

                    if (condition.Op == "<")
                    {
                        ranges[condition.Param!] = right;
                        newRanges[condition.Param!] = left;
                    }
                    else if (condition.Op == ">")
                    {
                        ranges[condition.Param!] = left;
                        newRanges[condition.Param!] = right;
                    }

                    queue.Enqueue((newRanges, condition.Target));
                }
            }
        }

        return validRanges;
    }

    int RangeLength(Range range) => range.Last - range.First + 1;

    long RangeProduct(Dictionary<string, Range> ranges) =>
        ranges.Values.Aggregate(1L, (current, range) => current * RangeLength(range));

    public override string Part2()
    {
        var (workflows, _) = ParseInput();
        return FindValidRanges(workflows).Select(r => RangeProduct(r)).Sum().ToString();
    }

    [GeneratedRegex(
        @"(?<name>[a-z]+){(((?<param>[a-z]+)(?<op>[<>])(?<value>[0-9]+):)?(?<target>[a-zAR]+),?)+"
    )]
    private static partial Regex WorkflowRegex();

    [GeneratedRegex(@"{((?<param>[a-z]+)=(?<value>[0-9]+),?)+}")]
    private static partial Regex PartRegex();
}
