# VegasScripts

![Badge](https://img.shields.io/badge/STATE-STABLE-green?style=flat-square)
![Badge](https://img.shields.io/badge/VERSION-4.14.23.1-orange?style=flat-square)

## Otomad Helper
[Get download link now!](https://github.com/otomad/VegasScripts/releases/latest)

### Install
1. [Download](https://github.com/otomad/VegasScripts/releases/latest) the latest version of this script.
2. Drag **ALL** files to the "Script Menu" folder in the Vegas install directory.
> (ie. C:\Program Files\VEGAS\VEGAS Pro 17.0\Script Menu)
3. Make sure that the DLL file (DLL/NAudio.dll) is not locked.
4. Open the Vegas Pro to launch it.

### **Attention**
**According to users test, the software does not work properly on the Vegas Pro older versions (v15 and under). And not tested on v16.**

**So It's actually works on Vegas Pro 17 and above.**

* Tested **fine** on Vegas Pro 17 and 19.

### Reference
* [https://github.com/Chaosinism/vegas_scripts](https://github.com/Chaosinism/vegas_scripts)
* [https://github.com/evankale/VegasScripts](https://github.com/evankale/VegasScripts)
* [https://github.com/naudio/NAudio](https://github.com/naudio/NAudio)
* [https://www.jetdv.com/](https://www.jetdv.com/)

---

## General Instructions (for commonly all scripts)

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
