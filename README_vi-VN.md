<div lang="vi">

[![Cover](cover.png)](#otomad-helper)
<div align="center">
	<h2 id="otomad-helper">Otomad Helper</h2>
	<p><b><i>Lan Âm</i></b></p>
	<p><a href="https://github.com/otomad/OtomadHelper/releases/latest"><img src="https://img.shields.io/badge/-Tải%20ngay%20phiên%20bản%20mới%20nhất!-brightgreen?style=for-the-badge&logo=data:image/svg+xml;base64,PHN2ZyByb2xlPSJpbWciIHZpZXdCb3g9IjAgMCAyNCAyNCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNMTEuMiAwYS44LjggMCAwIDAtLjguOHYxMS40TDcuMjYgOS40NGEuODAzLjgwMyAwIDAgMC0xLjEzLjA3NGwtMS4wNSAxLjJhLjguOCAwIDAgMCAuMDczIDEuMTNsNi4zMyA1LjU0YS43OTUuNzk1IDAgMCAwIDEuMDUgMGw2LjMyLTUuNTRhLjguOCAwIDAgMCAuMDc0LTEuMTNsLTEuMDUtMS4yYS44MDQuODA0IDAgMCAwLTEuMTMtLjA3NGwtMy4xNCAyLjc2Vi44YS44LjggMCAwIDAtLjgtLjh6bS04IDIwLjhhLjguOCAwIDAgMC0uOC44djEuNmEuOC44IDAgMCAwIC44LjhoMTcuNmEuOC44IDAgMCAwIC44LS44di0xLjZhLjguOCAwIDAgMC0uOC0uOHoiIGZpbGw9IndoaXRlIi8+PC9zdmc+" alt="Download" /></a></p>
	
[EN](README.md) | [简中](README_zh-CN.md) | [繁中](README_zh-TW.md) | [日](README_ja-JP.md) | **VI**
</div>

**Otomad Helper** dành cho **Vegas**, được thiết kế để cho phép Vegas chấp nhận các file MIDI làm đầu vào và tự động tạo các track Otomad/YTPMV.

Script được phát triển lại dựa theo mã nguồn mở của tác giả [@Chaosinism](https://github.com/Chaosinism) và sử dụng thư viện NAudio.

Nó cũng có thể được sử dụng để tạo YTP, Sonar Effect, Datamosh. Và Lyrics/Karaoke, Manual Vocaloid/Sentence Mixing, Kĩ thuật Shupelunker...v.v.... sẽ được hỗ trợ trong tương lai.

Cửa hàng (Store) sẽ được hỗ trợ tải các template liên quan trong tương lai.

Các tính năng liên quan của YTP đề cập đến ở [YTP+](https://github.com/YTP-Plus).

Datą̬͉̫̐͑̓̄ͅa̸͎͇͗̌͂̈̀ą̸̝̼̦̤̇̐ǎ̛͍́̑a̸̲͙͛̐̄̎̚͜a̢̨̝̟͎̾̔̊ǎ̤̞͈͑a͈̪̣̍mo̻̪̬̘̲͆͂͠o̸͍̞͔̓̆̊̀o̗͊̇̇̈́̇ǫ͇͗̏̕͜ơ̬͍͚̦̯̓̊͌ò͈̦̫̈́̓o̦̣̲̊̀o̪̪͚̺̘͛̽̏̈́sh là một kĩ thuật làm biến dạng video để tạo hiệu ứng glitch. Các tính năng liên quan của Datamosh đề cập đến ở [vegas-datamosh](https://github.com/delthas/vegas-datamosh).

Tính năng Camera Shake đề cập đến ở [VegasShakeScript](https://github.com/tmarplatt/VegasScripts).

Phiên bản Việt hoá của script do [@Cyahega](https://github.com/Cyahega) đóng góp.

**Các dự án khác:** [om_midi dành cho After Effects](https://github.com/otomad/om_midi).

### Cách sử dụng 
Chọn track event nguồn trong track window, hoặc chọn file phương tiện nguồn trong media pool window, hoặc duyệt (browse) tìm file phương tiện khác (trong biểu mẫu cấu hình/thiết lập script), rồi mở script để chỉnh sửa và tạo ra.

Bạn có thể thêm script thành nút toolbar thay vì luôn phải click vào menu phụ *Tools > Scripting*, bằng cách thêm vào toolbar thông qua menu *Options > Customize Toolbar*.

Bạn cũng có thể đọc  [tài liệu](https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng/edit) của [@Evauation](https://github.com/Evauation) và xem [video hướng dẫn](https://www.youtube.com/watch?v=8vSpzgL_86A) của [@Cassidy](https://github.com/composition-cassidy) *(Tiếng Anh)*.

[Bấm vào đây để xem giới thiệu về Datamosh](Datamosh/README.md). Bạn cũng có thể [xem video](https://www.youtube.com/watch?v=6D2lW6H0bb8) *(Tiếng Anh)*.

### Hướng dẫn cài đặt
1. [Tải xuống](https://github.com/otomad/OtomadHelper/releases/latest) phiên bản mới nhất của script này.
2. Giải nén **TẤT CẢ** file trong file zip bạn vừa tải về tới thư mục "Script Menu" trong đường dẫn cài đặt của Vegas.
> (Ví dụ. C:\\Program Files\\VEGAS\\VEGAS Pro 20.0\\Script Menu)
3. Đảm bảo rằng là file DLL `(DLL\NAudio.dll)` không bị khoá. **Các bước cụ thể:**
	1. Trong đường dẫn cài đặt của Vegas, vào thư mục `Script Menu\DLL`.
	2. Chuột phải vào file NAudio.dll và chọn "**Properties**".
	3. Nếu bạn thấy nút "**Unlock**" hoặc checkbox, bấm vào hoặc tích vào checkbox.
	4. Bấm nút OK là xong.
4. Mở Vegas Pro để khởi chạy. Chọn menu *Tools > Scripting > Otomad Helper* để mở.

#### Datamosh Extension Pack
Nếu bạn cần sử dụng đầy đủ chức năng của Datamosh, bạn cần cài đặt Datamosh extension pack.

1. [Tải xuống](https://github.com/otomad/OtomadHelper/releases/tag/v1.0-datamosh) Datamosh extension pack.
2. Giải nén file 7z và di chuyển thư mục `_internal` tới cùng đường dẫn như script `Otomad Helper.cs`.

### **Độ tương thích**
Vegas Pro 13+ được hỗ trợ.

Vegas Pro 16 và các phiên bản mới hơn trên phiên bản này hỗ trợ tất cả tính năng, và Vegas Pro 13 ~ 15 tương thích để chạy script (Một vài tính năng sẽ thiếu). Cần phải cài đúng phiên bản của script

Đã test **ổn định** ở Vegas Pro 17, 18, 19, 20.

### Chú giải
Bạn có thể tìm hiểu về nhiều tính năng dễ dàng hơn thông qua hình ảnh.

[Chú giải >](glossary.md)

### Khám phá các hiệu ứng Visual
[![Khám phá các hiệu ứng Visual](covers/otomad_helper_visual_effects_example.png)](https://youtu.be/cY2Qa3Owetw)

### Lịch sử cập nhật phiên bản chính
Đây là các bức ảnh chụp màn hình về UI (giao diện người dùng) của script dành cho lịch sử cập nhật phiên bản chính.

[Lịch sử cập nhật phiên bản chính >](history/README.md)

### Roadmap
[Roadmap >](roadmap.md)

### Các vấn đề gặp phải (Issues)
Khi bạn đang dùng script, nếu bạn:
1. Có một vấn đề trong script;
2. Tìm thấy bug;
3. Gửi một số đề xuất hoặc ý tưởng mới;
4. Giúp chúng tôi hợp tác sản xuất;
5. Tối ưu hóa giao diện;
6. Chỉnh sửa các bản dịch hiện có;
7. Cung cấp bản dịch cho ngôn ngữ mới;
8. …

Bạn có thể tạo issues trên trang GitHub này.

### Điều khoản sử dụng
1. Tôn trọng quyền và lợi ích của tác giả khi sáng tạo video bằng MIDI/nguồn tư liệu không tự tạo ra.
2. Sử dụng script này yêu cầu một số khả năng sáng tạo từ người dùng. Script này nên và chỉ nên được sử dụng để hỗ trợ trong việc sáng tạo video/audio. Không được dựa vào script này để tạo sản phẩm hoặc học cách tạo Otomad/YTPMV khi là người mới bắt đầu.  Nghe này, không ai muốn làm Otomidi đâu!

### Cấp quyền 
Nếu bạn dùng script này, script sẽ sử dụng các quyền sau.
1. Đọc và ghi file.<br />
	Script đọc và ghi một INI user profile trong bộ nhớ ổ đĩa của bạn để lưu dữ liệu người dùng của bạn.
	> Đường dẫn: C:\\Users\\*(Username của bạn)*\\AppData\\Roaming\\VEGAS Pro\\*(Phiên bản Vegas của bạn)*\\Otomad Helper.ini
2. Đọc và ghi registry.<br />
	Script cần đọc và ghi registry để cài đặt hoặc gỡ cài đặt preset pitch-shift plugin.<br />
	Nếu bạn không dùng phương pháp pitch shift plugin trong script, bạn có thể bỏ qua việc cài đặt preset và do đó không sử dụng quyền này.
	> Đường dẫn: HKEY_CURRENT_USER\\Software\\DirectShow\\Presets\\{ED1B4100-93BE-11D0-AEBC-00A0C9053912}

### Chú thích
* [Script Otomad Helper **bản gốc** của Chaosinism](https://github.com/Chaosinism/vegas_scripts)
* [**Vegas Scripts** của Evan Kale](https://github.com/evankale/VegasScripts)
* [Thư viện âm thanh **NAudio** .NET của Mark Heath](https://github.com/naudio/NAudio)
* [**YTP+** của Ben Brown, Kiwifruitdev, Nuppington](https://github.com/YTP-Plus)
* [Script **JETDV** của Edward](https://www.jetdv.com/)
* [**Diễn đàn** Vegas Pro Scripting](https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/)
* [**Alpha Color Dialog** của Opulos](https://sourceforge.net/projects/alpha-color-dialog/)
* [**Progress Dialog** của Ookii Dialogs WinForms](https://github.com/ookii-dialogs/ookii-dialogs-winforms)
* [**Vegas Datamosh** của Delthas](https://github.com/delthas/vegas-datamosh)
* [**VegasShakeScript** của Tmarplatt](https://github.com/tmarplatt/VegasScripts)

<!-- ### Logo
Lấy cảm hứng từ:
* [@Băng Cưu Anh Nãi](https://space.bilibili.com/13084550)
* [@Tô Phi Đại Ma Vương](https://space.bilibili.com/8569439) -->

### Trợ giúp và Khắc phục sự cố
**Các tài liệu của tôi:** *(Tiếng Trung)*
* [Ghi chú phát hành (v4.9.25.0)](https://www.bilibili.com/read/cv13335178)
* [Ghi chú phát hành (v4.10.17.0)](https://www.bilibili.com/read/cv13614419)

**Các tài liệu gốc của Chaosinism:** *(Tiếng Trung)*
* [Tài liệu (v0.1) (bilibili)](https://www.bilibili.com/read/cv392013)
* [Tài liệu dành cho Staff Visualizer (v0.1)](https://www.bilibili.com/read/cv1027442)
* [Khắc phục sự cố](https://www.bilibili.com/read/cv495309)
* [Video hướng dẫn (v0.1)](https://www.bilibili.com/video/av22226321)

**Các tài liệu gốc của Chaosinism:** *(Tiếng Nhật)*
* [Tài liệu (v0.1) (bowlroll)](https://bowlroll.net/user/261124)

### Giấy phép
* Dự án này nằm dưới giấy phép GPL 3.0.
	* Script gốc của Chaosinism nằm dưới giấy phép LGPL 3.0.
		* Script của Evan Kale nằm dưới giấy phép GPL 3.0.
			* NAudio nằm dưới giấy phép MIT.
* YTP+ nằm dưới giấy phép GPL 3.0.
* Ookii Dialogs WinForms nằm dưới giấy phép Điều khoản BSD 3 (BSD 3 Clause).
* Vegas Datamosh của Delthas nằm dưới giấy phép MIT.
	* FFmpeg nằm dưới giấy phép LGPL + GPL.
	* Avidemux nằm dưới giấy phép GPL.
	* Xvid codec nằm dưới giấy phép GPL.

</div>
