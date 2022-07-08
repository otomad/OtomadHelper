![VegasScripts](https://github.com/otomad/VegasScripts/blob/winform/banner.png?raw=true)
<h2 align="center">音 MAD 助手</h2>
<div align="center">
	<p><a href="https://github.com/otomad/VegasScripts/releases/latest">点击下载最新版！</a></p>
	<p>
		<a href="README.md">EN</a> |
		<strong>简中</strong> |
		<a href="README_zh-TW.md">繁中</a> |
		<a href="README_ja-JP.md">日</a>
	</p>
</div>

音 MAD 助手 Vegas 版，旨在使 Vegas 接受 MIDI 序列文件作为输入，自动生成音 MAD / YTPMV 的轨道。

本脚本基于原作者 [@Chaosinism](https://github.com/Chaosinism) 的开源代码二次开发，此外使用了 NAudio 库。

也可以制作 YTP。未来也可用于制作鼓组节奏区域切除、歌词/卡拉OK、人力/Rap、鞑靼战法。

未来也会增加商城功能用于下载其他用户制作的模板等相关素材。

### 使用方法
在轨道窗口中选中素材，或在项目媒体窗口中选中素材，或在（打开脚本窗口后）手动浏览选择其它素材。然后即可打开脚本配置并生成。

您可以选择菜单 *选项 > 自定义工具栏*，将脚本添加到工具栏中以便操作。

您也可以阅读 [@Evauation](https://github.com/Evauation) 的[文档](https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng/edit)并观看他的[教程视频](https://www.youtube.com/watch?v=8vSpzgL_86A)*（英语）*。

### 安装
1. [下载](https://github.com/otomad/VegasScripts/releases/latest)最新版脚本。
2. 解压**所有**压缩包内的文件并放置到 Vegas 安装目录下的“Script Menu”文件夹中。
> (例如：C:\Program Files\VEGAS\VEGAS Pro 17.0\Script Menu)
3. 确保 DLL 文件 (DLL/NAudio.dll) 没有被锁定。**具体步骤：**
	1. 在 Vegas 安装目录下，依次进入文件夹 Script Menu/DLL。
	2. 右键 NAudio.dll 文件，并选择“属性”。
	3. 如果您看到了“解除锁定”按钮，就点一下。
	4. 关闭属性对话框即可。
4. 打开您的 Vegas Pro 来启动脚本。选择菜单 *工具 > 脚本化 > Otomad Helper*。

### **注意**
支持 Vegas Pro 13 及以上版本。

Vegas 16 及以上版本支持所有功能，Vegas 13 ~ 15 可以兼容运行（会缺失部分功能）。前提是必须安装对应的版本。

Vegas Pro 17 和 19 测试**正常**。

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

### 路线图
[路线图 >](ROADMAP.md)

### 权限
使用脚本时，将会使用如下权限：
1. 文件读写。<br />
	脚本会在磁盘中创建一个文件用来保存您的用户配置设置。
2. 注册表读写。<br />
	脚本需要读写注册表来安装或卸载移调插件预设。<br />
	如果您不使用移调插件这个调音算法，您可以忽略该权限。

### 参考
* [https://github.com/Chaosinism/vegas_scripts](https://github.com/Chaosinism/vegas_scripts)
* [https://github.com/evankale/VegasScripts](https://github.com/evankale/VegasScripts)
* [https://github.com/naudio/NAudio](https://github.com/naudio/NAudio)
* [https://www.jetdv.com/](https://www.jetdv.com/)
* [https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/](https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/)

### 图标
灵感来源于:
* [@uid13084550](https://space.bilibili.com/13084550)
* [@uid8569439](https://space.bilibili.com/8569439)

### 帮助和疑难解答 *（中文）*
**我的文档：**
* [说明文档 (v4.9.25.0)](https://www.bilibili.com/read/cv13335178)
* [说明文档 (v4.10.17.0)](https://www.bilibili.com/read/cv13614419)

**Chaosinism 的原版文档：**
* [说明文档 (v0.1) (B 站)](https://www.bilibili.com/read/cv392013)
* [说明文档 (v0.1) (B 碗)](https://bowlroll.net/user/261124)
* [五线谱可视化文档 (v0.1)](https://www.bilibili.com/read/cv1027442)
* [疑难解答](https://www.bilibili.com/read/cv495309)
* [教程视频 (v0.1)](https://www.bilibili.com/video/av22226321)
