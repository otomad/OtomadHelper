@en # References
@zh # 参考

@en Otomad Helper is licensed under the [GPL 3.0][gpl-3].
@zh Otomad Helper基于[GPL 3.0][gpl-3]许可发布.

@en **Project Start Date:** Sunday, September 5th, Anno Domini 2021 4:14:26 AM +08:00
@zh 项目开工日期：公元2021年9月5日 星期日，凌晨4:14:26 东八区时间

@en Copyright © 2021–present - Licensed under GPLv3
@zh 版权所有 © 2021~至今，兰音 - 根据GPLv3许可

@en ## Terms of Use
@zh ## 使用条款

@en 1. Respect the rights and interests of authors when creating with non-self-made MIDI/sources/materials.
@zh 1. 若使用非自制MIDI / 素材进行创作，请尊重其作者的权益。
@en 2. Using this script requires some creative abilities from the users. This script should and should only be used to assist in creations. Do not rely solely on this script to generate productions nor learn to creating YTPMV/otoMAD as a beginner. Listen, don’t make **YTPMIDI/otoMIDI**!
@zh 2. 使用此脚本需要用户具备一定的创作能力，此脚本应当且仅应当用于辅助进行创作。不得纯依靠此脚本生成作品，也不应作为初学者学习创作音MAD/YTPMV使用。听话，不要做**音MIDI**！
@en 3. After using this script to create a video and posting it to media platforms, please do not mention the name or website link of this script anywhere—such as video description and comments—to claim that this script was participated in the creation.
@zh 3. 使用此脚本制作视频并投稿到各大媒体平台后，请勿在任何地方（如视频简介和评论区）提及此脚本的名称或网址链接以声称使用此脚本参与了创作。

@en ## Permissions
@zh ## 权限

@en If you use this script, the script will use the following permissions.
@zh 使用脚本时，将会使用如下权限：

@en 1. File read and write.
@zh 1. 文件读写。
@en    1. The script reads and writes an INI user profile in your disk memory to save your user data.
@zh    1. 脚本会在磁盘中创建一个ini文件用来保存你的用户配置设置。
       ::: code-group
@en       ```[Path]
@zh       ```[路径]
@en       C:\Users\(Your User Name)\AppData\Roaming\VEGAS Pro\(Your Vegas Version)\Otomad Helper.ini
@zh       C:\Users\(你的用户名)\AppData\Roaming\VEGAS Pro\(你的Vegas版本)\Otomad Helper.ini
       ```
       :::
@en 2. Registry read and write.
@zh 2. 注册表读写。
@en    1. The script need to read and write the registry to install or uninstall pitch-shift plugin presets.
@zh    1. 脚本需要读写注册表来安装或卸载移调插件预设。
       ::: code-group
@en       ```[Path]
@zh       ```[路径]
       HKEY_CURRENT_USER\Software\DirectShow\Presets\{ED1B4100-93BE-11D0-AEBC-00A0C9053912}
       ```
       :::
@en    2. Configurations related to Datamosh are saved in the registry.
@zh    2. 数据抹失的相关配置保存在注册表中。
       ::: code-group
