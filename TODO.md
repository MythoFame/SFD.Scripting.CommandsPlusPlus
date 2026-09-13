# TODO

This is the markdown TODO file for Commands++. It consists mainly of commands and their modules.

Required options are shown with <>, optional parameters are shown with [].

## Management

Required core module for managing all other modules. Always enabled and cannot be disabled.

- [ ] `/modules`: Display all modules along with whether they are enabled or disabled.
- [ ] `/commands [module]`: Display all commands along with their help. If a module is provided, then display only that module's commands and help.
- [ ] `/toggle_module <module>`: Toggles whether a module is enabled. Host-only, persisted. Argument cannot be the management module.
- [ ] `/reset_modules`: Resets all modules to their default enabled state. Host-only, persisted.

## Automation

Run commands automatically on events. Host-only, persisted.

- [ ] `/jobs`: Lists all jobs along with their index, trigger, arguments and command.
- [ ] `/add_job <trigger> <args> <command...>`: Adds a job that runs a command on a certain trigger. Supported triggers are `startup`, `shutdown`, `gameover` and `time`. Arguments depend on the trigger.
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
- [ ] `/auto_victory [true|false]`: Toggles or explicitly sets automatic victory condition detection. Used to make a map custom or versus. Moderator-only, not persisted.
- [ ] `/weapon_spawn [true|false]`: Toggles or explicitly sets whether weapons spawn on the map. Moderator-only, not persisted.
- [ ] `/tags [true|false]`: Toggles or explicitly sets nametag and status bar visibility for all players. Moderator-only, not persisted.
- [ ] `/reset_winratio`: Resets the stored win ratio statistics. Moderator-only.

## Spectation

Control who spectates and who plays.

- [ ] `/spectate [player]`: Toggles your own spectation for the next round. Moderators can specify a player to force-spectate them.
- [ ] `/spectate_whitelist`: Toggles whether only whitelisted players are allowed to play while everyone else spectates.
- [ ] `/spectate_add_whitelist <player>`: Adds a player to the spectate whitelist, allowing them to play. Moderator-only.
- [ ] `/spectate_rm_whitelist <player>`: Removes a player from the spectate whitelist. Moderator-only.

## Players

Interact with players. Moderator-only.

- [ ] `/noclip <player>`: Toggles noclip for a player, allowing them to pass through walls.
- [ ] `/fly <player>`: Toggles flying for a player.
- [ ] `/kill <player> [gib|remove]`: Kills a player, optionally gibbing them or removing them from the game.
- [ ] `/revive <player>`: Revives a dead player.
- [ ] `/input <player>`: Toggles whether a player can provide input, effectively freezing or unfreezing them.
- [ ] `/tp <from> [to]`: Teleports a player to another player. If no target is provided, teleports them to your position.
- [ ] `/team <player> <team>`: Sets the team of a player.
- [ ] `/trip <player>`: Trips a player, knocking them down.
- [ ] `/setmodifier <player> <modifier> <value>`: Sets a player modifier to the given value.
- [ ] `/spawn <id>`: Spawns an object with the given ID at your position.
- [ ] `/burn <player>`: Toggles whether a player is burning.

## Fun

Lightweight fun commands for everyone unless noted otherwise.

- [ ] `/graffiti <text>`: Creates floating graffiti text at your position.
- [ ] `/lightning <player>`: Summons a lightning strike upon a player. Moderator-only.
- [ ] `/bot [team]`: Spawns a randomized bot, optionally on a given team. Moderator-only.
- [ ] `/bullet <player> <id>`: Sets custom bullets for a player. Moderator-only.
