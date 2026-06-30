# Tools

***These QoL Functions within the script make creating YTPMVs and Otomads a little less painful for setting up or working with. These don’t require any adjustments to any of the previous parameters.***

* **Close this dialog after the operation completes**
  > Self-explanatory

## Auto Layout Tracks

_Layout controls for visuals_

![Auto Layout Tracks](/img/v4/tools/auto_layout_tracks.png)

::: warning
*Video tracks must be manually created and selected for usage, this will not create new video tracks.*
:::

### Grid Layout

![Auto Layout Tracks - Grid Layout](/img/v4/tools/grid_layout.png){.shadow-less}

#### Array

* **Square**
  * Makes the standard layout
    ::: info
    *This makes the Layout 2x2, 3x3, etc.*
    :::
* **Custom**
  * Customize the Layout Format

#### Parameters

* **Columns**
  * The value of Columns
* **Rows**
  * The value of Rows

::: info
*The Rows will adapt to the number of columns and vice versa*
:::

#### Fit

* **Cover**
  * Adapts the size of the boxes to fit with the layout
    ::: important
    *This will stretch them if applicable*
    :::
* **Contain**
  * Keep the boxes’ original size and adapt them to the layout
* **Overlay**
  * Crops the window of the visuals to the columns and rows specification

#### Mirror Edges

_Mirrors the Tracks in a Parity Pattern_

![Auto Layout Tracks](/img/v4/tools/mirror_edges.jpg)

* **Horizontal Flip**
  * Flips the Columns in the pattern chosen
* **Vertical Clip**
  * Flips the Rows in the pattern chosen

#### Descending

_Reverses the layout sorting method (when ticked off)_

#### Padding

_Adjust the track boxes to be less cluttered with other boxes._

### 3D Box Layout

_This will create a cube with the tracks you select._

![Auto Layout Tracks - 3D Box Layout](/img/v4/tools/3d_box_layout.png){.shadow-less}

**The Layout goes as follows.**

* **Front**
* **Back**
* **Left**
* **Right**
* **Top**
* **Bottom**

#### Delete Original Track

_Deletes the original track(s) when the layout generates._


#### Use Longer Side of Edge Length

_This makes the cube more natural._

::: important
*This scales with the video project or media file in the track.*
:::

### Gradient Tracks

_This makes the video tracks have a gradient style color effect in your layout._

![Gradient Tracks](/img/v4/tools/gradient_tracks.png){.shadow-less}

* **Gradient Effect**
  * The Effect Presets
* **Descending**
  * Reverses the Gradient Order

### Clear Tracks Motion
_Resets the Track Motion Box_
### Clear Tracks Effect
_Removes all the Track Effects_

### Track Legato

![Track Legato](/img/v4/tools/legato.png)

::: warning
*This affects all the track events on the select track(s)*
:::

* **Stacking Clips (for current tracks)**
  * Stacks all Clips together in the selected tracks
* **Stacking Clips (for all tracks)**
  * Stacks all Clips together on every track
* **Stretch Clips (within stretch limits)**
  * Stretch Clips without Extending the clips’ length.
* **Stretch Clips (lengthen when outside stretch limit)**
  * Stretch Clips and Lengthens the Clips.
* **Lengthen Clips (changes clips length)**
  * Lengthens the Clips.
* **Increase Spacing (current track)**
  * Divides the Spacing of Each Clip in the Track
* **Increase Spacing (all track)**
  * Divides the Spacing of Each Clip for all Tracks
* **For Selected Track Events Only**
  * Allows Track Legato to be applied on selected track events only
* **Include the Track Events within a Group**
  * Allows Track Legato to be applied on the Grouped Clips.
* **Reverse Direction**
  * Applies the Legato Effect Backwards

::: important
*Lengthening will extend to the next clip, even to clips that are far apart*
:::
::: info
*Make sure there are no clips beyond your final visual*
:::

## Quick Select Interval

_This can select specific clips in the track that are selected_

![Quick Select Interval](/img/v4/tools/quick_select_interval.png){.shadow-less}

