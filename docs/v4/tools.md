@en # Tools
@zh # 工具

@en ***These QoL Functions within the script make creating YTPMVs and otoMADs a little less painful for setting up or working with. These don't require any adjustments to any of the previous parameters.***
@zh ***脚本中的这些效率工具可略微简化音MAD的创作流程及相关设置，且无需对原先参数进行任何调整。***

@en * **Close this dialog after the operation completes**
@zh * **操作完成之后关闭本对话框**
@en   > *Self-explanatory.*
@zh   > *不言自明。*

@en ## Auto Layout Tracks
@zh ## 自动布局轨道

@en _Layout controls for visuals._
@zh _画面的布局控制。_

@en ![Auto Layout Tracks](/img/v4/tools/auto_layout_tracks.png)
@zh ![自动布局轨道](/img/v4/tools/auto_layout_tracks_zh-CN.png)

::: warning
@en *Video tracks must be manually created and selected for usage, this will not create new video tracks.*
@zh *视频轨道必须手动创建并选择使用，这不会创建新的视频轨道。*
:::

@en ### Grid Layout
@zh ### 网格布局

@en ![Auto Layout Tracks - Grid Layout](/img/v4/tools/grid_layout.png){.shadow-less}
@zh ![自动布局轨道 - 网格布局](/img/v4/tools/grid_layout_zh-CN.png){.shadow-less}

@en #### Array
@zh #### 阵列

@en * **Square**
@zh * **方阵**
@en   * Makes the standard layout.
@zh   * 创建标准布局。
    ::: info
@en     *This makes the Layout 2×2, 3×3, etc.*
@zh     *这会创建 2×2、3×3 等布局。*
    :::
@en * **Custom**
@zh * **自定义**
@en   * Customize the Layout Format.
@zh   * 自定义布局格式。

@en #### Parameters
@zh #### 参数

@en * **Columns**
@zh * **列数**
@en   * The value of Columns.
@zh   * 列的数量。
@en * **Rows**
@zh * **行数**
@en   * The value of Rows.
@zh   * 行的数量。

::: info
@en *The Rows will adapt to the number of columns and vice versa.*
@zh *行将自适应列数，反之亦然。*
:::

@en #### Fit
@zh #### 取景

@en * **Cover**
@zh * **裁切**
@en   * Adapts the size of the boxes to fit with the layout.
@zh   * 调整框的大小以适应布局。
    ::: important
@en     *This will stretch them if applicable.*
@zh     *如果适用的话，这将拉伸它们。*
    :::
@en * **Contain**
@zh * **遮幅**
@en   * Keep the boxes' original size and adapt them to the layout.
@zh   * 保持盒子的原始尺寸并使其适应布局。
@en * **Overlay**
@zh * **叠加**
@en   * Crops the window of the visuals to the columns and rows specification.
@zh   * 将画面的窗口裁剪到列和行规定。

@en #### Mirror Edges
@zh #### 镜像边缘

@en _Mirrors the Tracks in a Parity Pattern._
@zh _以奇偶校验模式镜像轨道。_

@en ![Mirror Edges](/img/v4/tools/mirror_edges.jpg)
@zh ![镜像边缘](/img/v4/tools/mirror_edges.jpg)

@en * **Horizontal Flip**
@zh * **水平翻转**
@en   * Flips the Columns in the pattern chosen.
@zh   * 以所选模式翻转列。
@en * **Vertical Clip**
@zh * **垂直翻转**
@en   * Flips the Rows in the pattern chosen.
@zh   * 以所选模式翻转行。

@en #### Descending
@zh #### 递减次序

@en _Reverses the layout sorting method (when ticked off)._
@zh _反转布局排序方法（勾选时）。_

@en #### Padding
@zh #### 边距

@en _Adjust the track boxes to be less cluttered with other boxes._
@zh _调整轨道框，使其与其它框不那么杂乱。_

@en ### 3D Box Layout
@zh ### 3D方盒布局

@en _This will create a cube with the tracks you select._
@zh _这将使用你所选的轨道创建一个立方体。_

