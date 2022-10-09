<div lang="en">

[![Cover](cover.png)](#otomad-helper)
<div align="center">
	<h1>VegasScripts</h1>
	<p>
		<img src="https://img.shields.io/badge/STATE-STABLE-green?style=flat-square" alt="State" />
		<img src="https://img.shields.io/badge/VERSION-4.22.8.0-orange?style=flat-square" alt="Version" />
		<img src="https://img.shields.io/github/downloads/otomad/OtomadHelper/total.svg?style=flat-square&label=DOWNLOADS" alt="Downloads">
	</p>
</div>
<div align="center">
	<h2>Otomad Helper</h2>
	<p><b><i>Ranne</i></b></p>
	<p><a href="https://github.com/otomad/OtomadHelper/releases/latest"><img src="https://img.shields.io/badge/-Get%20the%20download%20link%20now!-brightgreen?style=for-the-badge&logo=data:image/svg+xml;base64,PHN2ZyByb2xlPSJpbWciIHZpZXdCb3g9IjAgMCAyNCAyNCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNMTEuMiAwYS44LjggMCAwIDAtLjguOHYxMS40TDcuMjYgOS40NGEuODAzLjgwMyAwIDAgMC0xLjEzLjA3NGwtMS4wNSAxLjJhLjguOCAwIDAgMCAuMDczIDEuMTNsNi4zMyA1LjU0YS43OTUuNzk1IDAgMCAwIDEuMDUgMGw2LjMyLTUuNTRhLjguOCAwIDAgMCAuMDc0LTEuMTNsLTEuMDUtMS4yYS44MDQuODA0IDAgMCAwLTEuMTMtLjA3NGwtMy4xNCAyLjc2Vi44YS44LjggMCAwIDAtLjgtLjh6bS04IDIwLjhhLjguOCAwIDAgMC0uOC44djEuNmEuOC44IDAgMCAwIC44LjhoMTcuNmEuOC44IDAgMCAwIC44LS44di0xLjZhLjguOCAwIDAgMC0uOC0uOHoiIGZpbGw9IndoaXRlIi8+PC9zdmc+" alt="Download" /></a></p>

**EN** | [简中](README_zh-CN.md) | [繁中](README_zh-TW.md) | [日](README_ja-JP.md)
</div>

**Otomad Helper** for **Vegas**, which is designed to allow Vegas to accept MIDI files as input, and automatically generate Otomad/YTPMV tracks.

The script is redeveloped based on the original author [@Chaosinism](https://github.com/Chaosinism)'s open source code and uses the NAudio library.

It can also be used to make YTP, Sonar Effect, Datamosh. And Lyrics/Karaoke, Manual Vocaloid/Sentence Mixing, Shupelunker Tactics, etc. will be supported in the future.

The store will be supported for downloading related templates in the future.

The related features of YTP refer to [YTP+](https://github.com/YTP-Plus).

Datą̬͉̫̐͑̓̄ͅa̸͎͇͗̌͂̈̀ą̸̝̼̦̤̇̐ǎ̛͍́̑a̸̲͙͛̐̄̎̚͜a̢̨̝̟͎̾̔̊ǎ̤̞͈͑a͈̪̣̍mo̻̪̬̘̲͆͂͠o̸͍̞͔̓̆̊̀o̗͊̇̇̈́̇ǫ͇͗̏̕͜ơ̬͍͚̦̯̓̊͌ò͈̦̫̈́̓o̦̣̲̊̀o̪̪͚̺̘͛̽̏̈́sh is a technique of damaging clips to create glitchy effects. The related features of Datamosh refer to [vegas-datamosh](https://github.com/delthas/vegas-datamosh).

**Sister Projects:** [om_midi for After Effects](https://github.com/otomad/om_midi).

### Usage
Select the source track event in the track window, or select source media file in media pool window, or browse another media file (in the script configure form), then open the script to configure and generate.

You can add scripts as toolbar buttons rather than having to click inside the *Tools > Scripting* submenu, by adding them to the toolbar using the *Options > Customize Toolbar* menu.

You can also read [@Evauation](https://github.com/Evauation)'s [documentation](https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng/edit) and watch [@Cassidy](https://github.com/composition-cassidy)'s [tutorial video](https://www.youtube.com/watch?v=8vSpzgL_86A) *(English)*.

[Click here to see an introduction to using Datamosh](Datamosh/README.md). You can also [watch the video](https://www.youtube.com/watch?v=6D2lW6H0bb8) *(English)*.

### Install
1. [Download](https://github.com/otomad/OtomadHelper/releases/latest) the latest version of this script.
2. Unzip **ALL** the files in the zip file you've just downloaded to the "Script Menu" folder in the Vegas installation directory.
> (ie. C:\\Program Files\\VEGAS\\VEGAS Pro 20.0\\Script Menu)
3. Make sure that the DLL file `(DLL\NAudio.dll)` is not locked. **Specific steps:**
	1. In the Vegas installation directory, enter folder `Script Menu\DLL`.
	2. Right-click the NAudio.dll file and select Properties.
	3. If you see the "Unlock" button or checkbox, click or check it.
	4. Click OK button and you're done.
4. Open the Vegas Pro to launch it. Select menu *Tools > Scripting > Otomad Helper* to open.

#### Datamosh Extension Pack
If you need to use the full features of Datamosh, you need to install the Datamosh extension pack.

1. [Download](https://github.com/otomad/OtomadHelper/releases/latest) the Datamosh extension pack.
2. Unzip the zip file and move the `_internal` folder to the same directory as the script `Otomad Helper.cs`.

### **Attention**
Vegas Pro 13+ supported.

Vegas Pro 16 and above support all features, and Vegas Pro 13 ~ 15 are compatible to run (some features are missing). The correct version must be installed though.

Tested **fine** on Vegas Pro 17, 18, 19, 20.

### Glossary
You could learn about many features more easily through pictures.

[Glossary >](glossary.md)

### Major Version Update History
These are screenshots of the script user interface for major version updates in history.

[Major Version Update History >](history/README.md)

### Roadmap
[Roadmap >](roadmap.md)

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

You can create issues.

### Terms of Use
1. Respect the rights and interests of authors when creating with non-self-made MIDI/source.
2. Using this script requires some creative abilities from the users. This script should and should only be used to assist in creations. Do not rely solely on this script to generate productions or learn to creating Otomad/YTPMV as a beginner. Listen, don't make Otomidi!

### Permissions
If you use this script, the script will use the following permissions.
1. File read and write.<br />
	The script reads and writes an INI user profile in your disk memory to save your user data.
	> Path: C:\\Users\\*(Your User Name)*\\AppData\\Roaming\\VEGAS Pro\\*(Your Vegas Version)*\\Otomad Helper.ini
2. Registry read and write.<br />
	The script need to read and write the registry to install or uninstall pitch-shift plugin presets.<br />
	If you don't use pitch-shift plugin method in script, you can ignore the installation presets and thus not use this permission.
	> Path: HKEY_CURRENT_USER\\Software\\DirectShow\\Presets\\{ED1B4100-93BE-11D0-AEBC-00A0C9053912}

### References
* [Chaosinism's **Original** Otomad Helper Script](https://github.com/Chaosinism/vegas_scripts)
* [Evan Kale's **Vegas Scripts**](https://github.com/evankale/VegasScripts)
* [Mark Heath's **NAudio** .NET Audio Library](https://github.com/naudio/NAudio)
* [Ben Brown, Kiwifruitdev, Nuppington's **YTP+**](https://github.com/YTP-Plus)
* [Edward's **JETDV** Scripts](https://www.jetdv.com/)
* [Vegas Pro Scripting **Forum**](https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/)
* [Opulos's **Alpha Color Dialog**](https://sourceforge.net/projects/alpha-color-dialog/)
* [Ookii Dialogs WinForms **Progress Dialog**](https://github.com/ookii-dialogs/ookii-dialogs-winforms)
* [Delthas's **Vegas Datamosh**](https://github.com/delthas/vegas-datamosh)

### Logo
Inspired by:
* [@Koorihato Sakuno](https://space.bilibili.com/13084550)
* [@Sufei-King](https://space.bilibili.com/8569439)

### Help and Troubleshooting
**My documentations:** *(Chinese)*
* [Release notes (v4.9.25.0)](https://www.bilibili.com/read/cv13335178)
* [Release notes (v4.10.17.0)](https://www.bilibili.com/read/cv13614419)

**Chaosinism's original documentations:** *(Chinese)*
* [Documentation (v0.1) (bilibili)](https://www.bilibili.com/read/cv392013)
* [Documentation for Staff Visualizer (v0.1)](https://www.bilibili.com/read/cv1027442)
* [Troubleshooting](https://www.bilibili.com/read/cv495309)
* [Tutorial video (v0.1)](https://www.bilibili.com/video/av22226321)

**Chaosinism's original documentations:** *(Japanese)*
* [Documentation (v0.1) (bowlroll)](https://bowlroll.net/user/261124)

### License
* This project is under the GPL 3.0.
	* Chaosinism's original scripts are under the LGPL 3.0.
		* Evan Kale's scripts are under the GPL 3.0.
			* NAudio is under the MIT.
* YTP+ are under the GPL 3.0.
* Ookii Dialogs WinForms are under the BSD 3 Clause.
* Delthas's Vegas Datamosh are under the MIT.
	* FFmpeg is under the LGPL + GPL.
	* Avidemux is under the GPL.
	* Xvid codec is under the GPL.

---

<br />
<h2 align="center">General Instructions (for commonly all scripts)</h2>

### Tips
Various scripts for MAGIX Vegas **(v14 and above)**.

Compilation note for Sony Vegas **(v13 and under)**:
* The namespace name of the *.NET assembly* has changed from `Sony.Vegas` to `ScriptPortal.Vegas` in **v14 and onward**.
	* Change **`using ScriptPortal.Vegas;`** to **`using Sony.Vegas;`** in the scripts to compile for **v13**.
* Actually, the scripts is not supported **v12 and under**.

### Install
* Scripts belong in the Vegas install directory, in the "Script Menu" folder.
	* (ie. C:\\Program Files\\VEGAS\\VEGAS Pro 20.0\\Script Menu)
* Some scripts require additional DLLs, put them in a "DLL" folder in the "Script Menu" folder.
	* (ie. C:\\Program Files\\VEGAS\\VEGAS Pro 20.0\\Script Menu\\DLL)
* Make sure that the DLL files (such as `DLL\NAudio.dll`) are not locked.

</div>
