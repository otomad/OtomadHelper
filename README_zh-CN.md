<div lang="zh-CN">

[![Cover](cover.png)](#otomad-helper)
<div align="center">
	<h2 id="otomad-helper">音 MAD 助手</h2>
	<p><b>兰音</b></p>
	<p><a href="https://github.com/otomad/OtomadHelper/releases/latest"><img src="https://img.shields.io/badge/-点击下载最新版！-brightgreen?style=for-the-badge&logo=data:image/svg+xml;base64,PHN2ZyByb2xlPSJpbWciIHZpZXdCb3g9IjAgMCAyNCAyNCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNMTEuMiAwYS44LjggMCAwIDAtLjguOHYxMS40TDcuMjYgOS40NGEuODAzLjgwMyAwIDAgMC0xLjEzLjA3NGwtMS4wNSAxLjJhLjguOCAwIDAgMCAuMDczIDEuMTNsNi4zMyA1LjU0YS43OTUuNzk1IDAgMCAwIDEuMDUgMGw2LjMyLTUuNTRhLjguOCAwIDAgMCAuMDc0LTEuMTNsLTEuMDUtMS4yYS44MDQuODA0IDAgMCAwLTEuMTMtLjA3NGwtMy4xNCAyLjc2Vi44YS44LjggMCAwIDAtLjgtLjh6bS04IDIwLjhhLjguOCAwIDAgMC0uOC44djEuNmEuOC44IDAgMCAwIC44LjhoMTcuNmEuOC44IDAgMCAwIC44LS44di0xLjZhLjguOCAwIDAgMC0uOC0uOHoiIGZpbGw9IndoaXRlIi8+PC9zdmc+" alt="Download" /></a></p>
	
[EN](README.md) | **简** | [繁](README_zh-TW.md) | [日](README_ja-JP.md) | [VI](README_vi-VN.md)
</div>

**音 MAD 助手 Vegas**，旨在使 Vegas 接受 MIDI 序列文件作为输入，自动生成音 MAD / YTPMV 的轨道。

本脚本基于原作者 [@Chaosinism](https://github.com/Chaosinism) 的开源代码二次开发，此外使用了 NAudio 库。

也可以制作 YTP、声呐效果、幻术故障/数据抹失。未来也可用于制作歌词/卡拉OK、人力/Rap、原音系战法。

未来也会增加商城功能用于下载其他用户制作的模板等相关素材。

YTP 的相关功能参考自 [YTP+](https://github.com/YTP-Plus)。

锘挎藪琚沬妷是一种磨损素材以产生故障效果的技术。数据抹失的相关功能参考自 [Vegas 数据抹失](https://github.com/delthas/vegas-datamosh)。

镜头摇晃功能参考自 [Vegas 摇晃脚本](https://github.com/tmarplatt/VegasScripts)。

越南语翻译由 [@Cyahega](https://github.com/Cyahega) 提供。

**姊妹项目：**[om_midi for After Effects](https://github.com/otomad/om_midi)。

### 使用方法
在轨道窗口中选中素材，或在项目媒体窗口中选中素材，或在（打开脚本窗口后）手动浏览选择其它素材。然后即可打开脚本配置并生成。

您可以选择菜单 *选项 > 自定义工具栏*，将脚本添加到工具栏中以便操作。

您也可以阅读 [@Evauation](https://github.com/Evauation) 的[说明文档](https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng/edit)并观看 [@Cassidy](https://github.com/composition-cassidy) 的[教程视频](https://www.youtube.com/watch?v=8vSpzgL_86A)*（英语）*。

[点击此处查看数据抹失的使用介绍](Datamosh/README_zh-CN.md)。您也可以[观看视频](https://www.youtube.com/watch?v=6D2lW6H0bb8)*（英语）*。

### 安装
1. [下载](https://github.com/otomad/OtomadHelper/releases/latest)最新版脚本。
2. 解压**所有**压缩包内的文件并放置到 Vegas 安装目录下的“Script Menu”文件夹中。
> (例如：C:\\Program Files\\VEGAS\\VEGAS Pro 20.0\\Script Menu)
3. 确保 DLL 文件 `(DLL\NAudio.dll)` 没有被锁定。**具体步骤：**
	1. 在 Vegas 安装目录下，依次进入文件夹 `Script Menu\DLL`。
	2. 右键 NAudio.dll 文件，并选择“属性”。
	3. 如果您看到了“解除锁定”按钮或复选框，就点一下或勾选它。
	4. 点击确定按钮即可。
4. 打开您的 Vegas Pro 来启动脚本。选择菜单 *工具 > 脚本化 > Otomad Helper*。

#### 数据抹失扩展包
如果需要使用数据抹失的全部功能，需要安装数据抹失扩展包方可使用。

1. [下载](https://github.com/otomad/OtomadHelper/releases/tag/v1.0-datamosh)数据抹失扩展包。
2. 解压文件并将 `_internal` 文件夹移动到脚本 `Otomad Helper.cs` 所在的相同目录下。

### **兼容性**
支持 Vegas Pro 13 及以上版本。

Vegas Pro 16 及以上版本支持所有功能，Vegas Pro 13 ~ 15 可以兼容运行（会缺失部分功能）。前提是必须安装对应的版本。

Vegas Pro 17、18、19、20 测试**正常**。

### 词汇表
您可以通过图片更轻松地了解许多功能。

[词汇表 >](glossary.md)

### 探索视觉效果
[![探索视觉效果](covers/otomad_helper_visual_effects_example.png)](https://youtu.be/cY2Qa3Owetw)

### 主要版本更新历史
这些是历史上主要版本更新的脚本用户界面的屏幕截图。

[主要版本更新历史 >](history/README.md)

### 路线图
[路线图 >](roadmap.md)

### 问题
使用脚本时，如果您：
1. 遇到了问题；
2. 发现了错误；
3. 有新的建议或想法；
4. 协助维护项目；
5. 优化界面外观；
6. 修正翻译问题；
7. 提交新的翻译；
8. ……

都可以提出 issues。

### 使用条款
1. 若使用非自制 MIDI/素材进行创作，请尊重其作者的权益。
2. 使用此脚本需要用户具备一定的创作能力，此脚本应当且仅应当用于辅助进行创作。不得纯依靠此脚本生成作品，或作为初学者学习创作音 MAD/YTPMV 使用。听话，不要做音 MIDI！

### 权限
使用脚本时，将会使用如下权限：
1. 文件读写。<br />
	脚本会在磁盘中创建一个文件用来保存您的用户配置设置。
	> 路径：C:\\Users\\*(您的用户名)*\\AppData\\Roaming\\VEGAS Pro\\*(您的 Vegas 版本)*\\Otomad Helper.ini
2. 注册表读写。<br />
	脚本需要读写注册表来安装或卸载移调插件预设。<br />
	如果您不使用移调插件这个调音算法，您可以忽略该权限。
	> 路径：HKEY_CURRENT_USER\\Software\\DirectShow\\Presets\\{ED1B4100-93BE-11D0-AEBC-00A0C9053912}

### 参考
* [Chaosinism 的**原版**音 MAD 助手脚本](https://github.com/Chaosinism/vegas_scripts)
* [Evan Kale 的 **Vegas 脚本集**](https://github.com/evankale/VegasScripts)
* [Mark Heath 的 **NAudio** .NET 音频处理库](https://github.com/naudio/NAudio)
* [Ben Brown、Kiwifruitdev、Nuppington 的 **YTP+**](https://github.com/YTP-Plus)
* [Edward 的 **JETDV** 脚本集](https://www.jetdv.com/)
* [Vegas Pro 脚本**论坛**](https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/)
* [Opulos 的**带有透明通道的颜色选取器**](https://sourceforge.net/projects/alpha-color-dialog/)
* [Ookii Dialogs WinForms 的**进度条对话框**](https://github.com/ookii-dialogs/ookii-dialogs-winforms)
* [Delthas 的 **Vegas 数据抹失**](https://github.com/delthas/vegas-datamosh)
* [Tmarplatt 的 **Vegas 摇晃脚本**](https://github.com/tmarplatt/VegasScripts)

<!-- ### 图标
灵感来源于:
* [@冰鸠樱乃](https://space.bilibili.com/13084550)
* [@酥妃大魔王](https://space.bilibili.com/8569439) -->

### 帮助和疑难解答
**我的文档：***（中文）*
* [发行说明 (v4.9.25.0)](https://www.bilibili.com/read/cv13335178)
* [发行说明 (v4.10.17.0)](https://www.bilibili.com/read/cv13614419)

**Chaosinism 的原版文档：***（中文）*
* [说明文档 (v0.1) (B 站)](https://www.bilibili.com/read/cv392013)
* [五线谱可视化文档 (v0.1)](https://www.bilibili.com/read/cv1027442)
* [疑难解答](https://www.bilibili.com/read/cv495309)
* [教程视频 (v0.1)](https://www.bilibili.com/video/av22226321)

**Chaosinism 的原版文档：***（日语）*
* [说明文档 (v0.1) (B 碗)](https://bowlroll.net/user/261124)

### 协议
| 项目 | 协议 |
| ---- | ---- |
| Otomad Helper<br />*当前项目* | GPL 3.0 |
| otomad_helper<br />*Chaosinism 的原版脚本* | LGPL 3.0 |
| VegasScripts<br />*Evan Kale 的脚本* | GPL 3.0 |
| NAudio | MIT |
| YTP+ | GPL 3.0 |
| Alpha Color Dialog | BSD |
| Ookii Dialogs WinForms | BSD 3 Clause |
| vegas-datamosh | MIT |
| FFmpeg | LGPL + GPL |
| Avidemux | GPL |
| Xvid codec | GPL |

</div>