@en ![Auto Layout Tracks - 3D Box Layout](/img/v4/tools/3d_box_layout.png){.shadow-less}
@zh ![自动布局轨道 - 3D方盒布局](/img/v4/tools/3d_box_layout_zh-CN.png){.shadow-less}

@en ::: tip The Layout goes as follows
@zh ::: tip 布局如下
@en * **Front**
@zh * **前面**
@en * **Back**
@zh * **后面**
@en * **Left**
@zh * **左面**
@en * **Right**
@zh * **右面**
@en * **Top**
@zh * **顶面**
@en * **Bottom**
@zh * **底面**
:::

@en #### Delete Original Track
@zh #### 删除原轨道

@en _Deletes the original track(s) when the layout generates._
@zh _布局生成时删除原始轨道。_


@en #### Use Longer Side of Edge Length
@zh #### 使用视频的长边作为立方体的棱长

@en _This may make the cube more natural._
@zh _这可能使得立方体更加自然。_

::: important
@en *This scales with the video project or media file in the track.*
@zh *这会随着轨道中的视频项目或媒体文件而缩放。*
:::

@en ### Gradient Tracks
@zh ### 渐变轨道

@en _This makes the video tracks have a gradient style color effect in your layout._
@zh _这使得视频轨道在布局中具有渐变样式的颜色效果。_

@en ![Gradient Tracks](/img/v4/tools/gradient_tracks.png){.shadow-less}
@zh ![渐变轨道](/img/v4/tools/gradient_tracks_zh-CN.png){.shadow-less}

@en * **Gradient Effect**
@zh * **渐变效果**
@en   * The Effect Presets.
@zh   * 效果预设。
@en * **Descending**
@zh * **递减次序**
@en   * Reverses the Gradient Order.
@zh   * 反转渐变次序。

@en ### Clear Tracks Motion
@zh ### 清除轨道运动
@en _Resets the Track Motion Box._
@zh _重置轨道运动盒。_
@en ### Clear Tracks Effect
@zh ### 清除轨道效果
@en _Removes all the Track Effects._
@zh _移除所有轨道效果。_

@en ### Track Legato
@zh ### 填补轨道间隙

@en ![Track Legato](/img/v4/tools/legato.png)
@zh ![填补轨道间隙](/img/v4/tools/legato_zh-CN.png)

::: warning
@en *This affects all the track events on the select track(s).*
@zh *这会影响所选轨道上的所有轨道事件。*
:::

@en * **Stacking Clips (for current tracks)**
@zh * **堆积剪辑（应用于当前轨道）**
@en   * Stacks all Clips together in the selected tracks.
@zh   * 将选定轨道中的所有剪辑堆叠在一起。
@en * **Stacking Clips (for all tracks)**
@zh * **堆积剪辑（应用于所有轨道）**
@en   * Stacks all Clips together on every track.
@zh   * 将每个轨道上的所有剪辑堆叠在一起。
@en * **Stretch Clips (within stretch limits)**
@zh * **拉伸剪辑（限制在拉伸极限范围之内）**
@en   * Stretch Clips without Extending the clips' length.
@zh   * 拉伸剪辑而不延长剪辑的长度。
@en * **Stretch Clips (lengthen when outside stretch limit)**
@zh * **拉伸剪辑（超出拉伸极限范围之后再延长剪辑）**
@en   * Stretch Clips and Lengthens the Clips.
@zh   * 拉伸剪辑并延长剪辑。
@en * **Lengthen Clips (changes clips length)**
@zh * **延长剪辑（改变剪辑时长）**
@en   * Lengthens the Clips.
@zh   * 延长剪辑。
@en * **Increase Spacing (current track)**
@zh * **增加间隙（应用于当前轨道）**
@en   * Divides the Spacing of Each Clip in the Track.
@zh   * 划分轨道中每个剪辑的间距。
@en * **Increase Spacing (all track)**
@zh * **增加间隙（应用于所有轨道）**
@en   * Divides the Spacing of Each Clip for all Tracks.
@zh   * 划分所有轨道的每个剪辑的间距。
@en * **For Selected Track Events Only**
@zh * **仅应用于所选轨道剪辑**
@en   * Allows Track Legato to be applied on selected track events only.
@zh   * 允许将填补轨道间隙仅应用于所选轨道事件。
@en * **Include the Track Events within a Group**
@zh * **也应用于同分组内的其它轨道剪辑**
@en   * Allows Track Legato to be applied on the Grouped Clips.
@zh   * 允许将填补轨道间隙应用于分组剪辑。
@en * **Reverse Direction**
@zh * **反转方向**
@en   * Applies the Legato Effect Backwards.
@zh   * 向后应用填补间隙效果。

