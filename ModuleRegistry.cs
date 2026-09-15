namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Holds all known modules by case-insensitive name. Owns lookup plus
    /// reset-to-defaults (removing persisted flags so every module reads
    /// unblocked again). Per-module block/unblock lives on
    /// <see cref="CommandsModule"/>; the guarded <see cref="CommandsModule.IsBlocked"/>
    /// setter guarantees <c>OnBlocked</c> fires exactly once per
    /// transition, so this class never needs its own state checks.
    /// </summary>
    public static class ModuleRegistry
    {
        private static readonly Dictionary<string, CommandsModule> _modules = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Snapshot of registered modules for safe enumeration. Private: only
        /// the registry may walk it, and it refreshes on every
        /// <see cref="Register"/> so it never goes stale.
        /// </summary>
        private static CommandsModule[] _cachedModules = [];

        /// <summary>All registered modules.</summary>
        public static IEnumerable<CommandsModule> All => _cachedModules;

        /// <summary>Stores a module instance. Replaces any module with the same name.</summary>
        public static void Register(CommandsModule module)
        {
            _modules[module.Name] = module;
            _cachedModules = [.. _modules.Values];
        }

        /// <summary>Looks up a module without throwing.</summary>
        public static bool TryGet(string name, out CommandsModule module)
        {
            if (string.IsNullOrEmpty(name))
            {
                module = null;
                return false;
            }

            return _modules.TryGetValue(name, out module);
        }

        /// <summary>
        /// Resets all modules to their default (unblocked) state by removing every
        /// persisted flag under <see cref="CommandsModule.StorageKeyPrefix"/>
        /// and unblocking every module. Redundant transitions are no-ops thanks to
        /// the guarded setter, so already-unblocked modules are untouched.
        /// </summary>
        public static void ResetAll()
        {
            foreach (string key in Game.LocalStorage.GetKeys())
            {
                if (key.StartsWith(CommandsModule.StorageKeyPrefix, StringComparison.Ordinal))
                    Game.LocalStorage.RemoveItem(key);
            }

            foreach (CommandsModule module in _cachedModules)
                module.Unblock();
        }

        /// <summary>
        /// Creates, registers and exposes every module, then applies its persisted
        /// <see cref="CommandsModule.Blocked"/> value. Modules without a
        /// stored value default to unblocked; unblocked ones stay active without
        /// any spurious <c>OnBlocked</c> call.
        /// </summary>
        public static void RegisterAll()
        {
            Register(new ManagementModule());
            Register(new PlayerModule());
            Register(new GameplayModule());
            Register(new FunModule());

            foreach (CommandsModule module in _cachedModules)
            {
                module.Register();

                if (module.Blocked)
                    module.Block();
            }
        }
    }
}
