# GreedCave

This is a game done for the 4th Game Design Club challenge (Arcade) at the Universidade Lusófona Game Development degree.

## Story

Comming soon...

## Tech stuff

* Adapted bitcula's [Universal LPC Spritesheet Unity Importer] to auto slice the LPC sprites. LPC spritesheets filename must start with "LPC_".
* Created a helper Editor script (accessible by right-clicking on a LPC image, selecting Assets>LPC>Create Animations) that create the animation override controller and the animation clips for any LPC spritesheet. The base animation controller is hardcoded to be on the "Assets/Animations/Base/BaseAnimations.controller" file, but you can easily change that.
* Created a custom shader for sprites in 3d space, featuring:
  * Uses emissive and normal secondary textures on the sprite
  * Has a flash functionality, which uses the alpha channel of the color to lerp between base color and the texture, for effects
  * Has correct shadows (needed to expose a AlphaThreshould variable on the shader)

## Credits

* Code, art, game design done by Diogo de Andrade
* Character sprites generated using [Universal LPC Spritesheet Character Generator]
  * Check AUTHORS.TXT file for attribution as far as I could find it
* LPC import code based on bitcula's [Universal LPC Spritesheet Unity Importer]

## Licenses

All code in this repo is made available through the [GPLv3] license.
The text and all the other files are made available through the 
[CC BY-NC-SA 4.0] license.
LPC art made available through the [GPLv3] and [CC-BY-SA 3.0.] licenses.

## Metadata

* Autor: [Diogo Andrade][]

[Diogo Andrade]:https://github.com/DiogoDeAndrade
[GPLv3]:https://www.gnu.org/licenses/gpl-3.0.en.html
[CC-BY-SA 3.0.]:http://creativecommons.org/licenses/by-sa/3.0/
[CC BY-NC-SA 4.0]:https://creativecommons.org/licenses/by-nc-sa/4.0/
[Bfxr]:https://www.bfxr.net/
[Universal LPC Spritesheet Character Generator]:https://sanderfrenken.github.io/
[Universal LPC Spritesheet Unity Importer]:https://github.com/bitcula/Universal-LPC-Spritesheet-Unity-Importer