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

- [x] `/respawn <delay>`: Sets the custom respawn delay. Set to `0` or below to disable.
- [x] `/speech [bool]`: Toggles custom speech bubbles above players. Second parameter plays a sound when speech appears (default to `false`).
- [x] `/grab [bool]`: Toggles or sets whether players are able to grab and throw each other.
- [x] `/throw [bool]`: Toggles or sets whether players can throw objects.
- [x] `/dmg_numbers [bool] [players|objects|all]`: Toggles or sets whether damage is displayed.
- [x] `/dropin <delay>`: Sets the drop-in spawn delay. Set to `0` or below to disable.
- [ ] `/gmover {true|false|players}`: Controls automatic victory detection, i.e. whether the round may end. Host-only.
  - `/gmover true|false`: enables or disables game over for the current round. Not persisted.
  - `/gmover players`: game over triggers on players only; bots are ignored. Persisted.
- [x] `/wpnspawn [bool]`: Toggles or sets whether weapons spawn on the map. Moderator-only, not persisted.
- [x] `/camera {reset|static|dynamic|individual} [zoom]`: Sets camera type and optional zoom level for the current round. Not persisted.
  - `/camera static`: sets static camera.
  - `/camera dynamic`: sets dynamic camera.
  - `/camera individual`: sets individual camera with variable zoom.
  - `/camera individual 0.5`: sets individual camera with fixed zoom level.
  - `/camera reset`: resets the camera type.
- [x] `/rsboard`: Resets the stored win ratio statistics. Moderator-only.
- [x] `/refill_all [bool]`: Toggles or sets whether ammo is constantly refilled for all players.
- [x] `/regen <hp>`: Sets health regenerated per second for all players. Set to `0` or below to disable.
- [x] `/gravity <constant>`: Sets a constant applied to gravity. Set to `0` to disable.
- [x] `/friendly_fire`: Toggles friendly fire.

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
- [x] `/input <player>`: Toggles whether a player can provide input, effectively freezing or unfreezing them.
- [x] `/tp <to>`: Teleports you to a player.
- [x] `/tphere <player>`: Teleports a player to you.
- [x] `/tppos <player> <x> <y>`: Teleports a player to a world position.
- [x] `/team <player> <team>`: Sets the team of a player.
- [x] `/trip <player>`: Trips a player, knocking them down.
- [x] `/pos <player>`: Displays the world position of a player. Moderator-only.
- [x] `/tag <player> [name|status]`: Toggles nametag and status bar visibility for a player.
- [x] `/modifier <player> <modifier> <value>`: Sets a player modifier to the given value.
- [x] `/spawn <id>`: Spawns an object with the given ID at your position.
- [x] `/burn <player>`: Toggles whether a player is burning.
- [x] `/copy <from> <to>`: Copies one player's profile onto another player.
- [x] `/swap <from> <to>`: Swap one player's profile onto another player.
- [x] `/user <from> <to>`: Swaps the users of two players.
- [x] `/wear <player> <slot> <name> [color1] [color2]`: Gives a player a cosmetic item in the given slot with the given colors.
- [x] `/action <player> <action>`: Queues an action for a player whose input is disabled.
- [x] `/refill <player>`: Refills a player's ammo as if they used an ammo stash.
- [x] `/magnet <player> [area_size] [players|objects|all]`: Toggles attraction of nearby players and/or objects toward a player.
- [x] `/repulse <player> [area_size] [players|objects|all]`: Toggles repulsion of nearby players and/or objects away from a player.

## Fun

Lightweight fun commands for everyone unless noted otherwise.

- [x] `/graffiti <text>`: Creates floating graffiti text at your position.
- [x] `/lightning <player>`: Summons a lightning strike upon a player. Moderator-only.
- [x] `/bot [team] [ai] [name]`: Spawns a robot, optionally on a given team with the given AI and name. Moderator-only.
- [x] `/color <player> <color>`: Recolors all of a player's clothing. Moderator-only.
- [x] `/bullet <player> [id]`: Sets custom bullets for a player, or disables them if no ID is given. Moderator-only.
- [x] `/anvil <player>`: Drops a heavy object onto a player. Moderator-only.
- [x] `/clone <player> [team] [ai]`: Spawns a clone of a player, optionally on a given team with the given AI. Moderator-only.
- [x] `/fart`: Makes you fart.
- [x] `/suicide`: Die dramatically.
