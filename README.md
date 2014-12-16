#GarrisonBuddyBeta
#=================

##Garrison buddy, your personal butler!

###v0.7.1
* Fix Tailor work order item ID

###v0.7.0
* Major changes code side improving stability and speed
* New GUI
* Options have been rewritten
* Added HBRelog mode: will skip to next profile once done in you Garrison
* Too many changes to list, bug reports from old version are now null, please report with new versions.


###v0.6.4
* Fixed barn shipment not being collected
* Fixed completely full shipment not being collected 

###v0.6.4
* Fixed debug output for shipments
* Fixed work order options not used
* Fixed Inscription daily cd bug
* Fixed No mats for daily CD

###v0.6.3
* WARNING: This build is slower than it will be in the future for debugging purposes! 
* Fixed id spell for professions
* Fixed movement to Anvil: too fast between actions
* Improved Salvage crate run stability 
* Switched default value from true to false for individual work orders configuration
* Added un-mount order if needed before casting daily cd
* Improved stability of the code concerning UseItem actions and debugging output

###v0.6.2
* Right build pushed :D 

###v0.6.1
* Fixed ID for work order PNJ

###v0.6
* Added option to choose which work order to start
* Rewritten the whole navigation system : 
*  * Will now take less than a few ms to find a path.
   * Less freezes
   * Less computing power asked from the user
   * More stable navigation for harvesting
* Fixed Professions CD
* Fixed error on loots
* Improved logging for user and debugging
* Too much fixes involved by the new system to list here... 

###v0.5.4
* Fixed toon not close enough to node to interact. 
* Fixed missions not being completed

###v0.5.3
* Fixed a bug with picking up work orders, should now pick up all finished work orders.

###v0.5.2
* Deactived part of the experimental navigation system
* Improved speed of calculations of path and trimmed path
* Safer Interactions with Game objects.

###v0.5.1
* Fixed enchanting CD

###v0.5
* Added Feature: Now compatible as secondary bot with AutoAngler or GatherBuddy, you can fish while waiting for work orders or/and missions!
* Added Feature: New navigation system bringing longer waypoints when available, the bot will now use a mount if available.
* Added Feature: All shipments available will now be picked up.
* Added Feature: Supported work orders will be started automatically. If yours are not, please post the ID of the PNJ.
* Added Feature: The bot will now use your salvaging center and open Salvage crates.
* Fixed: Trying to take more than allowed of unique object, will now delete stacks when full.
* Fixed: Path generation was returning no path generated when already at destination, the correct behavior has been fixed.
* Fixed: Toons were sometimes getting stuck on the side of stairs.
* Fixed: Available mission were note always refreshed correctly.

###v0.4
* Remapped Horde Garrison, mines and garden(garden lvl 3 still not preoperly mapped)

###v0.3
* Added support for Garrison horde lvl 2 and 3
* Fixed countdown check for coffee
* Faster movements

###v0.2
* 83729758: GaB spams miner's coffee and mining pick when already buffed.
* 83729828: GaB will try to harvest again before looting which closes the loot window.
* 83656128: GaB will not always pick up mine shipment.