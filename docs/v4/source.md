@en # Source
@zh # 素材

@en ## Source Configuration
@zh ## 素材配置

![Configuration](/img/v4/source/configuration.png)

@en ### Select source from
@zh ### 选择素材来源

@en _Allows selecting a media file or a track event chosen to be used._
@zh _允许选择要使用的媒体文件或轨道事件。_

::: info {.italic-list}
@en * You must have a track event selected before you run the script.
@zh * 在运行脚本之前，你必须选择一个轨道事件。
@en * You also can choose a media file that has the visual ready to go for you when you browse.
@zh * 你还可以选择或浏览一个早已准备好了画面的媒体文件。
@en * You can select a video clip and audio clip separately, and the script will combine them to generate.
@zh * 你可以分别选择视频剪辑和音频剪辑，脚本会将它们组合起来一起生成。
@en * You can also drag and import clips to be used in Otomad Helper.
@zh * 你还可以拖动并导入剪辑以在Otomad Helper中使用。
:::

@en ### Start and End Time
@zh ### 起始和终止时间

@en _Trim clips with start time and end time._
@zh _使用开始时间和结束时间来修剪剪辑。_

@en ### Generate at
@zh ### Generate at

@en _Controls where the MIDI can be generated_
@zh _Controls where the MIDI can be generated_

@en * **Project Start**
@zh * **Project Start**
@en   * Generates the MIDI at the Start.
@zh   * Generates the MIDI at the Start.
@en * **Cursor**
@zh * **Cursor**
@en   * Generates the MIDI at the Cursor Position.
@zh   * Generates the MIDI at the Cursor Position.
@en * **Custom**
@zh * **Custom**
@en   * Generates the MIDI at any Time Position chosen.
@zh   * Generates the MIDI at any Time Position chosen.

@en ### Below Top Adjustment Tracks
@zh ### Below Top Adjustment Tracks

@en _Places the Generated Tracks below the Adjustment Tracks_
@zh _Places the Generated Tracks below the Adjustment Tracks_

@en ::: warning
@zh ::: warning
@en *This is available in Vegas Pro 19 and up.*
@zh *This is available in Vegas Pro 19 and up.*
@en :::
@zh :::

@en ### Remove track events assigned as source material after the generation completes
@zh ### Remove track events assigned as source material after the generation completes

@en _Deletes the original source clip after the MIDI has been generated_
@zh _Deletes the original source clip after the MIDI has been generated_

@en ### Select all generated events after the generation completes
@zh ### Select all generated events after the generation completes

@en _Selects all events generated after the MIDI has been generated_
@zh _Selects all events generated after the MIDI has been generated_

@en ### Move the Cursor to
@zh ### Move the Cursor to

@en _Positions the cursor to a specified location in the timeline_
@zh _Positions the cursor to a specified location in the timeline_

