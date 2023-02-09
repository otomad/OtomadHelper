<div lang="ja">

[![Cover](cover.png)](#otomad-helper)
<div align="center">
	<h2 id="otomad-helper">音MADヘルパー</h2>
	<p><b>蘭音</b></p>
	<p><a href="https://github.com/otomad/OtomadHelper/releases/latest"><img src="https://img.shields.io/badge/-ここをクリックして最新版をダウンロード！-brightgreen?style=for-the-badge&logo=data:image/svg+xml;base64,PHN2ZyByb2xlPSJpbWciIHZpZXdCb3g9IjAgMCAyNCAyNCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNMTEuMiAwYS44LjggMCAwIDAtLjguOHYxMS40TDcuMjYgOS40NGEuODAzLjgwMyAwIDAgMC0xLjEzLjA3NGwtMS4wNSAxLjJhLjguOCAwIDAgMCAuMDczIDEuMTNsNi4zMyA1LjU0YS43OTUuNzk1IDAgMCAwIDEuMDUgMGw2LjMyLTUuNTRhLjguOCAwIDAgMCAuMDc0LTEuMTNsLTEuMDUtMS4yYS44MDQuODA0IDAgMCAwLTEuMTMtLjA3NGwtMy4xNCAyLjc2Vi44YS44LjggMCAwIDAtLjgtLjh6bS04IDIwLjhhLjguOCAwIDAgMC0uOC44djEuNmEuOC44IDAgMCAwIC44LjhoMTcuNmEuOC44IDAgMCAwIC44LS44di0xLjZhLjguOCAwIDAgMC0uOC0uOHoiIGZpbGw9IndoaXRlIi8+PC9zdmc+" alt="Download" /></a></p>

[EN](README.md) | [简中](README_zh-CN.md) | [繁中](README_zh-TW.md) | **日本語** | [VI](README_vi-VN.md)
</div>

**音MADヘルパー Vegas**とは、Vegas に MIDI シーケンスファイルを読み込まさせて、音MAD / YTPMV トラックを自動的に生成するのスクリプトです。

このスクリプトは、元開発者[@Chaosinism](https://github.com/Chaosinism)さんのオープンソースコードに基づいて二次開発されたものであり、NAudio ライブラリを使用しています。

YTP・ソナーエフェクト・データモッシュも作れます。将来的には歌詞/カラオケ・ボカロ/ラップ・シュペランカー戦法の作成にも使えます。

他のユーザーが作成したテンプレートやその他の関連素材をダウンロードすることができるのストア機能も将来に追加する予定です。

YTP の関連機能は [YTP+](https://github.com/YTP-Plus) から参照しています。

縺ﾃﾞ繧ｰﾀ繝ﾓ譌ｯ縲｢ｼ譛ｭ･は素材に損傷を与えてグリッチ効果を作成する技術です。データモッシュの関連機能は [Vegas データモッシュ](https://github.com/delthas/vegas-datamosh)から参照しています。

手ぶれ機能とは [Vegas シェイク スクリプト](https://github.com/tmarplatt/VegasScripts) から参照しています。

ベトナム語翻訳は[@Cyahega](https://github.com/Cyahega)によって提供されています。

**姉妹プロジェクト：**[om_midi for After Effects](https://github.com/otomad/om_midi)。

### 使い方
「トラック」ウィンドウで素材アイテムを選択するか、「プロジェクトメディア」ウィンドウで素材アイテムを選択するか、「スクリプト」ウィンドウを呼び出してから手動で他の素材を選択します。そしてスクリプトで設定して、動画を作成します。

より簡単に操作できるには、*オプション > ツールバーのカスタマイズ* メニューで、このスクリプトツールバーに追加することができます。

また、[@Evauation](https://github.com/Evauation)さんの[ドキュメント](https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng/edit)と、[@cassidy](https://github.com/composition-cassidy)さんの[チュートリアル動画](https://www.youtube.com/watch?v=8vSpzgL_86A)もご参考になれると思います。<wbr />*（英語）*。

データモッシュの使い方については、[ここをクリックしてください](Datamosh/README.md)。または[動画を観ます](https://www.youtube.com/watch?v=6D2lW6H0bb8)<wbr />*（英語）*。

### インストール
1. スクリプトの最新バージョンを[ダウンロードします](https://github.com/otomad/OtomadHelper/releases/latest)。
2. Vegas インストールディレクトリの下の「Script Menu」フォルダにダウンロードした ZIP ファイル内の**すべて**のファイルを解凍します。
> (例えば：C:\\Program Files\\VEGAS\\VEGAS Pro 20.0\\Script Menu)
3. DLL ファイル `(DLL\NAudio.dll)` がロックされていないことを確認してください。**詳しい手順：**
	1. Vegas のインストールディレクトリで、`Script Menu\DLL` フォルダを開きます。
	2. ファイル NAudio.dll を右クリックし、［プロパティ］をクリックします。
	3. ［ロック解除］ボタンまたはチェックボックスが表示されている場合は、クリックするまたはチェックを外します。
	4. ［OK］ボタンをクリックすると、完了になります。
4. Vegas Pro を立ち上げてスクリプトを実行します。メニューから *ツール > スクリプト > Otomad Helper* を選択します。

#### データモッシュ拡張パッケージ
データモッシュのすべての機能を使いたい場合では、データモッシュ拡張パッケージのインストールが必要となります。

1. データモッシュ拡張パックを[ダウンロードします](https://github.com/otomad/OtomadHelper/releases/tag/v1.0-datamosh)。
2. 7Z ファイルを解凍し、`_internal` フォルダーをスクリプト `Otomad Helper.cs` と同じフォルダに移動します。

### **互換性**
Vegas Pro 13 以降。

Vegas 16 以降はすべての機能がサポートされており、Vegas 13〜15 にも実行できます（だが一部の機能が欠落になります）。対応するバージョンがインストールされていることが前提です。

Vegas Pro 17、18、19、20 で**正常**に作動します。

### 用語集
画像を通して多くの機能をより簡単に知ることができます。

[用語集 >](glossary.md)

### ビジュアルエフェクトの探索
[![ビジュアルエフェクトの探索](covers/otomad_helper_visual_effects_example.png)](https://youtu.be/cY2Qa3Owetw)

### メジャーバージョンの更新履歴
これらは過去のメジャーバージョンアップのスクリプトUIのスクリーンショットです。

[メジャーバージョンの更新履歴 >](history/README.md)

### ロードマップ
[ロードマップ >](roadmap.md)

### イシューズ
スクリプトを使用中に次のような状況がありましたら：
1. 問題が発生しました；
2. バグを見つけました；
3. 新しい提案やアイデアを伝えたい；
4. 当プロジェクトのメンテナンスに協力したい；
5. UIの外観を最適化したい；
6. 訳文を訂正したい；
7. 新しい訳文を提出したい；
8. ……

気軽で「Issues」で提出してください。

### 利用規約
1. 非自作 MIDI / 素材を使用して創作する場合は、その素材の利用規約もしっかり守ってください。
2. 当スクリプトを使用するには、使用者には一定の創作能力を持つ必要があります。当スクリプトは作成補助に限り使用すべきものであり、または初心者が音MAD / YTPMV の作り方を勉強するために使います。当スクリプトだけで作品を作ってはいけません。頼む、音MIDIを作らないでください！

### 権限
スクリプトを使用するには次の権限が要求されます。
1. ファイルの読み書き。<br />
	スクリプトはユーザー構成設定を保存するためディスク上にファイルを作成します。
	> パス：C:\\Users\\*(あなたのユーザーネーム)*\\AppData\\Roaming\\VEGAS Pro\\*(Vegas バージョン)*\\Otomad Helper.ini
2. レジストリの読み書き。<br />
	ピッチシフトプラグインプリセットをインストールまたはアンインストールするには、スクリプトでレジストリを読み書きで実現します。<br />
	ピッチシフトプラグインのチューニングアルゴリズムを使用しない場合は、この権限を無視してもいいです。
	> パス：HKEY_CURRENT_USER\\Software\\DirectShow\\Presets\\{ED1B4100-93BE-11D0-AEBC-00A0C9053912}

### 参考（敬称略）
* [Chaosinism の**元祖**音MADヘルパー スクリプト](https://github.com/Chaosinism/vegas_scripts)
* [evankale の **Vegas スクリプト集**](https://github.com/evankale/VegasScripts)
* [markheath の **NAudio** .NET オーディオ ライブラリ](https://github.com/naudio/NAudio)
* [Ben Brown, Kiwifruitdev, Nuppington の **YTP+**](https://github.com/YTP-Plus)
* [Edward の **JETDV** スクリプト集](https://www.jetdv.com/)
* [Vegas Pro スクリプト **フォーラム**](https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/)
* [Opulos の**アルファ付きカラーピッカー**](https://sourceforge.net/projects/alpha-color-dialog/)
* [Ookii Dialogs WinForms の**進捗ダイアログ**](https://github.com/ookii-dialogs/ookii-dialogs-winforms)
* [Delthas の **Vegas データモッシュ**](https://github.com/delthas/vegas-datamosh)
* [Tmarplatt の **Vegas シェイク スクリプト**](https://github.com/tmarplatt/VegasScripts)

<!-- ### アイコン
のインスピレーションは以下の方から得ました（敬称略）：
* [@氷鳩さくの](https://space.bilibili.com/13084550)
* [@酥妃大魔王](https://space.bilibili.com/8569439) -->

### ヘルプとトラブルシューティング
**私のドキュメント：***（中国語）*
* [リリースノート (v4.9.25.0)](https://www.bilibili.com/read/cv13335178)
* [リリースノート (v4.10.17.0)](https://www.bilibili.com/read/cv13614419)

**Chaosinismの元祖版スクリプトのドキュメント：***（中国語）*
* [ドキュメント (v0.1) (ビリビリ)](https://www.bilibili.com/read/cv392013)
* [五線譜可視化のドキュメント (v0.1)](https://www.bilibili.com/read/cv1027442)
* [トラブルシューティング](https://www.bilibili.com/read/cv495309)
* [チュートリアル動画 (v0.1)](https://www.bilibili.com/video/av22226321)

**Chaosinismの元祖版スクリプトのドキュメント：***（日本語）*
* [ドキュメント (v0.1) (ボウルロール)](https://bowlroll.net/user/261124)

<!-- ### ライセンス
* 当プロジェクトは GPL 3.0 に準拠しています。
	* Chaosinismのオリジナルスクリプトは LGPL 3.0 に準拠しています。
		* evankaleのスクリプトは GPL 3.0 に準拠しています。
			* NAudio は MIT に準拠しています。
* YTP+ は GPL 3.0 に準拠しています。
* Ookii Dialogs WinFormsは BSD 3 Clause に準拠しています。
* デルタスの Vegas データモッシュ は under the MIT に準拠しています。
	* FFmpeg は LGPL + GPL に準拠しています。
	* Avidemux は GPL に準拠しています。
	* Xvid codec は GPL に準拠しています。 -->
### ライセンス
* OtomadHelper（当プロジェクト）  
GPL 3.0
* VegasScripts (Chaosinismのオリジナルスクリプト)  
LGPL 3.0
* NAudio  
MIT
* YTP+  
GPL 3.0
* Ookii Dialogs WinForms  
BSD 3 Clause
* vegas-datamosh  
MIT
* FFmpeg  
LGPL + GPL
* Avidemux  
GPL
* Xvid codec  
GPL
</div>
