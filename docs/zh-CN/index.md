---
# https://vitepress.dev/reference/default-theme-home-page
layout: home

hero:
  name: 音MAD助手
  text: 在Vegas Pro中生成音MAD
  tagline: Vegas Pro辅助创作音MAD的扩展程序
  image:
    src: /favicon_light.svg
  actions:
    - theme: brand
      text: 立即下载！
      link: https://github.com/otomad/OtomadHelper/releases/latest
    - theme: alt
      text: 新版文档 (v8)
      link: ./introduction
    - theme: alt
      text: 旧版文档 (v4)
      link: ./v4/introduction

features:
  - icon: 🎼
    title: MIDI 驱动的生成
    details: 喂给它一份 MIDI，它就把每个音符铺成完美同步的音画剪辑。
    link: ./v4/score
    linkText: 了解更多
  - icon: 🎚️
    title: 录音室级调音
    details: 弹性变调、共振峰锁定与粒子振荡器——以手术刀般的精度雕琢采样。
    link: ./v4/audio
    linkText: 了解更多
  - icon: 🎨
    title: 动态图形画面
    details: 翻转、旋转、缩放、色相与 13+ 类效果，全部由你的音符驱动关键帧。
    link: ./v4/visual
    linkText: 了解更多
  - icon: 🎬
    title: 多素材魔法
    details: 素材盲盒、踩点与素材乐团，自动混搭任意数量的素材。
    link: ./v4/source
    linkText: 了解更多
  - icon: 🧰
    title: 面面俱到的工具
    details: 网格布局、3D 方盒、批量字幕、轨道填补等数十种效率工具。
    link: ./v4/tools
    linkText: 了解更多
  - icon: 🌀
    title: 抹失与 YTP
    details: 数据抹失、随机扰乱、结巴、摇晃，外加经典 YTP 全套特效。
    link: ./v4/mosh
    linkText: 了解更多
---

<HomeShowcase />

<TeamMembers />
<ProjectsView />
<PoweredBy />
