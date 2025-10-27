# Guide to Identifying Unknown Blocks

This guide explains how to identify unknown blocks in Pokemon Legends: Z-A save files using the PLZA Save Dumper.

## What Are Unknown Blocks?

Unknown blocks are save file data blocks that exist in the game but haven't been identified or named yet. They appear with names like `Unknown_0ABCDEF1` in the dump output.

**Important:** Unknown blocks are NOT corrupted or useless data - they're just blocks we haven't figured out yet!

## Why Do Unknown Blocks Exist?

1. **New game versions** - Game updates add new save data that PKHeX doesn't know about yet
2. **Undocumented features** - Some blocks haven't been researched thoroughly
3. **Version differences** - Different game releases may have different blocks

## Finding Unknown Blocks in Your Dump

### Method 1: Excel/CSV

1. Open `block_index.csv` in Excel
2. Click on the header row
3. Enable AutoFilter (Data → Filter)
4. Click the dropdown on the "Unknown" column
5. Check only "YES" to see all unknown blocks

**Tip:** Sort by Size to find the biggest unknown blocks - they often contain the most interesting data!

### Method 2: JSON Summary

Open `unknown_blocks_summary.json` to see:
- Total count of unknown blocks
- Analysis hints for each one
- Size and type information

### Method 3: Individual Metadata

Each unknown block has a `.meta.json` file that includes:
```json
{
  "key": "0x12345678",
  "name": "Unknown_12345678",
  "is_unknown": true,
  "size": 1024,
  "analysis_hints": {
    "first_bytes_hex": "00-01-02-03-...",
    "size_note": "Size is divisible by 4 (256 potential u32 values)",
    "mostly_zeros": "95.5% zeros (possibly unused/cleared data)"
  }
}
```

## Understanding Analysis Hints

The tool provides automatic analysis of each unknown block:

### Size-Based Hints

| Hint | Meaning | Example |
|------|---------|---------|
| "Might be a single u32/i32/float value" | Block is 4 bytes | Could be a counter, flag, or ID |
| "Might be a u64/i64/double" | Block is 8 bytes | Could be timestamp, large number |
| "Size divisible by 4 (N values)" | Array of integers | Possibly N counters or IDs |

### Content-Based Hints

| Hint | Meaning | Likely Contents |
|------|---------|-----------------|
| "95% zeros" | Mostly empty | Unused space, cleared data, or sparse array |
| "90% 0xFF bytes" | Uninitialized | Memory that hasn't been written to |
| "Repeating 4-byte pattern" | Structured data | Array with default values |
| "50% printable ASCII" | Text data | Strings, names, descriptions |

### Example Analysis

```json
"analysis_hints": {
  "size_note": "Size is divisible by 4 (10 potential u32 values)",
  "first_bytes_hex": "01-00-00-00-05-00-00-00-00-00-00-00-FF-FF-FF-FF",
  "mostly_zeros": "60.0% zeros"
}
```

**Interpretation:** This is likely an array of 10 integers (u32). The first value is 1, the second is 5, some are zero, and one is -1 (0xFFFFFFFF). Could be quest states, item counts, or achievement flags.

## How to Identify Unknown Blocks

### Step 1: Make Multiple Saves

1. Create a save before an action (e.g., before catching a Pokemon)
2. Perform the action
3. Create a save after the action
4. Dump both saves

### Step 2: Compare the Blocks

Use a hex editor or comparison tool to see what changed:

```bash
# On Windows, use a tool like:
# - WinMerge
# - HxD (with compare feature)
# - Beyond Compare

# Or use command line:
fc /b before\blocks\Unknown_12345678.bin after\blocks\Unknown_12345678.bin
```

### Step 3: Correlate Changes

**If the block changed, ask:**
- What did I do in the game?
- Did a number increase? (e.g., caught Pokemon counter)
- Did a flag flip? (e.g., quest completion)
- Did new data appear? (e.g., new Pokemon added)