* **Select one for every few**
  * Selects every other clip based on the interval value.
* **Select which one of the group**
  * Selects which order of track clips in the selection.
* **Select how many at a time**
  * Selects how many events at a time per group.
* **Reset Selection**
  * Resets back to the selection pre-running the script.

## Quick Config Properties

_Set the properties of the event clips selected_

![Quick Config Properties](/img/v4/tools/quick_config_properties.png){.shadow-less}

### Video clips
* Hide
  * Toggles the video
* Lock
  * Toggles video lock
* Loop
  * Toggles video looping
* Maintain aspect ratio
  * Lock the aspect ratio
* Reduce interlace flicker
  * Toggles the interlace flicker
* Resample Mode
  * Sets the resample mode of the video
* Playback rate settings
  * Sets the playback rate of the video
* Set undersample rate
  * Sets the undersample rate of the video
* Opacity Settings
  * Sets the opacity of the video

### Audio clips
* Mute
  * Toggles the audio
* Lock
  * Toggles audio lock
* Loop
  * Toggles audio looping
* Invert phase
  * Inverts the audio phase
* Normalize settings
  * Sets the normalize settings of the audio
* Playback rate settings
  * Sets the playback rate of the audio
* Volume settings
  * Sets the volume of the audio

## Replace Track Events

_Replaces selected track events with specific preferences._

### Specify simultaneously

![Replace Track Events Simultaneously](/img/v4/tools/replace_track_events_simultaneously.png){.shadow-less}

* *The Selected Track Events can be replaced with whatever you have selected at the end of the selection, you can choose a clip from another track and use that as the replacement and all the track clips that you have set before it will be replaced with that clip.*

::: important
*It’s recommended not to put the replacement clip in the same track*
:::

### Specify separately

![Replace Track Events Separately](/img/v4/tools/replace_track_events_separately.png){.shadow-less}

