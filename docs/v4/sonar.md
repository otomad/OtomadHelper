@en # Sonar
@zh # 声呐

@en _**Sonar *(Motion Graphics Generation Feature)* is for creating beat-style visuals for Drums.**_
@zh _**声呐*（动态图形生成功能）*用于为鼓组创建节拍风格的视觉效果。**_

<!-- Reference: [NOMA - Brain Power - LYRICS!](https://youtu.be/h-mUGj41hWA) -->

@en Reference:
@zh 参阅：
[<SocialIcon icon="youtube" />『図形素材』](https://www.youtube.com/watch?v=EbF-O3DpJDE){lang=ja}、[<SocialIcon icon="bilibili" />《图形练习》](https://www.bilibili.com/video/BV17f4y1y7sB/){lang=zh-CN}

@en **This setting can be toggled.**
@zh **该设置可开关。**
::: warning
@en **This requires a Drum Kit in the MIDI, you can’t use this with samples.**
@zh **这需要MIDI中有鼓组，你不能将其与采样一起使用。**
:::

@en ## Toggles
@zh ## 开关

@en ![Toggles](/img/v4/sonar/toggles.png)
@zh ![开关](/img/v4/sonar/toggles_zh-CN.png)

@en - **Separate Drums**
@zh - **分离不同鼓声**
@en   - Makes the drums’ visuals generate on multiple layers.
@zh   - 使鼓的视觉对象在多个图层上生成。
@en - **Different Composite Mode**
@zh - **差值轨道合成模式**
@en   - Makes the layers generated with difference compositing (blending).
@zh   - 通过差值合成（混合）生成图层。
@en - **Shadow**
@zh - **阴影**
@en   - Generates visuals with a shadow.
@zh   - 生成的视觉对象带有阴影。
@en - **Shadow Color**
@zh - **阴影颜色**
@en   - Changes the shadow color.
@zh   - 改变阴影的颜色。

@en ## Editor
@zh ## 编辑器

@en ![Command Bar](/img/v4/sonar/command_bar.png)
@zh ![命令栏](/img/v4/sonar/command_bar.png)

@en - **Reset**
@zh - **重置**
@en - **Delete (Selection)**
@zh - **删除（选区）**
@en - **Move Up (Selection)**
@zh - **上移（选区）**
@en - **Move Down (Selection)**
@zh - **下移（选区）**
@en - **Create New**
@zh - **新建**

@en ## Parameters
@zh ## 参数

@en ![Parameters](/img/v4/sonar/parameters.png)
@zh ![参数](/img/v4/sonar/parameters_zh-CN.png)

@en - **Matched Drum Sound**
@zh - **匹配鼓声**
@en   - The value of the drum sound used for the generation.
@zh   - 用于生成的鼓声的值。
    ::: important
@en     _This should match what the individual drum is in your MIDI._
@zh     _这应该与你的MIDI中的单个鼓相匹配。_
    :::
@en - **Shape**
@zh - **形状**
@en   - The shape of the visual.
@zh   - 视觉对象的形状。
@en - **Color**
@zh - **颜色**
@en   - Controls the color of the visual.
@zh   - 控制视觉对象的颜色。
@en - **Duration**
@zh - **时长**
@en   - The value of the duration for the visual.
@zh   - 视觉对象的时长值。
@en - **Curve**
@zh - **曲线**
@en   - The value of the keyframe type.
@zh   - 关键帧类型值。
@en - **Start Border**
@zh - **起始边框**
@en   - The value of the starting size of the border.
@zh   - 边框的起始尺寸值。
@en - **End Border**
@zh - **终止边框**
@en   - The value of the ending size of the border.
@zh   - 边框的终止尺寸值。
@en - **Start Size**
@zh - **起始大小**
@en   - The value of the starting overall size.
@zh   - 起始总体大小值。
@en - **End Size**
@zh - **终止大小**
@en   - The value of the ending overall size.
@zh   - 终止总体大小值。

::: important
@en _The center is where the visual will end, this will affect all other parameters._
@zh _中心是视觉对象最终的地方，这将影响所有其它参数。_
:::

@en - **X Center**
@zh - **水平中心**
@en   - The value of the center horizontal position.
@zh   - 水平中心位置的值。
@en - **Y Center**
@zh - **垂直中心**
@en   - The value of the center vertical position.
@zh   - 垂直中心位置的值。

::: important
@en _These values are the first values to be used for keyframing._
@zh _这些值是用于关键帧的第一个值。_
:::

@en - **Start X Offset Odd**
@zh - **起始水平偏移单数**
@en   - The value of the X starting position for every odd visual.
@zh   - 每个奇数视觉对象的水平起始位置的值。
@en - **Start Y Offset Odd**
@zh - **起始垂直偏移单数**
@en   - The value of the Y starting position for every odd visual.
@zh   - 每个奇数视觉对象的垂直起始位置的值。
@en - **Start X Offset Even**
@zh - **起始水平偏移双数**
@en   - The value of the X starting position for every even visual.
@zh   - 每个偶数视觉对象的水平起始位置的值。
@en - **Start Y Offset Even**
@zh - **起始垂直偏移双数**
@en   - The value of the Y starting position for every even visual.
@zh   - 每个偶数视觉对象的垂直起始位置的值。

::: important
@en _These values are the additional values to be used for keyframing, which creates interpolation._
@zh _这些值是用于创建插值的关键帧的附加值。_
:::

@en - **Prestart X Offset Odd**
@zh - **预先水平偏移单数**
@en   - Additional value of the X starting position for every odd visual.
@zh   - 每个奇数视觉对象的水平起始位置的附加值。
@en - **Prestart Y Offset Odd**
@zh - **预先垂直偏移单数**
@en   - Additional value of the Y starting position for every odd visual.
@zh   - 每个奇数视觉对象的垂直起始位置的附加值。
@en - **Prestart X Offset Even**
@zh - **预先水平偏移双数**
@en   - Additional value of the X starting position for every even visual.
@zh   - 每个偶数视觉对象的水平起始位置的附加值。
@en - **Prestart Y Offset Even**
@zh - **预先垂直偏移双数**
@en   - Additional value of the Y starting position for every even visual.
@zh   - 每个偶数视觉对象的垂直起始位置的附加值。

---

@en - **Start Rotation Odd**
@zh - **起始旋转单数**
@en   - The value of the starting rotation for every odd visual.
@zh   - 每个奇数视觉对象的起始旋转值。
@en - **Start Rotation Even**
@zh - **起始旋转双数**
@en   - The value of the starting rotation for every even visual.
@zh   - 每个偶数视觉对象的起始旋转值。
@en - **Fade In**
@zh - **渐入**
@en   - The value of the fade in.
@zh   - 渐入值。
@en - **Fade Out**
@zh - **渐出**
@en   - The value of the fade out.
@zh   - 渐出值。
@en - **Fade In Curve**
@zh - **渐入曲线**
@en   - The keyframe type of the fade in.
@zh   - 渐入值的关键帧类型。
@en - **Fade Out Curve**
@zh - **渐出曲线**
@en   - The keyframe type of the fade out.
@zh   - 渐出值的关键帧类型。
