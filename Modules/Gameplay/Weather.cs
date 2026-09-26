using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Weather(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /weather <none|snow|rain>", Color.Red, uid);
                return;
            }

            if (!Enum.TryParse(tokens[0], true, out WeatherType weather) || !Enum.IsDefined(weather))
            {
                Game.ShowChatMessage("Usage: /weather <none|snow|rain>", Color.Red, uid);
                return;
            }

            Game.SetWeatherType(weather);

            if (weather == WeatherType.None)
                Game.ShowChatMessage("Weather cleared.", Color.Green, uid);
            else
                Game.ShowChatMessage($"Weather set to {weather.ToString().ToLowerInvariant()}.", Color.Green, uid);
        }
    }
}