* *Replaces specific clips in the track with any clip you select in any order you want.*
* [*Explanation in Extra Info*](./faq.md#specify-separately-explanation)

### Options
* **Replace other clips in the track group**
  * Replaces Other Clips from the Same Track Group
    ::: important
    *Doesn’t apply if the Track is not in a group*
    :::
* **Reserve original clip name**
  * Reserves the original clip name of the original clips after the replacement.
* **Reserve original clip offset**
  * Reserves the original offset of the clip used in the timeline.

## Change Tuning Method

_Changes the Tuning Method to the selected track events_

![Change Tuning Method](/img/v4/tools/change_tuning_method.png){.shadow-less}

*Refer to [Audio](./audio.md) on details about the Audio Tuning Methods*

**Pitch Semitones**
* **Pitch Lock**
  * Locks the Pitch Value
* **Lock Pitch instead of rate**
  * Locks the Pitch instead of the Stretch Rate
* **Formant Shift \> Reserve Formant**
  * Locks Formant

## Batch Subtitle Generation

_Creates batches of text using the Title Media Generator with any duration._

![Batch Subtitle Generation](/img/v4/tools/batch_subtitle_generation.png){.shadow-less}

- **Import from File**
  * Import a text file to be used for subtitles
* **Preset**
  * Controls the Preset Selection
* **Input**
  * Input text, line by line
    ::: important
    *Blank lines are ignored*
    :::
* **Duration**
  * Duration of each line

## Find Track Events

![Find Track Events](/img/v4/tools/find_track_events.png){.shadow-less}

_Find and Select all the track events that match the specified_

::: info
*This is helpful for selecting clips for layout replacement, making template layouts easier*
:::

### Options
* *Matching Same Source*
* *Matching Same Source with Same Starting Offset*
* *Matching Specified Name*

### Search Text Box

_Search for any video, audio or both track events in your project_

* **Both**
  * Searches for both video and audio events
* **Video events**
  * Searches for only video events
* **Audio events**
  * Searches for only audio events

## Apply Visual Effect

_Apply Visual Effects to selected Track Events_

![Apply Visual Effect](/img/v4/tools/apply_visual_effect.png){.shadow-less}

*Refer to [Visual](./visual.md) for details on the Visual effects you can use*

## Convert Music Beats

_Converts Music Beats to a Different Time Signature_

![Convert Music Beats](/img/v4/tools/convert_music_beats.png){.shadow-less}

::: info
Useful for changing specific beats to a different time signature
:::

## Custom Fade Gain

_Adjusts Gain/Volume Range for Selected Track Events_

![Custom Fade Gain](/img/v4/tools/custom_fade_gain.png){.shadow-less}

* **From Value**
  * The Value of the Starting Fade Effect
* **To Value**
  * The Value of the Ending Fade Effect

## Export MIDI File

_Exports Tracks or Track Events to a Score Sequence File_

![Export MIDI File](/img/v4/tools/export_midi_file.png){.shadow-less}

::: info
*Naming your tracks beforehand will make the creation process smoother and help identify your tracks for future use*
:::

::: important Important Notice
**The MIDI being created can support the following**

- ☑️ **Use audio tracks and events.**
- ☑️ **Can generate multiple tracks.**
- ☑️ **Allows selecting instruments.**
- ☑️ **Allows set track name.**
- ☑️ **Allows setting base pitch.**
- ☑️ **Can export loop region only.**
- ☑️ **Adjust events gain (Audio: Volume / Video: Opacity) <sup>[1](#footnote-1)</sup>**
- ☑️ **Adjust tracks volume for audio tracks or opacity/composite level for video tracks1**
- ☑️ **Adjust audio tracks pan <sup>[1](#footnote-1)</sup>**
- ❎ **Use video tracks and events.**
- ❎ **Using the "Pitch Shift" Audio FX instead of tuning with [[+]] and [[-]] key.**
- ❎ **Using a version of Vegas Pro \< 16.**

> **☑️ - Good to go**
> **❎ - Not recommended - _This will generate notes with all base pitch._**
:::

> 1. These all support envelopes / automation control {#footnote-1}

**Settings**
* **All Tracks**
  * Selects all tracks
* **Video Tracks**
  * Selects only video tracks
* **Audio Tracks**
  * Selects only audio tracks
* **Base Pitch**
  * Sets the default base pitch (C5)
* **Export loop region only**
  * Exports the MIDI with only the selected loop region track events

### Vegas Track List

_Select the tracks to be added to the MIDI File_

::: important
*Track names can and will get automatically added to the track list and channel list*
:::

**Options**
* **Add to each new tracks**
  * Adds the selected track(s) to new MIDI tracks with MIDI channels
* **Add to a same new track**
  * Adds the selected track(s) to 1 new MIDI track with MIDI channels
* **Add to current track**
  * Adds the selected track(s) to the selected MIDI track with new MIDI channels
* **Preview**
  * Previews the track(s) selected

### MIDI Track List

_Manage the track list for the MIDI being prepared. Displays how many created tracks in the preparation list and how many Vegas tracks and notes in each MIDI Track_

::: important
*This is what Otomad Helper will be looking for. Adding any tracks to the channel list will add notes instead in the export*
:::

**Options**
* **Move up**
  * Moves the selected track up
* **Move down**
  * Moves the selected track down
* **Remove**
  * Removes the selected track from the list
* **Add a new empty track**
  * Creates a new MIDI track
* **Insert a new empty track**
  * Insert a new MIDI track on top of selected
- **Name**
  * Input a name for the track

### MIDI Channel List

_Manage the channel list from the select track in the track list. Displays how many Vegas tracks and notes created in the selected MIDI track._

**Options**
* **Remove**
  * Removes the selected channel from the list
* **Channel**
  * Set the channel the selected track is on

::: danger
*Use the same channel at your own risk*
:::

* Instrument - Displays used MIDI instrument for the channel

### MIDI Instrument list

_List of instruments selected to their according MIDI channel that can be changed_

Options
* **Dispatch Instrument to Channel**
  * Changes and sets instrument to the selected MIDI channel
