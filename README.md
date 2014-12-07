GarrisonBuddyBeta
=================

Garrison buddy, your personal butler!
----------------------------------------------------------------------

v0.5.2
Deactived part of the experimental navigation system
Improved speed of calculations of path and trimmed path
Safer Interactions with Game objects.

----------------------------------------------------------------------

v0.5.1
Fixed enchanting CD

----------------------------------------------------------------------
v0.5

Added Feature: Now compatible as secondary bot with AutoAngler or GatherBuddy, you can fish while waiting for work orders or/and missions!
Added Feature: New navigation system bringing longer waypoints when available, the bot will now use a mount if available.
Added Feature: All shipments available will now be picked up.
Added Feature: Supported work orders will be started automatically. If yours are not, please post the ID of the PNJ.
Added Feature: The bot will now use your salvaging center and open Salvage crates.

Fixed: Trying to take more than allowed of unique object, will now delete stacks when full.
Fixed: Path generation was returning no path generated when already at destination, the correct behavior has been fixed.
Fixed: Toons were sometimes getting stuck on the side of stairs.
Fixed: Available mission were note always refreshed correctly.


----------------------------------------------------------------------
v0.4

Remapped Horde Garrison, mines and garden(garden lvl 3 still not preoperly mapped)

----------------------------------------------------------------------
v0.3

Added support for Garrison horde lvl 2 and 3
Fixed countdown check for coffee
Faster movements

-----------------------------------------------------------------------
v0.2

Bug fixes:
#83729758: GaB spams miner's coffee and mining pick when already buffed.
#83729828: GaB will try to harvest again before looting which closes the loot window.
#83656128: GaB will not always pick up mine shipment.

Garrison - Movement (no mounts for now):
Alliance: Lvl 2/3
Horde: None

Mine:
Harvest ores
Harvest mine cart
Collect work orders
Use coffee and mining pick if available
Alliance: Lvl 1/2/3
Horde: None

Garden:
Harvest herbs
Collect work orders
Alliance: Lvl 1/2/3
Horde: None

Missions:
Included for all supported Garrison level
Logic: complete mission for which follower level is >= mission level and
all abilities countered

General:
Custom settings
Can use Garrison Hearthstone.
Activation of buildings if finished for all supported Garrisons.
Garrison cache pickup included for all supported Garrisons.
