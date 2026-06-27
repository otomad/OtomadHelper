# Audio

_This setting can be toggled. (If your clip has audio, this is enabled by default)_
_You can right-click or double-click any setting slider to reset it_

## Settings

![Toggles](/img/v4/audio/toggles.png)

### Loop

_Makes the clip loop if applicable._

### Normalize

_Normalize the audio._

::: info
*This is useful if the audio is quiet*
:::

### No Lengthening

_Trims to the MIDI note instead of stretching._

::: warning
*This conflicts with Legato*
:::

### Multitrack for Chords

_Generates multiple audio tracks for chords._

### Create Groups

_Groups the Video and Audio Clips represented by the MIDI note(s)._

::: info
*This makes it easier to change the position or length of the audio and video clips simultaneously*
:::

### Stack

_Creates stacked audio clips on separate tracks when the MIDI contains multiple tracks_

::: warning
*Only works when multiple MIDI tracks are selected.*
:::

### Time unremapping

_Disables all forms of time mapping of the clip! The start time will NOT reset as it will continue the clip from where it left off every note._

### Auto Pan

_Pans the Audio using Envelope Automation._

::: warning
*This is dependent on your pan automation from your MIDI, not the notes*
:::

### Stretch

_Makes the clip stretch if applicable._

- **None**
  * Applies no stretching to the clip.
- **Flex and Extend**
  * Stretches the clip completely
- **Extending Only**
  * Stretches clip out
    ::: warning
    *This works best with MIDI Notes that are longer than the audio clip*
    :::
- **Flexing Only**
  * Stretches clip in
    ::: warning
    *This works best with MIDI Notes that are shorter than the audio clip*
    :::

### Legato

_Extends the audio outside the note’s length with any length chosen._

- **Staccato**
  * No Extension
- **Up to 1 Beat**
  * Extends out 1 Beat
    ::: warning
    *This will only work if the next clip is less than 1 beat apart*
    :::
- **Up to 1 Bar**
  * Extends out 1 Bar
    ::: warning
    *This will only work if the next clip is less than 1 bar apart*
    :::
- **Unlimited**
  * Extends Unlimitedly

### Preferred Track

_Creates a new track(s) or uses the track selected for generation._

## Velocity

_Settings to assign gain values relative to the MIDI notes_

* **Mapping Velocity**
  * Assigns the Notes to the Velocity from the MIDI File
* **Multiply current gain**
  * Toggling multiplies the existing gain instead
* **Velocity**
  * The Value of Velocity from Minimum to Maximum
* **Volume**
  * The Value of Volume from Minimum to Maximum

::: warning
*Do not use the same value in both minimum and maximum values for Velocity and Volume*
:::

* **Reset**
  * Resets the settings to default

[*Mapping Velocity Explanation*](#bookmark=id.q5w06qf8h8y6)

## Tuning

![Tuning](/img/v4/audio/tuning.png)

### Tuning Methods

* **No Tuning**
  * No Pitch Effect
* **Pitch Shift Audio Effect**
  * Uses the Pitch Shift Plugin (default for Vegas 13-15)
* **Elastic Pitch Effect**
  * Uses the Elastic Efficient Method (by default)
* **Classic Pitch Effect**
  * Uses the Classic Stretch Method
* **Scaleless Tuning**
  * Locks the Stretch and Pitch
* **Granular Oscillator**
  * Makes the sample super short and plays it, generating a pulse sequence corresponding to the pitch while creating a continuous pitch by exploiting the perception fusion effect of fast pulses

### If the pitch exceeds the range

* **Switch to pitch shift plugin**
  * Repeatedly uses the pitch shift plugin for exceeding limits
    ::: warning
    Out of range notes will slow down the generation
    :::
* **Raise / Lower octaves**
  * Shifts by octaves to keep the note inside ±24 semitones (safest option).
* **Raise / Lower octaves (Experimental)**
  * Allows a much larger range, up to roughly ±39 $\pm\frac{12}{\lg{2}}\ \left(\approx\pm39.863137\right)$ semitones.
    ::: info
    If “Lock Stretch to Pitch” is on, it can go as low as -52 semitones (upper limit stays \+24).
    :::
    ::: danger
    _Use this at your risk! This method can CRASH Vegas Pro_
    :::
* **Dock at Top / Bottom**
  * Forces the note to the highest or lowest pitch still inside the normal range.
* **Silent**
  * Mutes notes that are out of range.

### Lock Attributes

* **Lock Stretch and Pitch**
  * Adjust the stretch to change the pitch (Resample)
* **Reserve Formant**
  * Locks Formant, works only with Mono and Professional
    ::: info
    *This locks formant so you get that “monophonic” effect*
    :::
* **Vocal Fry**
  * Forces the sample to be 1 tick long
    ::: info
    *Only usable with Granular Oscillator*
    :::

### Base Pitch

_Controls the base pitch for previewing and adjusting the Track Event Audio._

* **Note Setting**
  * Controls the base note used for the base pitch.
* **Note Octave**
  * Controls the note octave used for the base pitch.

### Preview

* **Preview the Base Pitch**
  * Previews the base pitch
* **Preview Audio**
  * Previews the track event audio

### Preview Attributes

_Tune generating methods for previewing the base pitch_

::: warning
*NAudio is the default method and is preferred*
:::

* **Adjust Audio to Base Pitch**
  * Adjusts the clip track event’s audio to the base pitch.
  ::: info
  *This is useful for older styles of remixes*
  :::

## Parameters

![Parameters](/img/v4/audio/parameters.png)

* **Set Fade by Percent**
  * Sets the Value Measure to Percent Value.
* **Set Fade by Timecode**
  * Sets the Value Measure to Timecode Value.

### Fade in

_Length and Fade Type of the Audio Fade in on the Track Event_

### Fade out

_Length and Fade Type of the Audio Fade out on the Track Event_
