using System;
using System.Collections.Generic;
using System.Linq;

namespace GameNote.CLI.Helpers
{

    public class ConsoleWriteTable
    {
        public static ConsoleWriteTable<T> New<T>(IEnumerable<T> list)
            => new ConsoleWriteTable<T>(list);
    }

    public class ConsoleWriteTable<T>
    {
        private readonly Dictionary<string, Func<T, string>> _columns = new Dictionary<string, Func<T, string>>();
        private readonly Dictionary<string, ColumnAlign> _columnAlign = new Dictionary<string, ColumnAlign>();
        private readonly IEnumerable<T> _list;
        private readonly Dictionary<string, int> _columnWidths = new Dictionary<string, int>();
        private readonly List<RowColor<T>> _rowColours = new List<RowColor<T>>();
        private static int _padding = 2;
        private int _width = 1;

        public ConsoleWriteTable(IEnumerable<T> list)
        {
            _list = list;
        }

        public ConsoleWriteTable<T> AddColumn(
            string columnName, 
            Func<T, string> column, 
            ColumnAlign align = ColumnAlign.Center)
        {
            _columns.Add(columnName, column);

            int columnWidth = 0;
            foreach (var item in _list)
            {
                int length = column(item).Length;
                if (length > columnWidth)
                    columnWidth = length;
            }

            if (columnWidth < columnName.Length)
                columnWidth = columnName.Length;

            columnWidth += (_padding * 2) + 1;
            _columnWidths.Add(columnName, columnWidth);
            _width += columnWidth;
            _columnAlign.Add(columnName, align);

            return this;
        }

        public ConsoleWriteTable<T> AddColour(Func<T, bool> evaluator, ConsoleColor colour)
        {
            this._rowColours.Add(new RowColor<T>()
            {
                Evaluator = evaluator,
                Colour = colour
            });

            return this;
        }

        public void Write()
        {
            string lineSeperator = ConstructLine(_width + _columns.Count);
            ConsoleColor originalColour = Console.ForegroundColor;

            Console.WriteLine(lineSeperator);
            string headerLine = "|";
            foreach (var column in _columns)
            {
                int width = _columnWidths[column.Key];
                headerLine += AlignCenter(column.Key, width) + "|";
            }
            Console.WriteLine(headerLine);
            Console.WriteLine(lineSeperator);

            foreach (T item in _list)
            {                
                SetForegroundColour(item, originalColour);
                string row = "|";
                foreach (var column in _columns)
                {
                    int width = _columnWidths[column.Key];
                    ColumnAlign align = _columnAlign[column.Key];

                    if (align == ColumnAlign.Center)
                        row += AlignCenter(column.Value(item), width) + "|";
                    else
                        row += AlignLeft(column.Value(item), width) + "|";
                }
                Console.WriteLine(row);
                Console.ForegroundColor = originalColour;
            }
            Console.WriteLine(lineSeperator);
        }

        private string AlignCenter(string text, int width)
        {
            if (string.IsNullOrEmpty(text))
                return new string(' ', width);
            else
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }

        private string AlignLeft(string text, int width)
        {
            if (string.IsNullOrEmpty(text))
                return new string(' ', width);
            else
                return text.PadLeft(_padding + text.Length).PadRight(width);
        }

        private string ConstructLine(int width)
        {
            string lineData = "";

            for(int i = 1; i <= width; i++)   
                lineData += "-";

            return lineData;
        }

        private void SetForegroundColour(T item, ConsoleColor original)
        {
            if (_rowColours.Any() == false)
            {
                Console.ForegroundColor = original;
                return;
            }
            
            foreach (var colour in _rowColours)
            {
                if (colour.Evaluator(item) == true)
                {
                    Console.ForegroundColor = colour.Colour;
                    return;
                }
            }

            Console.ForegroundColor = original;
        }
    }
}