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

```
[main+5F58488]+0B
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