::: important
@en *Lengthening will extend to the next clip, even to clips that are far apart.*
@zh *延长将延伸到下一个剪辑，甚至是相距较远的剪辑。*
:::
::: info
@en *Make sure there are no clips beyond your final visual.*
@zh *确保没有超出最终画面的剪辑。*
:::

@en ## Quick Select Interval
@zh ## 快速间隔选择

@en _This can select specific clips in the track that are selected._
@zh _这可以选择所选轨道中的特定剪辑。_

@en ![Quick Select Interval](/img/v4/tools/quick_select_interval.png){.shadow-less}
@zh ![快速间隔选择](/img/v4/tools/quick_select_interval_zh-CN.png){.shadow-less}

@en * **Select one for every few**
@zh * **每几段选择一段**
@en   * Selects every other clip based on the interval value.
@zh   * 根据间隔值选择每隔一个剪辑。
@en * **Select which one of the group**
@zh * **选择每组第几段**
@en   * Selects which order of track clips in the selection.
@zh   * 选择所选内容中轨道剪辑的顺序。
@en * **Select how many at a time**
@zh * **每次要选取几段**
@en   * Selects how many clips at a time per group.
@zh   * 选择每组一次有多少个剪辑。
@en * **Reset Selection**
@zh * **重置选择**
@en   * Resets back to the selection pre-running the script.
@zh   * 重置回运行脚本前的选择。

@en ## Quick Config Properties
@zh ## 快速配置属性

@en _Set the properties of the event clips selected._
@zh _设置所选事件剪辑的属性。_

@en ![Quick Config Properties](/img/v4/tools/quick_config_properties.png){.shadow-less}
@zh ![快速配置属性](/img/v4/tools/quick_config_properties_zh-CN.png){.shadow-less}

@en ### Video clips
@zh ### 视频剪辑
@en * Hide
@zh * 隐藏
@en   * Toggles the video.
@zh   * 开关视频。
@en * Lock
@zh * 锁定
@en   * Toggles video lock.
@zh   * 开关视频锁定。
@en * Loop
@zh * 循环
@en   * Toggles video looping.
@zh   * 开关视频循环。
@en * Maintain aspect ratio
@zh * 保持宽高比
@en   * Lock the aspect ratio.
@zh   * 锁定纵横比。
@en * Reduce interlace flicker
@zh * 减少隔行扫描闪烁
@en   * Toggles the interlace flicker.
@zh   * 开关隔行闪烁。
@en * Resample Mode
@zh * 重新采样模式
@en   * Sets the resample mode of the video.
@zh   * 设置视频的重采样模式。
@en * Playback rate settings
@zh * 播放速率设置
@en   * Sets the playback rate of the video.
@zh   * 设置视频的播放速率。
@en * Set undersample rate
@zh * 设置欠采样率
@en   * Sets the undersample rate of the video.
@zh   * 设置视频的欠采样率。
@en * Opacity Settings
@zh * 不透明度设置。
@en   * Sets the opacity of the video.
@zh   * 设置视频的不透明度。

@en ### Audio clips
@zh ### 音频剪辑
@en * Mute
@zh * 静音。
@en   * Toggles the audio.
@zh   * 开关音频。
@en * Lock
@zh * 锁定
@en   * Toggles audio lock.
@zh   * 开关音频锁定。
@en * Loop
@zh * 循环
@en   * Toggles audio looping.
@zh   * 开关音频循环。
@en * Invert phase
@zh * 反相
@en   * Inverts the audio phase.
@zh   * 反转音频相位。
@en * Normalize settings
@zh * 规范化音量
@en   * Sets the normalize settings of the audio.
@zh   * 设置音频的规范化增益设置。
@en * Playback rate settings
@zh * 播放速率设置
@en   * Sets the playback rate of the audio.
@zh   * 设置音频的播放速率。
@en * Volume settings
@zh * 音量设置
@en   * Sets the volume of the audio.
@zh   * 设置音频的音量。

