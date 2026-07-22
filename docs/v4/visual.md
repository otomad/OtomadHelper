@en # Visual
@zh # 画面

@en > Also known as **Video** or **PV**.
@zh > 也被称为：**视频**、**视觉**、**映像**、**PV**。

@en **This setting can be toggled.** (If your clip has visuals, this is enabled by default.)
@zh **该设置可开关。**（如果你的片段有画面，那么它将默认开启。）
@en **You can right-click or middle-click any setting slider to reset it.**
@zh **你可以右键或中键单击任意设置滑动条来重置它。**

@en ## Toggles
@zh ## 开关

@en ![Toggles](/img/v4/visual/toggles.png)
@zh ![开关](/img/v4/visual/toggles_zh-CN.png)

@en ### Loop
@zh ### 循环

@en _Makes the clip loop if applicable._
@zh _如果适用的话，使剪辑循环。_

@en ### Static Visual
@zh ### 静态画面

@en _Makes the clip freeze at the start of the clip._
@zh _使剪辑冻结在剪辑的开始处。_

::: important
@en *This gives you a static visual.*
@zh *这会呈现一个静态画面。*
:::

@en ### Truncate
@zh ### 截断

@en _Makes the clip freeze at the end of the clip._
@zh _使剪辑冻结在简介的末尾处。_

::: warning
@en *Not at the end of the note.*
@zh *不是在音符的末尾处。*
:::

@en ### Multitrack for Chords
@zh ### 复音多轨

@en _Generates multiple video tracks for visuals._
@zh _为画面生成多条视频轨道。_

::: info
@en *This is very useful for making chord visuals easier.*
@zh *这对于轻松制作和弦画面非常有用。*
:::

@en ### Create Groups
@zh ### 创建分组

@en _Groups the video clips and the related audio clips together._
@zh _将视频剪辑及其关联的音频剪辑组合在一起。_

