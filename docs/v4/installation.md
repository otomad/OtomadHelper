@en # Installation
@zh # 安装

@en ## Core Script
@zh ## 脚本本体

@en 1. <Button href="https://github.com/otomad/OtomadHelper/releases/latest"><b>Download</b> the latest version of this script</Button>
@zh 1. <Button href="https://github.com/otomad/OtomadHelper/releases/latest"><b>下载</b>最新版脚本</Button>

@en ::: tip Release Assets Description
@zh ::: tip 发行资源说明
@en > **Note:** The asset with word `.dll` is a precompiled script, which can start up twice as fast as the source code `.cs` script. You can choose to download according to your needs.
@zh > **注：**带有 `.dll` 字样的资源是预编译的脚本，它的启动速度可以比普通源代码 `.cs` 脚本快一倍。你可根据需要自行选择下载。

0. **`0_otomad_helper_v4.x.x.x_vegas16-2026.cs.zip`**
@en    * The source code script for *MAGIX VEGAS Pro 16*, *MAGIX VEGAS Pro 17*, *MAGIX VEGAS Pro 18*, *MAGIX VEGAS Pro 19*, *MAGIX VEGAS Pro 20*, *MAGIX VEGAS Pro 21*, *MAGIX VEGAS Pro 22*, *MAGIX VEGAS Pro 23*, *BorisFX Vegas Pro 2026* and later versions (if possible).
@zh    * 适用于 *MAGIX VEGAS Pro 16*、*MAGIX VEGAS Pro 17*、*MAGIX VEGAS Pro 18*、*MAGIX VEGAS Pro 19*、*MAGIX VEGAS Pro 20*、*MAGIX VEGAS Pro 21*、*MAGIX VEGAS Pro 22*、*MAGIX VEGAS Pro 23*、*BorisFX Vegas Pro 2026* 及更新版本（如果可能）的普通源代码脚本。
1. **`1_otomad_helper_v4.x.x.x_vegas1415.cs.zip`**
@en    * The source code script for *MAGIX VEGAS Pro 14*, *MAGIX VEGAS Pro 15*.
@zh    * 适用于 *MAGIX VEGAS Pro 14*、*MAGIX VEGAS Pro 15* 的普通源代码脚本。
2. **`2_otomad_helper_v4.x.x.x_vegas13.cs.zip`**
@en    * The source code script for *Sony Vegas Pro 13*.
@zh    * 适用于 *Sony Vegas Pro 13* 的普通源代码脚本。
3. **`3_otomad_helper_v4.x.x.x_vegas16-2026.dll.zip`**
@en    * The precompiled script for *MAGIX VEGAS Pro 16*, *MAGIX VEGAS Pro 17*, *MAGIX VEGAS Pro 18*, *MAGIX VEGAS Pro 19*, *MAGIX VEGAS Pro 20*, *MAGIX VEGAS Pro 21*, *MAGIX VEGAS Pro 22*, *MAGIX VEGAS Pro 23*, *BorisFX Vegas Pro 2026* and later versions (if possible).
@zh    * 适用于 *MAGIX VEGAS Pro 16*、*MAGIX VEGAS Pro 17*、*MAGIX VEGAS Pro 18*、*MAGIX VEGAS Pro 19*、*MAGIX VEGAS Pro 20*、*MAGIX VEGAS Pro 21*、*MAGIX VEGAS Pro 22*、*MAGIX VEGAS Pro 23*、*BorisFX Vegas Pro 2026* 及更新版本（如果可能）的预编译脚本。
4. **`4_otomad_helper_v4.x.x.x_vegas1415.dll.zip`**
@en    * The precompiled script for *MAGIX VEGAS Pro 14*, *MAGIX VEGAS Pro 15*
@zh    * 适用于 *MAGIX VEGAS Pro 14*、*MAGIX VEGAS Pro 15* 的预编译脚本。
:::

@en 2. Unzip **ALL** the files in the ZIP file you've just downloaded to the `Script Menu` folder in the Vegas installation directory, or the `Vegas Script Menu` folder in the `Documents` (formerly `My Documents`) folder. Specifically:
@zh 2. 将你刚刚下载到的zip文件中的**所有**文件解压缩到Vegas安装目录中的 `Script Menu` 文件夹，或 `文档`（原 `我的文档`）文件夹下的 `Vegas Script Menu` 文件夹中。具体来说：

