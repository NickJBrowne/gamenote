using System.Diagnostics;
using System.IO;

namespace GameNote.Core
{
    public class CLIHandler
    {
        private readonly string _pathToCLI = string.Empty;
        private readonly bool _isValid = false;

        public CLIHandler(string pathToCLI)
        {
            _pathToCLI = pathToCLI;

            if (string.IsNullOrEmpty(_pathToCLI))
                _isValid = false;
            else
            {
                _pathToCLI = Path.Combine(_pathToCLI, "GameNote.CLI.exe");
                _isValid = File.Exists(_pathToCLI);
            }
        }

        public bool IsValidPath()
            => _isValid;

        public void GameRun(string gameName)
            => ExecuteCommand($"game run --game {gameName}");

        public void ExecuteCommand(string arguments)
        {
            Process.Start(new ProcessStartInfo(
                _pathToCLI, 
                arguments
            ) { CreateNoWindow = true });
        }
    }
}