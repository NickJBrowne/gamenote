using System;
using FluentAssertions;
using Xunit;
using GameNote.CLI;
using GameNote.CLI.Helpers;
using GameNote.CLI.Commands.Game;
using System.Linq;

namespace GameNote.CLI.Tests
{
    public class CliBuilderTests
    {
        [Fact]
        public void ConstructsValidArguments()
        {
            var args = new CliBuilder<AddGameCommand>("game")
                .Option(c => c.DirectPath, "this-is-a-path")
                .GetArguments();

            args.Count().Should().Be(3);
            args[0].Should().Be("game");
            args[1].Should().Be("--full-path");
            args[2].Should().Be("this-is-a-path");
        }

        [Fact]
        public void WorkWithBooleans()
        {
            new CliBuilder<FindCommand>("game")
                .Option(c => c.Directory, "this-is-a-dir")
                .Option(c => c.ShowBlacklisted, false)
                .GetArguments()
                .Count().Should().Be(3);

            var args = new CliBuilder<FindCommand>("game")
                .Option(c => c.Directory, "this-is-a-dir")
                .Option(c => c.ShowBlacklisted, true)
                .GetArguments();

            args.Count().Should().Be(4);
            args[0].Should().Be("game");
            args[1].Should().Be("--dir");
            args[2].Should().Be("this-is-a-dir");
            args[3].Should().Be("--show-blacklisted");
        }
    }
}
