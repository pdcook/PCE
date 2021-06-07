# Pykess's Card Expansion (PCE)
-------------------------------
This is a BepInEx compatible mod for Rounds that adds a number of new cards to the game.

A massive thanks to the people who helped me learn how to mod, especially Ascyst, Willis, and Tilastokeskus.

## Installation Instructions
----------------------------
Install BepInEx, you can find it [here](https://discord.gg/tAQxJbV9RG).
Install the newest versions of [UnboundLib and MMHOOK](https://github.com/willis81808/UnboundLib/releases).

Copy `PCE.dll` to `/path/to/Steam/steamapps/common/ROUNDS/BepInEx/plugins/`
Copy `pceAssetBundle` to `/path/to/Steam/steamapps/common/ROUNDS/Rounds_Data/StreamingAssets/` (if the folder doesn't already exist, create it)

### Version Notes
-----------------
- v0.1.0.0: first release, most cards do not have art yet.
    * Known issues:
        - Ghost Gun should allow bullets to penetrate shields, this does not work yet.
	- There are cards in the source code which are not built by the plugin. This is because these cards do not work yet. These card are not included in this documentation either.

- v0.1.1.0: various balance changes and bug fixes
    - Gravity now properly resets between matches
    - Murder should work in online now
        * Murder no longer works in Sandbox
    - Grounded was given a slight buff to effect duration
    - Increased the damage debuff from Laser to `-99%`
    - Bullet speed from Laser is now capped at `100`

- v0.1.2.0: various attempts at fixes
    - Murder confirmed fixed in online and local versus
    - Jackpot, Small Jackpot, Gamble, and Risky Gamble still broken

- v0.1.3.0: fixed Jackpot, Small Jackpot, Gamble, and Risky Gamble

### Suggestions, Bug Reports, and Troubleshooting
-------------------------------------------------

Please submit all bug reports by opening a new issue on this Git repository, or by sending a message in the `#bug-reports` channel of [the ROUNDS Modding Community Discord Server](https://discord.gg/tAQxJbV9RG).

Please send all suggestions in the `#mod-suggestions` channel of [the ROUNDS Modding Community Discord Server](https://discord.gg/tAQxJbV9RG), or direct message me on Discord.


If you are having trouble installing this mod, BepInEx, UnboundLib, MMHOOK, or any other Rounds mod, please send a message with your question in the `#troubleshooting` channel of [the ROUNDS Modding Community Discord Server](https://discord.gg/tAQxJbV9RG).


## Mod Overview
---------------
This mod adds a number of cards to the game. Below they are listed in no particular order.


### Laser
---------
**Uncommon**

Replaces your projectile gun with a laser instead. When fired, the entire laser charge must be used.

### Ghost Gun
-------------
***Rare***

Makes your bullets completely invisible and go through walls.

### Tractor Beam
----------------
**Uncommon**

Reverses the direction of knockback that your bullets do.

### Moon Shoes
--------------
**Uncommon**

Reduces your gravity to moon gravity (1/6 of normal.)

### Flip
--------
**Uncommon**

Bullets temporarily flip the direction of victim's gravity.

### Grounded
------------
**Uncommon**

Bullets temporarily increase the strength of victim's gravity.

### Murder
----------
***Rare***

Kill your opponent.

### Jackpot
-----------
**Uncommon**

Get a random ***Rare*** card.

### Small Jackpot
-----------------
_Common_

Get a random **Uncommon** card.

### Gamble
----------
***Rare***

Get two random **Uncommon** cards.

### Risky Gamble
----------------
**Uncommon**

Get two random _Common_ cards.

### Close Quarters
------------------
**Uncommon**

Do significantly more damage when up close, but significantly less at long range.