---

@en ### Install for a Specific Installed Vegas Version
@zh ### 仅在已安装的特定Vegas版本下安装

@en If you want to install the script only for a specific installed Vegas version, please install into the `Script Menu` folder in the Vegas Pro directory.
@zh 如果你想仅为已安装的特定Vegas版本下安装脚本，请安装到Vegas Pro目录下的 `Script Menu` 文件夹中。
::: code-group
```[Vegas Pro 2026]
C:\Program Files\BorisFX\Vegas Pro 2026\Script Menu
```
```[VEGAS Pro 23]
C:\Program Files\VEGAS\VEGAS Pro 23.0\Script Menu
```
```[VEGAS Pro 22]
C:\Program Files\VEGAS\VEGAS Pro 22.0\Script Menu
```
```[VEGAS Pro 21]
C:\Program Files\VEGAS\VEGAS Pro 21.0\Script Menu
```
```[VEGAS Pro 20]
C:\Program Files\VEGAS\VEGAS Pro 20.0\Script Menu
```
```[VEGAS Pro 19]
C:\Program Files\VEGAS\VEGAS Pro 19.0\Script Menu
```
```[VEGAS Pro 18]
C:\Program Files\VEGAS\VEGAS Pro 18.0\Script Menu
```
```[VEGAS Pro 17]
C:\Program Files\VEGAS\VEGAS Pro 17.0\Script Menu
```
```[VEGAS Pro 16]
C:\Program Files\VEGAS\VEGAS Pro 16.0\Script Menu
```
```[VEGAS Pro 15]
C:\Program Files\VEGAS\VEGAS Pro 15.0\Script Menu
```
```[VEGAS Pro 14]
C:\Program Files\VEGAS\VEGAS Pro 14.0\Script Menu
```
```[Vegas Pro 13]
C:\Program Files\Sony\Vegas Pro 13.0\Script Menu
```
:::

@en ### Install for All Installed Vegas Versions
@zh ### 为已安装的所有Vegas版本下安装

@en If you want to install the script for all installed Vegas versions, please install into the `Vegas Script Menu` folder in the `Documents` folder.
@zh 如果你想为已安装的所有Vegas版本下安装脚本，请安装到 `文档` 文件夹下的 `Vegas Script Menu` 文件夹中。

::: code-group
@en ```[Direct Path]
@zh ```[直达路径]
%UserProfile%\Documents\Vegas Script Menu
```
@en ```[Manual Path]
@zh ```[手动路径]
@en C:\Users\(Your User Name)\Documents\Vegas Script Menu
@zh C:\Users\(你的用户名)\Documents\Vegas Script Menu
```
:::

@en If the folder does not exist, please follow these steps:
@zh 如果该文件夹并不存在，请按以下步骤操作：

::: tip
@en 1. Go to `Documents` (Windows 8+) or `My Documents` (Windows 7) folder in User's Personal Folder.
@zh 1. 进入用户个人文件夹下的 `文档` (Windows 8+) 或 `我的文档` (Windows 7) 文件夹。
@en 2. Create a new folder and name it `Vegas Script Menu`.
@zh 2. 新建文件夹并命名为 `Vegas Script Menu`。
@en 3. Insert the files into said folder.
@zh 3. 将文件插入到刚刚的文件夹中。
:::

::: danger
@en Vegas Pro 13, VEGAS Pro 14~15, VEGAS Pro 16+ each use different kinds of script. If install it for all installed Vegas versions, non corresponding Vegas versions will not recognize the correct script, which will still raise an error.
@zh Vegas Pro 13、VEGAS Pro 14~15、VEGAS Pro 16+ 各自分别使用不同的脚本。将其为已安装的所有Vegas版本安装，则不相对应的Vegas版本不会识别正确的脚本，此时依旧会导致报错。

