@en # Score
@zh # 乐曲

@en ## MIDI Configuration
@zh ## MIDI配置
@en *(THIS IS IMPORTANT)*
@zh *（这很重要）*

@en ![Configuration](/img/v4/score/configuration.png)
@zh ![配置](/img/v4/score/configuration_zh-CN.png)

@en * Requires a MIDI File to make the YTPMV/otoMAD.
@zh * 生成音MAD需要一个MIDI文件。
@en * You must select a MIDI file from your computer for the script to generate.
@zh * 你必须要从计算机选择一个MIDI文件以使脚本生成。

::: info
@en * This also works with other DAWs that support MIDI exporting.
@zh * 这也适用于其它支持MIDI导出的DAW*（数字音频工作站）*。
@en * In FL Studio, make a copy of your project before you convert it to MIDI.
@zh * 在FL Studio中，先将你的项目创建副本，然后再将其转换为MIDI。
:::

@en ### Start and End Time
@zh ### 起始和终止时间

@en _Controls the range where the generation of MIDI starts and ends._
@zh _控制MIDI生成开始到结尾的范围。_

::: tip
@en *If the start has been set at `0:05.000`. The MIDI would generate all notes from the 5th second, and the clips will be generated from the 5th second after where ["generate at"](./source.md#generate-at).*
@zh *如果起始时间设置为 `0:05.000`，则MIDI将从第5秒后生成所有音符，并且剪辑也将从[“生成开始位置”](./source.md#设定生成开始位置)之后的第5秒开始生成。*
:::
::: info
@en *This will still generate the visuals at the respected note time placements.*
@zh *这仍然会在相应的音符时间位置生成画面。*
:::

@en ### BPM Tempo
@zh ### 设定BPM速度为

@en _Controls the BPM the MIDI generates._
@zh _控制MIDI生成的BPM*（拍每分）*。_

@en * **MIDI Tempo**
@zh * **MIDI速度**
@en   * Uses the MIDI's Tempo.
@zh   * 使用MIDI的速度。
@en * **Project Tempo**
@zh * **Project Tempo**
@en   * Uses the current Vegas Project's Tempo.
@zh   * 使用当前Vegas项目的速度。
@en * **Custom**
@zh * **自定义**
@en   * Uses the Custom Setting.
@zh   * 使用自定义设置。

@en **If the MIDI has changing tempos, the script will adapt and let you select what options you want to do with them.**
@zh **如果MIDI的速度会发生变化，脚本将进行自适应并让你选择要对其执行的选项。**

@en * **Variable MIDI tempo**
@zh * **可变MIDI速度**
@en   * _Uses the changing tempo._
@zh   * _使用变化的速度。_
@en   * **Hold**
@zh   * **保持**
@en     * Notes Generated will stay held to their BPM.
@zh     * 生成的音符将保持在其BPM中。
@en   * **Linear**
@zh   * **线性**
@en     * Notes Generated will be based linearly on their BPM.
@zh     * 生成的音符将线性地基于其BPM。
@en * **MIDI Tempo**
@zh * **MIDI速度**
@en   * _Uses the constant first tempo._
@zh   * _使用固定的第一个速度。_

@en ### Auto Change Project Ruler Properties
@zh ### 自动更改项目标尺属性

@en _Adjusts the project's measure properties based on the MIDI._
@zh _根据MIDI调整项目的小节属性。_

@en * **Tempo**
@zh * **速度**
@en   * Uses tempo as the setting.
@zh   * 使用速度作为设置。
@en * **Time signature**
@zh * **拍号**
@en   * Uses time signature as the setting.
@zh   * 使用拍号作为设置。

@en - **Apply Now**
@zh - **立即应用**
@en   * Automatically applies the MIDI's tempo and time signature without generating the MIDI events.
@zh   * 自动应用MIDI的速度和拍号，无需生成MIDI事件。

::: info
@en Unfortunately, you can not use MIDIs with changing tempos or time signatures, only the first tempo/time signature will be used. Use [VariableBPM](https://github.com/zzzzzz9125/VariableBPM) for variable tempo.
@zh 很遗憾，你无法使用具有变化速度或拍号的MIDI，而是仅使用第一个速度/拍号。使用 [VariableBPM](https://github.com/zzzzzz9125/VariableBPM) 实现可变速度。
:::

@en ### Restrict Note Length
@zh ### 限制音符长度

@en _Controls the Note Output Length from the MIDI._
@zh _控制MIDI的音符输出长度。_

@en * **Unconstrained**
@zh * **不限制**
@en   * No note output change.
@zh   * 没有音符改变输出。
@en * **Max Length**
@zh * **最大长度**
@en   * Notes will be restricted to the length provided.
@zh   * 音符将限制于所提供的长度。
@en * **Fixed Length**
@zh * **固定长度**
@en   * Notes will attempt to generate to the length provided.
@zh   * 音符将尝试生成所提供的长度。

@en ::: warning 
@zh ::: warning 陷阱
@en _*This may conflict with [Legato](./audio.md#legato) even if it is set to "Unlimited" in Audio/Visual Settings.*_
@zh _*当音频/画面设置中的[填补间隙](./audio.md#填补间隙)设置为“无限填补”时则可能和本设置冲突。*_
:::

@en ## Use MIDI track
@zh ## 使用MIDI音轨

@en ![Track List View](/img/v4/score/track_list_view.png)
@zh ![音轨列表视图](/img/v4/score/track_list_view_zh-CN.png)

@en _This allows you to select what tracks you want to generate from your MIDI file._
@zh _这允许你选择要从 MIDI 文件生成的轨道。_

@en ### Commands
@zh ### 命令

@en * **Select all**
@zh * **全选**
@en   * Selects all tracks.
@zh   * 选中全部音轨。
@en * **Invert Selection**
@zh * **反选**
@en   * Inverts your selection.
@zh   * 反转你的选区。
@en * **Single**
@zh * **单选**
@en   * Toggles selecting a single track.
@zh   * 切换选择单条音轨。
@en * **Multi**
@zh * **多选**
@en   * Toggles selecting multiple tracks.
@zh   * 切换选择多条音轨。

::: important
@en *Starting Pan is shown, and the letter attributes go as follows:*
@zh *起始声像已显示，缩写如下：*

@en Letter Attributes | Starting Pan
@zh 缩写 | 起始声像
--- | ---
@en L | Left
@zh 左 | 左声道
@en R | Right
@zh 右 | 右声道
@en C | Center
@zh 中 | 中声道
@en V | Variable
@zh 起 | 起始的可变声像
:::

@en ## Auto Layout Tracks
@zh ## 自动布局轨道

@en ![Auto Layout Tracks](/img/v4/score/auto_layout_tracks.png)
@zh ![自动布局轨道](/img/v4/score/auto_layout_tracks_zh-CN.png)

@en [Refer to Tools](./tools.md#auto-layout-tracks){.vp-external-link-icon}
@zh [参阅工具](./tools.md#自动布局轨道){.vp-external-link-icon}
