using System.Collections.Generic;
using System.IO;

namespace GameNote.Core.GameList
{
    public class ExecutableBlacklist
    {
        private List<string> _files = new List<string>()
        {
            "VC_Redist",
            "CrashReport",
            "CrashLoader",
            "Setup",
            "vcredist",
            "RiotClient",
            "CrashHandler",
            "Updater",
            "LogServer",
            "WatchDog",
            "DirectXRedist"
        };

        public bool IsInBlackList(FileInfo file)
        {
            string fileName = file.Name.ToLower();
            for (int i = 0; i < _files.Count; i++)
            {
                var match = _files[i].ToLower();
                if (fileName.Contains(match))
                    return true;
            }

            return false;
        }
    }
}