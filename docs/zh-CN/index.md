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
      link: https://otomad.github.io/otomad/link/OtomadHelper.html#latest
    - theme: alt
      text: 新版文档 (v8)
      link: ./introduction
    - theme: alt
      text: 旧版文档 (v4)
      link: ./v4/introduction

features:
  - icon: 🆅️
    title: Vegas Pro
    details: 使Vegas能够接受如MIDI序列文件等乐谱作为输入并自动生成音MAD的轨道。
  - icon: 🎞️
    title: 音画
    details: 音频和画面两个都可以生成。
  - icon: 🔄
    title: YTP
    details: YTP也可以被生成。
---

<script setup>
import TeamMembers from "@vp/components/TeamMembers.vue";
</script>

<TeamMembers lang="zh-CN" />
