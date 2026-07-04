@en # Getting Started with Otomad Helper
@zh # Otomad Helper入门指南

@en ## To make visuals insanely quickly with Otomad Helper
@zh ## 要借助Otomad Helper快速制作视觉效果

@en * You will need a **Vegas Pro (13+)**;
@zh * 你需要一个**Vegas Pro (13+)**；
  ::: warning
@en   *[If you have ANY other editor that you use for remixes](https://github.com/users/otomad/projects/2), you’re out of luck.*
@zh   *[如果你有任何其它用于混音的编辑器](https://github.com/users/otomad/projects/2)，那么你就不那么走运了。*
  :::
@en * **You will need the Otomad Helper Script**;
@zh * **你需要Otomad Helper脚本**；
@en * You will need **FL Studio** *(or any other DAW, that you use for remixes)*.
@zh * 你需要**FL Studio***（或用于混音的任何其它DAW）*。

@en ## Using Otomad Helper is pretty straightforward from here.<br>Here’s how to use it.
@zh ## 从这里开始，使用Otomad Helper非常简单。以下是使用方法。

@en - You *import and use a [MIDI](./score.md) file to configure and generate the clips you want*. You should use a MIDI which **created by yourself** and not someone else. It’s discourteous. Don’t create YTPMIDIs or any form of remix you didn’t create.
@zh - 你可以*导入并使用[MIDI](./score.md)文件来配置和生成所需的剪辑*。你应该使用**你自己创建的**MIDI，而不是其他人的。这太不礼貌了。不要做音MIDI或者不是你创造的任何形式的混音。
@en - You need to *have a clip **selected** in the **timeline** or **media pool*** in order to generate anything from the MIDI onto Vegas Pro.
@zh - 你需要*在**时间轴**或**媒体柜**中**挑取**一段剪辑*，以便在Vegas Pro中生成MIDI中的任何内容。
@en   - Alternatively, you can use a clip or [source](./source.md) from your computer to be used for generating by the MIDI.
@zh   - 或者你可以使用电脑中的剪辑或[素材](./source.md)来进行MIDI生成。
@en - The [Audio](./audio.md) and [Visual](./visual.md) pages will allow you to edit the settings of how the clips are generated, there is a variety of settings, play with them and tweak them to sort your needs for the start of your creation.
@zh - [“音频”](./audio.md)和[“画面”](./visual.md)页面将允许你编辑剪辑的生成设置，有各种设置，玩弄它们并对它们进行调整，以便在开始创建时对你的需求进行排序。
@en - There is also a variety of [tools](./tools.md) to help work on the most ludicrous of tasks for your projects, making the chore of editing certain clips to applying different effects easier said than done.
@zh - 还有各种[工具](./tools.md)可以帮助你完成项目中最荒唐的任务，使编辑某些片段以应用不同效果的繁琐工作不再说起来容易做起来难。

@en ## To get started (with FL Studio)
@zh ## 要开始（与FL Studio协作）

<div class="bold-list">

@en 1. When you have all your audio done in FL Studio.
@zh 1. 当你在FL Studio中完成所有音频时。
   ::: info
@en    * Make a copy of your project.
@zh    * 将你的项目创建副本。
@en    * Press [[Ctrl + N]] to save a new version
@zh    * 按 [[Ctrl + N]] 键来另存为新版本
   :::
@en 2. Go to *Tools > Marcos > Prepare for MIDI Export*.
@zh 2. 转到 *工具 > 宏 > 准备进行MIDI导出*。
   ::: danger
@en    This will convert your entire copy of the project to a MIDI format. **No Undo!**
@zh    这会将你的整个项目副本转换为MIDI格式。**不可撤销！**
   :::
@en 3. Go to *File > Export > MIDI File*.
@zh 3. 转到 *文件 > 导出 > MIDI文件*。
   ::: important
@en    It’s also important to name your tracks accordingly.
@zh    相应地命名你的音轨也很重要。
   :::
@en 4. Export (Start Render).
@zh 4. 导出（开始渲染）。
@en 5. Go into Vegas.
@zh 5. 切换到Vegas。
   ::: important
@en    Before running the script, please select the clip you’re going to generate visuals from first.
@zh    在运行脚本之前，请先选择要从中生成画面的剪辑。
   :::
@en 6. Run the Otomad Helper by going to *Tools > Scripting > Otomad Helper*.
@zh 6. 转到 *工具 > 脚本化 > Otomad Helper* 以运行Otomad Helper。
@en 7. Select your MIDI File.
@zh 7. 选择你的MIDI文件。
@en 8. Select the Track that corresponds to the visual
@zh 8. 选择与画面相对应的音轨。
   ::: tip
@en    * Whatever your sample was, select what matches the clip’s sample.
@zh    * 无论你的采样是什么，选择与剪辑采样相匹配的采样。
@en    * [Turn off Audio if you don’t want audio in your generation.](./audio.md)
@zh    * [如果你不希望生成音频，请关闭音频。](./audio.md)
@en    * [You can customize your visuals in Visual Settings.](./visual.md)
@zh    * [你可以在画面设置中自定义你的画面。](./visual.md)
   :::
@en 9. Click Generate.
@zh 9. 点击生成。
@en 10. Repeat the process until you have all your visuals. <sup>[1](#footnote-1)</sup>
@zh 10. 重复这个过程，直到你拥有了全部的画面。 <sup>[1](#footnote-1)</sup>
@en 11. Done!
@zh 11. 完成！

</div>

@en > 1. **You can also select multiple tracks in the score section to generate at the same time.** {#footnote-1}
@zh > 1. **你还可以在乐曲部分同时选择多个音轨生成。** {#footnote-1}

@en ## There is now a feature where you can load the last configuration used instantly.
@zh ## 现在有一个功能，你可以立即加载上次使用的配置。

@en * *Hold [[Ctrl]] while running the script, it will load and generate the last configuration used in the script.*
@zh * *运行脚本时按住 [[Ctrl]] 键，脚本将加载最后一次使用的配置并生成。*
