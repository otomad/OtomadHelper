@en # Using Otomad Helper
@zh # 使用Otomad Helper

## To make visuals insanely quickly with Otomad Helper

* You will need **Vegas Pro (13+)**
  ::: warning
  *[If you have ANY other editor that you use for remixes](https://github.com/users/otomad/projects/2), you’re out of luck.*
  :::
* **You will need the Otomad Helper Script**
* You will need **FL Studio** *(or any other DAW, that you use for remixes)*

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

## To get started (with FL Studio)

<div class="bold-list">

1. When you have all your audio done in FL Studio.
   ::: info
   * Make a copy of your project
   * [[Ctrl]] + [[N]] to save a new version
   :::
2. Go to *Tools > Marcos > Prepare for MIDI Export*
   ::: danger
   This will convert your entire copy of the project to a MIDI format. **No Undo**
   :::
3. Go to *File > Export > MIDI File*
   ::: important
   It’s also important to name your channels accordingly
   :::
4. Export (Start Render)
5. Go into Vegas
   ::: important
   Before running the script, select the clip you’re going to generate visuals from
   :::
6. Run the Otomad Helper by going to *Tools > Scripting > Otomad Helper*
7. Select your MIDI File
8. Select the Channel that corresponds to the visual
   ::: tip
   * Whatever your sample was, select what matches the clip’s sample
   * [Turn off Audio if you don’t want audio in your generation](./audio.md)
   * [You can customize your visuals in Visual Settings](./visual.md)
   :::
9. Click Generate
10. Repeat the process until you have all your visuals <sup>[1](#footnote-1)</sup>
11. Done!

</div>

> 1. **You can also select advanced to select multiple channels to generate at the same time**
     (This was merged to be in the score section as of Version 4.64.x) {#footnote-1}

## There is now a feature where you can load the last configuration used instantly.

* *Hold [[Ctrl]] while running the script, it will load and generate the last configuration used in the script.*

<style>
  .bold-list li {
    font-weight: 600;
  }

  .bold-list .custom-block {
    font-weight: initial;
  }
</style>