@en * **Original Position**
@zh * **Original Position**
@en   * Retains the timeline cursor in the original position before you ran the script
@zh   * Retains the timeline cursor in the original position before you ran the script
@en * **Where Generate at**
@zh * **Where Generate at**
@en   * Moves the cursor to [the start of the MIDI file](#generate-at)
@zh   * Moves the cursor to [the start of the MIDI file](#generate-at)
@en * **Before the First Event**
@zh * **Before the First Event**
@en   * Moves the cursor to before the first generated event in the timeline
@zh   * Moves the cursor to before the first generated event in the timeline
@en * **After the Last Event**
@zh * **After the Last Event**
@en   * Moves the cursor to after the last generated event in the timeline
@zh   * Moves the cursor to after the last generated event in the timeline

@en ### Grouping Tracks
@zh ### Grouping Tracks

@en _Creates groups for the tracks_
@zh _Creates groups for the tracks_

@en * **Ungrouped**
@zh * **Ungrouped**
@en   * Disables track grouping
@zh   * Disables track grouping
@en * **Group by MIDI track**
@zh * **Group by MIDI track**
@en   * Groups tracks based on the MIDI track used and uses the MIDI track name
@zh   * Groups tracks based on the MIDI track used and uses the MIDI track name
@en * **Group by task session**
@zh * **Group by task session**
@en   * Creates a new group whenever a new generation is completed
@zh   * Creates a new group whenever a new generation is completed

@en ---
@zh ---

@en - **Collapse track groups by default**
@zh - **Collapse track groups by default**
@en   * Toggles collapsing the track group list
@zh   * Toggles collapsing the track group list
@en - **Reuse groups that have the same nonempty name**
@zh - **Reuse groups that have the same nonempty name**
@en   * Toggles the generation of newly added tracks to be added to existing track groups with the same name
@zh   * Toggles the generation of newly added tracks to be added to existing track groups with the same name

@en ### Audio Bus Track
@zh ### Audio Bus Track

@en _Routes audio tracks to audio buses (Mixing Console)_
@zh _Routes audio tracks to audio buses (Mixing Console)_

@en * **Unrouted**
@zh * **Unrouted**
@en   * Disables routing to audio buses
@zh   * Disables routing to audio buses
@en * **Route by MIDI track**
@zh * **Route by MIDI track**
@en   * Routes the audio track to the same bus based on the MIDI track used
@zh   * Routes the audio track to the same bus based on the MIDI track used
@en * **Route by task session**
@zh * **Route by task session**
@en   * Creates and routes the audio track to a new bus whenever a new generation is completed
@zh   * Creates and routes the audio track to a new bus whenever a new generation is completed

@en ::: danger
@zh ::: danger
@en There is currently an unknown bug that may prevent you from using the audio bus track feature. If you encounter such a problem, it is recommended to disable this feature or try changing another Vegas version.
@zh There is currently an unknown bug that may prevent you from using the audio bus track feature. If you encounter such a problem, it is recommended to disable this feature or try changing another Vegas version.

@en [View detailed explanation.](./faq.md#audio-bus-track-bugs-caused-by-vegas-pro)
@zh [View detailed explanation.](./faq.md#audio-bus-track-bugs-caused-by-vegas-pro)
@en :::
@zh :::

@en ---
@zh ---

@en - **Reuse audio bus tracks that have the same nonempty name**
@zh - **Reuse audio bus tracks that have the same nonempty name**
@en   * Toggles the generation of newly added tracks to be added to existing audio buses with the same name
@zh   * Toggles the generation of newly added tracks to be added to existing audio buses with the same name

@en ## Multisource Combination
@zh ## Multisource Combination

@en ![Multisource Comb](/img/v4/source/comb.png)
@zh ![Multisource Comb](/img/v4/source/comb.png)

@en ### Off
@zh ### Off

@en _Disables the multisource features_
@zh _Disables the multisource features_

@en ### Mystery Box
@zh ### Mystery Box

@en _Automatically picks from multiple selected sources in unpredictable ways_
@zh _Automatically picks from multiple selected sources in unpredictable ways_

@en #### Limited to selected sources
@zh #### Limited to selected sources
@en _Only uses the sources selected in the timeline/project media_
@zh _Only uses the sources selected in the timeline/project media_
@en #### Make each track different
@zh #### Make each track different
@en _Each track will generate with a different clip_
@zh _Each track will generate with a different clip_
@en #### Switch once per marker
@zh #### Switch once per marker
@en _Changes the source when a MIDI marker is reached_
@zh _Changes the source when a MIDI marker is reached_
@en #### Switch once per bar or beat
@zh #### Switch once per bar or beat
@en _Changes the source when a beat or bar has passed_
@zh _Changes the source when a beat or bar has passed_
@en #### Lotion Bath Tactics
@zh #### Lotion Bath Tactics
@en _Forces the source to sequentially change at your specified duration_
@zh _Forces the source to sequentially change at your specified duration_
@en ::: important
@zh ::: important
@en If [_limited to selected sources_](#limited-to-selected-sources) is enabled, Lotion Bath Tactics will use sources in sequential order
@zh If [_limited to selected sources_](#limited-to-selected-sources) is enabled, Lotion Bath Tactics will use sources in sequential order
@en :::
@zh :::

@en ### Source Syncopator
@zh ### Source Syncopator

@en _Applies sources in a patterned, musical way_
@zh _Applies sources in a patterned, musical way_

@en #### Order
@zh #### Order
@en - **Sequential**
@zh - **Sequential**
@en   * Uses clips in the order they were selected.
@zh   * Uses clips in the order they were selected.
@en - **Reversed**
@zh - **Reversed**
@en   * Uses clips in reverse order.
@zh   * Uses clips in reverse order.
@en - **Shuffled**
@zh - **Shuffled**
@en   * Shuffles the order of the generation
@zh   * Shuffles the order of the generation
@en #### Repetitions per clip
@zh #### Repetitions per clip
@en _Specifies the amount of times a source can be used before switching to the next clip_
@zh _Specifies the amount of times a source can be used before switching to the next clip_
@en #### Total repetition rounds
@zh #### Total repetition rounds
@en _Specifies the amount of times to loop_
@zh _Specifies the amount of times to loop_
@en ::: info
@zh ::: info
@en Setting to 0 loops infinitely
@zh Setting to 0 loops infinitely
@en :::
@zh :::
@en #### Apply visual effects by rounds
@zh #### Apply visual effects by rounds
@en _Applies video effects in cycles rather than every clip_
@zh _Applies video effects in cycles rather than every clip_
@en #### Mystery Box
@zh #### Mystery Box
@en _Combines the [Mystery Box](#mystery-box) behavior to the generation_
@zh _Combines the [Mystery Box](#mystery-box) behavior to the generation_
@en #### Accumulate overtones of chords separately
@zh #### Accumulate overtones of chords separately
@en _Makes use of multiple sources when a chord is generated_
@zh _Makes use of multiple sources when a chord is generated_
@en #### Sustain source at same pitch
@zh #### Sustain source at same pitch
@en _Uses the same source for the next note with the same pitch_
@zh _Uses the same source for the next note with the same pitch_
@en ##### Pitch cache capacity
@zh ##### Pitch cache capacity
@en _Specifics the amount of times it will repeat the source_
@zh _Specifics the amount of times it will repeat the source_
@en ::: important
@zh ::: important
@en Disables repetitions per clip
@zh Disables repetitions per clip
@en :::
@zh :::

@en ### Source Orchestra
@zh ### Source Orchestra

@en _Assigns source clips to MIDI tracks, like assigning instruments to an orchestra_
@zh _Assigns source clips to MIDI tracks, like assigning instruments to an orchestra_

@en #### Selection Mode
@zh #### Selection Mode
@en * **Mystery Box**
@zh * **Mystery Box**
@en   * Uses the [Mystery Box](#mystery-box) behavior
@zh   * Uses the [Mystery Box](#mystery-box) behavior
@en * **Source Syncopator**
@zh * **Source Syncopator**
@en   * Uses the [Source Syncopator](#source-syncopator) behavior
@zh   * Uses the [Source Syncopator](#source-syncopator) behavior
@en #### Descending
@zh #### Descending
@en _Reverses the order of sources used on each track_
@zh _Reverses the order of sources used on each track_
@en #### Allow reuse
@zh #### Allow reuse
@en _Allows the same clips to be used across different MIDI tracks multiple tracks_
@zh _Allows the same clips to be used across different MIDI tracks multiple tracks_
@en ::: info
@zh ::: info
@en *This is useful when you have fewer sources than tracks*
@zh *This is useful when you have fewer sources than tracks*
@en :::
@zh :::

@en ### Consonant Time
@zh ### Consonant Time
@en > *COMING SOON (patience)*
@zh > *COMING SOON (patience)*
@en ### Shupelunker Tactics & Tartar Tactics
@zh ### Shupelunker Tactics & Tartar Tactics
@en > *COMING SOON (patience)*
@zh > *COMING SOON (patience)*
@en
@zh
