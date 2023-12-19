using System.Text.RegularExpressions;

namespace Aoc2023.Day19;

record Condition(string Target, string? Param = null, string? Op = null, int? Value = null);

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

    public override string Part2() => "";

    [GeneratedRegex(
        @"(?<name>[a-z]+){(((?<param>[a-z]+)(?<op>[<>])(?<value>[0-9]+):)?(?<target>[a-zAR]+),?)+"
    )]
    private static partial Regex WorkflowRegex();

    [GeneratedRegex(@"{((?<param>[a-z]+)=(?<value>[0-9]+),?)+}")]
    private static partial Regex PartRegex();
}
