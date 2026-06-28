# Datamoshes

![Clips Folder](/img/v4/moshes/clips_folder.png)

**<GlitchyText normal="Datamosh" glitchy="Dataḁ̸̬͋ă̷͍ȧ̴͇͘͝ͅá̷̖̲͑ã̶̺̈́å̷͉̖͊̚ä̷̬̬â̷̡̱̑ Mo̴̞̜̯͋ȯ̴̪̏͂õ̷̧͚ͅȏ̸͇ö̵̟̻̹͌o̶͚͍̻̕ǒ̸̢͎̄o̵͔̘̓͘o̶͇̐o̸̬͒͗̎sh" /> is a technique of damaging clips to create glitchy effects which frames that need to progress, don’t progress!**
OH’s Datamosh was made possible with the use of [Vegas-Datamosh by delthas](https://github.com/delthas/vegas-datamosh)

### Datamosh

_Datamoshes the video with determined settings._

* **Frame Count**
  * Amount of frames to use.
* **Frame Repeat**
  * Amount of times the frames will repeat.

### Datamix

_Datamoshes the first frame of the selection_

::: important
Requires the selection to be set equal or greater than frame 1
:::

### Layer

_Layers’ the selected clip for moshing_

* **Layer count**
  * Amount of layers to generate.
* **Layering offset**
  * Offsets the layer by the number of frames specified.
* **Render**
  * Renders the clip selected after generating layers.

### Render

_Renders the clip within a timeline selection… (Yes that’s literally it.)_

### Scramble

_Scrambles the clip with random time placements dependent on the size._

* **Scramble Size**
  * The value of the size for the clips to be generated.

### Automator

_Automates and Randomizes the values of Visual effects in the clip_

::: important
This requires Visual effects to have automation on to work
:::

* **Scramble**
  * *It will ask if you wish to automate the Visual effects in the clip, for each effect.*

### Stutter

_Stutters clips by Forwarding and Reversing them in randomized intervals_

* **Length in Seconds**
  * Duration of clips to be generated.
* **Stutter Window Bias**
  * The value in which stutter will randomize the clips

### Shake

_Shakes the clips_

* **Speed**
  * The frequency value of the shaking effect
* **H/V Synchronicity**
  * The relative vertical speed value
* **Amount**
  * The value of pixels the camera shifts from the center.
    ::: important
    This also controls the margin of the zoom in.
    :::
* **H/V ratio of displacement**
  * The value of the horizontal distance multiplied
    ::: important
    This controls the zoom in
    :::

---

* **Reset Pan/Crop on first frame**
  (Leave unchecked to shake within the current video zoom)
* **Reset all frames before shaking**
  (Leave unchecked to apply and multiple the shake effect with the previous shake effect)
