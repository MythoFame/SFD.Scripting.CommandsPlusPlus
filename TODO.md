# TODO

This is the markdown TODO file for Commands++. It consists mainly of commands and their modules.

## Specification

### Parameters

The brackets `<>`, `[]`, `{}` for parameters have different meanings.

- `<param>`: required parameter.
- `[param]`: optional parameter.
- `{param}`: choice from a set.

### Types

- `bool`: can be `{true|false}` or `{yes|no}`.
- `string`: any text.
- `int`: integer value like `1`, `-20`, `0`, `520`.
- `float`: floating value like `1`, `-20`, `0`, `520`.
- `player`: a player name or index.
- `user`: a user name, index or account name.

---

## Modules

Commands are organized into modules, each grouping related commands. Every module can be independently enabled or disabled with `/toggle_module`, and the state persists between sessions. 

Use `/modules` to list all modules with their state and `/commands [module]` to view command help. The Management module is required and cannot be disabled.

### Management

Required core module for managing all other modules. Always enabled and cannot be disabled.

- [ ] `/modules`: Display all modules along with whether they are enabled or disabled.
- [ ] `/commands [module]`: Display all commands along with their help. If a module is provided, then display only that module's commands and help.
- [ ] `/toggle_module <module>`: Toggles whether a module is enabled. Host-only, persisted. Argument cannot be the management module.
- [ ] `/reset_modules`: Resets all modules to their default enabled state. Host-only, persisted.

### Automation

Run commands automatically on events. Host-only, persisted.

- [ ] `/jobs`: Lists all jobs along with their index, trigger, arguments and command.
- [ ] `/add_job {startup|shutdown|gameover|spawn|time} <args> <command...>`: Adds a job that runs a command on a certain trigger. Arguments depend on the trigger.
- [ ] `/remove_job <index>`: Removes the job with the given index.
- [ ] `/clear_jobs`: Removes all jobs.

## Gameplay

Custom gameplay rules. Host-only, persisted unless noted otherwise.

- [ ] `/respawn <delay>`: Toggles custom respawn and sets the respawn delay in seconds.
- [ ] `/speech`: Toggles custom speech bubbles above players.
- [ ] `/grab`: Toggles whether players are able to grab and throw each other.
- [ ] `/dmg_numbers`: Toggles whether damage is displayed with floating numbers.
- [ ] `/drop_in`: Toggles whether joining players spawn instantly instead of waiting for the next round.
- [ ] `/no_bot_gameover`: Toggles whether the game ends when only bots are left alive.
- [ ] `/auto_victory [bool]`: Toggles or explicitly sets automatic victory condition detection. Used to make a map custom or versus. Moderator-only, not persisted.
- [ ] `/weapon_spawn [bool]`: Toggles or explicitly sets whether weapons spawn on the map. Moderator-only, not persisted.
- [ ] `/tags [bool]`: Toggles or explicitly sets nametag and status bar visibility for all players. Moderator-only, not persisted.
- [x] `/reset_winratio`: Resets the stored win ratio statistics. Moderator-only.
- [ ] `/refill_ammo`: Toggles whether ammo is constantly refilled for all players.
- [ ] `/regen <hp>`: Sets health regenerated per second for all players. Set to 0 to disable.
- [ ] `/gravity <multiplier>`: Sets a multiplier applied to gravity. Set to 0 to disable custom gravity.

## Spectation

Control who spectates and who plays.

- [ ] `/spectate [player]`: Toggles your own spectation for the next round. Moderators can specify a player to force-spectate them.
- [ ] `/spectate_whitelist`: Toggles whether only whitelisted players are allowed to play while everyone else spectates.
- [ ] `/spectate_add_whitelist <player>`: Adds a player to the spectate whitelist, allowing them to play. Moderator-only.
- [ ] `/spectate_rm_whitelist <player>`: Removes a player from the spectate whitelist. Moderator-only.

## Player

Interact with players. Moderator-only.

- [x] `/noclip <player>`: Toggles noclip for a player, allowing them to pass through walls.
- [x] `/fly <player>`: Toggles flying for a player.
- [x] `/kill <player> [gib|rm]`: Kills a player, optionally gibbing them or removing them.
- [x] `/revive <player>`: Revives a dead player.
- [x] `/input <player>`: Toggles whether a player can provide input, effectively freezing or unfreezing them.
- [x] `/tp <from> [to]`: Teleports a player to another player. If no target is provided, teleports them to your position.
- [x] `/team <player> <team>`: Sets the team of a player.
- [x] `/trip <player>`: Trips a player, knocking them down.
- [ ] `/setmodifier <player> <modifier> <value>`: Sets a player modifier to the given value.
- [x] `/spawn <id>`: Spawns an object with the given ID at your position.
- [x] `/burn <player>`: Toggles whether a player is burning.
- [x] `/copy_skin <from> <to>`: Copies one player's profile onto another player.
- [x] `/swap_skin <from> <to>`: Swap one player's profile onto another player.
- [x] `/user <from> <to>`: Swaps the users of two players.
- [ ] `/wear <name> <type> [primary_color] [secondary_color]`: Gives your profile a cosmetic item of the given type and colors.
- [ ] `/action <player> <action>`: Queues an action for a player whose input is disabled.
- [x] `/refill <player>`: Refills a player's ammo as if they used an ammo stash.
- [ ] `/magnet <player> [area_size] [players|objects]`: Toggles attraction of nearby players and/or objects toward a player.
- [ ] `/repulse <player> [area_size] [players|objects]`: Toggles repulsion of nearby players and/or objects away from a player.

## Fun

Lightweight fun commands for everyone unless noted otherwise.

- [x] `/graffiti <text>`: Creates floating graffiti text at your position.
- [x] `/lightning <player>`: Summons a lightning strike upon a player. Moderator-only.
- [ ] `/bot [team] [ai] [name]`: Spawns a robot, optionally on a given team with the given AI and name. Moderator-only.
- [ ] `/bullet <player> <id>`: Sets custom bullets for a player. Moderator-only.
- [ ] `/noreload <player> <slot>`: Attempts to toggle no reload for the specified weapon. Only works for some specific weapons.
- [ ] `/anvil <player>`: Drops a heavy object onto a player. Moderator-only.
- [ ] `/clone <player> [team] [ai]`: Spawns a clone of a player, optionally on a given team with the given AI. Moderator-only.
- [ ] `/fart`: Makes you fart.
- [ ] `/suicide`: Die dramatically.
