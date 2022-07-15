![Cover](https://github.com/otomad/VegasScripts/blob/winform/cover.png?raw=true)
<h2 align="center">音 MAD ヘルパー</h2>
<p align="center"><a href="https://github.com/otomad/VegasScripts/releases/latest"><img src="https://img.shields.io/badge/-最新版をダウンロードするにはクリック！-brightgreen?style=for-the-badge&logo=data:image/svg+xml;base64,PHN2ZyByb2xlPSJpbWciIHZpZXdCb3g9IjAgMCAyNCAyNCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNMTEuMiAwYS44LjggMCAwIDAtLjguOHYxMS40TDcuMjYgOS40NGEuODAzLjgwMyAwIDAgMC0xLjEzLjA3NGwtMS4wNSAxLjJhLjguOCAwIDAgMCAuMDczIDEuMTNsNi4zMyA1LjU0YS43OTUuNzk1IDAgMCAwIDEuMDUgMGw2LjMyLTUuNTRhLjguOCAwIDAgMCAuMDc0LTEuMTNsLTEuMDUtMS4yYS44MDQuODA0IDAgMCAwLTEuMTMtLjA3NGwtMy4xNCAyLjc2Vi44YS44LjggMCAwIDAtLjgtLjh6bS04IDIwLjhhLjguOCAwIDAgMC0uOC44djEuNmEuOC44IDAgMCAwIC44LjhoMTcuNmEuOC44IDAgMCAwIC44LS44di0xLjZhLjguOCAwIDAgMC0uOC0uOHoiIGZpbGw9IndoaXRlIi8+PC9zdmc+" alt="Download" /></a></p>

[EN](README.md) | [简中](README_zh-CN.md) | [繁中](README_zh-TW.md) | **日**

ベガスの音MADヘルパー、VegasがMIDIシーケンスファイルを入力として受け入れ、音MAD/YTPMVトラックを自動的に生成できるように設計されています。

このスクリプトは、元の作成者 [@Chaosinism](https://github.com/Chaosinism) のオープンソースコードの二次開発に基づいており、NAudioライブラリを使用しています。

YTPを作成することも可能です。 将来的にはドラムビート範囲切除・歌詞/カラオケ・ボカロ/鬼畜・シュペランカー戦法の作成にも使用できます。

将来的には、他のユーザーが作成したテンプレートやその他の関連資料をダウンロードするためのストア機能が追加される予定です。

YTPの関連機能は[YTP+](https://github.com/YTP-Plus)から参照。

### 使用法
「トラック」ウィンドウでクリップを選択するか、「プロジェクトメディア」ウィンドウでクリップを選択するか、手動で参照して他のクリップを選択します（「スクリプト」ウィンドウを開いた後）。 その後、スクリプト構成を開いて生成できます。

メニューで *オプション > ツールバーのカスタマイズ* を選択して、簡単な操作のためにスクリプトをツールバーに追加できます。

[@Evauation](https://github.com/Evauation) の[ドキュメンテーション](https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng/edit)を読んだり、[@Cassidy](https://github.com/composition-cassidy) の[チュートリアルビデオ](https://www.youtube.com/watch?v=8vSpzgL_86A)を視聴したりすることもできます *（英語）*。

### インストール
1. スクリプトの最新バージョンを[ダウンロード](https://github.com/otomad/VegasScripts/releases/latest)します。
2. zipファイル内の**すべて**のファイルを解凍し、Vegasインストールディレクトリの下の「スクリプトメニュー」フォルダに配置します。
> (例えば：C:\Program Files\VEGAS\VEGAS Pro 17.0\Script Menu)
3. DLLファイル (DLL/NAudio.dll) がロックされていないことを確認してください。**具体的な手順：**
	1. Vegasのインストールディレクトリで、Script Menu/DLLフォルダを順番に入力します。
	2. NAudio.dllファイルを右クリックし、［プロパティ］を選択します。
	3. 「ロック解除」ボタンが表示されたら、それをクリックします。
	4. ［プロパティ］ダイアログを閉じれば完了です。
4. Vegas Proを開いてスクリプトを開始します。メニューの *ツール > スクリプト > Otomad Helper* を選択します。

### **注意**
Vegas Pro 13 以降をサポートします。

Vegas 16 以降はすべての機能をサポートしており、Vegas 13〜15 は互換性のある実行が可能です（一部の機能が欠落しています）。対応するバージョンをインストールする必要があることが前提です。

Vegas Pro 17 および 19 は**正常**にテストされました。

### 問題
スクリプトを使用する場合、次の場合：
1. 問題が発生しました；
2. バグを見つけました；
3. 新しい提案やアイデアがあります；
4. プロジェクトの維持を支援する；
5. インターフェイスの外観を最適化する；
6. 翻訳の問題を修正する；
7. 新しい翻訳を送信する；
8. ……

「issues」を提出できます。

### ロードマップ
[ロードマップ >](ROADMAP.md)

### 権限
スクリプトを使用する場合、次の権限が使用されます。
1. ファイルの読み取りと書き込み。<br />
	スクリプトは、ユーザー構成設定を保持するファイルをディスク上に作成します。
2. レジストリの読み取りと書き込み。<br />
	Pitch Shiftプラグインプリセットをインストールまたはアンインストールするには、スクリプトでレジストリを読み書きする必要があります。<br />
	Pitch Shiftプラグインのチューニングアルゴリズムを使用しない場合は、この権限を無視できます。

### 参考
* [https://github.com/Chaosinism/vegas_scripts](https://github.com/Chaosinism/vegas_scripts)
* [https://github.com/evankale/VegasScripts](https://github.com/evankale/VegasScripts)
* [https://github.com/naudio/NAudio](https://github.com/naudio/NAudio)
* [https://github.com/YTP-Plus](https://github.com/YTP-Plus)
* [https://www.jetdv.com/](https://www.jetdv.com/)
* [https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/](https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/)

### アイコン
に触発された：
* [@氷鳩さくの](https://space.bilibili.com/13084550)
* [@酥妃大魔王](https://space.bilibili.com/8569439)

### ヘルプとトラブルシューティング
**私のドキュメンテーション：***（中国語）*
* [リリースノート (v4.9.25.0)](https://www.bilibili.com/read/cv13335178)
* [リリースノート (v4.10.17.0)](https://www.bilibili.com/read/cv13614419)

**Chaosinismの元のドキュメンテーション：***（中国語）*
* [ドキュメンテーション (v0.1) (ビリビリ)](https://www.bilibili.com/read/cv392013)
* [ステーブ視覚化のドキュメンテーション (v0.1)](https://www.bilibili.com/read/cv1027442)
* [トラブルシューティング](https://www.bilibili.com/read/cv495309)
* [チュートリアルビデオ (v0.1)](https://www.bilibili.com/video/av22226321)

**Chaosinismの元のドキュメンテーション：***（日本語）*
* [ドキュメンテーション (v0.1) (ボウルロール)](https://bowlroll.net/user/261124)