@en ## Replace Track Events
@zh ## 替换轨道剪辑

@en _Replaces selected track events with specific preferences._
@zh _用特定首选项替换所选轨道事件。_

@en ### Specify simultaneously
@zh ### 同时指定

@en ![Replace Track Events Simultaneously](/img/v4/tools/replace_track_events_simultaneously.png){.shadow-less}
@zh ![替换轨道剪辑 - 同时指定](/img/v4/tools/replace_track_events_simultaneously_zh-CN.png){.shadow-less}

@en * *The Selected Track Events can be replaced with whatever you have selected at the end of the selection, you can choose a clip from another track and use that as the replacement and all the track clips that you have set before it will be replaced with that clip.*
@zh * *所选轨道事件可以替换为你在选择结束时所选的任何内容，你可以从另一个轨道中选择一个剪辑并将其用作替换，并且你之前设置的所有轨道剪辑都将被该剪辑替换。*

::: important
@en *It's recommended not to put the replacement clip in the same track.*
@zh *建议不要将替换剪辑放在同一轨道中。*
:::

@en ### Specify separately
@zh ### 分别指定

@en ![Replace Track Events Separately](/img/v4/tools/replace_track_events_separately.png){.shadow-less}
@zh ![替换轨道剪辑 - 分别指定](/img/v4/tools/replace_track_events_separately_zh-CN.png){.shadow-less}

