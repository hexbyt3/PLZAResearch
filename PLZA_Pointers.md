# Pokemon Legends: Z-A - Memory Pointers

Memory pointer chains for Pokemon Legends: Z-A.

## Game Version: 1.0.2

---

## Box 1 - Slot 1

Box 1 Slot 1 Pokemon data. Successfully tested with Pokemon injection/reading.

**Pointer (recommended):**
```
[[[main+5F0C250]+B0]+978]
```

---

## Online Connection Status

Used for detecting if the player is connected to Nintendo Switch Online. Returns 1 byte.

**Value meanings:**
- `0x01` = Connected online
- `0x00` = Offline / not connected

**Pointer (recommended):**
```
[[main+3CE1510]+08]
```

---

## Link Code Trade

Returns the 4-byte integer link code currently entered (0x0 if empty).

**Pointer (recommended):**
```
[[main+3CC8C20]+24]
```

---

## Link Trade Partner NID

Returns 8-byte ulong Nintendo ID of connected trade partner. Works correctly on first trade and all subsequent trades.

**Pointer (recommended):**
```
[[[[[main+3EFE058]+120]+38]+10]+38]
```

---

## MyStatus

Contains player name, IDs, and trainer data.

**Pointer (recommended):**
```
[[[main+5F0C250]+A0]+40]
```

---

## Overworld State

Used for detecting overworld state. Returns 1 byte.

**Value meanings:**
- `0x00` = Player in overworld
- `0x01` = In menus/trades

**Pointer (recommended):**
```
[[main+3CC5990]+11]
```

---

## Game State / Trade Animation Indicator

Used to actively detect when trade animations complete. Returns 1 byte.

**Value meanings:**
- `0x01` = Normal (before trade, after animation completes)
- `0x02` = Trade animation in progress

**Pointer (recommended):**
```
[[main+5F59608]+58]
```

---

## Trade Partner Trainer Status (Trader 1)

Contains trainer info for trade partner in multi-trade. Seems inconsistent at times, may need a more resilient pointer.

**Pointer (recommended):**
```
[[[[[main+3EFE058]+1D8]+180]+80]+74]
```

---

## Trade Box Status Indicator

Used to detect when we've entered the trade box and partner data is loaded. Returns 1 byte.

**Value meanings:**
- `0x01` = In trade box screen with partner
- `0x00` = Not in trade box (link code screen, searching, etc.)

**Pointer (recommended):**
```
[[main+41FE140]+108]
```

---

## Link Trade Partner Pokemon

Points to the Pokemon offered by the trade partner during Link Trades. Returns 344 bytes (0x158).

**Pointer (recommended):**
```
[[[main+5F0E2B0]+128]+30]
```

---

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

**Pointer (recommended):**
```
[main+3CDDF00]+D35
```

**Alternative pointers:**
```
[main+3DE5790]+D35
[main+3CDDE10]+E39
[main+3CDDD28]+E3D
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

## Trade Partner NID

Points to the trade partner's Network ID (NID) during Link Trades.

**Pointer (recommended):**
```
[[[[main+3EFD058]+120]+38]+10]+38
```

**Pattern:** Shares the same base (0x3EFD058) as Trade Partner Trainer Status.

---

## MyStatus

Points to the player's trainer status information.

**Pointer (recommended):**
```
[main+41F0960]+18
```

**Alternative pointers:**
```
[[[[main+5F2CBC0]+10]+10]+10]+40
[[[[main+41F08E0]+A8]+10]+10]+40
[[[[main+5F0B250]+A0]+10]+10]+40
[[[[main+41F0B00]+278]+10]+10]+40
[[[main+5F2CBC0]+10]+10]+40
[[[main+41F08E0]+A8]+10]+40
[[[main+5F0B250]+A0]+10]+40
[[[main+41F0B00]+278]+10]+40
[[main+5F2CBC0]+10]+40
[[main+41F08E0]+A8]+40
[[main+5F0B250]+A0]+40
[[main+41F0B00]+278]+40
[main+5F2CBC0]+40
```

---

*Last updated: 2025-10-28*
