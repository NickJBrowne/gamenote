using CommandLine;

namespace GameNote.CLI.Verbs
{
    [Verb("game-list", HelpText = "Get a list of potential games to add")]
    public class ListGamesVerb
    {
        [Option("directory", Required=true, HelpText = "The directory and all sub directories to look in")]
        public string Directory { get; set; }

        [Option("show-blacklist", Required=false, Default = false, HelpText="Set to true to show items that are hidden because they are on the blacklist")]
        public bool ShowBlacklist { get; set; }
    }
}