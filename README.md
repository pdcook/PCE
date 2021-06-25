# Pykess's Card Expansion (PCE)
-------------------------------
This is a BepInEx compatible mod for Rounds that adds a number of new cards to the game.

A massive thanks to the people who helped me learn how to mod, especially Ascyst, Willis, and Tilastokeskus.

## Easy Installation Instructions
---------------------------------

Download [r2modman](https://rounds.thunderstore.io/package/ebkr/r2modman/), set up a Rounds profile, and add `PCE` to the profile. All dependencies will be automatically installed. Just click `Start Modded` to start playing!

## Manual Installation Instructions
----------------------------
1. Install BepInEx, you can find it [here](https://rounds.thunderstore.io/package/BepInEx/BepInExPack_ROUNDS/).

2. Install the *newest versions* of [UnboundLib and MMHOOK](https://github.com/Rounds-Modding/UnboundLib/releases).

3. Install `PlayerJumpPatch` which you can find [here](https://rounds.thunderstore.io/package/Pykess/PlayerJumpPatch/).

4. Copy `PCE.dll` to `/path/to/Steam/steamapps/common/ROUNDS/BepInEx/plugins/`

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

- v0.1.4.0: various bug fixes and balancing
    - Jackpot, Small Jackpot, Gamble, and Risky Gamble fixed in online and confirmed working
    - Increased damage debuff on Ghost Bullets to `-75%`
    - Decreased damage debuff on Laser to `-98%`
    - Increased rarity of Laser to Rare
    - Increased simulation speed of Laser projectiles
    - Fixed errors/warnings from `CharacterStatModifiers` `Postfix`
    - Known issues:
    	* BepInEx warnings from custom cards that return `null` for `CardInfo`
        * Murder does not work in Sandbox
        * Laser has issues rendering on the remote side
        * Ghost Bullets do not penetrate shields
        * All cards except Laser are missing art

- v0.1.5.0: more bug fixes
    - Jackpot, Small Jackpot, Gamble, and Risky Gamble fixed again (???)
        - Can no longer roll a Jackpot/Gamble card from another Jackpot/Gamble card
    - Known issues:
        - Murder does not work with the FFA mod

- v0.1.5.1: typo fix
    - Fixed logic typo in Jackpot, Small Jackpot, Gamble, and Risky Gamble which caused them to always return another Jackpot/Gamble card

- v0.1.5.2: bugfixes and added art
    - ***pceAssetBundle now goes in the BepInEx plugins folder***
    - Murder now works in Sandbox, local versus, and online versus
    - Added art to a number of cards
    	* Jackpot
        * Small Jackpot
        * Murder
        * Ghost Bullets

- v0.1.5.3: bugfixes, Jotunn, balancing
    - ***Assets are now included directly in*** `PCE.dll` ***no need to download or place any asset files anywhere***
    - ***REQUIRES THE NEWEST VERSION OF UNBOUNDLIB***
    - Fixed issue where gravity effects would not properly reset between rounds if player was shot multiple times in quick succession
    - Increased damage debuff on Laser to `-99%`

- v0.1.6.0: new card
    - new card: Discombobulate
    - Known issues:
        - Cards without art do not display properly in the card bar, this is an issue with the newest version of UnboundLib
        - Discombobulate does not have any particle effects when activated

- v0.1.6.1: bugfix
    - Once again, fixing gravity effects between rounds

- v0.1.6.2: bugfixes, custom gamemode support
    - Fixed gravity effects reset between rounds
    - Murder and other cards should work with custom gamemodes now

- v0.1.6.3: bugfixes, balancing
    - Fixed Discombobulate not resetting after death
    - Fixed Close Quarters not resetting between games
    - Balanced gravity effects
    - Gravity effects no longer launch you at the ground once finished

- v0.1.7.0: new card, Old Jetpack
    - New card
    - Patched PlayerJump logic so that arbitrary numbers of jumps are possible

- v0.1.7.1: moved PlayerJump Patch to separate utility mod

- v0.1.7.2: bugfix - updated Gamemode hooks to be compatible with newest version of unbound

- v0.1.8.0: new cards, new code, new bugfixes
    - New cards:
        - Ant: become twice as small, but twice as powerful. Be careful not to get stepped on.
        - Low Ground: Get boosted stats when beneath an enemy player.
        - Demonic Possession: Become a being of pure chaos.
    - New code:
        - PCE now depends on LegRaycastersPatch
        - Complete refactor and rewrite of many effects to streamline development
    - New bugfixes:
        - Gravity effects have been completely rewritten *again* to properly reset
        - Laser has been adjusted to work better when stacked with other cards
        - Effects that change the player's color will now properly reset
        - Effects that change the player's stats (i.e. Discombobulate) will now properly stack and reset
    - New additions:
        - Discombobulate now has a color effect on enemy players
        - Old Jetpack and Moon Shoes are now _common_ cards
    - New known issues:
        - Most cards are still missing art because art takes a long time for me
        - Low Ground's formatting looks terrible
        - Discombobulate still doesn't have a visual effect like Overpower. If you know how to make/add one, please let me know.

- v0.1.8.1: Silent update
    - New card: Thank You Sir, May I Have Another?

- v0.1.8.2: Lots of bugfixes, one new card
    - Fixed issue with gravity effects not stacking properly
    - Fixed issue with gravity effects not resetting properly between rounds
    - Fixed issue with multiple jumps persisting between games (See PlayerJumpPatch)
    - Fixed issue where having multiple Low Ground cards at the end of a match could cause the player's stats to be messed up in the next match
    - Slowed down the effects of DemonicPossession
    - New card: Glare


### Suggestions, Bug Reports, and Troubleshooting
-------------------------------------------------

Please submit all bug reports by opening a new issue on this Git repository, or by sending a message in the `#bug-reports` channel of [the ROUNDS Modding Community Discord Server](https://discord.gg/tAQxJbV9RG).

Please send all suggestions in the `#mod-suggestions` channel of [the ROUNDS Modding Community Discord Server](https://discord.gg/tAQxJbV9RG), or direct message me on Discord.


If you are having trouble installing this mod, BepInEx, UnboundLib, MMHOOK, or any other Rounds mod, please send a message with your question in the `#troubleshooting` channel of [the ROUNDS Modding Community Discord Server](https://discord.gg/tAQxJbV9RG).


## Mod Overview
---------------
This mod adds a number of cards to the game. Below they are listed in no particular order.

---

### Laser
***Rare***

Replaces your projectile gun with a laser instead. When fired, the entire laser charge must be used.

---

### Ghost Gun
***Rare***

Makes your bullets completely invisible and go through walls.

---

### Tractor Beam
**Uncommon**

Reverses the direction of knockback that your bullets do.

---

### Moon Shoes
**Uncommon**

Reduces your gravity to moon gravity (1/6 of normal.)

---

### Flip
**Uncommon**

Bullets temporarily flip the direction of victim's gravity.

---

### Grounded
**Uncommon**

Bullets temporarily increase the strength of victim's gravity.

---

### Murder
***Rare***

Kill your opponent.

---

### Jackpot
**Uncommon**

Get a random ***Rare*** card.

---

### Small Jackpot
_Common_

Get a random **Uncommon** card.

---

### Gamble
***Rare***

Get two random **Uncommon** cards.

---

### Risky Gamble
**Uncommon**

Get two random _Common_ cards.

---

### Close Quarters
**Uncommon**

Do significantly more damage when up close, but significantly less at long range.

---

### Discombobulate
**Uncommon**

Blocking reverses nearby player's controls

---

### Old Jetpact
_Common_

Sputter around in the air for a bit by continually firing an old jetpack

---

### Ant
**Uncommon**

Halve in size, double in strength.

---

### Demonic Possession
***Rare***

Become a being of pure chaos.

---

### Thank You Sir, May I Have Another?
**Uncommon**

Pick up the bullets that hit you

---

### Glare
**Uncommon**

Enemies freeze in fear when you see them.
