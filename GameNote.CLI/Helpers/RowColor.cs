using System;

namespace GameNote.CLI.Helpers
{
    public class RowColor<T>
    {
        public Func<T, bool> Evaluator { get; set; }
        public ConsoleColor Colour { get; set; }
    }
}