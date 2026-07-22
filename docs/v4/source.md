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
@en * You can also choose a media file that has the visual ready to go for you when you browse.
@zh * 你还可以选择或浏览一个早已准备好了画面的媒体文件。
@en * You can select a video clip and audio clip separately, and the script will combine them to generate.
@zh * 你可以分别选择视频剪辑和音频剪辑，脚本会将它们组合起来一起生成。
@en * You can also drag and import clips to be used in Otomad Helper.
@zh * 你还可以拖动并导入剪辑以在Otomad Helper中使用。
:::

@en ### Start and End Time
@zh ### 起始和终止时间

@en _Trim the start time and end time of the clip used._
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
@zh   * 在所选的任何时间位置生成MIDI。

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
@en   * Moves the cursor to [the start of the MIDI file](#generate-at).
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
@en There is a bug. If you find that you cannot render the project properly when using audio bus track feature, please click the link below to fix it now.
@zh 目前存在错误。如果你在使用音频总线轨道功能时发现无法正常渲染项目，请立即点击下方链接以修复。

@en [How to Fix](./faq.md#audio-bus-track-bug-caused-by-vegas-pro){.vp-external-link-icon}
@zh [如何修复](./faq.md#vegas-pro引起的音频总线轨道错误){.vp-external-link-icon}
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
@zh ### 关

@en _Disables the multisource features._
@zh _禁用多素材功能。_

@en ### Mystery Box
@zh ### 素材盲盒

@en _Automatically picks from multiple selected sources in unpredictable ways._
@zh _以不可预测的方式自动从多个选定素材中选挑选。_

@en #### Limited to selected sources
@zh #### 限制在所选素材
@en _Only uses the sources selected in the timeline/project media._
@zh _仅使用在时间线/项目媒体中的所选素材。_
@en #### Make each track different
@zh #### 使每条音轨不同
@en _Each track will generate with a different clip._
@zh _各音轨会使用一个不同的剪辑来生成。_
@en #### Switch once per marker
@zh #### 每处标记切换一次
@en _Changes the source when a MIDI marker is reached._
@zh _到达MIDI标记时更换素材。_
@en #### Switch once per bar or beat
@zh #### 每小节或每拍切换一次
@en _Changes the source when a beat or bar has passed._
@zh _通过拍或小节时更换素材。_
@en #### Lotion Bath Tactics
@zh #### 润肤浴战法（[ローション風呂戦法]{lang=ja}）
@en _Forces the source to sequentially change at your specified duration._
@zh _强制素材在指定的持续时间内顺序更改。_
::: important
@en If [_limited to selected sources_](#limited-to-selected-sources) is enabled, Lotion Bath Tactics will use sources in sequential order
@zh 如果[_限制在所选素材_](#限制在所选素材)已开启，润肤浴战法将按顺序依次使用素材。
:::

@en ### Source Syncopator
@zh ### 踩点

@en _Applies sources in a patterned, musical way._
@zh _以音乐模式应用素材。_

@en #### Order
@zh #### 次序
@en - **Sequential**
@zh - **顺序**
@en   * Uses clips in the order they were selected.
@zh   * 按所选顺序使用剪辑。
@en - **Reversed**
@zh - **逆序**
@en   * Uses clips in reverse order.
@zh   * 以相反的顺序使用剪辑。
@en - **Shuffled**
@zh - **乱序**
@en   * Shuffles the order of the generation.
@zh   * 以打乱的顺序生成。
@en #### Repetitions per clip
@zh #### 每段重复次数
@en _Specifies the amount of times a source can be used before switching to the next clip._
@zh _指定在切换到下一个剪辑之前可以使用素材的次数。_
@en #### Total repetition rounds
@zh #### 每轮重复总数
@en _Specifies the amount of times to loop._
@zh _指定循环的次数。_
@en ::: info
@en Setting to 0 loops infinitely.
@zh 设为0表示可以无限循环。
@en :::
@en #### Apply visual effects by rounds
@zh #### 按轮次应用视觉效果
@en _Applies video effects in cycles rather than every clip._
@zh _按周期而不是各剪辑应用视频视觉效果。_
@en #### Mystery Box
@zh #### 素材盲盒
@en _Combines the [Mystery Box](#mystery-box) behavior to the generation._
@zh _将[素材盲盒](#素材盲盒)的行为与生成相结合。_
@en #### Accumulate overtones of chords separately
@zh #### 对和弦的泛音列独立累加
@en _Makes use of multiple sources when a chord is generated._
@zh _生成和弦时使用多个素材。_
@en #### Sustain source at same pitch
@zh #### 相同音高时保持素材
@en _Uses the same source for the next note with the same pitch._
@zh _下一个音高相同的音符将使用相同的素材。_
@en ##### Pitch cache capacity
@zh ##### 音高缓存容量
@en _Specifies the amount of times it will repeat the source._
@zh _指定它将重用素材的数量_
::: important
@en Disables repetitions per clip.
@zh 将禁用每段重复次数。
:::

@en ### Source Orchestra
@zh ### 素材乐团

@en _Assigns source clips to MIDI tracks, like assigning instruments to an orchestra._
@zh _将素材剪辑分配给MIDI音轨，就像将乐器分配给管弦乐队一样。_

@en #### Selection Mode
@zh #### 挑取模式
@en * **Mystery Box**
@zh * **素材盲盒**
@en   * Uses the [Mystery Box](#mystery-box) behavior
@zh   * 使用[素材盲盒](#素材盲盒)的行为
@en * **Source Syncopator**
@zh * **踩点**
@en   * Uses the [Source Syncopator](#source-syncopator) behavior
@zh   * 使用[踩点](#踩点)的行为
@en #### Descending
@zh #### 递减次序
@en _Reverses the order of sources used on each track._
@zh _反转各音轨上使用的素材的顺序。_
@en #### Allow reuse
@zh #### 允许重用
@en _Allows the same clips to be used across different MIDI tracks multiple tracks._
@zh _允许在不同的MIDI多轨道中使用相同的剪辑。_
::: info
@en *This is useful when you have fewer sources than tracks.*
@zh *当素材少于轨道数时很有用。*
:::

@en ### Consonant Time
@zh ### 辅音时间
@en > *COMING SOON (patience)*
@zh > *即将到来（要有耐心）*
@en ### Shupelunker Tactics & Tartar Tactics
@zh ### 原音系战法和鞑靼战法
@en > *COMING SOON (patience)*
@zh > *即将到来（要有耐心）*
