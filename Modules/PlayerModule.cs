using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Player interaction commands (kill, revive, teleport, etc).
    /// </summary>
    public sealed partial class PlayerModule : CommandsModule
    {
        public override string Name => "Player";

        public override string Description => "Player module";

        public PlayerModule()
        {
            AddCommand(new Command(
      "kill",
      "<player> - Kills a player.",
      Kill
  ));

            AddCommand(new Command(
                "gib",
                "<player> - Gibs a player.",
                Gib
            ));

            AddCommand(new Command(
                "remove",
                "<player> - Removes a player.",
                Remove
            ));

            AddCommand(new Command(
                "dmg",
                "<player> <amount> - Deals damage to a player.",
                Dmg
            ));

            AddCommand(new Command(
                "notarget",
                "<player> - Toggles whether bots target a player.",
                NoTarget
            ));

            AddCommand(new Command(
                "tp",
                "<to> - Teleports you to a player.",
                Tp
            ));

            AddCommand(new Command(
                "tphere",
                "<player> - Teleports a player to you.",
                Tphere
            ));

            AddCommand(new Command(
                "tppos",
                "<player> <x> <y> - Teleports a player to a world position.",
                Tppos
            ));

            AddCommand(new Command(
                "trip",
                "<player> - Trips a player, knocking them down.",
                Trip
            ));

            AddCommand(new Command(
                "pos",
                "<player> - Displays the world position of a player. Moderator-only.",
                Pos
            ));

            AddCommand(new Command(
                "modifier",
                "<player> <modifier> <value> - Sets a player modifier to the given value.",
                Modifier
            ));

            AddCommand(new Command(
                "tag",
                "<player> [name|status] - Toggles nametag and status bar visibility for a player.",
                Tag
            ));

            AddCommand(new Command(
                "input",
                "<player> - Toggles whether a player can provide input, effectively freezing or unfreezing them.",
                Input
            ));

            AddCommand(new Command(
                "team",
                "<player> <team> - Sets the team of a player.",
                Team
            ));

            AddCommand(new Command(
                "burn",
                "<player> - Toggles whether a player is burning.",
                Burn
            ));

            AddCommand(new Command(
                "spawn",
                "<id> - Spawns an object with the given ID at your position.",
                Spawn
            ));

            AddCommand(new Command(
                "revive",
                "<player> - Revives a dead player.",
                Revive
            ));

            AddCommand(new Command(
                "refill",
                "<player> - Refills a player's ammo as if they used an ammo stash.",
                Refill
            ));

            AddCommand(new Command(
                "fly",
                "<player> - Toggles flying for a player.",
                FlyCommand
            ));

            AddCommand(new Command(
                "noclip",
                "<player> - Toggles noclip for a player, allowing them to pass through walls.",
                Noclip
            ));

            AddCommand(new Command(
                "copy",
                "<from> <to> - Copies one player's profile onto another player.",
                Copy
            ));

            AddCommand(new Command(
                "swap",
                "<from> <to> - Swaps the profiles of two players.",
                Swap
            ));

            AddCommand(new Command(
                "user",
                "<from> <to> - Swaps the users of two players.",
                User
            ));

            AddCommand(new Command(
                "wear",
                "<player> <slot> <name> [color1] [color2] - Gives a player a cosmetic item in the given slot with the given colors.",
                Wear
            ));

            AddCommand(new Command(
                "action",
                "<player> <action> - Queues an action for a player whose input is disabled.",
                ActionCommand
            ));

            AddCommand(new Command(
                "magnet",
                "<player> [area_size] [players|objects|all] - Toggles attraction of nearby players and/or objects toward a player.",
                MagnetCommand
            ));

            AddCommand(new Command(
                "repulse",
                "<player> [area_size] [players|objects|all] - Toggles repulsion of nearby players and/or objects away from a player.",
                RepulseCommand
            ));

            AddCommand(new Command(
                "boost",
                "<player> <left|down|up|right> [speed] - Boosts a player in a direction.",
                Boost
            ));
        }
    }
}
