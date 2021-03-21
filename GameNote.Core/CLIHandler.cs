using System.Diagnostics;
using System.IO;

namespace GameNote.Core
{
    public class CLIHandler
    {
        private readonly bool _isValid = false;
        private readonly string _pathToCLI = string.Empty;
        private const string _cliFileName = "GameNote.CLI.exe";

        public CLIHandler(string pathToCLI)
        {
            _pathToCLI = pathToCLI;

            if (string.IsNullOrEmpty(_pathToCLI))
                _isValid = false;
            else
            {
                _pathToCLI = Path.Combine(_pathToCLI, _cliFileName);
                _isValid = File.Exists(_pathToCLI);
            }
        }

        public void ExecuteCommand(string arguments)
        {
            Process.Start(new ProcessStartInfo(
                _pathToCLI,
                arguments
            )
            { CreateNoWindow = true });
        }

        public void GameRun(string gameName, bool force = false)
            => ExecuteCommand($"game run --game {gameName} {(force == true ? "--force" : "")}");

        public bool IsValidPath()
            => _isValid;
    }
}