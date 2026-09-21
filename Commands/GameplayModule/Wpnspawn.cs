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
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /wpnspawn [bool]", Color.Red, uid);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = _savedSpawnChances != null;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /wpnspawn [bool]", Color.Red, uid);
                return;
            }

            if (enable)
            {
                if (_savedSpawnChances == null)
                {
                    Game.ShowChatMessage("Weapon spawning is already enabled.", Color.Yellow, uid);
                    return;
                }

                Game.UpdateWeaponSpawnChances(_savedSpawnChances);
                _savedSpawnChances = null;

                Game.ShowChatMessage("Weapon spawning enabled.", Color.Green, uid);
            }
            else
            {
                if (_savedSpawnChances != null)
                {
                    Game.ShowChatMessage("Weapon spawning is already disabled.", Color.Yellow, uid);
                    return;
                }

                _savedSpawnChances = Game.GetWeaponSpawnChances();
                Game.ClearWeaponSpawnChances();

                Game.ShowChatMessage("Weapon spawning disabled.", Color.Green, uid);
            }
        }
    }
}
