using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Utils.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Fun;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private static readonly string[] _botSecondaryColors =
        [
            "ClothingLightGray",
            "ClothingLightPink",
            "ClothingLightRed",
            "ClothingLightOrange",
            "ClothingLightYellow",
            "ClothingLightGreen",
            "ClothingLightCyan",
            "ClothingLightBlue",
            "ClothingLightPurple",
            "ClothingLightBrown",
        ];

        private static void Bot(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length > 3)
            {
                Game.ShowChatMessage("Usage: /bot [team] [ai] [name]", Color.Red, args.User.UserIdentifier);
                return;
            }

            PlayerTeam team = PlayerTeam.Independent;

            if (tokens.Length >= 1)
            {
                if (!Enum.TryParse(tokens[0], true, out team) || !Enum.IsDefined(team))
                {
                    Game.ShowChatMessage("Invalid team. Use independent (or 0) or team 1-8.", Color.Red, args.User.UserIdentifier);
                    return;
                }
            }

            PredefinedAIType ai = PredefinedAIType.BotA;

            if (tokens.Length >= 2)
            {
                if (!Enum.TryParse(tokens[1], true, out ai) || !Enum.IsDefined(ai))
                {
                    Game.ShowChatMessage("Invalid AI type.", Color.Red, args.User.UserIdentifier);
                    return;
                }
            }

            string name = tokens.Length >= 3 ? tokens[2] : $"ROBOT-{Random.Shared.Next(1000, 10000)}K";

            IPlayer bot = Game.CreatePlayer(PathGridHelper.GetRandomSpawnPosition(Random.Shared));

            bot.SetBotBehavior(new(true, ai));
            bot.SetTeam(team);
            bot.SetProfile(new()
            {
                Skin = new IProfileClothingItem("MechSkin", "ClothingLightGray",
                    _botSecondaryColors[Random.Shared.Next(_botSecondaryColors.Length)]),
            });

            bot.SetBotName(name);
            bot.SetHitEffect(PlayerHitEffect.Metal);

            EffectsWrapper.PlayTraceSpawner(bot, EffectName.Electric, 1);

            Game.ShowChatMessage($"Spawned bot '{name}' (team {team}, AI {ai}).", Color.Green, args.User.UserIdentifier);
        }
    }
}
