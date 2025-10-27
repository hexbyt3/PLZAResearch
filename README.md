# Pokemon Legends: Z-A Save File Dumper

A Windows desktop application for decrypting and analyzing Pokemon Legends: Z-A save files. This tool extracts all save data blocks, providing researchers and enthusiasts with complete access to game data structures.

![Platform](https://img.shields.io/badge/platform-Windows-blue)
![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![License](https://img.shields.io/badge/license-GPLv3-green)

## What Does This Do?

This application decrypts Pokemon Legends: Z-A save files using the SwishCrypto encryption system (implemented by PKHeX). Once decrypted, it exports all save data in both raw binary format and structured JSON, making it easy to:

- Research game mechanics and save file structure
- Analyze Pokemon data, items, and progression
- Compare save states to understand game behavior
- Build additional tools and utilities
- Document the save format for preservation

## Features

- **Complete Block Extraction** - Dumps all 104+ save data blocks individually
- **Hash Validation** - Verifies save file integrity before processing
- **Structured Exports** - Party Pokemon, boxes, and items in readable JSON format
- **Comprehensive Indexing** - CSV and JSON indexes of all blocks for easy reference
- **User-Friendly GUI** - Simple Windows application, no command line needed
- **Automatic Output** - Opens the dump folder when complete

## Quick Start

1. **Download or clone** this repository
2. **Double-click** `LAUNCH.bat`
3. **Select your save file** using the file browser
4. **Click "Decrypt & Dump Save"** and wait for completion

That's it! The first run will automatically build the application (takes about 5 seconds).

## Requirements

- **Windows** - This is a native WinForms application
- **.NET 9.0 Runtime** - [Download here](https://dotnet.microsoft.com/download/dotnet/9.0) if you don't have it
- **PKHeX.Core.dll** - Already included in the `lib/` folder

## Output Structure

Each dump creates a timestamped folder containing:

```
dumps/dump_20251027_143022/
├── blocks/                          # Individual block dumps
│   ├── 0D66012C_BoxData.bin        # Raw binary data
│   ├── 0D66012C_BoxData.meta.json  # Block metadata (type, size, analysis hints)
│   └── ... (100+ more blocks)
│
├── structured/                      # Human-readable exports (if supported)
│   ├── party/                       # Individual Pokemon JSONs
│   │   ├── slot_0_Pikachu.json
│   │   └── ...
│   ├── box_summary.json             # Pokemon count per box
│   └── items.json                   # All items and quantities
│
├── block_index.json                 # Complete block reference with "is_unknown" flag
├── block_index.csv                  # Excel-friendly list (filter by Unknown=YES)
├── unknown_blocks_summary.json      # Analysis of unidentified blocks (if any)
└── save_metadata.json               # Trainer info and save details (if supported)
```

## Key Save Blocks

The save file contains over 100 named blocks. Here are some of the most important ones:

### Core Game Data
- **BoxData** (0x0D66012C) - All Pokemon in storage (391,680 bytes)
- **PartyData** (0x3AA1A9AD) - Your 6-party Pokemon (2,880 bytes)
- **Items** (0x21C9BD44) - Inventory and item quantities (48,128 bytes)
- **MyStatus** (0xE3E89BD1) - Trainer details (name, ID, gender, etc.)
- **Pokedex** (0x2D87BE5C) - Pokedex completion data (159,848 bytes)

### Progression & State
- **EventFlag** (0x58505C5E) - Game event flags (32,768 bytes)
- **EventWorkQuest** (0xB9B223B9) - Quest variables (16,384 bytes)
- **Money** (0x4F35D0DD) - Current money (4 bytes)
- **PlayTime** (0xEDAFF794) - Total play time (12 bytes)
- **Coordinates** (0x910D381F) - Player position (112 bytes)

### Customization
- **PlayerFashion** (0x64235B3D) - Current outfit
- **FashionTops/Bottoms/etc.** - Unlocked clothing items
- **HairMake** series - Hair, makeup, and appearance unlocks

See the generated `block_index.csv` for a complete list with sizes and descriptions.

## Identifying Unknown Blocks

When you dump a save file, you may encounter "unknown" blocks - these are blocks that exist in the save but haven't been identified yet.

### How to Find Unknown Blocks

1. **Open `block_index.csv` in Excel** - Filter by `Unknown = YES` to see all unidentified blocks
2. **Check `unknown_blocks_summary.json`** - Contains analysis hints for each unknown block
3. **Look at the metadata files** - Each unknown block's `.meta.json` includes analysis hints

### Analysis Hints

The tool automatically analyzes unknown blocks and provides hints like:

- **Size patterns** - "Size divisible by 4" suggests an array of integers
- **Content type** - "Mostly zeros" or "Possible text" helps narrow down what it might be
- **First bytes preview** - See the start of the data in hex format
- **Pattern detection** - Repeating patterns might indicate array structures

### How to Help Identify Them

**Compare across saves:**
1. Dump multiple saves (before/after specific events)
2. Compare unknown blocks to see what changed
3. Correlate changes with in-game actions

**Example:** If an unknown block increases by 1 after catching a Pokemon, it might be a catch counter!

**Share your findings:**
- If you identify a block, contribute to the [PKHeX project](https://github.com/kwsch/PKHeX)
- Submit a pull request with the block name mapping
- Help the community by documenting your discoveries

### Why Keep Unknown Blocks?

- They contain valuable game data
- Future PKHeX updates might identify them
- They're essential for complete save file research
- Comparison across saves helps identify their purpose

## Building from Source

The application auto-builds on first launch, but if you want to build manually:

```bash
# Using the provided batch file
Build.bat

# Or using .NET CLI
dotnet build -c Release

# The executable will be at:
bin\Release\net9.0-windows\PLZASaveDumper.exe
```

## Technical Details

### SwishCrypto Encryption

Pokemon Legends: Z-A uses a multi-layered encryption scheme:

1. **Static XOR Pad** - 128-byte repeating key applied to the entire save file
2. **Block-Level Encryption** - Each block is encrypted with a seeded XorShift32 cipher
3. **SHA256 Hash** - Integrity verification with salted hash (64-byte intro + payload + 64-byte outro)

The decryption process is handled entirely by PKHeX.Core's SwishCrypto implementation.

### Block Format

Each block consists of:
- **Key** (4 bytes) - FNV-1a hash of the block's name
- **Type** (1 byte, encrypted) - Bool, Object, Array, or primitive type
- **Metadata** (variable, encrypted) - Size/count information
- **Data** (variable, encrypted) - The actual block content

### Project Architecture

- **MainForm.cs** - GUI implementation and user interaction
- **BlockNames.cs** - Static mapping of block keys to human-readable names
- **Program.cs** - Application entry point
- **PLZASaveDumper.csproj** - .NET project configuration

## Credits

This project wouldn't exist without the incredible work of the Pokemon reverse engineering community:

### Primary Contributors

- **Kurt (kwsch)** - Creator of [PKHeX](https://github.com/kwsch/PKHeX), which provides the core SwishCrypto decryption implementation and save file handling. PKHeX is the gold standard for Pokemon save editing and has enabled countless research projects like this one.

### Technology Stack

- **.NET 9.0** - Microsoft's modern, cross-platform framework
- **Windows Forms** - Classic Windows GUI framework
- **PKHeX.Core** - Save file handling and encryption (GPLv3)

### Additional Thanks

- The PKHeX community for documenting Pokemon save structures
- Nintendo and The Pokemon Company for creating Pokemon
- All the data miners and researchers who contribute to understanding these games

## License

This project is licensed under **GPLv3**, the same license as PKHeX.

This means:
- ✅ You can use, modify, and distribute this software
- ✅ You can use it for commercial purposes
- ⚠️ You must disclose the source code of any modifications
- ⚠️ You must license derivative works under GPLv3
- ⚠️ You must provide attribution

See the full [GPLv3 license text](https://www.gnu.org/licenses/gpl-3.0.en.html) for details.

## Legal Disclaimer

This tool is for **educational and research purposes only**.

- You should only use this with your own legally obtained save files
- This tool does not modify save files - it only reads and exports data
- Respect Nintendo's and The Pokemon Company's intellectual property
- Use of this software is at your own risk

## Troubleshooting

### "Application won't start"

Make sure you have .NET 9.0 Runtime installed:
```bash
dotnet --version
```

If not installed, download from [Microsoft's website](https://dotnet.microsoft.com/download/dotnet/9.0).

### "PKHeX.Core.dll not found"

The DLL should be in the `lib/` folder. If it's missing, you'll need to build PKHeX.Core yourself:

1. Clone [PKHeX](https://github.com/kwsch/PKHeX)
2. Build the Release version: `dotnet build -c Release`
3. Copy `PKHeX.Core.dll` to this project's `lib/` folder

### "Hash validation failed"

This means the save file's integrity check didn't pass. Possible causes:
- The file is corrupted
- The file was manually edited
- The file isn't a valid Z-A save

The tool will ask if you want to continue anyway. Decryption might still work even with an invalid hash.

### "Could not create SAV9ZA instance" / Unknown block type

If you see warnings about unknown block types or SAV9ZA creation failing, this means:
- Your save is from a newer game version/update than the PKHeX.Core.dll was built for
- The game added new block types that PKHeX doesn't recognize yet

**Don't worry!** The tool will still:
- Decrypt and dump all blocks successfully
- Create the block index files
- Export all raw binary data

You just won't get the parsed structured data (party Pokemon JSONs, etc.). The raw blocks contain all the data - they're just not automatically parsed into readable format.

**To fix:** Update PKHeX.Core.dll from the latest PKHeX build once it supports your save version.

## Contributing

Contributions are welcome! If you'd like to add features or fix bugs:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

Please ensure your code follows the existing style and includes appropriate comments.

## Support

If you encounter issues or have questions:
- Check the [Troubleshooting](#troubleshooting) section
- Review existing issues on GitHub
- Create a new issue with detailed information

## Changelog

### v1.0.0 (2025-10-27)
- Initial release
- Complete save file decryption
- All 104+ blocks identified and dumped
- GUI interface with progress tracking
- Structured JSON exports for party, boxes, and items
- Comprehensive documentation

---

**Built with appreciation for the Pokemon research community** 🎮✨

*Special thanks to Kurt for maintaining PKHeX and making Pokemon research accessible to everyone.*
