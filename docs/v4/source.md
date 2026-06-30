@en # Source
@zh # 素材

@en ## Source Configuration
@zh ## 素材属性

@en ![Configuration](/img/v4/source/configuration.png)
@zh ![Configuration](/img/v4/source/configuration_zh-CN.png)

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
@zh ### 设定生成开始位置

@en _Controls where the MIDI can be generated._
@zh _控制要生成MIDI的位置。_

@en * **Project Start**
@zh * **项目开始处**
@en   * Generates the MIDI at the Start.
@zh   * 在开始时生成MIDI。
@en * **Cursor**
@zh * **光标处**
@en   * Generates the MIDI at the Cursor Position.
@zh   * 在光标位置生成MIDI。
@en * **Custom**
@zh * **自定义**
@en   * Generates the MIDI at any Time Position chosen.
@zh   * 在选择的任何时间位置生成MIDI。

@en ### Below Top Adjustment Tracks
@zh ### 生成在顶层调整轨道的下方

@en _Places the Generated Tracks below the Adjustment Tracks._
@zh _将生成的轨道放置在调整轨道的下方。_

::: warning
@en *This is available in Vegas Pro 19 and up.*
@zh *此功能仅在Vegas Pro 19及更高版本中可用。*
:::

@en ### Remove track events assigned as source material after the generation completes
@zh ### 生成完成后移除作为源素材的轨道事件

@en _Deletes the original source clip after the MIDI has been generated._
@zh _生成MIDI后删除源剪辑。_

@en ### Select all generated events after the generation completes
@zh ### 生成完成后选中生成的所有事件

@en _Selects all events generated after the MIDI has been generated._
@zh _MIDI生成后选中生成的所有事件。_

@en ### Move the Cursor to
@zh ### 生成完成后将光标移动到

@en _Positions the cursor to a specified location in the timeline._
@zh _将光标定位到时间线中的指定位置。_

@en * **Original Position**
@zh * **原位置**
@en   * Retains the timeline cursor in the original position before you ran the script.
@zh   * 将时间线光标保留在运行脚本之前的原始位置。
@en * **Where Generate at**
@zh * **生成开始位置**
@en   * Moves the cursor to [the start of the MIDI file](#generate-at)
@zh   * 将光标移动到[MIDI文件的开头](#generate-at)。
@en * **Before the First Event**
@zh * **第一个事件之前**
@en   * Moves the cursor to before the first generated event in the timeline.
@zh   * 将光标移动到时间线中第一个生成的事件之前。
@en * **After the Last Event**
@zh * **最后一个事件之后**
@en   * Moves the cursor to after the last generated event in the timeline.
@zh   * 将光标移动到时间线中最后一个生成的事件之后。

@en ### Grouping Tracks
@zh ### 轨道组

@en _Creates groups for the tracks._
@zh _为轨道创建分组。_

@en * **Ungrouped**
@zh * **不分组**
@en   * Disables track grouping.
@zh   * 禁用轨道组。
@en * **Group by MIDI track**
@zh * **按MIDI音轨分组**
@en   * Groups tracks based on the MIDI track used and uses the MIDI track name.
@zh   * 根据使用的MIDI音轨对轨道进行分组并使用MIDI音轨名称。
@en * **Group by task session**
@zh * **按任务会话分组**
@en   * Creates a new group whenever a new generation is completed.
@zh   * 每当生成完成时就会创建一个新分组。

---

@en - **Collapse track groups by default**
@zh - **默认情况下折叠轨道组**
@en   * Collapses the track group list
@zh   * 折叠轨道组列表。
@en - **Reuse groups that have the same nonempty name**
@zh - **重用非空同名轨道组**
@en   * The generation of newly added tracks to be added to existing track groups with the same name.
@zh   * 生成时新添加的轨道，将添加到具有相同名称的现有轨道组中。

@en ### Audio Bus Track
@zh ### 音频总线轨道

@en _Routes audio tracks to audio buses (Mixing Console)._
@zh _将音轨路由到音频总线（混音控制台）。_

@en * **Unrouted**
@zh * **不路由**
@en   * Disables routing to audio buses.
@zh   * 禁用路由到音频总线。
@en * **Route by MIDI track**
@zh * **按MIDI音轨路由**
@en   * Routes the audio track to the same bus based on the MIDI track used.
@zh   * 根据使用的MIDI轨道将音轨路由到同一总线。
@en * **Route by task session**
@zh * **按任务会话路由**
@en   * Creates and routes the audio track to a new bus whenever a new generation is completed.
@zh   * 每当生成完成时就会路由音轨到新建总线。

::: danger
@en There is currently an unknown bug that may prevent you from using the audio bus track feature. If you encounter such a problem, it is recommended to disable this feature or try changing another Vegas version.
@zh 目前存在一个未知错误，可能会阻止您使用音频总线轨道功能。如果遇到此类问题，建议禁用此功能或尝试更换其他Vegas版本。

@en [View detailed explanation >](./faq.md#audio-bus-track-bugs-caused-by-vegas-pro)
@zh [了解更多 >](./faq.md#audio-bus-track-bugs-caused-by-vegas-pro)
:::

---

@en - **Reuse audio bus tracks that have the same nonempty name**
@zh - **重用非空同名音频总线轨道**
@en   * The generation of newly added tracks to be added to existing audio buses with the same name
@zh   * 生成时新添加的轨道，将添加到具有相同音频总线的现有轨道组中。

@en ## Multisource Combination
@zh ## 多素材梳子

@en ![Multisource Comb](/img/v4/source/comb.png)
@zh ![Multisource Comb](/img/v4/source/comb_zh-CN.png)

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
