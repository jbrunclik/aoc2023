using System.Data;

namespace Aoc2023
{
    abstract class Solution
    {
        string[] _input = new string[] { };

        public string[] Input
        {
            get => _input;
            set => _input = value;
        }

        public void ParseInput(string inputPath)
        {
            Console.WriteLine($"Parsing input file \"{inputPath}\"...");
            Input = File.ReadAllLines(inputPath);
        }

        public abstract string Solve();
    }
}
