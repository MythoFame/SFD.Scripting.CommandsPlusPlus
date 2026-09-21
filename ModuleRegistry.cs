namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Holds all known modules by case-insensitive name. Owns lookup plus
    /// reset-to-defaults (removing persisted flags so every module reads
    /// its default again). Per-module allow/restrict lives on
    /// <see cref="CommandsModule"/>; the guarded <see cref="CommandsModule.IsRestricted"/>
    /// setter guarantees <c>OnRestricted</c> fires exactly once per
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
        /// Resets all modules to their default state by removing every
        /// persisted flag under <see cref="CommandsModule.StorageKeyPrefix"/>
        /// and restoring every module's default. Redundant transitions are
        /// no-ops thanks to the guarded setter, so already-default modules
        /// are untouched.
        /// </summary>
        public static void ResetAll()
        {
            foreach (string key in Game.LocalStorage.GetKeys())
            {
                if (key.StartsWith(CommandsModule.StorageKeyPrefix, StringComparison.Ordinal))
                    Game.LocalStorage.RemoveItem(key);
            }

            foreach (CommandsModule module in _cachedModules)
            {
                if (module.DefaultRestricted)
                    module.Restrict();
                else
                    module.Allow();
            }
        }

        /// <summary>
        /// Creates, registers and exposes every module, then applies its persisted
        /// <see cref="CommandsModule.Restricted"/> value. Modules without a
        /// stored value fall back to <see cref="CommandsModule.DefaultRestricted"/>;
        /// ones already at their target state stay active without any
        /// spurious <c>OnRestricted</c> call.
        /// </summary>
        public static void RegisterAll()
        {
            Register(new ManagementModule());
            Register(new PlayerModule());
            Register(new GameplayModule());
            Register(new FunModule());
            Register(new SpectationModule());

            foreach (CommandsModule module in _cachedModules)
            {
                module.Register();

                bool restricted = module.HasPersistedState ? module.Restricted : module.DefaultRestricted;

                if (restricted)
                    module.Restrict();
            }
        }
    }
}