@en If you have installed multiple versions of Vegas Pro simultaneously, please try to use other solutions, such as renaming the script.
@zh 如果你同时安装了多个版本的Vegas Pro，请尝试使用其它解决办法，例如重命名脚本。
:::

---

@en 3. Make sure that the DLL file `(DLL\NAudio.dll)` is not locked.
@zh 3. 请确保DLL文件 `(DLL\NAudio.dll)` 未锁定。

@en ::: tip Specific steps
@zh ::: tip 具体步骤
@en 1. Enter folder `DLL` from the same directory where `Otomad Helper.{cs,dll}` file is located. That is:
@zh 1. 从 `Otomad Helper.{cs,dll}` 文件所在的相同目录下，进入 `DLL` 文件夹。也就是说：
@en    1. In the Vegas installation directory, enter folder `Script Menu\DLL`.
@zh    1. 在Vegas安装目录中，依次进入 `Script Menu\DLL` 文件夹。
@en    2. In the `Documents` folder, enter folder `Vegas Script Menu\DLL`.
@zh    2. 在`文档`文件夹下，依次进入 `Vegas Script Menu\DLL` 文件夹。
@en 2. Right-click the `NAudio.dll` file and select Properties.
@zh 2. 右键单击 `NAudio.dll` 文件，然后选择“属性”。
@en 3. If you see the [[Unblock]] button (Windows 7/8.x) or checkbox (Windows 10+), click or check it.
@zh 3. 如果你看到了 [[解除锁定]] 按钮 (Windows 7/8.x) 或复选框 (Windows 10+)，请单击或选中它。
@en 4. Click [[OK]] button and you're done.
@zh 4. 单击 [[确定]] 按钮即可。
:::

@en At this point, your folder should look like this:
@zh 此时你的文件夹看起来应该像是这样：

![Expected Files in Folder](/img/v4/expected_files_in_folder.png)

@en 4. Open Vegas Pro to launch it. Select menu [[Tools > Scripting > Rescan Script Menu Folder]].
@zh 4. 打开 Vegas Pro 来启动。选择菜单 [[工具 > 脚本化 > 重新扫描脚本菜单文件夹]]。
@en 5. Select menu [[Tools > Scripting > Otomad Helper]] to open.
@zh 5. 选择菜单 [[工具 > 脚本化 > Otomad Helper]] 打开。


@en ## Datamosh Extension Pack
@zh ## 数据抹失扩展包

@en If you want to use the full features of [Datamosh](./mosh.md), you have to install the Datamosh extension pack.
@zh 如果需要使用[数据抹失](./mosh.md)的全部功能，需要安装数据抹失扩展包方可使用。

@en 1. <Button href="https://github.com/otomad/OtomadHelper/releases/tag/v1.0-datamosh"><b>Download</b> the Datamosh extension pack</Button>
@zh 1. <Button href="https://github.com/otomad/OtomadHelper/releases/tag/v1.0-datamosh"><b>下载</b>数据抹失扩展包</Button>
@en 2. Unzip the 7Z file and move the `_internal` folder to the same directory as the script `Otomad Helper.{cs,dll}`.
@zh 2. 解压7z文件并将 `_internal` 文件夹移动到脚本 `Otomad Helper.{cs,dll}` 所在的相同目录下。

@en ## Quick Launch
@zh ## 快速启动

@en You can add the script as a toolbar button for quick access rather than having to click troublesomely inside the [[Tools > Scripting]] submenu every times, by adding them to the toolbar using the [[Options > Customize Toolbar]] menu.
@zh 你可以选择菜单 [[选项 > 自定义工具栏]]，将脚本添加到工具栏中以便快捷操作。从而无需麻烦地每次到 [[工具 > 脚本化]] 子菜单中单击。

@en ## **Compatibility**
@zh ## **兼容性**

@en Vegas Pro 13+ supported.
@zh 支持Vegas Pro 13及以上版本。

@en Vegas Pro 16 and above support all features, and Vegas Pro 13 – 15 are compatible to run (some features are missing). The correct version must be installed though.
@zh Vegas Pro 16及以上版本支持所有功能，Vegas Pro 13 ~ 15可以兼容运行（会缺失部分功能）。前提是必须安装对应的版本。
