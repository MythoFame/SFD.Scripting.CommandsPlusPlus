# TODO

This is the markdown TODO file for Commands++. It consists mainly of commands and their modules.

## Specification

### Parameters

The brackets `<>`, `[]`, `{}` for parameters have different meanings.

- `<param>`: required parameter.
- `[param]`: optional parameter.
- `{param}`: choice from a set.

### Types

- `bool`: can be `{true|false}` or `{1|0}`.
- `string`: any text.
- `int`: integer value like `1`, `-20`, `0`, `520`.
- `float`: floating value like `1.0`, `-20.25`, `0`, `520.3`.
- `player`: a player name or index.
- `user`: a user name, index or account name.

---

## Modules

Commands are organized into modules, each grouping related commands. Every module can be independently allowed or restricted with `/toggle_module`, and the state persists between sessions. Restricting a module limits all of its commands to the host.

Use `/modules` to list all modules with their state and `/commands [module]` to view command help. The Management module is required and cannot be restricted.

### Management

Required core module for managing all other modules. Always allowed and cannot be restricted.

- [x] `/modules`: Display all modules along with whether they are allowed or restricted.
- [x] `/commands [module]`: Display all commands along with their help. If a module is provided, then display only that module's commands and help.
- [x] `/toggle_module <module>`: Toggles whether a module is restricted. Restricted modules have all their commands limited to the host. Host-only, persisted. Argument cannot be the management module.
- [x] `/reset_modules`: Resets all modules to their default allowed state. Host-only, persisted.

### Automation

Run commands automatically on events. Host-only, persisted.

- [ ] `/jobs`: Lists all jobs along with their index, trigger, arguments and command.
- [ ] `/add_job {startup|shutdown|gameover|spawn|time} <args> <command...>`: Adds a job that runs a command on a certain trigger. Arguments depend on the trigger.
- [ ] `/remove_job <index>`: Removes the job with the given index.
- [ ] `/clear_jobs`: Removes all jobs.

## Gameplay

Custom gameplay rules. Host-only, persisted unless noted otherwise.

- [ ] `/respawn <delay>`: Toggles custom respawn and sets the respawn delay in seconds.
- [ ] `/speech [playSound]`: Toggles custom speech bubbles above players. Second parameter plays a sound when speech appears (default to `true`).
- [ ] `/grab [enabled]`: Toggles whether players are able grab and throw each other.
- [ ] `/throw [enabled]`: Toggles whether players can throw objects.
- [ ] `/dmg_numbers [enabled]`: Toggles whether damage is displayed.
- [ ] `/dropin [enabled]`: Toggles whether joining players spawn instantly instead of waiting for the next round.
- [ ] `/gmover {players|enabled}`: Toggles whether the game may end, and if only players are left alive.
  - `/gmover off`: game over is disabled.
  - `/gmover on`: game over is enabled.
  - `/gmover players`: game over works only for players; ignore bots.
- [ ] `/vctory [bool]`: Toggles or explicitly sets automatic victory condition detection. Used to make a map custom or versus. Moderator-only, not persisted.
- [x] `/wpnspawn [bool]`: Toggles or explicitly sets whether weapons spawn on the map. Moderator-only, not persisted.
- [ ] `/tags [bool]`: Toggles or explicitly sets nametag and status bar visibility for all players. Moderator-only, not persisted.
- [ ] `/camera {reset|static|dynamic|individual} [zoom|persistent]`: Sets camera type and optional zoom level.
  - `/camera static`: sets static camera for current round.
  - `/camera individual 0.5`: sets individual camera with fixed zoom level.
  - `/camera dynamic true` sets dynamic camera for all rounds.
  - `/camera individual true` sets individual camera with variable zoom for all rounds.
  - `/camera individual 0.2 true` sets individual camera level with fixed zoom level for all rounds.
  - Camera type for all rounds is persistent between different games and can be reset with `/camera reset` (this will reset the camera type for current round too).
- [x] `/rsboard`: Resets the stored win ratio statistics. Moderator-only.
- [ ] `/refill [bool]`: Toggles whether ammo is constantly refilled for all players.
- [ ] `/regen <hp>`: Sets health regenerated (or damaged) per second for all players. Set to `0` to disable.
- [ ] `/gravity [multiplier]`: Sets a multiplier applied to gravity. Provide no argument to disable.
- [ ] `/friendly_fire`: Toggles friendly fire.

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
- [x] `/kill <player>`: Kills a player.
- [x] `/gib <player>`: Gibs a player.
- [x] `/remove <player>`: Removes a player.
- [x] `/dmg <player> <amount>`: Deals damage to a player.
- [x] `/notarget <player>`: Toggles whether bots target a player.
- [x] `/revive <player>`: Revives a dead player.
- [x] `/input <player> [bool]`: Toggles whether a player can provide input, effectively freezing or unfreezing them.
- [x] `/tp <from> <to>`: Teleports a player to another player.
- [x] `/team <player> <team>`: Sets the team of a player.
- [x] `/trip <player>`: Trips a player, knocking them down.
- [x] `/pos <player>`: Displays the world position of a player. Moderator-only.
- [ ] `/modifier <player> <modifier> <value>`: Sets a player modifier to the given value.
- [x] `/spawn <id>`: Spawns an object with the given ID at your position.
- [x] `/burn <player>`: Toggles whether a player is burning.
- [x] `/copy <from> <to>`: Copies one player's profile onto another player.
- [x] `/swap <from> <to>`: Swap one player's profile onto another player.
- [x] `/user <from> <to>`: Swaps the users of two players.
- [ ] `/wear <name> <type> [primary_color] [secondary_color]`: Gives your profile a cosmetic item of the given type and colors.
- [x] `/action <player> <action>`: Queues an action for a player whose input is disabled.
- [x] `/refill <player>`: Refills a player's ammo as if they used an ammo stash.
- [ ] `/magnet <player> [area_size] [players|objects]`: Toggles attraction of nearby players and/or objects toward a player.
- [ ] `/repulse <player> [area_size] [players|objects]`: Toggles repulsion of nearby players and/or objects away from a player.

## Fun

Lightweight fun commands for everyone unless noted otherwise.

- [x] `/graffiti <text>`: Creates floating graffiti text at your position.
- [x] `/lightning <player>`: Summons a lightning strike upon a player. Moderator-only.
- [x] `/bot [team] [ai] [name]`: Spawns a robot, optionally on a given team with the given AI and name. Moderator-only.
- [ ] `/bullet <player> <id>`: Sets custom bullets for a player. Moderator-only.
- [ ] `/noreload <player> <slot>`: Attempts to toggle no reload for the specified weapon. Only works for some specific weapons. Moderator-only.
- [x] `/anvil <player>`: Drops a heavy object onto a player. Moderator-only.
- [x] `/clone <player> [team] [ai]`: Spawns a clone of a player, optionally on a given team with the given AI. Moderator-only.
- [x] `/fart`: Makes you fart.
- [x] `/suicide`: Die dramatically.
