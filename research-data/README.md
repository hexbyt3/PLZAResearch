# Pokemon Legends: Z-A Research Data

This folder contains sample dumps from Pokemon Legends: Z-A save files for research purposes.

## Sample Dumps

### sample-dump-20251027

First research dump containing:
- **162 total blocks** decrypted and documented
- **58 unknown blocks** requiring identification
- Complete block index with analysis hints
- Unknown blocks summary with automatic analysis

**Files included:**
- `block_index.json` - Complete block reference with metadata
- `block_index.csv` - Excel-friendly format (filter by Unknown=YES)
- `unknown_blocks_summary.json` - Analysis of unidentified blocks
- `blocks/*.meta.json` - Individual block metadata with analysis hints

**Note:** Binary `.bin` files are excluded from the repository for privacy and file size reasons. Only metadata and analysis files are included.

## Block Analysis

This dump includes automatic analysis for all unknown blocks:

- **Size patterns** - Helps identify data types (u32, arrays, etc.)
- **Content analysis** - Detects zeros, patterns, possible text
- **First bytes preview** - Hex dump of block starts
- **Type suggestions** - Educated guesses about block purpose

## How to Use This Data

1. **Open `block_index.csv` in Excel** - Filter by "Unknown=YES" to see unidentified blocks
2. **Review `unknown_blocks_summary.json`** - See analysis hints for all unknowns
3. **Compare with your own dumps** - Help identify what blocks do
4. **Contribute findings** - Share discoveries with the PKHeX project

## Contributing

If you identify any unknown blocks:

1. Document your findings
2. Test thoroughly across multiple saves
3. Submit a pull request with updated block names
4. Help the community by sharing your research!

See [IDENTIFYING_BLOCKS.md](../IDENTIFYING_BLOCKS.md) for a complete guide on identifying unknown blocks.

## Research Notes

### Known Blocks (104)

Major categories:
- **Core Data**: BoxData, PartyData, Items, Pokedex, MyStatus
- **Progression**: EventFlag, EventWork, Money, PlayTime
- **Fashion**: 20+ blocks for clothing and appearance
- **Pictures**: SBC, Initial, and Current photos
- **Other**: Coordinates, Royale data, Mystery Gifts

### Unknown Blocks (58)

Requires research to identify. Common patterns observed:
- Small blocks (4-8 bytes) - Likely counters or flags
- Medium blocks (hundreds of bytes) - Possibly arrays or structured data
- Large blocks (thousands of bytes) - May contain complex game state

## Privacy Note

Binary save data (`.bin` files) are not included in this repository to:
- Protect user privacy (trainer names, IDs, etc.)
- Keep repository size manageable
- Focus on research metadata rather than raw data

All analysis can be performed using the metadata files alone.

---

*Research data for Pokemon Legends: Z-A save file structure analysis*
