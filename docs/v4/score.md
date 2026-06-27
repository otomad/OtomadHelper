# Score

## MIDI Configuration
*(THIS IS IMPORTANT)*

![Configuration](/img/v4/score/configuration.png)

* Requires a MIDI File to make the otoMAD/YTPMV.
* You must select a MIDI file for the script to generate from your computer.

::: info
* This also works with other DAWs that support MIDI exporting
* In FL Studio, make a copy of your project before you convert it to MIDI
:::

### Start and End Seconds

_Controls where the MIDI starts and ends._

::: tip
*If the start has been set at 0:05.000. The MIDI would generate all notes after 5 seconds and the clips will be generated 5 seconds at the Generate Setting.*
:::
::: info
*This will still generate the visuals at the respected note time placements.*
:::

### BPM Tempo

_Controls the BPM the MIDI generates_

* **MIDI Tempo**
  * Uses the MIDI’s Tempo
* **Project**
  * Uses the Project’s Tempo
* **Custom**
  * Uses the Custom Setting

**If the MIDI has changing tempos, the script will adapt and let you select what options you want to do with them.**

* **Variable MIDI tempo**
  * _Uses the changing tempo_
  * **Hold**
    * Notes Generated will stay held to their BPM
  * **Linear**
    * Notes Generated will be based linearly on their BPM
* **MIDI Tempo**
  * Uses the first tempo

### Auto Change Project Ruler Properties

_Adjusts the project’s measure properties based on the MIDI_

* **Tempo**
  * Uses tempo as the setting
* **Time signature**
  * Uses time signature as the setting

- **Apply Now**
  * Automatically applies the MIDI’s tempo and time signature without generating the MIDI events

::: info
Unfortunately, you can not use MIDIs with changing tempos or time signatures, only the first tempo/time signature will be used. Use [VariableBPM](https://github.com/zzzzzz9125/VariableBPM) for variable tempo.
:::

### Restrict Note Length

_Controls the Note Output Length from the MIDI_

* **Unrestricted**
  * No note output change
* **Max Length**
  * Notes will be restricted to the length provided
* **Fixed Length**
  * Notes will attempt to generate to the length provided

::: warning TRAP
_*This will conflict with [Legato](./audio.md#legato) if it is set to UNLIMITED in Audio / Visual Settings*_
:::

## Use MIDI track

![Track List View](/img/v4/score/track_list_view.png)

_This allows you to select what tracks you want to generate from your MIDI file_

### Commands

* **Select all**
  * Selects all channels
* **Invert Selection**
  * Inverts your selection
* **Single**
  * Toggles selecting a single channel
* **Multi**
  * Toggles selecting multiple channels

::: important
*Starting Pan is shown, and the letter attributes go as follows:*

Letter Attributes | Starting Pan
--- | ---
L | Left
R | Right
C | Center
V | Variable
:::

## Auto Layout Tracks

![Auto Layout Tracks](/img/v4/score/auto_layout_tracks.png)

[Refer to Tools](./tools.md#auto-layout-tracks)
