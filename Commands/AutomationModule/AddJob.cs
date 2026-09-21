using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class AutomationModule
    {
        private static void AddJob(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length < 2)
            {
                Game.ShowChatMessage("Usage: /add_job {startup|shutdown|gameover|spawn|time} <args> <command...>", Color.Red, uid);
                return;
            }

            if (!Enum.TryParse(tokens[0], true, out JobsRule.JobTrigger trigger) || !Enum.IsDefined(trigger))
            {
                Game.ShowChatMessage($"Unknown trigger '{tokens[0]}'.", Color.Red, uid);
                return;
            }

            string[] rest = tokens[1..];
            string triggerArgs = string.Empty;
            string[] commandTokens;

            if (trigger == JobsRule.JobTrigger.Time)
            {
                if (rest.Length < 3)
                {
                    Game.ShowChatMessage("Usage: /add_job time <time> <repeats> <command...>", Color.Red, uid);
                    return;
                }

                if (!uint.TryParse(rest[0], out uint interval) || interval < 1)
                {
                    Game.ShowChatMessage($"Invalid interval '{rest[0]}'.", Color.Red, uid);
                    return;
                }

                if (!int.TryParse(rest[1], out int repeats) || repeats < 0)
                {
                    Game.ShowChatMessage($"Invalid repeats '{rest[1]}'.", Color.Red, uid);
                    return;
                }

                triggerArgs = $"{rest[0]} {rest[1]}";
                commandTokens = rest[2..];
            }
            else
            {
                commandTokens = rest;
            }

            string commandName = commandTokens[0].TrimStart('/');

            CommandHandler.Command command = CommandHandler.ActiveCommands
                .FirstOrDefault(c => c.Name.Equals(commandName, StringComparison.InvariantCultureIgnoreCase));

            if (command == null)
            {
                Game.ShowChatMessage($"Unknown command '{commandTokens[0]}'.", Color.Red, uid);
                return;
            }

            JobsRule.Add(new JobsRule.Job
            {
                Trigger = trigger,
                TriggerArgs = triggerArgs,
                Command = commandName,
                CommandArgs = commandTokens.Length > 1 ? string.Join(" ", commandTokens[1..]) : string.Empty
            });

            Game.ShowChatMessage($"Added job {JobsRule.Jobs.Count - 1}.", Color.Green, uid);
        }
    }
}
