# VegasScripts

![Badge](https://img.shields.io/badge/STATE-STABLE-green?style=flat-square)
![Badge](https://img.shields.io/badge/VERSION-4.11.8.0-orange?style=flat-square)

Various scripts for MAGIX Vegas **(v14 and above)**.
* Tested on v17.0

Compilation note for Sony Vegas **(v13 and under)**:
* The namespace name of the *.NET assembly* has changed from `Sony.Vegas` to `ScriptPortal.Vegas` in **v14 and onward**.
  * Change **`using ScriptPortal.Vegas;`** to **`using Sony.Vegas;`** in the scripts to compile for **v13**.
* Actually, the scripts is not supported **v12 and under**.

# Install
* Scripts belong in the Vegas install directory, in the "Script Menu" folder.
  * (ie. C:\Program Files\VEGAS\VEGAS Pro 17.0\Script Menu)
* Some scripts require additional DLLs, put them in a "DLL" folder in the "Script Menu" folder.
  * (ie. C:\Program Files\VEGAS\VEGAS Pro 17.0\Script Menu\DLL)
* Make sure that the DLL files (such as DLL/NAudio.dll) is not locked.

# Reference
* https://github.com/Chaosinism/vegas_scripts
* https://github.com/evankale/VegasScripts
* https://github.com/naudio/NAudio
