# Visual

> Also known as **Video** or **PV**.

**This setting can be toggled.** (If your clip has visuals, this is enabled by default)
**You can right-click or double-click any setting slider to reset it.**

## Toggles

![Toggles](/img/v4/visual/toggles.png)

### Loop

_Makes the clip loop if applicable._

### Static Visual

_Makes the clip freeze at the clip’s start._

::: important
*This gives you a static visual*
:::

### Truncate

_Makes the clip freeze at the end of the clip._

::: warning
*Not at the end of the note*
:::

### Multitrack for Chords

_Generates multiple video tracks for visuals_

::: info
*This is very useful for making chord visuals easier*
:::

### Create Groups

_Groups the video clips and the related audio clips together_

::: info
*This is useful when used with [Multitrack for Chords](#multitrack-for-chords)*
:::

### Glissando

_Creates a Swirl Effect if the note pitch bends or slides_

### Stack

_Creates stacked video clips on separate tracks when the MIDI contains multiple tracks._

::: important
*Only works when multiple MIDI tracks are selected*
:::

### Time Unremapping

_Disables all forms of time mapping of the clip! The start time will NOT reset as it will continue the clip from where it left off every note._

### Stretch

_Makes the clip stretch if applicable_

  * **None**
    * Applies no stretching to the clip
  * **Flex and Extend**
    * Stretches the clip completely
  * **Extending Only**
    * Stretches clip out
      ::: important
      *This works best with MIDI Notes that are longer than the Visual clip*
      :::
  * **Flexing Only**
    * Stretches clip in
      ::: important
      *This works best with MIDI Notes that are shorter than the Visual clip*
      :::

### Legato

_Extends the clip beyond the note’s duration._

- **Staccato**
  * No Extension
- **Up to 1 Beat**
  * Extends out 1 Beat
    ::: important
    *This will only work if the next clip is less than 1 beat apart*
    :::
- **Up to 1 Bar**
  * Extends out 1 Bar
    ::: important
    *This will only work if the next clip is less than 1 bar apart*
    :::
- **Unlimited**
  * Extends Unlimitedly

### Preferred Track

_Creates a new track(s) or uses the track selected for generation._

## Velocity

_Settings to assign opacity values relative to the MIDI notes_

* **Mapping Velocity**
  * Assigns the Notes to the Velocity from the MIDI File
* **Multiply Current Gain**
  * Toggling multiplies the existing gain instead
* **Velocity**
  * The Value of Velocity from Minimum to Maximum
* **Opacity**
  * The Value of Opacity from Minimum to Maximum

::: warning
*Do not use the same value in both minimum and maximum values for Velocity and Opacity*
:::

* **Reset**
  * Resets the settings to default

[*Mapping Velocity Explanation*](./faq.md#mapping-velocity-explanation)

## Effects

![Effects](/img/v4/visual/effects.png)

### Visual Effects

_Make your visuals whatever you want. The default is Horizontal Flip._

<!-- * Demonstration of all the visual effects here
  [**https://youtu.be/cY2Qa3Owetw**](https://youtu.be/cY2Qa3Owetw) -->

### Initial Step

_The Value Setting for the visuals_

::: important
*This will differ based on the visual you selected*
:::

### Advanced - PV Rhythm Visual Effect

* **Flip Class**
  * Selects a Visual type from the Flip Class
* **Rotation Class**
  * Selects a Visual type from the Rotation Class
* **Scale Class**
  * Selects a Visual type from the Scale Class
* **Mirror Class**
  * Selects a Visual type from the Mirror Class
* **Invert Class**
  * Selects a Visual type from the Invert Class
* **Hue Class**
  * Selects a Visual type from the Hue Class
* **Monochrome Class**
  * Selects a Visual type from the Monochrome Class
* **Time Class**
  * Selects a Visual type from the Time Class
* **Time 2 Class**
  * Selects a Visual type from the Time 2 Class
* **Expansion & Compression Class**
  * Selects a Visual type from the Expansion & Compression Class
* **Swing Class**
  * Selects a Visual type from the Swing Class
* **Blur Class**
  * Selects a Visual type from the Blur Class

## Parameters

_Various Effects for the visual generation._

![Parameters](/img/v4/visual/parameters.png)

### Presets

_Premade effect visuals for your convenience._

* **Set Fade by Percent**
  * Sets the Value Measure to Percent Value.
* **Set Fade by Timecode**
  * Sets the Value Measure to Timecode Value.

### Fade
#### Fade in
_Length of the Video Fade in on the Track Event_
#### Fade out
_Length of the Video Fade out on the Track Event_

### Glow
#### Glow
_Value of the “Glow” Effect that adds a glow to your visual!_
#### Glow Brightness
_Value of the Glow Brightness in your visual_

### Constrain keyframes length

_Specifies the length of keyframes to generate for the following properties_

* **Unconstrained**
  * Keyframes will generate normally from start to end of the MIDI note
* **Min length**
  * Keyframes will never be shorter than the set length. If the note is shorter, they get truncated.
* **Fixed length**
  * Keyframes are forced to exactly the set length

### Transform
#### Start Size
_Value of the Starting Size for Crop/Pan_
::: info
*Anything over 100% will reduce the video clip size*
:::
#### End Size
_Value of the Ending Size for Crop/Pan_
::: info
*Anything over 100% will reduce the video clip size*
:::
#### Start Rotation
_Value of the Starting Rotation for Crop/Pan_
#### End Rotation
_Value of the Ending Rotation for Crop/Pan_
#### Start X Shift
_Value of the Starting X Shift Position for Crop/Pan_
::: important
*This will make the visual position shift horizontally, depending on the other parameters*
:::
#### End X Shift
_Value of the Ending X Shift Position for Crop/Pan_
::: important
*This will make the visual position shift horizontally, depending on the other parameters*
:::
#### Start Y Shift
_Value of the Starting Y Shift Position for Crop/Pan_
::: important
*This will make the visual position shift vertically, depending on the other parameters*
:::
#### End Y Shift
_Value of the Ending Y Shift Position for Crop/Pan_
::: important
*This will make the visual position shift vertically, depending on the other parameters*
:::

### Color Grading
#### Start Hue
_Value of the Starting Hue_
#### End Hue
_Value of the Ending Hue_
#### Start Saturation
_Value of the Starting Saturation_
#### End Saturation
_Value of the Ending Saturation_
#### Start Contrast
_Value of the Starting Contrast_
#### End Contrast
_Value of the Ending Contrast_
#### Start Threshold
_Value of the Starting Contrast Threshold_
::: important
*This makes the contrast’s depth higher or lower*
:::
#### End Threshold
_Value of the Ending Contrast Threshold_
::: important
*This makes the contrast’s depth higher or lower*
:::
