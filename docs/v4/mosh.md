@en # Datamoshes
@zh # 数据抹失

@@@en
@@@zh
> *Datamosh*
> 也被称为：[資料狂舞]{lang=zh-TW}、毒蘑菇、幻术故障。
@@@

@en ![Clips Folder](/img/v4/moshes/clips_folder.png)
@zh ![片段目录](/img/v4/moshes/clips_folder_zh-CN.png)

@en **<GlitchyText normal="Datamosh" glitchy="Dataḁ̸̬͋ă̷͍ȧ̴͇͘͝ͅá̷̖̲͑ã̶̺̈́å̷͉̖͊̚ä̷̬̬â̷̡̱̑ Mo̴̞̜̯͋ȯ̴̪̏͂õ̷̧͚ͅȏ̸͇ö̵̟̻̹͌o̶͚͍̻̕ǒ̸̢͎̄o̵͔̘̓͘o̶͇̐o̸̬͒͗̎sh" /> is a technique of damaging clips to create glitchy effects which frames that need to progress, don't progress!**
@zh **<GlitchyText normal="数据抹失" glitchy="锘挎薮琚沬妷" />是一种通过磨损素材以产生故障效果的技术，其中需要进展的帧却无法进展！**
@en OH's Datamosh was made possible with the use of [Vegas-Datamosh by delthas](https://github.com/delthas/vegas-datamosh)
@zh 噢！数据抹失功能得益于[delthas开发的Vegas-Datamosh](https://github.com/delthas/vegas-datamosh)工具。

@en ::: info Note
@zh ::: info 注释
@en * **I-frame:** Intra Frame
@zh * **Ｉ帧：**帧内帧
@en * **P-frame:** Forward Predicted Frame
@zh * **Ｐ帧：**预测帧
@en * **B-frame:** Bi-Directional Predictive Frame
@zh * **Ｂ帧：**双向帧
:::

@en ### Datamosh
@zh ### 数据抹失

@en _Datamoshes the video with determined settings._
@zh _使用确定的设置对视频进行*datamosh*。_

@en ![Datamosh](/img/v4/moshes/datamosh.png){.shadow-less}
@zh ![数据抹失](/img/v4/moshes/datamosh_zh-CN.png){.shadow-less}

@en * **Frame Count**
@zh * **帧计数**
@en   * Amount of frames to use.
@zh   * 要使用的帧数。
@en * **Frame Repeat**
@zh * **帧重复**
@en   * Amount of times the frames will repeat.
@zh   * 帧重复的次数。

@en This repeats N P-frames M times. N is the `Frame count`, M is the `Frame repeats`.
@zh 这会重复N个P帧M次。N是`帧计数`，M是`帧重复`。

@en To use, make a selection in the timeline using [[I]] and [[O]], then it will be rendered and datamoshed and added to your project, all in one click. It can take quite some time for long selections so wait if Vegas seems to freeze.
@zh 要使用，请使用 [[I]] 和 [[O]] 在时间轴中选区，然后它将被预渲染和*datamosh*，并添加到你的项目中，所有操作只需单击一次。如果Vegas看起来卡住了，那么可能需要较长的时间来处理。

@en The start of the selection is the first P-frame that will be repeated. The P-frame will be relative to the previous frame, which will be rendered as an I-frame. So for best results, the start of the selection should be a frame with a lot of movement.
@zh 选区的起始是第一个将被重复的P帧。这个P帧将是相对于前一帧的，而前一帧将被渲染I帧。因此为了获得最佳效果，选区的开始应该是一个运动幅度大的帧。

@en The end of the selection simply tells the script until which frame it should render the datamoshed file. The longer the selection, the longer the render time.
@zh 选区的末尾只是告诉脚本应该将*datamosh*文件渲染到哪一帧。所选时间越长，渲染时间就越长。

@en ### Datamix
@zh ### 数据抹拭

@en _Datamoshes the first frame of the selection._
@zh _*datamosh*选区的第一帧。_

::: important
@en Requires the selection to be set equal or greater than frame 1.
@zh 要求将选择设置为等于或大于第1帧。
:::

@en This replaces the first I-frames of a clip by a frame of another clip.
@zh 这将用另一个片段的一帧来替换一个片段的第一个I帧。

@en To use, make a selection in the timeline using [[I]] and [[O]], then it will be rendered and datamixed and added to your project, all in one click. It can take quite some time for long selections so wait if Vegas seems to freeze.
@zh 要使用，请使用 [[I]] 和 [[O]] 在时间轴中选区，然后它将被预渲染和*datamix*，并添加到你的项目中，所有操作只需单击一次。如果Vegas看起来卡住了，那么可能需要较长的时间来处理。

@en The start of the selection is the I-frame of the clip that will be replaced by another I-frame. *The image used for the new I-frame will be the frame just before the start of the selection.* So make sure to place the image you want to datamix from, just before the selection start.
@zh 所选的开始是剪辑的I帧，它将被另一个I帧替换。用于新I帧的图像将是选择开始前的帧。所以请确保将你要*datamix*的图像放在选择开始之前。

@en If you want to datamix on a scene change, you can typically split the clip at the exact frame where the scene changes, and select the right clip. This will datamix the right-hand side clip onto the last frame of the left-hand side clip.
@zh 如果你想在场景变化时进行*datamix*，通常可以在场景变化的那一帧分割剪辑，然后选择右边的剪辑。这会将右侧剪辑*datamix*到左侧剪辑的最后一帧。

@en The end of the selection simply tells the script until which frame it should render the datamoshed file. The longer the selection, the longer the render time.
@zh 选区的末尾只是告诉脚本应该将*datamosh*文件渲染到哪一帧。所选时间越长，渲染时间就越长。

@en ### Layer
@zh ### 多层叠化

@en _Layers' the selected clip for moshing._
@zh _将所选剪辑分层以进行*mosh*。_

@en ![Layer](/img/v4/moshes/layer.png){.shadow-less}
@zh ![多层叠化](/img/v4/moshes/layer_zh-CN.png){.shadow-less}

@en * **Layer count**
@zh * **层数**
@en   * Amount of layers to generate.
@zh   * 要生成的层数。
@en * **Layering offset**
@zh * **层叠偏移**
@en   * Offsets the layer by the number of frames specified.
@zh   * 将图层偏移指定的帧数。
@en * **Render**
@zh * **预渲染**
@en   * Renders the clip selected after generating layers.
@zh   * 渲染生成图层后所选剪辑。

@en This does multilayering, by copying the select video clip/event N times, each time offsetting the clip by M frames. N is the `Layer count`, M is the `Layering offset`. You can also choose to automatically render the multilayered clip by checking the `Render` hitbox, otherwise the copies clips will simply be added to the timeline.
@zh 这将通过复制所选视频剪辑/事件N次来执行*layering*，每次将剪辑偏移M帧。N是`层数`，M是`层叠偏移`。你还可以通过选中`预渲染`复选框来选择自动渲染*layering*剪辑，否则复制的剪辑只会添加到时间轴上。

@en To use, select a single video clip/event, then it will be multilayered automatically, and rendered transparently if specified.
@zh 使用时，请选择单个视频剪辑/事件，然后它将自动多层叠化，并在指定的情况下进行透明渲染。

@en *You can use a negative offset (eg -2 instead of 2), in which case the newest clips/events will be added at the back, instead of the front of the previous events.*
@zh *你可以使用一个负的偏移量（例如-2而不是2），在这种情况下，新的剪辑/事件将添加在后面，而不是在前面。*

@en **If you choose to render automatically, the rendered file will support alpha/transparency, meaning you don't need to add a green screen and remove it after, the alpha is handled automatically.**
@zh **如果选择自动渲染，渲染后的文件将支持阿尔法/透明度，这意味着你不需要添加绿幕并在之后删除它，阿尔法将自动处理。**

@en ### Render
@zh ### 预渲染化

@en _Renders the clip within a timeline selection... (Yes that's literally it.)_
@zh _在时间轴选区内渲染剪辑……（是的，就只是这样。）_

@en This simply renders a part of the timeline and places it into the timeline in a single click.
@zh 简便地渲染时间轴的一部分，并一键将其放入时间轴中。

@en To use, make a selection in the timeline using [[I]] and [[O]], then it will be rendered and added to your project, all in one click.
@zh 要使用，请使用 [[I]] 和 [[O]] 在时间轴中选区，然后它将被预渲染并添加到你的项目中，所有操作只需单击一次。

@en **The rendered file will support alpha/transparency, meaning you don't need to add a green screen and remove it after, the alpha is handled automatically.**
@zh **渲染后的文件将支持阿尔法/透明度，这意味着你不需要添加绿幕并在之后删除它，阿尔法将自动处理。**

@en ### Scramble
@zh ### 随机扰乱

@en _Scrambles the clip with random time placements dependent on the size._
@zh _根据大小使用随机时间位置对剪辑进行打乱。_

@en ![Scramble](/img/v4/moshes/scramble.png){.shadow-less}
@zh ![随机扰乱](/img/v4/moshes/scramble_zh-CN.png){.shadow-less}

@en * **Scramble Size**
@zh * **扰乱大小**
@en   * The value of the size for the clips to be generated.
@zh   * 要生成的剪辑的大小值。

@en This simply scrambles multiple clips/events, by cutting them in subclips of length N, and shuffling the subclips. N is the `Scramble size`, and is usually 1.
@zh 简便地通过将多个剪辑/事件分割为长度为N的子剪辑，并对子剪辑进行扰乱，即可对其进行扰乱。N是`扰乱大小`，通常为1。

@en To use, select several clips/events (they must be actually selected, not only in group-selected), then they will be scrambled automatically.
@zh 要使用，请选择几个剪辑/事件（它们必须被实际选中，而不仅仅是在所选组中），然后它们将被自动扰乱。

@en *Clips starting and ending at the same time will be scrambled together, ie their subclips will be shuffled the same way.*
@zh *在同一时间开始和结束的剪辑将被一起扰乱，即它们的子剪辑将以同样的方式被扰乱。*

@en ### Automator
@zh ### 自动乱调

@en _Automates and Randomizes the values of Visual effects in the clip._
@zh _自动化并随机化剪辑中视觉效果的值。_

@en ![Automator](/img/v4/moshes/automator.png){.shadow-less}
@zh ![自动乱调](/img/v4/moshes/automator_zh-CN.png){.shadow-less}

::: important
@en This requires Visual effects to have automation on to work.
@zh 这需要视觉效果自动化才能发挥作用。
:::

@en * **Scramble**
@zh * **扰乱**
@en   * *It will ask if you wish to automate the Visual effects in the clip, for each effect.*
@zh   * *它会询问你是否希望为每个效果自动执行剪辑中的视觉效果。*

@en This randomizes the video effects of selected clips/events, by adding random keyframes every frame for each parameter type you select.
@zh 这将通过为你所选的每个参数类型在每一帧添加随机关键帧，随机化选定剪辑/事件的视频效果。

@en To use, select several clips/events which have video effects on them (they must be actually selected, not only in group-selected), then start the script. For each type of video effect parameter on any of the clips, you will be prompted for whether you want the script to *scramble* the parameter (replace all the current keyframes of the parameter with random keyframes), or not (leave the keyframes as is).
@zh 要使用，请选择几个具有视频效果的剪辑/事件（它们必须被实际选中，而不仅仅是在所选组中），然后启动脚本。对于任何剪辑上的每种类型的视频效果参数，系统都会提示你是否希望脚本对参数进行*扰乱*（用随机关键帧替换参数的所有关键帧），或不扰乱（保持关键帧不变）。

@en **Only *OFX* video effects are supported (newsprint, mirror, ...); they all have the same look, it't easy to identify which effects are OFX. Other effects are ignored.**
@zh **仅支持*OFX*视频效果（新闻用纸、镜像……）；它们都有相同的外观，很容易辨识哪些是OFX效果。其它效果将被忽略。**

@en ### Stutter
@zh ### 结巴演说

@en _Stutters clips by Forwarding and Reversing them in randomized intervals._
@zh _通过以随机间隔正放和倒放剪辑来使剪辑出现卡顿的现象。_

@en ![Stutter](/img/v4/moshes/stutter.png){.shadow-less}
@zh ![结巴演说](/img/v4/moshes/stutter_zh-CN.png){.shadow-less}

@en * **Length in Seconds**
@zh * **长度（秒）**
@en   * Duration of clips to be generated.
@zh   * 要生成的剪辑的时长。
@en * **Stutter Window Bias**
@zh * **结巴窗口偏移**
@en   * The value in which stutter will randomize the clips.
@zh   * 剪辑随机化的结巴值。

@en ### Shake
@zh ### 镜头摇晃

@en _Shakes the clips._
@zh _摇晃剪辑。_

@en ![Shake](/img/v4/moshes/shake.png){.shadow-less}
@zh ![镜头摇晃](/img/v4/moshes/shake_zh-CN.png){.shadow-less}

@en * **Speed**
@zh * **速度**
@en   * The frequency value of the shaking effect.
@zh   * 摇晃效果的频率值。
@en * **H/V Synchronicity**
@zh * **水平/垂直同步系数**
@en   * The relative vertical speed value.
@zh   * 相对垂直速度值。
@en * **Amount**
@zh * **数量**
@en   * The value of pixels the camera shifts from the center.
@zh   * 相机从中心偏移的像素值。
    ::: important
@en     This also controls the margin of the zoom in.
@zh     这也控制放大的边距。
    :::
@en * **H/V ratio of displacement**
@zh * **水平/垂直位移比**
@en   * The value of the horizontal distance multiplied.
@zh   * 水平距离的值相乘。
    ::: important
@en     This controls the zoom in.
@zh     这控制放大。
    :::

---

@en * **Reset Pan/Crop on first frame**
@zh * **在第一帧上重置平移/裁切**
@en   * Leave unchecked to shake within the current video zoom.
@zh   * 若不勾选，可在当前视频缩放范围内摇晃。
@en * **Reset all frames before shaking**
@zh * **在摇晃前重置所有帧**
@en   * Leave unchecked to apply and multiple the shake effect with the previous shake effect.
@zh   * 若不勾选，则会应用摇晃效果并将其与之前的摇晃效果相乘。
