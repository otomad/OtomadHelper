@en # Audio
@zh # 音频

@en **This setting can be toggled.** (If your clip has audios, this is enabled by default.)
@zh **该设置可开关。**（如果你的片段有音频，那么它将默认开启。）
@en **You can right-click or middle-click any setting slider to reset it.**
@zh **你可以右键或中键单击任意设置滑动条来重置它。**

@en ## Toggles
@zh ## 开关

@en ![Toggles](/img/v4/audio/toggles.png)
@zh ![开关](/img/v4/audio/toggles_zh-CN.png)

@en ### Loop
@zh ### 循环

@en _Makes the clip loop if applicable._
@zh _如果适用的话，使剪辑循环。_

@en ### Normalize
@zh ### 规范化音量

@en _Normalize the audio._
@zh _规范化音频的音量。_

::: info
@en *This is useful if the audio is quiet.*
@zh *适用于太安静的音频。*
:::

@en ### Truncate
@zh ### 截断

@en _Trims to the MIDI note instead of stretching._
@zh _修剪MIDI音符而不是拉伸。_

::: important
@en *This conflicts with [Legato](#legato)*.
@zh *这与[填补间隙](#填补间隙)冲突。*
:::

@en ### Multitrack for Chords
@zh ### 复音多轨

@en _Generates multiple audio tracks for chords._
@zh _为和弦生成多条音频轨道。_

@en ### Create Groups
@zh ### 创建分组

@en _Groups the Video and Audio Clips represented by the MIDI note(s)._
@zh _将表示同一MIDI音符的视频和音频剪辑组合在一起。_

::: info
@en *This makes it easier to change the position or length of the audio and video clips simultaneously.*
@zh *这使得同时更改音频和视频剪辑的位置或长度变得更加容易。*
:::

@en ### Stack
@zh ### 堆叠

@en _Creates stacked audio clips on separate tracks when the MIDI contains multiple tracks._
@zh _当MIDI包含多个轨道时，在单独的轨道上创建堆叠音频剪辑。_

::: important
@en *Only works when multiple MIDI tracks are selected.*
@zh *仅在选择MIDI多音轨时有作用。*
:::

@en ### Time unremapping
@zh ### 持续时间流

@en _Disables all forms of time remapping of the clip! The start time will NOT reset as it will continue the clip from where it left off every note._
@zh _禁用剪辑的所有形式的时间重映射！起始时间不会重置，而是将从每个音符停止的位置继续。_

@en ### Auto Pan
@zh ### 自动声像。

@en _Pans the Audio using Envelope Automation._
@zh _使用包络线自动控制音频的声像。_

::: important
@en *This is dependent on your pan automation from your MIDI channel, not the notes.*
@zh *这取决于MIDI通道的声像自动化，而不是音符。*
:::

@en ### Stretch
@zh ### 拉伸

@en _Makes the clip stretch if applicable._
@zh _如果适用的话，使剪辑拉伸。_

@en - **None**
@zh - **不拉伸**
@en   * Does not apply stretching to the clip.
@zh   * 不为剪辑应用拉伸。
@en - **Flex and Extend**
@zh - **可屈伸**
@en   * Stretches the clip completely.
@zh   * 完全地拉伸剪辑。
@en - **Extending Only**
@zh - **仅伸展**
@en   * Stretches clip out.
@zh   * 使剪辑拉伸得更长。
    ::: important
@en     *This works best with MIDI Notes that are longer than the audio clip.*
@zh     *这对于MIDI音符比音频剪辑更长时效果最佳。*
    :::
@en - **Flexing Only**
@zh - **仅屈折**
@en   * Stretches clip in.
@zh   * 使剪辑拉伸得更短。
    ::: important
@en     *This works best with MIDI Notes that are shorter than the audio clip.*
@zh     *这对于MIDI音符比音频剪辑更短时效果最佳。*
    :::

@en ### Legato
@zh ### 填补间隙

@en _Extends the audio outside the note's length with any length chosen._
@zh _以特定的长度将音频延伸到音符长度之外。_

@en - **Staccato**
@zh - **不填补**
@en   * No Extension.
@zh   * 不延长。
@en - **Up to 1 Beat**
@zh - **最长一拍**
@en   * Extends out 1 Beat
@zh   * 延长到1拍。
    ::: important
@en     *This will only work if the next clip is less than 1 beat apart.*
@zh     *仅当到下一个剪辑的间隔小于1拍时，此功能才有效。*
    :::
@en - **Up to 1 Bar**
@zh - **最长一小节**
@en   * Extends out 1 Bar
@zh   * 延长到1小节。
    ::: important
@en     *This will only work if the next clip is less than 1 bar apart.*
@zh     *仅当到下一个剪辑的间隔小于1小节时，此功能才有效。*
    :::
@en - **Unlimited**
@zh - **无限填补**
@en   * Extends Unlimitedly.
@zh   * 无限延长。

::: important
@en *This conflicts with [Truncate](#truncate)*.
@zh *这与[截断](#截断)冲突。*
:::

@en ### Preferred Track
@zh ### 首选轨道

@en _Creates a new track(s) or uses the track selected for generation._
@zh _创建新轨道或使用所选轨道来生成。_

@en ## Velocity
@zh ## 力度

@en _Settings to assign gain values relative to the MIDI notes._
@zh _用于分配与MIDI音符相关的增益值的设置。_

@en * **Mapping Velocity**
@zh * **映射力度**
@en   * Assigns the Notes to the Velocity from the MIDI File.
@zh   * 将音符分配给MIDI文件中的力度。
@en * **Multiply Current Gain**
@zh * **乘以当前增益**
@en   * Toggling multiplies the existing gain instead.
@zh   * 开启后会倍增现有增益。
@en * **Velocity**
@zh * **音符力度**
@en   * The Value of Velocity from Minimum to Maximum.
@zh   * 力度值从最小值到最大值。
@en * **Volume**
@zh * **音量增益**
@en   * The Value of Volume from Minimum to Maximum.
@zh   * 音量值从最小值到最大值。

::: warning
@en *Do not use the same value in both minimum and maximum values for Velocity and Volume.*
@zh *不要在力度和音量的最小值和最大值中使用相同的值。*
:::

@en * **Reset**
@zh * **重置**
@en   * Resets the settings to default.
@zh   * 重置为默认设置。

@en [*Mapping Velocity Explanation*](./faq.md#mapping-velocity-explanation){.vp-external-link-icon}
@zh [*映射力度解释*](./faq.md#映射力度解释){.vp-external-link-icon}

@en ## Tuning
@zh ## 调音

@en ![Tuning](/img/v4/audio/tuning.png)
@zh ![调音](/img/v4/audio/tuning_zh-CN.png)

@en ### Tuning Methods
@zh ### 调音方法

@en * **No Tuning**
@zh * **不调音**
@en   * No Pitch Effect.
@zh   * 无变调效果。
@en * **Pitch Shift Audio Effect**
@zh * **移调音效插件**
@en   * Uses the "Pitch Shift" Plugin (default for Vegas 13–15).
@zh   * 使用“移调”插件（Vegas 13~15的默认值）。
@en * **Elastic Pitch Effect**
@zh * **弹性音调更改**
@en   * Uses the Elastic Efficient Method (by default).
@zh   * 使用弹性高效方法（默认值）。
@en * **Classic Pitch Effect**
@zh * **古典音调更改**
@en   * Uses the Classic Stretch Method.
@zh   * 使用古典拉伸方法。
@en * **Scaleless Tuning**
@zh * **无音阶调音**
@en   * Locks the Stretch and Pitch.
@zh   * 锁定伸缩与音调。
@en * **Granular Oscillator**
@zh * **粒子振荡器**
@en   * Makes the sample super short and plays it, generating a pulse sequence corresponding to the pitch while creating a continuous pitch by exploiting the perception fusion effect of fast pulses.
@zh   * 使采样变成超短并播放，产生与音高相对应的脉冲序列，同时利用快速脉冲的感知融合效应创建连续的音调。

@en ### If the pitch exceeds the range
@zh ### 如果音高超出了音域

@en * **Switch to pitch shift plugin**
@zh * **切换到移调音效插件**
@en   * Repeatedly uses the pitch shift plugin for exceeding limits.
@zh   * 重复使用“移调”插件来超出限制。
    ::: important
@en     Out of range notes will slow down the generation.
@zh     超出音域的音符会减慢生成。
    :::
@en * **Raise / Lower octaves**
@zh * **升 / 降八度**
@en   * Shifts by octaves to keep the note inside ±24 semitones (safest option).
@zh   * 平移八度以使音符保持在 ±24 个半音之内（最安全的选项）。
@en * **Raise / Lower octaves (Experimental)**
@zh * **升 / 降八度（实验性）**
@en   * Allows a much larger range, up to roughly $\pm\frac{12}{\lg{2}}\ \left(\approx\pm39.863137\right)$ semitones.
@zh   * 允许一个非常大的范围，约高达 $\pm\frac{12}{\lg{2}}\ \left(\approx\pm39.863137\right)$ 个半音。
    ::: info
@en     If "Lock Stretch to Pitch" is on, it can go as low as $-\frac{12}{\log_{20}{2}} \left(\approx-51.863137\right)$ semitones (upper limit stays +24).
@zh     如果“锁定伸缩与音调”已开启，则可低至 $-\frac{12}{\log_{20}{2}} \left(\approx-51.863137\right)$ 个半音（上限仍为 +24）。
    :::
    ::: danger
@en     _Use this at your risk! This method can CRASH Vegas Pro._
@zh     _使用它需要你承担风险！此方法可能会使Vegas Pro崩溃。_
    :::
@en * **Dock at Top / Bottom**
@zh * **停靠在边缘**
@en   * Forces the note to the highest or lowest pitch still inside the normal range.
@zh   * 将音符强制调整至仍在正常范围内的最高或最低音高。
@en * **Silent**
@zh * **不发声**
@en   * Mutes notes that are out of range.
@zh   * 将超出音域的音符静音。

@en ### Lock Attributes
@zh ### 锁定属性

@en * **Lock Stretch and Pitch**
@zh * **锁定伸缩与音调**
@en   * Adjust the stretch to change the pitch (Resample).
@zh   * 调整伸缩以改变音调（重采样）。
@en * **Reserve Formant**
@zh * **保持共振峰**
@en   * Locks Formant, works only with "Mono" and "Professional"
@zh   * 锁定共振峰，仅限“单声道”和“专业”有效。
    ::: info
@en     *This locks formant so you get that "monophonic" effect.*
@zh     *这锁定了共振峰因此你可以得到所谓的“单声道”效果。*
    :::
@en * **Vocal Fry**
@zh * **气泡音**
@en   * Forces the sample to be 1 tick long.
@zh   * 强制采样1刻长度。
    ::: info
@en     *Only usable with Granular Oscillator.*
@zh     *仅在粒子振荡器下使用。*
    :::

@en ### Base Pitch
@zh ### 原始音高

@en _Controls the base pitch for previewing and adjusting the Audio Track Event._
@zh _控制用于预览和调整音频轨道事件的基本音高。_

@en * **Note Name**
@zh * **音名**
@en   * Controls the note name used for the base pitch.
@zh   * 控制基音所用的音名。
@en * **Octave**
@zh * **八度**
@en   * Controls the note octave used for the base pitch.
@zh   * 控制基音所用的八度。

@en ### Preview
@zh ### 预听

@en * **Preview the Base Pitch**
@zh * **预听标准音高**
@en   * Previews the base pitch.
@zh   * 预听标准音高。
@en * **Preview Audio**
@zh * **预听音频**
@en   * Previews the track event audio.
@zh   * 预听轨道事件音频。

@en ### Preview Attributes
@zh ### 预听属性

@en _Tune generating methods for previewing the base pitch._
@zh _用于预听主音高的音调生成方法。_

::: important
@en *`NAudio` is the default method and is preferred.*
@zh *`NAudio` 是默认首选方法。*
:::

@en * **Adjust Audio to Base Pitch**
@zh * **调整音频到主音高**
@en   * Adjusts the clip track event's audio to the base pitch.
@zh   * 调整剪辑轨道事件的音频到主音高。
  ::: info
@en   *This is useful for older styles of remixes.*
@zh   *这适用于旧式调音。*
  :::

@en ## Parameters
@zh ## 参数

@en ![Parameters](/img/v4/audio/parameters.png)
@zh ![参数](/img/v4/audio/parameters_zh-CN.png)

@en * **Set Fade by Percent**
@zh * **设定淡化为百分比**
@en   * Sets the Value Measure to Percent Value.
@zh   * 将值度量设置为百分比值。
@en * **Set Fade by Timecode**
@zh * **设定淡化为时间码**
@en   * Sets the Value Measure to Timecode Value.
@zh   * 将值度量设置为时间码值。

@en ### Fade
@zh ### 淡化

@en #### Fade in
@zh #### 渐入
@en _Length and Fade Type of the Audio Fade in on the Track Event._
@zh _轨道事件中音频渐入的长度和渐入类型。_
@en #### Fade out
@zh #### 渐出
@en _Length and Fade Type of the Audio Fade out on the Track Event._
@zh _轨道事件中音频渐出的长度和渐出类型。_
