@en # Installation
@zh # 安装

@en ## Core Script
@zh ## 脚本本体

@en 1. [Download](https://github.com/otomad/OtomadHelper/releases/latest) the latest version of this script.
@zh 1. [下载](https://github.com/otomad/OtomadHelper/releases/latest)最新版脚本。
@en 2. Unzip **ALL** the files in the zip file you’ve just downloaded to the “Script Menu” folder in the Vegas installation directory. ie.
@zh 2. 将你刚刚下载到的ZIP文件中的**所有**文件解压缩到Vegas安装目录中的“Script Menu”文件夹中。例：
   ::: code-group
@en    ```[Path]
@zh    ```[路径]
   C:\Program Files\BorisFX\Vegas Pro 2026\Script Menu
   ```
   :::
@en 3. Make sure that the DLL file `(DLL\NAudio.dll)` is not locked.
@zh 3. 请确保DLL文件 `(DLL\NAudio.dll)` 未锁定。

@en ::: tip Specific steps
@zh ::: tip 具体步骤
@en 1. In the Vegas installation directory, enter folder `Script Menu\DLL`.
@zh 1. 在Vegas安装目录中，依次进入 `Script Menu\DLL` 文件夹。
@en 2. Right-click the “NAudio.dll” file and select Properties.
@zh 2. 右键单击“NAudio.dll”文件，然后选择“属性”。
@en 3. If you see the “Unblock” button (Windows 7/8.x) or checkbox (Windows 10+), click or check it.
@zh 3. 如果你看到了“解除锁定”按钮 (Windows 7/8.x) 或复选框 (Windows 10+)，请单击或选中它。
@en 4. Click OK button and you’re done.
@zh 4. 单击“确定”按钮即可。
:::

@en At this point, your folder should look like this:
@zh 此时你的文件夹看起来应该像是这样：

![Expected Files in Folder](/img/v4/expected_files_in_folder.png)

@en 4. Open Vegas Pro to launch it. Select menu *Tools > Scripting > Otomad Helper* to open.
@zh 4. 打开 Vegas Pro 来启动。选择菜单 *工具 > 脚本化 > Otomad Helper* 打开。

@en ## Datamosh Extension Pack
@zh ## 数据抹失扩展包

@en If you want to use the full features of [Datamosh](./mosh.md), you have to install the Datamosh extension pack.
@zh 如果需要使用[数据抹失](./mosh.md)的全部功能，需要安装数据抹失扩展包方可使用。

@en 1. [Download](https://github.com/otomad/OtomadHelper/releases/tag/v1.0-datamosh) the Datamosh extension pack.
@zh 1. [下载](https://github.com/otomad/OtomadHelper/releases/tag/v1.0-datamosh)数据抹失扩展包。
@en 2. Unzip the 7z file and move the `_internal` folder to the same directory as the script `Otomad Helper.cs`.
@zh 2. 解压7z文件并将 `_internal` 文件夹移动到脚本 `Otomad Helper.cs` 所在的相同目录下。

@en ## Quick Launch
@zh ## 快速启动

@en You can add scripts as toolbar buttons rather than having to click inside the *Tools > Scripting* submenu, by adding them to the toolbar using the *Options > Customize Toolbar* menu.
@zh 你可以选择菜单 *选项 > 自定义工具栏*，将脚本添加到工具栏中以便操作。

@en ## **Compatibility**
@zh ## **兼容性**

@en Vegas Pro 13+ supported.
@zh 支持Vegas Pro 13及以上版本。

@en Vegas Pro 16 and above support all features, and Vegas Pro 13 – 15 are compatible to run (some features are missing). The correct version must be installed though.
@zh Vegas Pro 16及以上版本支持所有功能，Vegas Pro 13 ~ 15可以兼容运行（会缺失部分功能）。前提是必须安装对应的版本。