@en * *Replaces specific clips in the track with any clip you select in any order you want.*
@zh * *将轨道中的特定剪辑替换为你按所需顺序所选的任何剪辑。*
@en * [*Explanation in Extra Info*](./faq.md#replace-track-events-separately-explanation){.vp-external-link-icon}
@zh * [*额外信息中的解释*](./faq.md#替换轨道剪辑-分别指定-解释){.vp-external-link-icon}

@en ### Options
@zh ### 选项
@en * **Replace other clips in a event group**
@zh * **同时替换分组内的其它剪辑**
@en   * Replaces Other Clips from the Same Track Event Group.
@zh   * 替换同一轨道事件组中的其它剪辑。
    ::: important
@en     *Doesn't apply if the clip is not in a group.*
@zh     *若剪辑不在组中则不应用。*
    :::
@en * **Reserve original clip name**
@zh * **保留原剪辑名称**
@en   * Reserves the original clip name of the original clips after the replacement.
@zh   * 保留替换后原始剪辑的原始剪辑名称。
@en * **Reserve original clip offset**
@zh * **保留原剪辑偏移量**
@en   * Reserves the original offset of the clip used in the timeline.
@zh   * 保留时间线中使用的剪辑的原始偏移量。

@en ## Change Tuning Method
@zh ## 更改调音算法

@en _Changes the Tuning Method to the selected track events._
@zh _更改所选轨道事件的调音方法。_

@en ![Change Tuning Method](/img/v4/tools/change_tuning_method.png){.shadow-less}
@zh ![更改调音算法](/img/v4/tools/change_tuning_method_zh-CN.png){.shadow-less}

@en *Refer to ["Audio"](./audio.md#tuning) on details about the Audio Tuning Methods*
@zh *有关音频调音方法的详细信息，请参阅[“音频”](./audio.md#调音)部分。*

@en ### Pitch Semitones
@zh ### 音调更改
@en * **Pitch Lock**
@zh * **锁定以拉伸**
@en   * Locks the Pitch Value.
@zh   * 锁定音高值。
@en * **Lock Pitch instead of rate**
@zh * **锁定音高而不是速度**
@en   * Locks the Pitch instead of the Stretch Rate.
@zh   * 锁定音高而不是拉伸速度。
@en ### Formant Shift
@zh ### 共振峰移位
@en * **Reserve Formant**
@zh * **保持共振峰**
@en   * Locks Formant.
@zh   * 锁定共振峰。

@en ## Batch Subtitle Generation
@zh ## 批量生成字幕

@en _Creates batches of text using the Title Media Generator with any duration._
@zh _使用标题媒体生成器创建任意持续时间的批量文本。_

@en ![Batch Subtitle Generation](/img/v4/tools/batch_subtitle_generation.png){.shadow-less}
@zh ![批量生成字幕](/img/v4/tools/batch_subtitle_generation_zh-CN.png){.shadow-less}

@en - **Import from File**
@zh - **从文件中导入**
@en   * Import a text file to be used for subtitles.
@zh   * 导入用于字幕的文本文件。
@en * **Preset**
@zh * **预设**
@en   * Controls the Preset Selection.
@zh   * 控制预设选择。
@en * **Input**
@zh * **输入**
@en   * Input text, line by line.
@zh   * 逐行输入文本。
    ::: important
@en     *Blank lines are ignored.*
@zh     *空行将被忽略。*
    :::
@en * **Duration**
@zh * **时长**
@en   * Duration of each line.
@zh   * 每条的时长。

@en ## Find Track Events
@zh ## 查找轨道剪辑

@en ![Find Track Events](/img/v4/tools/find_track_events.png){.shadow-less}
@zh ![查找轨道剪辑](/img/v4/tools/find_track_events_zh-CN.png){.shadow-less}

@en _Find and Select all the track events that match the specified._
@zh _查找并选择与指定匹配的所有轨道事件。_

::: info
@en *This is helpful for selecting clips for layout replacement, making template layouts easier.*
@zh *这有助于选择用于布局替换的剪辑，使模板布局更容易。*
:::

@en ### Options
@zh ### 选项
@en * *Matching Same Source*
@zh * *匹配与选中轨道剪辑相同的所有剪辑*
@en * *Matching Same Source with Same Starting Offset*
@zh * *匹配与选中轨道剪辑相同且开始偏移量相等的所有剪辑*
@en * *Matching Specified Name*
@zh * *匹配与指定名称相匹配的剪辑*

@en ### Search Text Box
@zh ### 搜索文本框

@en _Search for any video, audio or both track events in your project._
@zh _搜索项目中的任何视频、音频或全部轨道事件。_

@en * **Both**
@zh * **全部**
@en   * Searches for both video and audio events.
@zh   * 搜索视频和音频事件。
@en * **Video events**
@zh * **仅视频**
@en   * Searches for only video events.
@zh   * 仅搜索视频事件。
@en * **Audio events**
@zh * **仅音频**
@en   * Searches for only audio events.
@zh   * 仅搜索音频事件。

@en ## Apply Visual Effect
@zh ## 应用视觉效果

@en _Apply Visual Effects to selected Track Events._
@zh _将视觉效果应用于所选轨道事件。_

@en ![Apply Visual Effect](/img/v4/tools/apply_visual_effect.png){.shadow-less}
@zh ![应用视觉效果](/img/v4/tools/apply_visual_effect_zh-CN.png){.shadow-less}

@en *Refer to ["Visual"](./visual.md#effects) for details on the Visual effects you can use.*
@zh *有关你可以使用的视觉效果的详细信息，请参阅[“画面”](./visual.md#效果)部分。*

@en ## Convert Music Beats
@zh ## 转换音乐节拍

@en _Converts Music Beats to a Different Time Signature._
@zh _将音乐节拍转换为不同的拍号。_

@en ![Convert Music Beats](/img/v4/tools/convert_music_beats.png){.shadow-less}
@zh ![转换音乐节拍](/img/v4/tools/convert_music_beats_zh-CN.png){.shadow-less}

::: info
@en Useful for changing specific beats to a different time signature.
@zh 用于将特定节拍更改为不同的拍号。
:::

@en ## Custom Fade Gain
@zh ## 自定淡化增益

@en _Adjusts Gain/Volume Range for Selected Track Events._
@zh _调整所选轨道事件的增益/音量范围。_

@en ![Custom Fade Gain](/img/v4/tools/custom_fade_gain.png){.shadow-less}
@zh ![自定淡化增益](/img/v4/tools/custom_fade_gain_zh-CN.png){.shadow-less}

@en * **From Value**
@zh * **从**
@en   * The Value of the Starting Fade Effect.
@zh   * 起始渐入或渐出效果的值。
@en * **To Value**
@zh * **至**
@en   * The Value of the Ending Fade Effect.
@zh   * 终止渐入或渐出效果的值。

@en ## Export MIDI File
@zh ## 导出MIDI文件

@en _Exports Tracks or Track Events to a Score Sequence File._
@zh _将轨道或轨道事件导出到乐谱序列文件。_

@en ![Export MIDI File](/img/v4/tools/export_midi_file.png){.shadow-less}
@zh ![导出MIDI文件](/img/v4/tools/export_midi_file_zh-CN.png){.shadow-less}

::: info
@en *Naming your tracks beforehand will make the creation process smoother and help identify your tracks for future use.*
@zh *预先命名你的音轨将使创建过程更加顺利，并有助于识别你的音轨以供将来使用。*
:::

::: important
@en **The MIDI being created can support the following:**
@zh **要创建的MIDI可支持以下条目：**

@en - ☑️ **Use audio tracks and events.**
@zh - ☑️ **使用音频轨道和事件。**
@en - ☑️ **Can generate multiple tracks.**
@zh - ☑️ **可生成多音轨。**
@en - ☑️ **Allows selecting instruments.**
@zh - ☑️ **允许选择乐器。**
@en - ☑️ **Allows set track name.**
@zh - ☑️ **允许设置音轨名称。**
@en - ☑️ **Allows setting base pitch.**
@zh - ☑️ **允许设置原始音高。**
@en - ☑️ **Can export loop region only.**
@zh - ☑️ **可以仅导出循环区域。**
@en - ☑️ **Adjust events gain (Audio: Volume; Video: Opacity.)** [^1]
@zh - ☑️ **调整事件增益（音频：音量；视频：不透明度。）**[^1]
@en - ☑️ **Adjust tracks volume for audio tracks or opacity/composite level for video tracks.** [^1]
@zh - ☑️ **调整音频轨道的轨道音量或视频轨道的不透明度/合成级别。**[^1]
@en - ☑️ **Adjust audio tracks pan.** [^1]
@zh - ☑️ **调整音频轨道的声像。**[^1]
@en - ❎ **Use video tracks and events.**
@zh - ❎ **使用视频轨道和事件。**
@en - ❎ **Using the "Pitch Shift" Audio FX instead of tuning with [[+]] and [[-]] key.**
@zh - ❎ **使用“移调”音频FX，而不是使用 [[+]] 和 [[-]] 键进行调音。**
@en - ❎ **Using a version of Vegas Pro \< 16.**
@zh - ❎ **所用Vegas Pro版本 < 16。**

@en > **☑️ - Good to go.**
@zh > **☑️ - 做得好。**
@en > **❎ - Not recommended - _This will generate notes with all base pitch._**
@zh > **❎ - 不建议 - _这将生成全部具有原始音高的音符。_**
:::

@en [^1]: These all support envelopes / automation control.
@zh [^1]: 这些都支持包络/自动化控制。

@en **Settings**
@zh **设置**
@en * **All Tracks**
@zh * **所有轨道**
@en   * Selects all tracks.
@zh   * 选择所有轨道。
@en * **Video Tracks**
@zh * **视频轨道**
@en   * Selects only video tracks.
@zh   * 仅选择视频轨道。
@en * **Audio Tracks**
@zh * **音频轨道**
@en   * Selects only audio tracks.
@zh   * 仅选择音频轨道。
@en * **Base Pitch**
@zh * **原始音高**
@en   * Sets the default base pitch (C5).
@zh   * 设置默认原始音高 (C5)。
@en * **Export loop region only**
@zh * **仅导出循环区域**
@en   * Exports the MIDI with only the selected loop region track events.
@zh   * 仅导出包含选定循环片段轨道事件的MIDI。

@en ### Vegas Track List
@zh ### Vegas轨道列表

@en _Select the tracks to be added to the MIDI File._
@zh _选择要添加到MIDI文件的轨道。_

::: important
@en *Track names can and will get automatically added to the track list and channel list.*
@zh *轨道名称会自动添加到音轨列表和通道列表中。*
:::

@en **Options**
@zh **选项**
@en * **Add to each new tracks**
@zh * **添加到各自新轨道**
@en   * Adds the selected track(s) to new MIDI tracks with MIDI channels.
@zh   * 将所选轨道添加到不同的新MIDI音轨及MIDI通道。
@en * **Add to a same new track**
@zh * **添加到同一新轨道**
@en   * Adds the selected track(s) to 1 new MIDI track with MIDI channels.
@zh   * 将所选轨道添加到1个新MIDI音轨及不同的MIDI通道。
@en * **Add to current track**
@zh * **添加到当前轨道**
@en   * Adds the selected track(s) to the selected MIDI track with new MIDI channels.
@zh   * 将所选轨道添加到所选的MIDI轨道及不同的MIDI通道。
@en * **Preview**
@zh * **预览**
@en   * Previews the track(s) selected.
@zh   * 预览所选轨道。

@en ### MIDI Track List
@zh ### MIDI音轨列表

@en _Manage the track list for the MIDI being prepared. Displays how many created tracks in the preparation list and how many Vegas tracks and notes in each MIDI Track._
@zh _管理正备用的MIDI音轨列表。显示备用列表中有多少个已创建的音轨以及每个MIDI音轨中有多少个Vegas轨道和音符。_

::: important
@en *This is what Otomad Helper will be looking for. Adding any tracks to the channel list will add notes instead in the export.*
@zh *这正是Otomad Helper所查找的。将任何音轨添加到通道列表都会在导出中添加音符。*
:::

@en **Options**
@zh **选项**
@en * **Move up**
@zh * **上移**
@en   * Moves the selected track up.
@zh   * 将所选音轨向上移动。
@en * **Move down**
@zh * **下移**
@en   * Moves the selected track down.
@zh   * 将所选音轨向下移动。
@en * **Remove**
@zh * **移除**
@en   * Removes the selected track from the list.
@zh   * 从列表中删除所选音轨。
@en * **Add a new empty track**
@zh * **添加新的空音轨**
@en   * Creates a new MIDI track.
@zh   * 创建一条新的MIDI音轨。
@en * **Insert a new empty track**
@zh * **插入新的空音轨**
@en   * Insert a new MIDI track on top of selected.
@zh   * 在所选音轨上插入新的MIDI音轨。
@en - **Name**
@zh - **名称**
@en   * Input a name for the track.
@zh   * 输入音轨的名称。

@en ### MIDI Channel List
@zh ### MIDI通道列表

@en _Manage the channel list from the select track in the track list. Displays how many Vegas tracks and notes created in the selected MIDI track._
@zh _从轨道列表中所选轨道管理通道列表。显示在所选MIDI轨道中创建的Vegas轨道和音符的数量。_

@en **Options**
@zh **选项**
@en * **Remove**
@zh * **移除**
@en   * Removes the selected channel from the list.
@zh   * 从列表中删除所选通道。
@en * **Channel**
@zh * **通道**
@en   * Set the channel the selected track is on.
@zh   * 设置所选轨道所在的通道。

::: danger
@en *Use the same channel at your own risk.*
@zh *使用同一通道需自行承担风险。*
:::

@en * Instrument
@zh * 乐器
@en   * Displays used MIDI instrument for the channel.
@zh   * 显示通道使用的MIDI乐器。

@en ### MIDI Instrument list
@zh ### MIDI乐器列表

@en _List of instruments selected to their according MIDI channel that can be changed._
@zh _已选择乐器并分配到相应MIDI通道的列表，这些通道可以更改。_

@en **Options**
@zh **选项**
@en * **Dispatch Instrument to Channel**
@zh * **为通道指派乐器**
@en   * Changes and sets instrument to the selected MIDI channel.
@zh   * 为所选MIDI通道更改并设置乐器。