@en       ```[Path]
@zh       ```[路径]
       HKEY_CURRENT_USER\SOFTWARE\VEGAS Creative Software\Custom Presets
       ```
       :::

@en ## Credits
@zh ## 开源软件许可

@en This project references the following open source code:
@zh 此项目参考了以下开源代码：
@en We would like to thank their authors and contributors for their contributions to the open source cause!
@zh 感谢作者和贡献者对开源事业的贡献！

@@@en
Project | Authors | License | Notes
---- | ---- | ---- | ----
[**Otomad Helper**][this-repo] | *N/A* | GPL 3.0 | *Current project*
[otomad_helper][chaosinism-scripts] | Chaosinism | LGPL 3.0 | *The original project this is based on*
[VegasScripts][evankale-scripts] | Evan Kale | GPL 3.0 | *The project Chaosinism’s referenced*
[NAudio][naudio] | Mark Heath | MIT | *MIDI read/write<br>Waveform sound*
[YTP+][ytp-plus] | Ben Brown<br>Kiwifruitdev<br>Nuppington | GPL 3.0 | *YTP feature reference*
[Alpha Color Dialog][alpha-color-dialog] | Opulos | BSD | *Color picker with alpha*
[Ookii Dialogs WinForms][ookii-dialogs] | C. Augusto Proiete | BSD 3-Clause | *Vista-style progress dialog<br>Vista-style folder dialog*
[Vegas Camera Shake Script][camera-shake] | Tommy Marplatt | None | *Camera shake effect*
@@@zh
项目 | 作者 | 协议 | 备注
---- | ---- | ---- | ----
[**Otomad Helper**][this-repo] | 兰音 | GPL 3.0 | *当前项目*
[otomad_helper][chaosinism-scripts] | Chaosinism | LGPL 3.0 | *基于的原始项目*
[VegasScripts][evankale-scripts] | Evan Kale | GPL 3.0 | *Chaosinism参考的项目*
[NAudio][naudio] | Mark Heath | MIT | *MIDI读写<br>波形发声*
[YTP+][ytp-plus] | Ben Brown<br>Kiwifruitdev<br>Nuppington | GPL 3.0 | *YTP功能参考*
[Alpha Color Dialog][alpha-color-dialog] | Opulos | BSD | *带透明度的取色器*
[Ookii Dialogs WinForms][ookii-dialogs] | C. Augusto Proiete | BSD 3 Clause | *Vista风格的进度对话框<br>Vista风格的文件夹对话框*
[Vegas Camera Shake Script][camera-shake] | Tommy Marplatt | None | *镜头摇晃效果*
@@@

@en ### Vegas Datamosh Libs
@zh ### Vegas数据抹失依赖

@en The following libs were used in [Vegas Datamosh][delthas-datamosh] Extension Pack.
@zh [Vegas数据抹失][delthas-datamosh]扩展包使用了这些依赖。

@@@en
Project | Authors | License | Notes
---- | ---- | ---- | ----
[Vegas Datamosh][delthas-datamosh] | Delthas | MIT | *Datamosh features*
[FFmpeg][ffmpeg] | Fabrice Bellard<br>Michael Niedermayer<br>[View All ▾][ffmpeg-authors] | LGPL 2.1+<br>GPL 2.0+ *(optional parts)* | *Transcoding videos to XviD*
[Avidemux][avidemux] | Mean<br>[View All ▾][avidemux-authors] | GPL 2.0 | *Edit video frame info*
[Xvid codec][xvid-codec] | [View All ▾][xvid-codec-authors] | GPL 2.0 | *XviD encoding support*
@@@zh
项目 | 作者 | 协议 | 备注
---- | ---- | ---- | ----
[Vegas Datamosh][delthas-datamosh] | Delthas | MIT | *数据抹失功能*
[FFmpeg][ffmpeg] | Fabrice Bellard<br>Michael Niedermayer<br>[查看全部 ▾][ffmpeg-authors] | LGPL 2.1+<br>GPL 2.0+ *(optional parts)* | *转码视频为XviD*
[Avidemux][avidemux] | Mean<br>[查看全部 ▾][avidemux-authors] | GPL 2.0 | *编辑视频帧信息*
[Xvid codec][xvid-codec] | [查看全部 ▾][xvid-codec-authors] | GPL 2.0 | *XviD编码支持*
@@@

@en ## Old Documentation Links
@zh ## 旧文档链接

@@@en
Post | Author | Language | Kind | Last Updated
---- | ---- | ---- | ---- | ----
[<SocialIcon icon="google" />Otomad Helper Features and Details][documentation_evauation] | Evauation | English | Article
[<SocialIcon icon="bilibili" />Release Notes (v4.10.17.0)][releaseNotes_v4_10_17_0] | *N/A* | Chinese | Article | <Date value="2021/10/17" />
[<SocialIcon icon="bilibili" />Release Notes (v4.9.25.0)][releaseNotes_v4_9_25_0] | *N/A* | Chinese | Article | <Date value="2021/09/26" />
[<SocialIcon icon="bilibili" />Documentation (v0.1)][documentation_chaosinism]<br>[<SocialIcon icon="bowlroll" />Documentation (v0.1)][documentation_chaosinism_ja] | Chaosinism | Chinese<br>Japanese | Article | <Date value="2019/09/01" />
[<SocialIcon icon="bilibili" />Documentation for Staff Visualizer (v0.1)][documentation_staffVisualizer_chaosinism] | Chaosinism | Chinese | Article | <Date value="2018/08/24" />
[<SocialIcon icon="bilibili" />Troubleshooting (v0.1)][troubleshooting_chaosinism] | Chaosinism | Chinese | Article | <Date value="2018/05/19" />
[<SocialIcon icon="youtube" />Tutorial Video (v4.26.14.0)][tutorialVideo]<br>[<SocialIcon icon="bilibili" />Tutorial Video (v4.26.14.0)][tutorialVideo_v4_26_14_0] | *N/A* | English<br>Chinese | Video | <Date value="2023/04/27" />
[<SocialIcon icon="youtube" />Explore Visual Effects][exploreVisualEffects] | *N/A* | English | Video | <Date value="2022/11/19" />
[<SocialIcon icon="youtube" />Tutorial Video (v4.16.4.0)][tutorialVideo_greenBean] | GreenBean | English | Video | <Date value="2022/05/09" />
[<SocialIcon icon="youtube" />Tutorial Video (v4.26.14.0)][tutorialVideo_cyahega] | Cyahega | Vietnamese | Video | <Date value="2023/03/11" />
[<SocialIcon icon="bilibili" />Tutorial Video (v0.1)][tutorialVideo_chaosinism] | Chaosinism | Chinese | Video | <Date value="2018/04/17" />
[<SocialIcon icon="youtube" />Tutorial video for Datamosh (v1.4.0)][tutorialVideo_datamosh_delthas] | Delthas | English | Video | <Date value="2020/09/24" />
[<SocialIcon icon="youtube" />Tutorial video for YTP][tutorialVideo_ytpPlus] | EthanNow | English | Video | <Date value="2019/07/21" />
@@@zh
稿件 | 作者 | 语言 | 种类 | 最后更新
---- | ---- | ---- | ---- | ----
[<SocialIcon icon="google" />Otomad Helper特点和细节][documentation_evauation] | Evauation | 英语 | 文章
[<SocialIcon icon="bilibili" />更新日志 (v4.10.17.0)][releaseNotes_v4_10_17_0] | 兰音 | 中文 | 文章 | <Date value="2021/10/17" />
[<SocialIcon icon="bilibili" />更新日志 (v4.9.25.0)][releaseNotes_v4_9_25_0] | 兰音 | 中文 | 文章 | <Date value="2021/09/26" />
[<SocialIcon icon="bilibili" />说明文档 (v0.1)][documentation_chaosinism]<br>[<SocialIcon icon="bowlroll" />说明文档 (v0.1)][documentation_chaosinism_ja] | Chaosinism | 中文<br>日语 | 文章 | <Date value="2019/09/01" />
[<SocialIcon icon="bilibili" />五线谱可视化说明文档 (v0.1)][documentation_staffVisualizer_chaosinism] | Chaosinism | 中文 | 文章 | <Date value="2018/08/24" />
[<SocialIcon icon="bilibili" />疑难解答 (v0.1)][troubleshooting_chaosinism] | Chaosinism | 中文 | 文章 | <Date value="2018/05/19" />
[<SocialIcon icon="bilibili" />教程视频 (v4.26.14.0)][tutorialVideo_v4_26_14_0]<br>[<SocialIcon icon="youtube" />教程视频 (v4.26.14.0)][tutorialVideo] | 兰音 | 中文<br>英语 | 视频 | <Date value="2023/04/27" />
[<SocialIcon icon="youtube" />探索视觉效果][exploreVisualEffects] | 兰音 | 英语 | 视频 | <Date value="2022/11/19" />
[<SocialIcon icon="youtube" />教程视频 (v4.16.4.0)][tutorialVideo_greenBean] | GreenBean | 英语 | 视频 | <Date value="2022/05/09" />
[<SocialIcon icon="youtube" />教程视频 (v4.26.14.0)][tutorialVideo_cyahega] | Cyahega | Vietnamese | 视频 | <Date value="2023/03/11" />
[<SocialIcon icon="bilibili" />教程视频 (v0.1)][tutorialVideo_chaosinism] | Chaosinism | 中文 | 视频 | <Date value="2018/04/17" />
[<SocialIcon icon="youtube" />数据抹失教程视频 (v1.4.0)][tutorialVideo_datamosh_delthas] | Delthas | 英语 | 视频 | <Date value="2020/09/24" />
[<SocialIcon icon="youtube" />YTP教程视频][tutorialVideo_ytpPlus] | EthanNow | 英语 | 视频 | <Date value="2019/07/21" />
@@@

<!-- Markdown Links -->
[gpl-3]: https://opensource.org/license/gpl-3.0

[this-repo]: https://github.com/otomad/OtomadHelper
[chaosinism-scripts]: https://github.com/Chaosinism/vegas_scripts
[evankale-scripts]: https://github.com/evankale/VegasScripts
[naudio]: https://github.com/naudio/NAudio
[ytp-plus]: https://github.com/YTP-Plus
[alpha-color-dialog]: https://sourceforge.net/projects/alpha-color-dialog/
[ookii-dialogs]: https://github.com/ookii-dialogs/ookii-dialogs-winforms
[camera-shake]: https://github.com/tmarplatt/VegasScripts
[delthas-datamosh]: https://github.com/delthas/vegas-datamosh
[ffmpeg]: https://github.com/FFmpeg/FFmpeg
[avidemux]: https://github.com/mean00/avidemux2
[xvid-codec]: http://websvn.xvid.org/

[ffmpeg-authors]: https://github.com/FFmpeg/FFmpeg/blob/master/MAINTAINERS
[avidemux-authors]: https://github.com/mean00/avidemux2/blob/master/AUTHORS
[xvid-codec-authors]: http://websvn.xvid.org/cvs/viewvc.cgi/trunk/xvidcore/AUTHORS?view=co

[documentation_chaosinism]: https://www.bilibili.com/read/cv392013
[troubleshooting_chaosinism]: https://www.bilibili.com/read/cv495309
[tutorialVideo_chaosinism]: https://www.bilibili.com/video/av22226321
[documentation_staffVisualizer_chaosinism]: https://www.bilibili.com/read/cv1027442
[documentation_chaosinism_ja]: https://bowlroll.net/user/261124
[releaseNotes_v4_9_25_0]: http://www.bilibili.com/read/cv13335178
[releaseNotes_v4_10_17_0]: https://www.bilibili.com/read/cv13614419
[tutorialVideo_v4_26_14_0]: https://www.bilibili.com/video/av613241077
[documentation_evauation]: https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng
[tutorialVideo_ytpPlus]: https://www.youtube.com/watch?v=_zqUvTr-Y1I
[tutorialVideo_datamosh_delthas]: https://www.youtube.com/watch?v=6D2lW6H0bb8
[tutorialVideo_greenBean]: https://www.youtube.com/watch?v=fVWfUAf063o
[tutorialVideo_cassidy]: https://www.youtube.com/watch?v=8vSpzgL_86A
[exploreVisualEffects]: https://www.youtube.com/watch?v=cY2Qa3Owetw
[tutorialVideo]: https://www.youtube.com/watch?v=amDtqY_HsGM
[tutorialVideo_cyahega]: https://www.youtube.com/watch?v=vLqYIaw0hMc
