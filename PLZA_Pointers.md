# Pokemon Legends: Z-A - Memory Pointers

Memory pointer chains for Pokemon Legends: Z-A.

## Game Version: 1.0.1

---

## Box 1 - Slot 1

```
[[main+41F09A0]+950]
[[[main+5F0B250]+B0]+978]
[[[main+41F0B00]+288]+978]
[[[[main+41F04C0]+1E8]+2B0]+978]
[[[[main+5F28C08]+210]+2B0]+978]
[[[[main+5F0B250]+F8]+2B0]+978]
[[[[main+41F0B00]+2D0]+2B0]+978]
[[[[main+5F0B250]+B0]+10]+978]
[[[[main+41F0B00]+288]+10]+978]
[[[[main+3D7FE48]+AB0]+10]+978]
[[[[main+5F2C870]+B50]+10]+978]
[[[main+3D7FE48]+AB0]+978]
[[[main+5F2C870]+B50]+978]
[[main+3D7FE48]+1418]
```

---

## Overworld State

Used for detecting menu/UI state. Returns 1 byte.

**Value meanings:**
- `0x01` = Player in overworld (can move freely, no menus open)
- `0x00` = Menu open or player movement blocked

**Pointer (recommended):**
```
[main+3CC4D30]+110
```

**Alternative pointers:**
```
[main+3CE0208]+E0
[main+3CC5100]+130
```

---

## Online Connection Status

Used for detecting if the player is connected to Nintendo Switch Online. Returns 1 byte.

**Value meanings:**
- `0x01` = Connected online
- `0x00` = Offline / not connected

```
[[[main+5F2A1F0]+18]]+08
```

---

## Link Trade Partner Pokemon

Points to the Pokemon offered by the trade partner during Link Trades. Returns 344 bytes (0x158).

**Pointer (recommended - same region as Box data):**
```
[[[main+5F0E2B0]+128]+30
```

**Alternative pointers:**
```
[[[main+45E5960]+100]+30
[[[main+45E67E0]+88]+30
[[main+45E67E0]+A8]
```

**Note:** The recommended pointer base (0x5F0E2B0) is very close to BoxStartPointer (0x5F0B250), indicating it's in the same stable memory region.

---

## Trade Partner Trainer Status

Points to the trade partner's trainer information (TID/SID/Name) during Link Trades.

**Note:** ID32 (combined TID16+SID16) is located at offset +0x74 within the structure these pointers reference.

### Trader 1 Status
```
[[[[main+3EFD058]+1D8]+180]+80]+74
```

### Trader 2 Status
```
[[[[main+3EFD058]+1E8]+180]+80]+74
```

**Pattern:** Both pointers share the same base (0x3EFD058) and differ only at the second level (0x1D8 vs 0x1E8, 16 bytes apart).

---

*Last updated: 2025-10-27*
