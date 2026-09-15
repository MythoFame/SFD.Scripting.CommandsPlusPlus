using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        /// <summary>
        /// Saved spawn chances while weapon spawning is disabled. Null means
        /// spawning is currently enabled.
        /// </summary>
        private static Dictionary<short, int> _savedSpawnChances = null;

        private static void Wpnspawn(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /wpnspawn [bool]", Color.Red, args.User.UserIdentifier);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = _savedSpawnChances != null;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /wpnspawn [bool]", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (enable)
            {
                if (_savedSpawnChances == null)
                {
                    Game.ShowChatMessage("Weapon spawning is already enabled.", Color.Yellow, args.User.UserIdentifier);
                    return;
                }

                Game.UpdateWeaponSpawnChances(_savedSpawnChances);
                _savedSpawnChances = null;

                Game.ShowChatMessage("Weapon spawning enabled.", Color.Green, args.User.UserIdentifier);
            }
            else
            {
                if (_savedSpawnChances != null)
                {
                    Game.ShowChatMessage("Weapon spawning is already disabled.", Color.Yellow, args.User.UserIdentifier);
                    return;
                }

                _savedSpawnChances = Game.GetWeaponSpawnChances();
                Game.ClearWeaponSpawnChances();

                Game.ShowChatMessage("Weapon spawning disabled.", Color.Green, args.User.UserIdentifier);
            }
        }
    }
}
