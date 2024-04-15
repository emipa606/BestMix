# BestMix

![Image](https://i.imgur.com/buuPQel.png)

Update of peladors mod
https://steamcommunity.com/sharedfiles/filedetails/?id=2002971559

- Added support for Softness from Soft Warm Beds
https://steamcommunity.com/sharedfiles/filedetails/?id=1713295858
- Added chinese languange, thanks NB
- Added support for Nutrition-stat, High and Low
- Updated chinese translation, thanks NBurger500

[strike]**As the bill-logic have been changed and my skills in Transpiling-code is not high enough to fix it, the mod is as of now not working.**[/strike]

Restored functionality thanks to:


-  Taranchuk - replacing the old complex code with a more maintainable one
-  Kayedon - commissioned the work



![Image](https://i.imgur.com/pufA0kM.png)

	
![Image](https://i.imgur.com/Z4GOv8H.png)


# Overview
 V1.1

A QoL mod that allows variance in how benches select their ingredients.

# Mod details


Vanilla has a default setting to search for ingredients by distance. With this mod you have the capability to use a gizmo on a work bench to change the criterion of that search to relate to alternative factors or elements. There are also optional individual best mix settings that you can set per bill. It will default to the gizmo value where these are not set.

The criterion specified for use are as follows:

**Beauty Prettiest** - highest beauty,
**Beauty Ugliest** - least beauty, 
**Insulate Cold** - highest material property of cold insulation,
**Insulate Heat** - highest material property of heat insulation,
**Item Ignition** - most flammable,
**Item Damaged** - least hit points against maximum (as applicable), 
**Item Expiry** - most rapid perishable items,
**Item Robust** - most hit points vs max (as applicable),
**Mass Heaviest** - most mass, 
**Mass Lightest** - least mass,
**Nearest** (Vanilla) - default normal behaviour, 
**Protect Blunt** - highest property of protection against blunt damage,
**Protect Sharp** - highest material property of protection against sharp damage,
**Random** - random selection,
**Stock Fraction** - least apportioned stack number against maximum, 
**Stock Least** - least amount of stock for item (mod options),
**Stock Most** - most amount of stock for item (mod options),
**Temp. Coldest** - ingredient in coldest ambient temperature, 
**Temp. Warmest** - ingredient in warmest ambient temperature,
**Value Cheapest** - Least value, 
**Value Expensive** - Most value,
**Weapon Bluntest** - material blunt damage factor,
**Weapon Sharpest** - material sharp damage factor.

And will default to the vanilla or nearest behaviour.

Note if the ingredients used in the bill will have a similar property they will be equally compared. So some of the filters are more capable of general use, whereas others are more designed for specific types of ingredients where the filter has some relevancy to the end product based on the associated qualities.

So if you now want your chef to randomly select ingredients from an organised room fridge you can do so. If you want to use the cheapest materials first, or the most damaged, you can do that also. Want to make clothes based on the best sharp protection or insulates against the cold, does that. Want to use the stacks that are not full first to keep things tidy this way, got that covered as well. And so on.

Otherwise the bills will follow the normal filtering process as specified in the bill and within whatever radius is set for them. And would recommend using bill radius and trying to store objects more locally to their associated bench/bill giver.

Please note there is a specific discussion for additional filter suggestions.

# Mod Notes


Can be added and removed mid save.

Has a mod option to turn off the functionality and also an option to limit the use to only benches that are recognised as a meal source (e.g. stoves, campfire), which will include modded benches that are also recognised as these kind of bill givers.

Additional mod options are provided for the stock selection types, where you can modify the behaviour to include the whole map for evaluation as opposed to the bill radius and also whether items are stored in valid storage.

Does not apply to pawn based bills.

The mod will also set the maximum number of bills for a billgiver to 125. (Compatible with Better Workbench Management).

# Compatibility
 *** Subject to conversion ***

**Combat Extended** - Adds Protect Electric - highest material property of protection from electric damage.
**Multiplayer** - Native support

# Credits


https://steamcommunity.com/id/madeline1324/myworkshopfiles/?appid=294100]Madeline - a co-author, for the involved harmony transpiler work that makes this mod possible.

https://steamcommunity.com/id/littlewhitemouse/myworkshopfiles/?appid=294100]LWM - helping with testing.

KV - "No max bills" allowing more bills for a billgiver. Added functionality (slightly modified).

French translation - Rebecca

# Recommendations


Deep Storage (V1.1 when converted) - to further improve the ingredient handling of bills by organising storage more conveniently and locally for their associated benches.


(CC BY-NC-SA 4.0)


![Image](https://i.imgur.com/PwoNOj4.png)



-  See if the the error persists if you just have this mod and its requirements active.
-  If not, try adding your other mods until it happens again.
-  Post your error-log using https://steamcommunity.com/workshop/filedetails/?id=818773962]HugsLib or the standalone https://steamcommunity.com/sharedfiles/filedetails/?id=2873415404]Uploader and command Ctrl+F12
-  For best support, please use the Discord-channel for error-reporting.
-  Do not report errors by making a discussion-thread, I get no notification of that.
-  If you have the solution for a problem, please post it to the GitHub repository.
-  Use https://github.com/RimSort/RimSort/releases/latest]RimSort to sort your mods



https://steamcommunity.com/sharedfiles/filedetails/changelog/2195986094]![Image](https://img.shields.io/github/v/release/emipa606/BestMix?label=latest%20version&style=plastic&color=9f1111&labelColor=black)

