<div lang="zh-TW">

[![Cover](cover.png)](#otomad-helper)
<div align="center">
	<h2 id="otomad-helper">音 MAD 助手</h2>
	<p><b>蘭音</b></p>
	<p><a href="https://github.com/otomad/OtomadHelper/releases/latest"><img src="https://img.shields.io/badge/-點擊下載最新版！-brightgreen?style=for-the-badge&logo=data:image/svg+xml;base64,PHN2ZyByb2xlPSJpbWciIHZpZXdCb3g9IjAgMCAyNCAyNCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNMTEuMiAwYS44LjggMCAwIDAtLjguOHYxMS40TDcuMjYgOS40NGEuODAzLjgwMyAwIDAgMC0xLjEzLjA3NGwtMS4wNSAxLjJhLjguOCAwIDAgMCAuMDczIDEuMTNsNi4zMyA1LjU0YS43OTUuNzk1IDAgMCAwIDEuMDUgMGw2LjMyLTUuNTRhLjguOCAwIDAgMCAuMDc0LTEuMTNsLTEuMDUtMS4yYS44MDQuODA0IDAgMCAwLTEuMTMtLjA3NGwtMy4xNCAyLjc2Vi44YS44LjggMCAwIDAtLjgtLjh6bS04IDIwLjhhLjguOCAwIDAgMC0uOC44djEuNmEuOC44IDAgMCAwIC44LjhoMTcuNmEuOC44IDAgMCAwIC44LS44di0xLjZhLjguOCAwIDAgMC0uOC0uOHoiIGZpbGw9IndoaXRlIi8+PC9zdmc+" alt="Download" /></a></p>

[EN](README.md) | [简中](README_zh-CN.md) | **繁中** | [日](README_ja-JP.md) | [VI](README_vi-VN.md)
</div>

**音 MAD 助手 Vegas**，旨在使 Vegas 接受 MIDI 序列檔案作為輸入，自動生成音 MAD / YTPMV 的軌道。

本腳本基於原作者 [@Chaosinism](https://github.com/Chaosinism) 的開原始程式碼二次開發，此外使用了 NAudio 庫。

也可以製作 YTP、聲呐效果、幻術故障/數據抹失。未來也可用於製作歌詞/卡拉OK、人力/Rap、原音系戰法。

未來也會增加商城功能用於下載其他使用者製作的範本等相關素材。

YTP 的相關功能參考自 [YTP+](https://github.com/YTP-Plus)。

嚜踵藪璩沬妷是一種磨損素材以產生故障效果的技術。數據抹失的相關功能參考自 [Vegas 數據抹失](https://github.com/delthas/vegas-datamosh)。

鏡頭搖晃功能參考自 [Vegas 搖晃腳本](https://github.com/tmarplatt/VegasScripts)。

越南語翻譯由 [@Cyahega](https://github.com/Cyahega) 提供。

**姊妹專案：**[om_midi for After Effects](https://github.com/otomad/om_midi)。

### 使用方法
在軌道視窗中選中素材，或在專案媒體視窗中選中素材，或在（打開腳本視窗後）手動流覽選擇其它素材。然後即可打開腳本配置並生成。

您可以選擇功能表列 *選項 > 自訂工具列*，將腳本添加到工具列中以便操作。

您也可以閱讀[@伊尤阿省](https://github.com/Evauation)的[說明文件](https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng/edit)並觀看[@卡西迪](https://github.com/composition-cassidy)的[教程影片](https://www.youtube.com/watch?v=8vSpzgL_86A)*（英語）*。

[點擊此處查看數據抹失的使用介紹](Datamosh/README.md)。您也可以[觀看影片](https://www.youtube.com/watch?v=6D2lW6H0bb8)*（英語）*。

關於數據抹失的使用請[觀看影片](https://www.youtube.com/watch?v=6D2lW6H0bb8)*（英語）*。

### 安裝
1. [下載](https://github.com/otomad/OtomadHelper/releases/latest)最新版腳本。
2. 解壓**所有**壓縮包內的檔案並放置到 Vegas 安裝目錄下的「Script Menu」資料夾中。
> (例如：C:\\Program Files\\VEGAS\\VEGAS Pro 20.0\\Script Menu)
3. 確保 DLL 檔案 `(DLL\NAudio.dll)` 沒有被鎖定。**具體步驟：**
	1. 在 Vegas 安裝目錄下，依次進入資料夾 `Script Menu\DLL`。
	2. 右鍵 NAudio.dll 檔案，並選擇「屬性」。
	3. 如果您看到了「解除鎖定」按鈕或復選框，就點一下或勾選它。
	4. 點擊確定按鈕即可。
4. 打開您的 Vegas Pro 來啟動腳本。選擇功能表列 *工具 > 腳本化 > Otomad Helper*。

#### 數據抹失擴展包
如果需要使用數據抹失的全部功能，需要安裝數據抹失擴展包方可使用。

1. [下載](https://github.com/otomad/OtomadHelper/releases/tag/v1.0-datamosh)數據抹失擴展包。
2. 解壓檔案並將 `_internal` 資料夾移動到腳本 `Otomad Helper.cs` 所在的相同目錄下。

### **相容性**
支援 Vegas Pro 13 及以上版本。

Vegas Pro 16 及以上版本支援所有功能，Vegas Pro 13 ~ 15 可以兼容運行（會缺失部分功能）。前提是必須安裝對應的版本。

Vegas Pro 17、18、19、20 測試**正常**。

### 詞彙表
您可以通過圖片更輕鬆地瞭解許多功能。

[詞彙表 >](glossary.md)

### 探索視覺效果
[![探索視覺效果](covers/otomad_helper_visual_effects_example.png)](https://youtu.be/cY2Qa3Owetw)

### 主要版本更新歷史
這些是歷史上主要版本更新的腳本使用者介面的螢幕截圖。

[主要版本更新歷史 >](history/README.md)

### 路線圖
[路線圖 >](roadmap.md)

### 問題
使用腳本時，如果您：
1. 遇到了麻煩；
2. 發現了錯誤；
3. 有新的建議或想法；
4. 協助維護項目；
5. 優化介面外觀；
6. 修正翻譯問題；
7. 提交新的翻譯；
8. ……

都可以提出 issues。

### 使用條款
1. 若使用非自製 MIDI/素材進行創作，請尊重其作者的權益。
2. 使用此腳本需要用戶具備一定的創作能力，此腳本應當且僅應當用於輔助進行創作。不得純依靠此腳本生成作品，或作為初學者學習創作音 MAD/YTPMV 使用。聽話，不要做音 MIDI！

### 許可權
使用腳本時，將會使用如下許可權：
1. 文件讀寫。<br />
	腳本會在磁碟中創建一個檔案用來保存您的使用者配置設置。
	> 路徑：C:\\Users\\*(您的使用者名稱)*\\AppData\\Roaming\\VEGAS Pro\\*(您的 Vegas 版本)*\\Otomad Helper.ini
2. 註冊表讀寫。<br />
	腳本需要讀寫註冊表來安裝或卸載移調外掛插件預設。<br />
	如果您不使用移調插件這個調音算法，您可以忽略該許可權。
	> 路徑：HKEY_CURRENT_USER\\Software\\DirectShow\\Presets\\{ED1B4100-93BE-11D0-AEBC-00A0C9053912}

### 參考
* [Chaosinism 的**原版**音 MAD 助手腳本](https://github.com/Chaosinism/vegas_scripts)
* [埃文·凱爾的 **Vegas 腳本集**](https://github.com/evankale/VegasScripts)
* [馬克·希斯的 **NAudio** .NET 音訊處理庫](https://github.com/naudio/NAudio)
* [本·布朗、奇異果開發、紐平頓的 **YTP+**](https://github.com/YTP-Plus)
* [愛德華的 **JETDV** 腳本集](https://www.jetdv.com/)
* [Vegas Pro 腳本**論壇**](https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/)
* [奧普洛斯的**帶有透明通道的顔色選取器**](https://sourceforge.net/projects/alpha-color-dialog/)
* [大的對話方塊 (WinForm) 的**進度條對話方塊**](https://github.com/ookii-dialogs/ookii-dialogs-winforms)
* [德爾薩斯的 **Vegas 數據抹失**](https://github.com/delthas/vegas-datamosh)
* [特瑪普拉特的 **Vegas 搖晃腳本**](https://github.com/tmarplatt/VegasScripts)

<!-- ### 圖示
靈感來源於:
* [@冰鳩櫻乃](https://space.bilibili.com/13084550)
* [@酥妃大魔王](https://space.bilibili.com/8569439) -->

### 幫助和故障排解
**我的說明文件：***（中文）*
* [發行説明 (v4.9.25.0)](https://www.bilibili.com/read/cv13335178)
* [發行説明 (v4.10.17.0)](https://www.bilibili.com/read/cv13614419)

**Chaosinism 的原版說明文件：***（中文）*
* [說明文件 (v0.1) (B 站)](https://www.bilibili.com/read/cv392013)
* [五線譜視覺化文件 (v0.1)](https://www.bilibili.com/read/cv1027442)
* [故障排解](https://www.bilibili.com/read/cv495309)
* [教程影片 (v0.1)](https://www.bilibili.com/video/av22226321)

**Chaosinism 的原版說明文件：***（日語）*
* [說明文件 (v0.1) (B 碗)](https://bowlroll.net/user/261124)

### 協定
* 該專案使用 GPL 3.0。
	* Chaosinism 的原版腳本使用 LGPL 3.0。
		* 埃文·凱爾的腳本使用 GPL 3.0。
			* NAudio 使用 MIT。
* YTP+ 使用 GPL 3.0。
* Ookii Dialogs WinForms 使用 BSD 3 Clause。
* 德爾薩斯的 Vegas 數據抹失使用 MIT。
	* FFmpeg 使用 LGPL + GPL。
	* Avidemux 使用 GPL。
	* Xvid codec 使用 GPL。

</div>
