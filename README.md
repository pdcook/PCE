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

3. Install all other dependencies, which are listed [here](https://rounds.thunderstore.io/package/Pykess/PCE/).

4. Copy `PCE.dll` to `/path/to/Steam/steamapps/common/ROUNDS/BepInEx/plugins/`

### Version Notes
-----------------

#### v2.7.3
-----------

- Lots of code refactoring
- Laser v3 has been rebalanced, should be more satisfying to use
- Discombobulate now has a visual effect!
- Fixed Shuffle's description to make since in the new DrawNCards meta
- Jackpot/Gamble cards now work to give _any_ card if there are no valid cards; so you can play a Jackpot/Gamble only match by disabling all other cards

#### v2.7.2
-----------

- Balancing
    - Comb
        - No longer increases projectile speed
        - Now reduces total ammo by 1/3
        - Comb bullets now have 50% the damage of the main bullet
    - Flip
        - Increased gravity strength
        - Removed cap on duration
    - Grounded
        - Increased duration
    - Discombobulate
        - Increased duration
    - Ant
        - Ants can now shield being squished

#### v2.7.1
-----------

- Bugfix: Laser Syncing online

#### v2.7.0
-----------

- Laser rework
    - Laser now functions completely differently (again!)
    - Press middle-mouse (or d-pad down) to switch weapons to or from your laser cannon
- Pac-Bullets now applies before any screenedgebounce effects
- Switched version strings to x.x.x format consistent with ThunderStore

#### v0.2.6.0
-------------

- New card: Comb
- Balance changes;
    - Murder now kills _all_ opponents
- Bugfixes:
    - Pac-Player now works in Sandbox

#### v0.2.5.1
-------------

- Fixed bug that caused Pac-Player to not work online
- Corrected typo in Pac-Player stats

#### v0.2.5.0
-------------

- New card: Pac-Player
- Moon shoes' no-height-limit effect now lasts 5 seconds per moon shoes
- Thank You Sir May I Have Another now grants +50% health
- Demonic Possession's pop effect now goes through walls

#### v0.2.4.4
-------------

- Fixed compatibility issue with Shuffle in the upcoming update to Competitive Rounds
- BossSloth migrated several tools to ModdingUtils

#### v0.2.4.3
-------------

- Completely overhauled classed cards code, gameplay should mostly be uneffected aside from bugs being fixed

#### v0.2.4.2
-------------

- Fixed bug with Moon shoes out of bounds effect persisting
- Changed how Super Jump's out of bounds effect worked
- Fixed minor bug with Pacifist/Masochist/Survivalist/Wildcard
- Fixed bug where Low Ground would not trigger if the player started below another player during the RFW FFA countdown

#### v0.2.4.1
-------------

- Nerfed Moon Shoes

#### v0.2.4.0
-------------

- Added several new cards
    - Masochist I/II/III/IV
    - Piercing Bullets
    - Punching Bullets
- Balance changes
    - Moon shoes now allows players to jump above the top of the screen
    - Moon shoes now causes the player to automatically block (for free) at the apex of their first jump

#### v0.2.3.4
-------------

- Changes to existing cards
    - Mulligan can now stack! And is now compatible with Beetle from CR
    - Laser's size has been drastically increased
    - Laser shots now go precisely where the cursor is aimed
    - Straight Shot bullets now go precisely where the cursor is aimed

#### v0.2.3.3
-------------

- Fixed glitched cards not working with RWF

#### v0.2.3.2
-------------

- Added art for WildCards

#### v0.2.3.1
-------------

- New Art
    - All Pacifist Cards
    - Pac-Bullets
    - Shuffle
- Balance changes
    - Retreat and Last Stand now activate at 50% max HP
- Bugfixes
    - Glitched cards no longer freeze players
- Code stuffs
    - Updated for compatibility with Unbound v2.5.0


#### v0.2.3.0
-------------

- **Collab with ZZComic:** Added three new cards, one for each rarity
- Significantly buffed Laser's damage, removed attack speed debuff
- Reworked Super Jump - players can now use a super jump to jump past the top of the screen. Players also automatically block at the apex of a super jump.

#### v0.2.2.3
-------------

- Shuffle now triggers immediately after a player picks it
- Shuffle should work with PickTwoCards mod now
- Offloaded a lot of utilities to ModdingUtils mod
- Updated dependency for Unbound
- Fixed desync of the cards that are shown automatically from Jackpot/Gamble cards

#### v0.2.2.2
-------------

- Readded laser which should work online again

#### v0.2.2.1
-------------

- "Emergency" Hotfix
    - Fixed some gamebreaking bugs
        - All secret cards were prevented from spawning online
        - Shuffle did not work consistently
        - Laser did not work online (has once again been temporarily removed while its being fixed)
        - Gamble and Risky Gamble could give players unallowed cards online
    - Players can now pick multiple shuffle cards without issue
    - Risky Gamble can now give players shuffle cards

#### v0.2.2.0
-------------

- New Cards, New Menus, New Displays, New Dependencies, New Things!
    - Cards:
        - Fireworks
        - Fragmentation
        - Pac-Bullets
        - Shuffle
    - Using the newest version of Unbound, PCE now has a credits menu
    - Jackpot and Gamble cards now automatically show what cards they drew at the end of the pick phase
    - New dependency: TemporaryStatsPatch. Patches bugs in the base game that caused issues with some cards when combined with modded cards

#### v0.2.1.2
-------------

- Hotfix: Laser's hurtbox should be better synced with its visuals now.

#### v0.2.1.1
-------------

- Laser Balancing
    - Buffed damage of Laser

- Important bugfixes
    - Ghost Bullets and Phantom (from Cards+) are now exclusive
    - Laser now works after going to the main menu
    - Switched to using better card category system built into CardChoiceSpawnUniqueCardPatch
    - Fixed issue that caused one of the secret cards to not spawn properly

#### v0.2.1.0
-------------

- Laser has returned! And hopefully works.
    - A few hopefully small known issues:
        - After main-menuing, laser no longer works
        - Laser does not work while the Rain/Wall/Nuke effects from Demonic possession are active
- Using the new version of Unbound, cards now say PCE in the bottom left

#### v0.2.0.3
-------------

- Hotfix: fixed issue where certain effects wouldn't work when Target Bounce card was disabled

#### v0.2.0.2
-------------

- Balance changes
    - Straight shot now comes with reload speed buff
- New cards
    - Retreat
    - Super Ball
- Bugfixes
    - King Midas effect should no longer persist between rounds
    - _Temporarily_ removed Laser. Currently working on a complete overhaul.

#### v0.2.0.1
-------------

- Balance changes
    - Friendly bullets is now common
    - Super jump now charges twice as fast
- New card
    - Straight Shot

#### v2.0
---------

- **v2.0.0: 20+ new cards**
    - New cards! (more details below)
        - Survivalist I, II, III, and IV
        - Pacifist I, II, III, and IV
        - Wildcard I, II, III, and IV
        - Jetpack
        - Last Stand
        - Super Jump
        - Mulligan
        - Friendly Bullets
        - King Midas
        - *and some secret cards too*
    - Rigorous playtesting
        - Cards should be significantly less buggy and more balanced
        - Ant speed increase is now idempotent and adds -2 bullets
        - Demonic Possession effects have been nerfed all around
        - Flip and Grounded now deactivate when the player hits the killbox
        - Flip is slightly weaker
        - Grounded lasts slightly longer
        - Players are now ejected from the ground when hit by Flip
        - Ghost Bullets now penetrate shields properly
        - Low Ground's stat bonuses have been reduced
        - Old Jetpack (and Jetpack) now preserve the player's original jump height and jump bonuses (like double jump)
        - Thank You Sir May I Have Another now only triggers on bullet hit, not damage over time
    - More code nonsense
        - New dependencies
            - UnboundLib 2.1.5+
            - CardChoiceSpawnUniqueCardPatch
            - PlayerJumpPatch 0.0.0.2+
            - GunUnblockablePatch
        - New frameworks
            - Added abstracts for Hit and WasHit effects
            - Revised ReversibleEffects and added CounterReversibleEffects
            - Player Color Effects now stack properly

#### Older Versions
-------------------

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

Add a laser to your gun

---

### Ghost Bullets
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

Kill your opponents.

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

### Old Jetpack
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

---

### Low Ground
**Uncommon**

Gain a significant stat boost when below an enemy player

---

### Friendly Bullets
*Common*

Friendly fire damage is significantly reduced

Idea credit: @Pepper\_Jack on the rounds modding discord.

---

### Jetpack
**Uncommon**

A shiny ***new*** jetpack that actually works!

---

### King Midas
**Uncommon**

Touching any other player turns them to solid gold

---

### Last Stand
*Common*

Gain increased attack stats when below 50% of your max HP

---

### Retreat
*Common*

Gain increased defense stats when below 50% of your max HP

---

### Mulligan
**Uncommon**

Always survive a fatal blow

---

### Super Jump
*Common*

Crouch to charge a super jump

---

### **Survivalist I / II / III / IV**

*Cannot be combined with any card from any other class (Masochist, Pacifist, or Wildcard)*

- I (*Common*) - Increased reload speed the longer you go without taking damage
- II (**Uncommon**) - Decreased block cooldown the longer you go without taking damage
- III (**Uncommon**) - Increased movement speed the longer you go without taking damage
- IV (***Rare***) - Increased damage the longer you go without taking damage

### **Pacifist I / II / III / IV**

*Cannot be combined with any card from any other class (Masochist, Survivalist, or Wildcard)*

- I (*Common*) - Increased reload speed the longer you go without dealing damage
- II (**Uncommon**) - Decreased block cooldown the longer you go without dealing damage
- III (**Uncommon**) - Increased movement speed the longer you go without dealing damage
- IV (***Rare***) - Increased damage the longer you go without dealing damage

### **Wildcard I / II / III / IV**

*Cannot be combined with any card from any other class (Masochist, Survivalist, or Pacifist)*

- I (*Common*) - Randomly Increased reload speed at random intervals
- II (**Uncommon**) - Randomly Decreased block cooldown at random intervals
- III (**Uncommon**) - Randomly Increased movement speed at random intervals
- IV (***Rare***) - Randomly Increased damage at random intervals

### **Masochist I / II / III / IV**

*Cannot be combined with any card from any other class (Survivalist, Wildcard, or Pacifist)*

- I (*Common*) - Increased reload speed the longer you go without blocking damage
- II (**Uncommon**) - Decreased block cooldown the longer you go without blocking damage
- III (**Uncommon**) - Increased movement speed the longer you go without blocking damage
- IV (***Rare***) - Increased damage the longer you go without blocking damage

---

### Straight Shot
**Uncommon**

Bullets travel in a straight line. Idea credit: @LowFee in the modding discord.

---

### Fireworks
*Common*

Bullets pop like fireworks when above the battlefield

---

### Fragmentation
**Uncommon**

Bullets split into fragments on impact

---

### Pac-Bullets
*Common*

Bullets wrap around the edge of the screen

---

### Pac-Player
**Uncommon**

**You** wrap around the edge of the screen

---

### Shuffle
*Common*

Draw five new cards to choose from

---

### *??????*
*Common*

*??? ? ????????? ?????? ???? ???? ??????*

---

### **????????**
**Uncommon**

**??? ? ????????? ?????? ???? ???? ??????**

---

### _**????**_
_**Rare**_

_**??? ? ????????? ?????? ???? ???? ??????**_

---

### Piercing Bullets
*Common*

Bullets partially pierce shields

---

### Punching Bullets
**Uncommon**

Bullet effects apply through shields

---

### Comb
**Uncommon**

Fire multiple bullets side-by-side
