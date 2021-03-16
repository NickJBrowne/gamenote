using System;
using System.Collections.Generic;

namespace GameNote.CLI.Helpers
{
    public class ConsoleWriteTable<T>
    {
        private readonly Dictionary<string, Func<T, string>> _columns = new Dictionary<string, Func<T, string>>();
        private readonly IEnumerable<T> _list;
        private readonly Dictionary<string, int> _columnWidths = new Dictionary<string, int>();
        private static int _padding = 10;
        private int _width = 1;

        public ConsoleWriteTable(IEnumerable<T> list)
        {
            _list = list;
        }

        public ConsoleWriteTable<T> AddColumn(string columnName, Func<T, string> column)
        {
            _columns.Add(columnName, column);

            int columnWidth = 0;
            foreach (var item in _list)
            {
                int length = column(item).Length;
                if (length > columnWidth)
                    columnWidth = length;
            }
            _columnWidths.Add(columnName, columnWidth);
            _width += (_padding * 2) + columnWidth + 1;

            return this;
        }

        public void Write()
        {
            string line = ConstructLine(_width);

            Console.WriteLine(line);
            string headerLine = "|";
            foreach (var column in _columns)
            {
                int width = _columnWidths[column.Key];
                headerLine += AlignCenter(column.Key, width) + "|";
            }
            Console.WriteLine(headerLine);
            Console.WriteLine(line);

            foreach (var item in _list)
            {
                string row = "|";
                foreach (var column in _columns)
                {
                    int width = _columnWidths[column.Key];
                    row += AlignCenter(column.Value(item), width);
                }
                Console.WriteLine(row);
            }
            Console.WriteLine(line);
        }

        private string AlignCenter(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
                return new string(' ', width);
            else
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }

        private string ConstructLine(int width)
        {
            string lineData = "";

            for(int i = 1; i <= width; i++)   
                lineData += "-";

            return lineData;
        }
    }
}