::: info
@en *This is useful when used with [Multitrack for Chords](#multitrack-for-chords).*
@zh *这适用于与[复音多轨](#复音多轨)协同使用。*
:::

@en ### Glissando
@zh ### 滑音效果

@en _Creates a Swirl Effect if the note pitch bends or slides._
@zh _当音符音高弯音或滑音时产生漩涡效果。_

@en ### Stack
@zh ### 堆叠

@en _Creates stacked video clips on separate tracks when the MIDI contains multiple tracks._
@zh _当MIDI包含多个轨道时，在单独的轨道上创建堆叠视频剪辑。_

::: important
@en *Only works when multiple MIDI tracks are selected.*
@zh *仅在选择MIDI多音轨时有作用。*
:::

@en ### Time Unremapping
@zh ### 持续时间流

@en _Disables all forms of time mapping of the clip! The start time will NOT reset as it will continue the clip from where it left off every note._
@zh _禁用剪辑的所有形式的时间重映射！起始时间不会重置，而是将从每个音符停止的位置继续。_

@en ### Stretch
@zh ### 拉伸

@en _Makes the clip stretch if applicable_
@zh _Makes the clip stretch if applicable_

@en * **None**
@zh * **不拉伸**
@en   * Applies no stretching to the clip.
@zh   * 不为剪辑应用拉伸。
@en * **Flex and Extend**
@zh * **可屈伸**
@en   * Stretches the clip completely.
@zh   * 完全地拉伸剪辑。
@en * **Extending Only**
@zh * **仅伸展**
@en   * Stretches clip out.
@zh   * 使剪辑拉伸得更长。
    ::: important
@en     *This works best with MIDI Notes that are longer than the Visual clip.*
@zh     *这对于MIDI音符比画面剪辑更长时效果最佳。*
    :::
@en * **Flexing Only**
@zh * **仅屈折**
@en   * Stretches clip in.
@zh   * 使剪辑拉伸得更短。
    ::: important
@en     *This works best with MIDI Notes that are shorter than the Visual clip.*
@zh     *这对于MIDI音符比画面剪辑更短时效果最佳。*
    :::

@en ### Legato
@zh ### 填补间隙

@en _Extends the clip beyond the note's duration._
@zh _将剪辑延长到音符持续时间之外。_

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

@en ### Preferred Track
@zh ### 首选轨道

@en _Creates a new track(s) or uses the track selected for generation._
@zh _创建新轨道或使用所选轨道来生成。_

@en ## Velocity
@zh ## 力度

@en _Settings to assign opacity values relative to the MIDI notes._
@zh _用于分配与MIDI音符相关的不透明度值的设置。_

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
@en * **Opacity**
@zh * **不透明度**
@en   * The Value of Opacity from Minimum to Maximum.
@zh   * 不透明度值从最小值到最大值。

::: warning
@en *Do not use the same value in both minimum and maximum values for Velocity and Opacity.*
@zh *不要在力度和不透明度的最小值和最大值中使用相同的值。*
:::

@en * **Reset**
@zh * **重置**
@en   * Resets the settings to default.
@zh   * 重置为默认设置。

@en [*Mapping Velocity Explanation*](./faq.md#mapping-velocity-explanation){.vp-external-link-icon}
@zh [*映射力度解释*](./faq.md#映射力度解释){.vp-external-link-icon}

@en ## Effects
@zh ## 效果

@en ![Effects](/img/v4/visual/effects.png)
@zh ![效果](/img/v4/visual/effects_zh-CN.png)

@en ### Visual Effects
@zh ### 视觉效果

@en _Make your visuals whatever you want. The default is *Horizontal Flip*._
@zh _做出你想要的视觉效果。默认值是*水平翻转*。_

@en ### Initial Step
@zh ### 初始步

@en _The Value Setting for the visuals._
@zh _视觉效果的值设置。_

::: important
@en *This will differ based on the visual you selected.*
@zh *这将根据你所选视觉效果而有所不同。*
:::

@en ### Advanced - PV Rhythm & Cadence Dynamic Pulsing Visual Effect
@zh ### 高级 - 画面节奏韵律动感跳动视觉效果

@en ![Advanced](/img/v4/visual/prve.png){.shadow-less}
@zh ![高级](/img/v4/visual/prve_zh-CN.png){.shadow-less}

@en * **Flip Class**
@zh * **翻转类**
@en   * Selects a Visual type from the *Flip Class*.
@zh   * 从*翻转类*中选择视觉效果类型。
@en * **Rotation Class**
@zh * **旋转类**
@en   * Selects a Visual type from the *Rotation Class*.
@zh   * 从*旋转类*中选择视觉效果类型。
@en * **Scale Class**
@zh * **缩放类**
@en   * Selects a Visual type from the *Scale Class*.
@zh   * 从*缩放类*中选择视觉效果类型。
@en * **Mirror Class**
@zh * **镜像类**
@en   * Selects a Visual type from the *Mirror Class*.
@zh   * 从*镜像类*中选择视觉效果类型。
@en * **Invert Class**
@zh * **反转类**
@en   * Selects a Visual type from the *Invert Class*.
@zh   * 从*反转类*中选择视觉效果类型。
@en * **Hue Class**
@zh * **色相类**
@en   * Selects a Visual type from the *Hue Class*.
@zh   * 从*色相类*中选择视觉效果类型。
@en * **Monochrome Class**
@zh * **单色类**
@en   * Selects a Visual type from the *Monochrome Class*.
@zh   * 从*单色类*中选择视觉效果类型。
@en * **Time Class**
@zh * **时间类**
@en   * Selects a Visual type from the *Time Class*.
@zh   * 从*时间类*中选择视觉效果类型。
@en * **Time Class 2**
@zh * **时间类2**
@en   * Selects a Visual type from the *Time Class 2*.
@zh   * 从*时间类2*中选择视觉效果类型。
@en * **Expansion & Compression Class**
@zh * **扩缩类**
@en   * Selects a Visual type from the *Expansion & Compression Class*.
@zh   * 从*扩缩类*中选择视觉效果类型。
@en * **Swing Class**
@zh * **摇摆类**
@en   * Selects a Visual type from the *Swing Class*.
@zh   * 从*摇摆类*中选择视觉效果类型。
@en * **Blur Class**
@zh * **模糊类**
@en   * Selects a Visual type from the *Blur Class*.
@zh   * 从*模糊类*中选择视觉效果类型。
@en * **Wipe Class**
@zh * **擦除类**
@en   * Selects a Visual type from the *Wipe Class*.
@zh   * 从*擦除类*中选择视觉效果类型。

@en ## Parameters
@zh ## 参数

@en _Various Effects for the visual generation._
@zh _画面生成的各种效果。_

@en ![Parameters](/img/v4/visual/parameters.png)
@zh ![参数](/img/v4/visual/parameters_zh-CN.png)

@en ### Presets
@zh ### 预设

@en _Premade effect visuals for your convenience._
@zh _为方便使用而预定制的视觉效果。_

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
@en _Length of the Video Fade in on the Track Event._
@zh _轨道事件中视频渐入的长度。_
@en #### Fade out
@zh #### 渐出
@en _Length of the Video Fade out on the Track Event._
@zh _轨道事件中视频渐出的长度。_

@en ### Glow
@zh ### 发光
@en #### Glow
@zh #### 发光
@en _Value of the "Glow" Effect that adds a glow to your visual!_
@zh _“发光”效果的值可为你的画面增添光彩！_
@en #### Glow Brightness
@zh #### 发光亮度
@en _Value of the Glow Brightness in your visual._
@zh _画面中发光亮度的值。_

@en ### Constrain keyframes length
@zh ### 限制关键帧长度

@en _Specifies the length of keyframes to generate for the following properties._
@zh _指定为以下属性生成的关键帧的长度。_

@en * **Unconstrained**
@zh * **不限制**
@en   * Keyframes will generate normally from start to end of the MIDI note.
@zh   * 关键帧将从MIDI音符的开始到结束正常生成。
@en * **Min length**
@zh * **最小长度**
@en   * Keyframes will never be shorter than the set length. If the note is shorter, they get truncated.
@zh   * 关键帧永远不会短于设置的长度。如果音符较短，它们会被截断。
@en * **Fixed length**
@zh * **固定长度**
@en   * Keyframes are forced to exactly the set length.
@zh   * 关键帧被强制设为设定的长度。

@en ### Transform
@zh ### 变换
@en #### Start Size
@zh #### 起始尺寸
@en _Value of the Starting Size for Pan/Crop._
@zh _平移/裁切的起始尺寸值。_
::: info
@en *Anything over 100% will reduce the video clip size.*
@zh *任何超过100%的值都会减小视频剪辑的大小。*
:::
@en #### End Size
@zh #### 终止尺寸
@en _Value of the Ending Size for Pan/Crop._
@zh _平移/裁切的终止尺寸值。_
::: info
@en *Anything over 100% will reduce the video clip size.*
@zh *任何超过100%的值都会减小视频剪辑的大小。*
:::
@en #### Start Rotation
@zh #### 起始旋转
@en _Value of the Starting Rotation for Pan/Crop._
@zh _平移/裁切的起始旋转值。_
@en #### End Rotation
@zh #### 终止旋转
@en _Value of the Ending Rotation for Pan/Crop._
@zh _平移/裁切的终止旋转值。_
@en #### Start X Shift
@zh #### 起始平移
@en _Value of the Starting X Shift Position for Pan/Crop._
@zh _平移/裁切的起始水平位置值。_
::: important
@en *This will make the visual position shift horizontally, depending on the other parameters.*
@zh *这将使画面水平移动，具体取决于其它参数。*
:::
@en #### End X Shift
@zh #### 终止平移
@en _Value of the Ending X Shift Position for Pan/Crop._
@zh _平移/裁切的终止水平位置值。_
::: important
@en *This will make the visual position shift horizontally, depending on the other parameters.*
@zh *这将使画面水平移动，具体取决于其它参数。*
:::
@en #### Start Y Shift
@zh #### 起始直移
@en _Value of the Starting Y Shift Position for Pan/Crop._
@zh _平移/裁切的起始垂直位置值。_
::: important
@en *This will make the visual position shift vertically, depending on the other parameters.*
@zh *这将使画面垂直移动，具体取决于其它参数。*
:::
@en #### End Y Shift
@zh #### 终止直移
@en _Value of the Ending Y Shift Position for Pan/Crop._
@zh _平移/裁切的终止垂直位置值。_
::: important
@en *This will make the visual position shift vertically, depending on the other parameters.*
@zh *这将使画面垂直移动，具体取决于其它参数。*
:::

@en ### Color Grading
@zh ### 调色
@en #### Start Hue
@zh #### 起始色相
@en _Value of the Starting Hue._
@zh _起始色相的值。_
@en #### End Hue
@zh #### 终止色相
@en _Value of the Ending Hue._
@zh _终止色相的值。_
@en #### Start Saturation
@zh #### 起始饱和
@en _Value of the Starting Saturation._
@zh _起始饱和度的值。_
@en #### End Saturation
@zh #### 终止饱和
@en _Value of the Ending Saturation._
@zh _终止饱和度的值。_
@en #### Start Contrast
@zh #### 起始对比
@en _Value of the Starting Contrast._
@zh _起始对比度的值。_
@en #### End Contrast
@zh #### 终止对比
@en _Value of the Ending Contrast._
@zh _终止对比度的值。_
@en #### Start Threshold
@zh #### 起始阈值
@en _Value of the Starting Contrast Threshold._
@zh _起始对比度阈值的值。_
::: important
@en *This makes the contrast's depth higher or lower.*
@zh *这使得对比度的深度更高或更低。*
:::
@en #### End Threshold
@zh #### 终止阈值
@en _Value of the Ending Contrast Threshold._
@zh _终止对比度阈值的值。_
::: important
@en *This makes the contrast's depth higher or lower.*
@zh *这使得对比度的深度更高或更低。*
:::
