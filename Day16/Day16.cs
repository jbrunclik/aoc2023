namespace Aoc2023.Day16;

partial class Day16 : Solution
{
    char[,] ParseInput()
    {
        var rows = Input.Count();
        var cols = Input[0].Count();

        var contraption = new char[rows, cols];
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                contraption[i, j] = Input[i][j];
            }
        }

        return contraption;
    }

    void TraceBeam(
        char[,] contraption,
        Beam beam,
        ref Stack<Beam> stack,
        ref HashSet<(int, int, int, int)> visited,
        ref HashSet<(int, int)> energized
    )
    {
        if (
            beam.Y < 0
            || beam.Y >= contraption.GetLength(0)
            || beam.X < 0
            || beam.X >= contraption.GetLength(1)
        )
            return;

        var k = (beam.X, beam.Y, beam.DX, beam.DY);
        if (visited.Contains(k))
            return;
        visited.Add(k);
        energized.Add((beam.Y, beam.X));

        var tile = contraption[beam.Y, beam.X];
        switch (tile)
        {
            case '.':
                stack.Push(new Beam(beam.X + beam.DX, beam.Y + beam.DY, beam.DX, beam.DY));
                return;
            case '|':
                if (beam.DY != 0)
                {
                    stack.Push(new Beam(beam.X, beam.Y + beam.DY, beam.DX, beam.DY));
                    return;
                }
                else
                {
                    stack.Push(new Beam(beam.X, beam.Y - 1, 0, -1));
                    stack.Push(new Beam(beam.X, beam.Y + 1, 0, 1));
                    return;
                }
            case '-':
                if (beam.DX != 0)
                {
                    stack.Push(new Beam(beam.X + beam.DX, beam.Y, beam.DX, beam.DY));
                    return;
                }
                else
                {
                    stack.Push(new Beam(beam.X - 1, beam.Y, -1, 0));
                    stack.Push(new Beam(beam.X + 1, beam.Y, 1, 0));
                    return;
                }
            case '/':
                switch (beam.DX, beam.DY)
                {
                    case (1, 0):
                        stack.Push(new Beam(beam.X, beam.Y - 1, 0, -1));
                        return;
                    case (-1, 0):
                        stack.Push(new Beam(beam.X, beam.Y + 1, 0, 1));
                        return;
                    case (0, 1):
                        stack.Push(new Beam(beam.X - 1, beam.Y, -1, 0));
                        return;
                    case (0, -1):
                        stack.Push(new Beam(beam.X + 1, beam.Y, 1, 0));
                        return;
                    default:
                        throw new ApplicationException("Invalid beam direction");
                }
            case '\\':
                switch (beam.DX, beam.DY)
                {
                    case (1, 0):
                        stack.Push(new Beam(beam.X, beam.Y + 1, 0, 1));
                        return;
                    case (-1, 0):
                        stack.Push(new Beam(beam.X, beam.Y - 1, 0, -1));
                        return;
                    case (0, 1):
                        stack.Push(new Beam(beam.X + 1, beam.Y, 1, 0));
                        return;
                    case (0, -1):
                        stack.Push(new Beam(beam.X - 1, beam.Y, -1, 0));
                        return;
                    default:
                        throw new ApplicationException("Invalid beam direction");
                }
            default:
                throw new ApplicationException($"Unknown tile {tile}");
        }
    }

    public override string Part1()
    {
        var contraption = ParseInput();
        var stack = new Stack<Beam>();
        var visited = new HashSet<(int, int, int, int)>();
        var energized = new HashSet<(int, int)>();
        var beam = new Beam(0, 0, 1, 0);
        stack.Push(beam);

        while (stack.Count > 0)
        {
            var currentBeam = stack.Pop();
            TraceBeam(contraption, currentBeam, ref stack, ref visited, ref energized);
        }
        return energized.Count().ToString();
    }

    public override string Part2()
    {
        var contraption = ParseInput();

        var startBeams = new List<Beam>();
        // Top & bottom
        for (var col = 0; col < contraption.GetLength(1); col++)
        {
            startBeams.Add(new Beam(col, 0, 0, 1));
            startBeams.Add(new Beam(col, contraption.GetLength(0) - 1, 0, -1));
        }

        // Left & right
        for (var row = 0; row < contraption.GetLength(0); row++)
        {
            startBeams.Add(new Beam(0, row, 1, 0));
            startBeams.Add(new Beam(contraption.GetLength(1) - 1, row, -1, 0));
        }

        var maxEnergized = 0;
        foreach (var startBeam in startBeams)
        {
            var stack = new Stack<Beam>();
            var visited = new HashSet<(int, int, int, int)>();
            var energized = new HashSet<(int, int)>();
            stack.Push(startBeam);

            while (stack.Count > 0)
            {
                var currentBeam = stack.Pop();
                TraceBeam(contraption, currentBeam, ref stack, ref visited, ref energized);
            }

            maxEnergized = Math.Max(maxEnergized, energized.Count());
        }

        return maxEnergized.ToString();
    }

    record Beam(int X, int Y, int DX, int DY);
}
