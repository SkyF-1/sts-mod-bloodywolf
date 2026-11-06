# Card audio

Card audio is discovered automatically. Do not add card-specific entries to the
audio configuration code.

Use the card class name in snake_case with the `stsmodbloodywolf-` prefix as the
file name:

```text
cards/name/<card-id>.<ogg|wav|mp3>
cards/flavor/<card-id>.<ogg|wav|mp3>
```

Example for `STS_MOD_BLOODYWOLF_CARD_STRIKE_WOLF`:

```text
cards/name/stsmodbloodywolf-strike_wolf.ogg
cards/flavor/stsmodbloodywolf-strike_wolf.ogg
```

A card may provide either file, both files, or neither file. Reimport the audio
in Godot and rebuild the PCK after adding files.
