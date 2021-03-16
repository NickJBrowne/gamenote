using System;
using System.Collections.Generic;

namespace GameNote.CLI.Helpers
{
    public class ConsoleWriteTable<T>
    {
        private Dictionary<string, Func<T, string>> _columns = new Dictionary<string, Func<T, string>>();

        public ConsoleWriteTable<T> AddColumn(string columnName, Func<T, string> column)
        {
            _columns.Add(columnName, column);
            return this;
        }

        public void Write(IEnumerable<T> list)
        {
            // Write the headers
            
        }
    }
}