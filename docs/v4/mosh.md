# Datamoshes

![Clips Folder](/img/v4/moshes/clips_folder.png)

**<GlitchyText normal="Datamosh" glitchy="Dataḁ̸̬͋ă̷͍ȧ̴͇͘͝ͅá̷̖̲͑ã̶̺̈́å̷͉̖͊̚ä̷̬̬â̷̡̱̑ Mo̴̞̜̯͋ȯ̴̪̏͂õ̷̧͚ͅȏ̸͇ö̵̟̻̹͌o̶͚͍̻̕ǒ̸̢͎̄o̵͔̘̓͘o̶͇̐o̸̬͒͗̎sh" /> is a technique of damaging clips to create glitchy effects which frames that need to progress, don’t progress!**
OH’s Datamosh was made possible with the use of [Vegas-Datamosh by delthas](https://github.com/delthas/vegas-datamosh)

@en ::: info Note
@zh ::: info 注释
@en * **I-frame:** Intra Frame
@zh * **I帧：**帧内帧
@en * **P-frame:** Forward Predicted Frame
@zh * **P帧：**预测帧
@en * **B-frame:** Bi-Directional Predictive Frame
@zh * **B帧：**双向帧
:::

### Datamosh

_Datamoshes the video with determined settings._

![Datamosh](/img/v4/moshes/datamosh.png){.shadow-less}

* **Frame Count**
  * Amount of frames to use.
* **Frame Repeat**
  * Amount of times the frames will repeat.

@en This repeats N P-frames M times. N is the `Frame count`, M is the `Frame repeats`.
@zh 这会重复N个P帧M次。N是`帧计数`，M是`帧重复`。

@en To use, make a selection in the timeline using [[I]] and [[O]], then it will be rendered and datamoshed and added to your project, all in one click. It can take quite some time for long selections so wait if Vegas seems to freeze.
@zh 要使用，请使用 [[I]] 和 [[O]] 在时间轴中选区，然后它将被预渲染和Datamosh，并添加到您的项目中，所有操作只需单击一次。如果Vegas看起来卡住了，那么可能需要较长的时间来处理。

@en The start of the selection is the first P-frame that will be repeated. The P-frame will be relative to the previous frame, which will be rendered as an I-frame. So for best results, the start of the selection should be a frame with a lot of movement.
@zh 选区的起始是第一个将被重复的P帧。这个P帧将是相对于前一帧的，而前一帧将被渲染 I帧。因此为了获得最佳效果，选区的开始应该是一个运动幅度大的帧。

@en The end of the selection simply tells the script until which frame it should render the datamoshed file. The longer the selection, the longer the render time.
@zh 选区的末尾只是告诉脚本应该将Datamosh文件渲染到哪一帧。选择的时间越长，渲染时间就越长。

### Datamix

_Datamoshes the first frame of the selection_

::: important
Requires the selection to be set equal or greater than frame 1
:::

@en This replaces the first I-frames of a clip by a frame of another clip.
@zh 这将用另一个片段的一帧来替换一个片段的第一个 I 帧。

@en To use, make a selection in the timeline using [[I]] and [[O]], then it will be rendered and datamixed and added to your project, all in one click. It can take quite some time for long selections so wait if Vegas seems to freeze.
@zh 要使用，请使用 [[I]] 和 [[O]] 在时间轴中选区，然后它将被预渲染和Datamix，并添加到您的项目中，所有操作只需单击一次。如果Vegas看起来卡住了，那么可能需要较长的时间来处理。

@en The start of the selection is the I-frame of the clip that will be replaced by another I-frame. *The image used for the new I-frame will be the frame just before the start of the selection.* So make sure to place the image you want to datamix from, just before the selection start.
@zh 选择的开始是剪辑的I帧，它将被另一个I帧替换。用于新I帧的图像将是选择开始前的帧。所以请确保将你要Datamix的图像放在选择开始之前。

@en If you want to datamix on a scene change, you can typically split the clip at the exact frame where the scene changes, and select the right clip. This will datamix the right-hand side clip onto the last frame of the left-hand side clip.
@zh 如果你想在场景变化时进行Datamix，通常可以在场景变化的那一帧分割剪辑，然后选择右边的剪辑。这会将右侧剪辑Datamix到左侧剪辑的最后一帧。

@en The end of the selection simply tells the script until which frame it should render the datamoshed file. The longer the selection, the longer the render time.
@zh 选区的末尾只是告诉脚本应该将Datamix 文件渲染到哪一帧。选择的时间越长，渲染时间就越长。

### Layer

_Layers’ the selected clip for moshing_

![Layer](/img/v4/moshes/layer.png){.shadow-less}

* **Layer count**
  * Amount of layers to generate.
* **Layering offset**
  * Offsets the layer by the number of frames specified.
* **Render**
  * Renders the clip selected after generating layers.

@en This does multilayering, by copying the select video clip/event N times, each time offsetting the clip by M frames. N is the `Layer count`, M is the `Layering offset`. You can also choose to automatically render the multilayered clip by checking the `Render` hitbox, otherwise the copies clips will simply be added to the timeline.
@zh 这将通过复制所选视频剪辑/事件N次来执行Layering，每次将剪辑偏移M帧。N是`层数`，M是`层叠偏移`。您还可以通过选中`“预渲染”`复选框来选择自动渲染Layering剪辑，否则复制的剪辑只会添加到时间轴上。

@en To use, select a single video clip/event, then it will be multilayered automatically, and rendered transparently if specified.
@zh 使用时，请选择单个视频剪辑/事件，然后它将自动多层叠化，并在指定的情况下进行透明渲染。

@en *You can use a negative offset (eg -2 instead of 2), in which case the newest clips/events will be added at the back, instead of the front of the previous events.*
@zh *您可以使用一个负的偏移量（例如-2而不是2），在这种情况下，新的剪辑/事件将添加在后面，而不是在前面。*

@en **If you choose to render automatically, the rendered file will support alpha/transparency, meaning you don't need to add a green screen and remove it after, the alpha is handled automatically.**
@zh **如果选择自动渲染，渲染后的文件将支持阿尔法/透明度，这意味着您不需要添加绿幕并在之后删除它，阿尔法将自动处理。**

### Render

_Renders the clip within a timeline selection… (Yes that’s literally it.)_

@en This simply renders a part of the timeline and places it into the timeline in a single click.
@zh 简便地渲染时间轴的一部分，并一键将其放入时间轴中。

@en To use, make a selection in the timeline using [[I]] and [[O]], then it will be rendered and added to your project, all in one click.
@zh 要使用，请使用 [[I]] 和 [[O]] 在时间轴中选区，然后它将被预渲染并添加到您的项目中，所有操作只需单击一次。

@en **The rendered file will support alpha/transparency, meaning you don't need to add a green screen and remove it after, the alpha is handled automatically.**
@zh **渲染后的文件将支持阿尔法/透明度，这意味着您不需要添加绿幕并在之后删除它，阿尔法将自动处理。**

### Scramble

_Scrambles the clip with random time placements dependent on the size._

![Scramble](/img/v4/moshes/scramble.png){.shadow-less}

* **Scramble Size**
  * The value of the size for the clips to be generated.

@en This simply scrambles multiple clips/events, by cutting them in subclips of length N, and shuffling the subclips. N is the `Scramble size`, and is usually 1.
@zh 简便地通过将多个剪辑/事件分割为长度为N的子剪辑，并对子剪辑进行扰乱，即可对其进行扰乱。N是`扰乱大小`，通常为1。

@en To use, select several clips/events (they must be actually selected, not only in group-selected), then they will be scrambled automatically.
@zh 要使用，请选择几个剪辑/事件（它们必须被实际选中，而不仅仅是在选定的组中），然后它们将被自动扰乱。

@en *Clips starting and ending at the same time will be scrambled together, ie their subclips will be shuffled the same way.*
@zh *在同一时间开始和结束的剪辑将被一起扰乱，即它们的子剪辑将以同样的方式被扰乱。*

### Automator

_Automates and Randomizes the values of Visual effects in the clip_

![Automator](/img/v4/moshes/automator.png){.shadow-less}

::: important
This requires Visual effects to have automation on to work
:::

* **Scramble**
  * *It will ask if you wish to automate the Visual effects in the clip, for each effect.*

@en This randomizes the video effects of selected clips/events, by adding random keyframes every frame for each parameter type you select.
@zh 这将通过为您选择的每个参数类型在每一帧添加随机关键帧，随机化选定剪辑/事件的视频效果。

@en To use, select several clips/events which have video effects on them (they must be actually selected, not only in group-selected), then start the script. For each type of video effect parameter on any of the clips, you will be prompted for whether you want the script to *scramble* the parameter (replace all the current keyframes of the parameter with random keyframes), or not (leave the keyframes as is).
@zh 要使用，请选择几个具有视频效果的剪辑/事件（它们必须被实际选中，而不仅仅是在选定的组中），然后启动脚本。对于任何剪辑上的每种类型的视频效果参数，系统都会提示您是否希望脚本对参数进行*扰乱*（用随机关键帧替换参数的所有关键帧），或不扰乱（保持关键帧不变）。

@en **Only *OFX* video effects are supported (newsprint, mirror, ...); they all have the same look, it's easy to identify which effects are OFX. Other effects are ignored.**
@zh **仅支持*OFX*视频效果（新闻用纸、镜像……）；它们都有相同的外观，很容易辨识哪些是OFX效果。其它效果将被忽略。**

### Stutter

_Stutters clips by Forwarding and Reversing them in randomized intervals_

![Stutter](/img/v4/moshes/stutter.png){.shadow-less}

* **Length in Seconds**
  * Duration of clips to be generated.
* **Stutter Window Bias**
  * The value in which stutter will randomize the clips

### Shake

_Shakes the clips_

![Shake](/img/v4/moshes/shake.png){.shadow-less}

* **Speed**
  * The frequency value of the shaking effect
* **H/V Synchronicity**
  * The relative vertical speed value
* **Amount**
  * The value of pixels the camera shifts from the center.
    ::: important
    This also controls the margin of the zoom in.
    :::
* **H/V ratio of displacement**
  * The value of the horizontal distance multiplied
    ::: important
    This controls the zoom in
    :::

---

* **Reset Pan/Crop on first frame**
  (Leave unchecked to shake within the current video zoom)
* **Reset all frames before shaking**
  (Leave unchecked to apply and multiple the shake effect with the previous shake effect)
