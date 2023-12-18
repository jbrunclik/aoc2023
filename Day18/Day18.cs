namespace Aoc2023.Day18;

record Coords(int Row, int Col);

record Instruction(Coords Coords, int Steps, string Color);

partial class Day18 : Solution
{
    static Coords DirectionToCoords(string direction) =>
        direction switch
        {
            "L" => new Coords(Row: 0, Col: -1),
            "R" => new Coords(Row: 0, Col: 1),
            "U" => new Coords(Row: -1, Col: 0),
            "D" => new Coords(Row: 1, Col: 0),
            _ => throw new Exception($"Unknown direction {direction}")
        };

    List<Instruction> ParseInput()
    {
        var instructions = new List<Instruction>();
        foreach (var line in Input)
        {
            var lineParts = line.Split(' ', 3);
            var direction = lineParts[0];
            var steps = int.Parse(lineParts[1]);
            var color = lineParts[2]; // FIXME

            var coords = DirectionToCoords(direction);
            instructions.Add(new Instruction(Coords: coords, Steps: steps, Color: color));
        }
        return instructions;
    }

    static List<Coords> ExecuteInstructions(List<Instruction> instructions)
    {
        var row = 0;
        var col = 0;

        var minRow = int.MaxValue;
        var minCol = int.MaxValue;

        var trench = new List<Coords>();
        foreach (var instruction in instructions)
        {
            foreach (var x in Enumerable.Range(1, instruction.Steps))
            {
                row += instruction.Coords.Row;
                col += instruction.Coords.Col;
                trench.Add(new Coords(Row: row, Col: col));

                minRow = Math.Min(minRow, row);
                minCol = Math.Min(minCol, col);
            }
        }

        return trench;
    }

    // https://en.wikipedia.org/wiki/Shoelace_formula
    static int TrenchArea(List<Coords> trench)
    {
        int area = 0;
        int j = trench.Count - 1;
        for (int i = 0; i < trench.Count; i++)
        {
            area += (trench[j].Col + trench[i].Col) * (trench[j].Row - trench[i].Row);
            j = i;
        }
        return Math.Abs(area / 2);
    }

    static int BoundaryPoints(List<Instruction> instructions) =>
        instructions.Aggregate(0, (current, i) => current + i.Steps);

    // https://en.wikipedia.org/wiki/Pick%27s_theorem
    static int InteriorPoints(int area, int boundary) => area - boundary / 2 + 1;

    static int TrenchPoints(List<Coords> trench, List<Instruction> instructions)
    {
        var area = TrenchArea(trench);
        var boundary = BoundaryPoints(instructions);
        var interior = InteriorPoints(area, boundary);
        return (boundary + interior);
    }

    public override string Part1()
    {
        var instructions = ParseInput();
        var trench = ExecuteInstructions(instructions);
        return TrenchPoints(trench, instructions).ToString();
    }

    public override string Part2() => "";
}
