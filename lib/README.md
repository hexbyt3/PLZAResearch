# Library Dependencies

This folder contains the .NET assemblies required by the PLZA Save Dumper.

## Contents

- **PKHeX.Core.dll** (~17 MB) - Core PKHeX library for Pokemon data structures and SwishCrypto decryption

## Version Information

- **Build**: Release (net9.0)
- **Source**: https://github.com/kwsch/PKHeX
- **License**: GPLv3

## Updating PKHeX.Core.dll

To update to the latest version from the PKHeX repository:

```bash
# Navigate to your PKHeX source directory
cd path/to/PKHeX/PKHeX.Core

# Build Release version
dotnet build -c Release

# Copy to this project's lib folder
cp bin/Release/net9.0/PKHeX.Core.dll path/to/PLZAResearch/lib/
```

## License

PKHeX.Core is licensed under the GPLv3. See the [PKHeX repository](https://github.com/kwsch/PKHeX) for full license details.
