![VegasScripts](https://github.com/otomad/VegasScripts/blob/winform/banner.png?raw=true)
<h1 align="center">VegasScripts</h1>
<div align="center">
	<img src="https://img.shields.io/badge/STATE-STABLE-green?style=flat-square" alt="Badge" />
	<img src="https://img.shields.io/badge/VERSION-4.18.4.0-orange?style=flat-square" alt="Badge" />
</div>
<h2 align="center">Otomad Helper</h2>
<p align="center"><a href="https://github.com/otomad/VegasScripts/releases/latest"><img src="https://img.shields.io/badge/-Get%20the%20download%20link%20now!-brightgreen?style=for-the-badge&logo=data:image/svg+xml;base64,PHN2ZyByb2xlPSJpbWciIHZpZXdCb3g9IjAgMCAyNCAyNCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNMTEuMiAwYS44LjggMCAwIDAtLjguOHYxMS40TDcuMjYgOS40NGEuODAzLjgwMyAwIDAgMC0xLjEzLjA3NGwtMS4wNSAxLjJhLjguOCAwIDAgMCAuMDczIDEuMTNsNi4zMyA1LjU0YS43OTUuNzk1IDAgMCAwIDEuMDUgMGw2LjMyLTUuNTRhLjguOCAwIDAgMCAuMDc0LTEuMTNsLTEuMDUtMS4yYS44MDQuODA0IDAgMCAwLTEuMTMtLjA3NGwtMy4xNCAyLjc2Vi44YS44LjggMCAwIDAtLjgtLjh6bS04IDIwLjhhLjguOCAwIDAgMC0uOC44djEuNmEuOC44IDAgMCAwIC44LjhoMTcuNmEuOC44IDAgMCAwIC44LS44di0xLjZhLjguOCAwIDAgMC0uOC0uOHoiIGZpbGw9IndoaXRlIi8+PC9zdmc+" alt="Download" /></a></p>

**EN** | [简中](README_zh-CN.md) | [繁中](README_zh-TW.md) | [日](README_ja-JP.md)

Otomad Helper for Vegas, which is designed to allow Vegas to accept MIDI files as input, and automatically generate Otomad/YTPMV tracks.

The script is based on the original author [@Chaosinism](https://github.com/Chaosinism)'s open source code redevelopment and uses the NAudio library.

It can also be used to make YTP. And Drums beat cookie cutter, Lyrics/Karaoke, Manual vocaloid/Sentence mixing, Tatar tactics, etc. will be supported in the future.

The store will be supported for downloading related templates in the future.

### Usage
Select the source track event in the track window, or select source media file in media pool window, or browse another media file (in the script configure form), then open the script to configure and generate.

You can add scripts as toolbar buttons rather than having to click inside the *Tools > Scripting* submenu, by adding them to the toolbar using the *Options > Customize Toolbar* menu.

You can also read [@Evauation](https://github.com/Evauation)'s [documentation](https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng/edit) and watch his [tutorial video](https://www.youtube.com/watch?v=8vSpzgL_86A) *(English)*.

### Install
1. [Download](https://github.com/otomad/VegasScripts/releases/latest) the latest version of this script.
2. Unzip **ALL** the files in the zip file you've just downloaded to the "Script Menu" folder in the Vegas installation directory.
> (ie. C:\Program Files\VEGAS\VEGAS Pro 17.0\Script Menu)
3. Make sure that the DLL file (DLL/NAudio.dll) is not locked. **Specific steps:**
	1. In the Vegas installation directory, enter folder Script Menu/DLL.
	2. Right-click the NAudio.dll file and select Properties.
	3. If you see the "Unlock" button, click it.
	4. Close the Properties dialog and you're done.
4. Open the Vegas Pro to launch it. Select menu *Tools > Scripting > Otomad Helper* to open.

### **Attention**
Vegas Pro 13+ supported.

Vegas 16 and above support all features, and Vegas 13 ~ 15 are compatible to run (some features are missing). The correct version must be installed though.

Tested **fine** on Vegas Pro 17 and 19.

### Issues
When you are using the script, if you:
1. Have a problem in it;
2. Found a bug;
3. Submit some new suggestions or ideas;
4. Help us collaborate on the production;
5. Optimize interface appearance;
6. Make corrections to existing translations;
7. Provide a new language translation;
8. …

All can create issues.

### Roadmap
[Roadmap >](ROADMAP.md)

### Permissions
If you use this script, the script will use the following permissions.
1. File read and write.<br />
	The script reads and writes an INI user profile in your disk memory to save your user data.
2. Registry read and write.<br />
	The script need to read and write the registry to install or uninstall pitch-shift plugin presets.<br />
	If you don't use pitch-shift plugin method in script, you can ignore the installation presets and thus not use this permission.

### References
* [https://github.com/Chaosinism/vegas_scripts](https://github.com/Chaosinism/vegas_scripts)
* [https://github.com/evankale/VegasScripts](https://github.com/evankale/VegasScripts)
* [https://github.com/naudio/NAudio](https://github.com/naudio/NAudio)
* [https://www.jetdv.com/](https://www.jetdv.com/)
* [https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/](https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/)

### Logo
Inspired by:
* [@uid13084550](https://space.bilibili.com/13084550)
* [@uid8569439](https://space.bilibili.com/8569439)

### Help and Troubleshooting *(Chinese)*
**My docs:**
* [Documentation (v4.9.25.0)](https://www.bilibili.com/read/cv13335178)
* [Documentation (v4.10.17.0)](https://www.bilibili.com/read/cv13614419)

**Chaosinism's original docs:**
* [Documentation (v0.1) (bilibili)](https://www.bilibili.com/read/cv392013)
* [Documentation (v0.1) (bowlroll)](https://bowlroll.net/user/261124)
* [Documentation for Staff Visualizer (v0.1)](https://www.bilibili.com/read/cv1027442)
* [Troubleshooting](https://www.bilibili.com/read/cv495309)
* [Tutorial video (v0.1)](https://www.bilibili.com/video/av22226321)

---

<br />
<h2 align="center">General Instructions (for commonly all scripts)</h2>

### Tips
Various scripts for MAGIX Vegas **(v14 and above)**.
* Tested on v17.0.

Compilation note for Sony Vegas **(v13 and under)**:
* The namespace name of the *.NET assembly* has changed from `Sony.Vegas` to `ScriptPortal.Vegas` in **v14 and onward**.
  * Change **`using ScriptPortal.Vegas;`** to **`using Sony.Vegas;`** in the scripts to compile for **v13**.
* Actually, the scripts is not supported **v12 and under**.

### Install
* Scripts belong in the Vegas install directory, in the "Script Menu" folder.
  * (ie. C:\Program Files\VEGAS\VEGAS Pro 17.0\Script Menu)
* Some scripts require additional DLLs, put them in a "DLL" folder in the "Script Menu" folder.
  * (ie. C:\Program Files\VEGAS\VEGAS Pro 17.0\Script Menu\DLL)
* Make sure that the DLL files (such as DLL/NAudio.dll) are not locked.
