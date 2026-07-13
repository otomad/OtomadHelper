@en # Staff Visualizer
@zh # 五线谱可视化

@en _**Staff Visualizer is for creating Sheet style visuals in a similar fashion to Piano Sheets.**_
@zh _**五线谱可视化用于以与钢琴乐谱类似的方式创建乐谱风格的视觉效果。**_

@en **This setting can be toggled.** (This is disabled by default.)
@zh **该设置可开关。**（默认关闭。）
::: important
@en *Requires [Visual](./visual.md) to be enabled.*
@zh *需要先启用[画面](./visual.md)。*
:::

@en ## Notes Parameters
@zh ## 音符参数

@en ![Notes Parameters](/img/v4/staff/notes.png)
@zh ![音符参数](/img/v4/staff/notes_zh-CN.png)

@en - **Using Relative Values**
@zh - **使用相对值**
@en   * Uses Values relative to 1920×1080 size.
@zh   * 使用相对于1920×1080尺寸的值。
    ::: important
@en     *If off, it will use values based on your project size.*
@zh     *关闭后，将基于你项目尺寸的值。*
    :::
@en - **Legacy Positioning Method**
@zh - **旧版定位方式**
@en   * Uses the old method of placing visuals, uses Track Motion instead for the Notes and Lines.
@zh   * 使用旧方法放置画面，使用轨道运动表示音符和谱线。
@en - **Freeze at note off**
@zh - **冻结在音符结尾处**
@en   * Freezes the note upon MIDI note's end.
@zh   * 冻结MIDI音符末尾的音符。
@en - **Lengthen to bar end**
@zh - **持续到小节结尾**
@en   * Extends the clip to the end of the measure.
@zh   * 将剪辑延伸到小节的末尾。

---

@en * **Clef**
@zh * **谱号**
@en   * Controls what the Clef is.
@zh   * 控制使用什么谱号。
@@@en
  ::: info
  *Treble is for Higher Sounding Notes, Bass is for Lower Sounding Notes.*
  :::
@@@zh
@@@
@en * **Line Gap**
@zh * **谱线间距**
@en   * Controls the Pixel Gap for the Visuals and the Sheet.
@zh   * 控制画面和谱表之间的像素间隙。
@en * **Padding Left**
@zh * **谱左边距**
@en   * Controls the Value of the Gap Distance from the Left.
@zh   * 控制与左侧的间隙距离的值。
@en * **Padding Right**
@zh * **谱右边距**
@en   * Controls the Value of the Gap Distance from the Right
@zh   * 控制与右侧的间隙距离的值。
@en * **Surface Position**
@zh * **谱面位置**
@en   * Controls the Value of the Gap Distance from the Middle Line.
@zh   * 控制距中线的间隙距离的值。
@en * **Notes Shift**
@zh * **音符偏移**
@en   * Shifts Notes in the Sheet based on the Value of the Key Change.
@zh   * 根据音调更改的值平移谱线中的音符。

@en ## Lines Parameters
@zh ## 谱线参数

@en ![Lines Parameters & Clef Parameters](/img/v4/staff/lines_clef.png)
@zh ![谱线参数和谱号参数](/img/v4/staff/lines_clef_zh-CN.png)

@en - **Generate Lines**
@zh - **生成谱线**
@en   * Generates the lines for the Staff.
@zh   * 生成五线谱的线条。
@en * **Line Thickness**
@zh * **谱线粗细**
@en   * Controls the size of the Line Thickness in the Staff Sheet.
@zh   * 控制五线谱表中线宽的大小。
@en * **Line Color**
@zh * **谱线颜色**
@en   * Controls the Line Color.
@zh   * 控制线条的颜色。

::: important
@en Applies when Generate Staff is On.
@zh 开启生成五线谱时才有效。
:::

@en ## Clef Parameters
@zh ## 谱号参数

@en - **Generate Clef**
@zh - **生成谱号**
@en   * Generates the Clef Symbol at the front beginning of the sheet.
@zh   * 在乐谱的开头生成谱号符号。
@en * **Clef Scale**
@zh * **谱号缩放**
@en   * Controls the Clef Symbol scale generation.
@zh   * 控制生成谱号符号的缩放。
@en * **Clef Color**
@zh * **谱号颜色**
@en   * Controls the Clef Symbol color.
@zh   * 控制谱号符号的颜色。
