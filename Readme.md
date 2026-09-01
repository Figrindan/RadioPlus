# Radio Plus

Improvements to the custom radio station (FM 88.1) in [Easy Delivery Co.](https://store.steampowered.com/app/3293010/Easy_Delivery_Co/)

Easy Delivery Co. already lets you play your own music by dropping files into
`EasyDeliveryCo_Data/StreamingAssets/CustomRadio`. This mod makes that feature more robust: it
stops a single stray file from breaking the station, shuffles your playlist, and loads large
music libraries without the long startup wait.

I love the game and wanted to be able to listen to custom music during a full playthrough, so I made this mod. 

## Features

- **Non-audio files no longer break the station.** Previously, a single `.txt`, image, or other
  stray file in the CustomRadio folder would stop the entire station from working. Unsupported
  files are now skipped, with a note in the log saying which and why.
- **Shuffle.** Your playlist is shuffled instead of always playing in alphabetical order.
  Toggleable. This is a truly random shuffle, results may vary.
- **Faster loading, larger libraries.** Tracks now stream instead of being fully decoded into
  memory at startup. This substantially reduces both load time and memory use, making large
  music folders practical.
- **Performance fix.** The game recalculated the total length of your entire playlist every
  frame. This is now cached.
- **Early unlock.** Gives FM 88.1 full signal without needing to activate the radio
  tower first. On by default, because I assume this is why most people will install the mod.
- **Wider format support.** In addition to MP3, WAV, OGG, and FLAC, the mod will attempt to load
  other audio formats rather than assuming they will fail.

## Installation

1. Install [BepInEx 5](https://thunderstore.io/c/easy-delivery-co/p/BepInEx/BepInExPack/)
   (Mono, x64) into your Easy Delivery Co. folder.
2. Run the game once to generate the BepInEx folders, then close it.
3. Drop `RadioPlus.dll` into `BepInEx/plugins/`.

Put your music in `EasyDeliveryCo_Data/StreamingAssets/CustomRadio` as usual, then tune to
FM 88.1 in game.

Mods do not work on the Microsoft Store version of the game.

## Configuration

A config file is generated at `BepInEx/config/com.figrindan.radioplus.cfg` after the first run.

| Setting | Default | Description |
| --- | --- | --- |
| `Shuffle` | `true` | Shuffle the custom radio playlist. Set to `false` for the original alphabetical order. |
| `UnlockCustomStation` | `false` | Give FM 88.1 full signal from the start, without activating the radio tower. |

### Notes

- Shuffling happens once when the game loads, so the order stays fixed for a session. Restart the
  game for a new order.
- `UnlockCustomStation` skips a piece of the game's intended progression. It is on by default, but toggleable for
  that reason.

## Compatibility

- Tested against Easy Delivery Co. on Unity 6000.0.58, BepInEx 5.4.23.4, Windows x64.
- Should be compatible with other mods. It patches only the audio loading and radio signal code.
- The game receives regular updates. If a patch breaks something, please open an issue.

## Building

1. Copy `RadioPlus.csproj.user.example` to `RadioPlus.csproj.user`.
2. Edit it so `GameDir` points at your Easy Delivery Co. install folder.
3. `dotnet build`

The build references assemblies from your game install, so step 2 is required.

## License

MIT — see [LICENSE](LICENSE).

## Credits

Easy Delivery Co. is made by Sam C. and published by Oro Interactive. Go
[buy it](https://store.steampowered.com/app/3293010/Easy_Delivery_Co/).