### Example Research Process

**Hypothesis:** Block `Unknown_1A2B3C4D` might be a Pokemon catch counter

**Test:**
1. Dump save with 50 Pokemon caught
2. Catch 1 more Pokemon (total: 51)
3. Dump save again
4. Compare blocks

**Result:** If the block contains a u32 that changed from 50 to 51, you've likely identified it!

## Common Block Types to Look For

### Counters (4-8 bytes)
- Pokemon caught
- Battles won
- Steps taken
- Items collected

### Flags (Small blocks)
- Quest completion
- Event triggers
- Feature unlocks
- Seen/obtained flags

### Arrays (Divisible by 4 or more)
- Pokemon encounter history
- Item unlock states
- Location visit tracking
- Achievement progress

### Large Blocks (>1KB)
- Map state data
- NPC positions
- Dynamic world data
- Cache/temporary storage

## Tools for Block Research

### Hex Editors
- **HxD** (Windows) - Free, excellent for comparing files
- **010 Editor** - Powerful with template support
- **ImHex** - Modern, pattern-based analysis

### Comparison Tools
- **WinMerge** - Visual file comparison
- **Beyond Compare** - Professional diff tool
- **KDiff3** - Cross-platform comparison

### Analysis Scripts

Use the raw `.bin` files programmatically:

```python
# Example: Check if block is all zeros
with open('Unknown_12345678.bin', 'rb') as f:
    data = f.read()
    if all(b == 0 for b in data):
        print("Block is entirely zeros - likely unused")
```

```python
# Example: Read as u32 array
import struct
with open('Unknown_12345678.bin', 'rb') as f:
    data = f.read()
    values = struct.unpack(f'<{len(data)//4}I', data)  # Little-endian u32
    print(f"Values: {values}")
```

## Contributing Your Discoveries

### If You Identify a Block

1. **Verify** - Test your hypothesis multiple times
2. **Document** - Write down what the block does
3. **Contribute** - Share with the PKHeX project

### How to Contribute to PKHeX

1. Fork the [PKHeX repository](https://github.com/kwsch/PKHeX)
2. Add the block name to `SaveBlockAccessor9ZA.cs`:
   ```csharp
   private const uint KYourBlockName = 0x12345678;
   ```
3. Update `BlankBlocks9a.cs` if needed
4. Submit a pull request with description

### Documentation Format

When documenting a block:

```
Block: 0x12345678
Name: PokemonCaughtCounter
Size: 4 bytes
Type: UInt32
Purpose: Total number of Pokemon caught by the player
Location: Increments by 1 when any Pokemon is caught
Notes: Resets to 0 if save is corrupted/reset
```

## Tips for Successful Research

1. **Start small** - Focus on one unknown block at a time
2. **Be systematic** - Keep notes of your tests
3. **Test thoroughly** - Verify your findings multiple times
4. **Share findings** - Help the community!
5. **Update PKHeX.Core.dll** - Check for new versions regularly

## Example Success Stories

Real-world examples of how blocks were identified:

### Mystery Gift Counter
- **Observation:** Block size is 4 bytes
- **Test:** Received mystery gift, block increased by 1
- **Result:** Identified as mystery gift counter

### Clothing Unlock Flags
- **Observation:** Large array, mostly zeros
- **Test:** Bought clothing, specific bit changed
- **Result:** Each bit represents one clothing item

### Quest State Array
- **Observation:** Array of u16 values
- **Test:** Completed quest, value changed from 0 to 1
- **Result:** Each entry tracks quest completion

## Need Help?

If you're stuck identifying a block:
1. Check the PKHeX Discord/community
2. Compare with others' dumps
3. Look for patterns across multiple saves
4. Ask the community for help

## Remember

**Every identified block helps the entire Pokemon research community!**

Your discoveries make tools like PKHeX better for everyone. Even small contributions matter!

---

*Happy researching! 🔍✨*
