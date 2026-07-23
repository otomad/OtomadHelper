# 音MAD助手 说明文档

[![en](https://img.shields.io/badge/lang-English-blue?style=flat-square)](./README.md)
[![zh-CN](https://img.shields.io/badge/语言-简体中文-blue?style=flat-square)](./README_zh-CN.md)

欢迎来到**音MAD助手（Otomad Helper）**<wbr>的说明文档仓库——一个Vegas Pro的音MAD/YTPMV/YTP扩展程序。本站点同时涵盖了**新版（v8，扩展程序）**<wbr>和**旧版（v4，脚本）**<wbr>的说明文档内容。

- 📖 **在线文档**: [https://otomadhelper.readthedocs.io/zh-CN/](https://otomadhelper.readthedocs.io/zh-CN/)
- 🗂️ **源码分支**: `docs`

---

## 目录

- [项目概述](#项目概述)
- [技术架构](#技术架构)
- [单文件多语言文档格式](#单文件多语言文档格式)
- [如何编辑文档](#如何编辑文档)
  - [一次性准备工作](#一次性准备工作)
  - [日常编辑流程](#日常编辑流程)
- [项目结构](#项目结构)
- [构建与部署原理](#构建与部署原理)
- [许可证](#许可证)

---

## 项目概述

音MAD助手是一个使Vegas Pro能够接受乐谱（如MIDI序列文件）作为输入并自动生成音MAD/YTPMV/YTP轨道的工具。本仓库包含该项目的**说明文档网站**源代码。

文档涵盖两个主要版本：

| 版本 | 类型 | 说明 |
|---------|------|-------------|
| **v8**（新版） | 扩展程序/自定义命令 | 最新版本，以Vegas Pro扩展程序（也被称为自定义命令）的形式实现 |
| **v4**（旧版） | 脚本 | 旧版，以Vegas Pro脚本的形式实现 |

## 技术架构

| 组件 | 技术 |
|-----------|-----------|
| **文档框架** | [VitePress](https://vitepress.dev/)（v2） |
| **包管理器** | [pnpm](https://pnpm.io/) |
| **源码托管** | [GitHub](https://github.com/otomad/OtomadHelper)（分支: `docs`） |
| **构建与托管** | [Read the Docs](https://readthedocs.org/) |
| **自定义插件** | i18n-macro、KaTeX数学公式、图片预览、pagefind搜索、RSS订阅源、llms.txt，等 |

**工作流程：** 当代码推送到GitHub的`docs`分支后，Read the Docs服务器会自动重新构建项目，并生成静态HTML网页。用户可通过 [https://otomadhelper.readthedocs.io/zh-CN/](https://otomadhelper.readthedocs.io/zh-CN/) 直接阅读最新文档内容。

## 单文件多语言文档格式

本项目采用了一种创新的**单文件多语言**格式。与传统的每种语言单独维护一个文件不同，所有语言的翻译都写在同一个文件中。这样当需要修改错误时，可以同时看到所有语言的对应内容，一次性全部修正。

### 为什么使用这种格式？

传统的多语言文档方式中，如果文档包含7种语言，修改一个错误需要：

1. 同时打开7个文件（每种语言一个）。
2. 在每个文件中找到错误内容所在的行。
3. 逐个文件修改同样的内容。

使用单文件多语言格式后，各语言的翻译紧挨在一起，可以在同一个位置一次性修改所有语言。

### 行多语言

以行为单位为其编写多语言翻译。格式以一个 **`@`符号**开头，后面紧跟着语言标签，空一格后填写该语言在本行的翻译内容：

```markdown
@en This is English content.
@zh 这是中文内容。
@ja これは日本語の内容です。
```

目前支持的语言标签：`en`（英语）、`zh`（简体中文）。

### 块多语言

以块为单位为其编写多语言翻译。适用于大量内容差异或复杂格式的多语言翻译，例如整个表格、警告框等。格式以**三个`@`符号**开头，后面紧跟语言标签：

```markdown
@@@en
This is a large block of English content.
It can span multiple lines and include **formatting**.
@@@zh
这是一大段中文内容。
它可以跨越多行并包含**格式**。
@@@
```

- 用 `@@@` 加语言标签开始一个块。
- 用单独的 `@@@`（不跟标签）结束整个多语言块。

### 回退（Fallback）机制

如果在某一段行多语言或块多语言内容中缺少编写了某种语言的翻译内容，网站会**自动回退显示为英文文档内容**。这意味着你只需要为你会说的语言编写翻译，缺失的语言会自动安全地显示英文。

### 重要规则

- **不要**将行多语言（`@`）和块多语言（`@@@`）语法混用于同一段内容——选择其中一种即可。
- `@`或`@@@`标记必须出现在一行的最开头。
- 对于行多语言翻译，语言的出现顺序可以任意，但保持统一的顺序（例如始终`@en`在前、`@zh`在后）有助于提高可读性。

---

## 如何编辑文档

以下步骤是为**非程序员**编写的。你不需要任何编程经验，只需一步步跟着操作即可。如果遇到困难，可以请熟悉电脑的朋友帮忙。

> ⚠️ **重要提示：** 所有文档修改都应在 **`docs`** 分支上进行，而不是 `main`/`master` 分支。

### 一次性准备工作

以下步骤仅需**在第一次配置项目时做一次**，以后无需重复。

---

#### 第一步：安装 Node.js

Node.js 是运行文档工具链所需的运行时环境。

1. 打开浏览器，访问 **[https://nodejs.org/](https://nodejs.org/)**
2. 点击 **LTS**（长期支持版）下载按钮——这是推荐大多数用户使用的稳定版本。
3. 下载完毕后，双击安装文件（Windows上为 `node-vXX.XX.X-x64.msi`）。
4. 按照安装向导操作——可以全部使用默认设置。
5. 点击**完成**。

> ✅ 验证是否安装成功：打开开始菜单，输入 `cmd`，按回车打开命令提示符，输入 `node --version`。如果看到版本号（例如 `v22.x.x`），说明安装成功。

---

#### 第二步：安装 pnpm

pnpm 是本项目使用的包管理器。

1. 打开**命令提示符**（开始菜单 → 输入 `cmd` → 按回车）。
2. 输入以下命令并按回车：
   ```bash
   npm install -g pnpm
   ```
3. 等待安装完成。

> ✅ 验证：在命令提示符中输入 `pnpm --version`，应能看到版本号。

---

#### 第三步：安装 GitHub Desktop

GitHub Desktop 提供了 Git 的图形化界面，_这样你就不需要记忆和输入 Git 命令行指令了。_

1. 访问 **[https://desktop.github.com/](https://desktop.github.com/)**
2. 点击**下载**按钮。
3. 下载完成后，运行安装程序。
4. 首次打开 GitHub Desktop 时，会要求你登录 GitHub 账号。如果你还没有账号：
   - 访问 **[https://github.com/signup](https://github.com/signup)** 创建一个免费账号。
   - 然后用新账号登录 GitHub Desktop。
5. 按照设置提示操作（对于Git配置相关的提示可以使用默认设置直接跳过）。

---

#### 第四步：克隆仓库

"克隆"的意思是将项目的副本下载到你的电脑上。

1. 在 **GitHub Desktop** 中，点击 **File（文件）→ Clone repository...（克隆仓库...）**
2. 选择 **URL** 选项卡。
3. 在 "Repository URL（仓库链接）" 字段中输入：
   ```
   https://github.com/otomad/OtomadHelper
   ```
4. 在 "Local path（本地路径）" 中，点击 **Choose...（选择...）**，选择你电脑上想要存放项目的文件夹（例如 `D:\Documents\OtomadHelper`）。
5. 点击 **Clone（克隆）**。
6. **重要：** 克隆完成后，需要切换到 `docs` 分支：
   - 在 GitHub Desktop 顶部附近，你会看到一个标有 **"Current branch（当前分支）"** 的按钮（可能显示为 `main` 或 `master`）。
   - 点击它，然后从列表中选择 **`docs`** 分支。
   - 如果在列表中找不到 `docs`，请点击分支列表顶部的 **"Origin"** 选项卡——仅在GitHub上存在的分支会出现在这里。点击 `docs` 即可检出该分支。

---

#### 第五步：安装项目依赖库

依赖库是项目运行所需的外部程序库。

1. 在 **GitHub Desktop** 中，确保仓库已打开，点击 **Repository（仓库）→ Open in Terminal（在终端中打开）**（或按 <kbd>Ctrl</kbd> + <kbd>`</kbd>）。
   - 这会打开一个已定位到项目文件夹的终端窗口。
2. 在终端中输入以下命令并按回车：
   ```bash
   pnpm i
   ```
3. 等待安装完成——可能需要一两分钟。你会看到进度指示，最终会显示类似于 "Done" 的信息。

> 你只需要在初始配置时运行一次 `pnpm i`。以后通常不需要再次运行，除非有人告知你项目新增了依赖库。

---

#### 第六步<wbr>*（不必要但是推荐）*<wbr>：安装 Visual Studio Code

虽然你可以直接使用**记事本**编辑文件，但 *Visual Studio Code (VS Code)* 是一款免费且功能强大的编辑器，具有语法高亮和文件浏览功能，能让编辑工作轻松很多。

1. 访问 **[https://code.visualstudio.com/](https://code.visualstudio.com/)**
2. 点击**下载**并安装（可以全部使用默认设置）。
3. 安装完成后，在 **GitHub Desktop** 中，你可以右键点击仓库并选择 **"Open in Visual Studio Code（在VS Code中打开）"** 直接开始编辑。

> 💡 **小提示：** 本项目已预先配置了 VS Code 的相关设置。当你用 VS Code 打开项目后，编辑器会自动推荐安装相关扩展，并配置好保存时自动格式化等功能。你可以在右下角的弹窗提示中点击"安装"来安装推荐的扩展。

---

### 日常编辑流程

每次想要修改文档时，按照以下流程操作。

---

#### 第一步：拉取最新更改

在编辑之前，务必先拉取最新版本，以避免与其他人可能已经做出的更改产生冲突。

1. 打开 **GitHub Desktop**。
2. 确认当前仓库为 **OtomadHelper**，当前分支为 **`docs`**。
3. 点击顶部工具栏的 **"Fetch origin（拉取远端）"** 按钮。
4. 如果有新的更改，按钮会变为 **"Pull origin（同步远端）"** ——点击它下载最新更新。

> 💡 **提示：** 每次开始编辑前都要做这一步。这样可以避免因其他人同时修改了相同内容而导致你的工作被覆盖的麻烦。

---

#### 第二步：编辑文档文件

文档文件是位于 `docs/` 文件夹中的 Markdown（`.md`）文件。

1. 在文件资源管理器中打开项目文件夹，或在 **VS Code** 中打开。
2. 进入 `docs/` 文件夹。里面的内容包括：
   - **`.vitepress/`** — 配置和主题文件（通常不需要修改这些）
   - **`zh-CN/**/*.md`** — 中文专属页面（主页等）
   - **`**/*.md` 文件** — 新版 v8 扩展的文档页面（这些页面使用[单文件多语言格式](#单文件多语言文档格式)）
   - **`v4/**/*.md`** — 旧版 v4 脚本的文档页面（这些页面使用[单文件多语言格式](#单文件多语言文档格式)）
3. 打开你想要编辑的文件。大多数内容文件使用单文件多语言格式，因此英文和中文内容在一起编写。详细格式说明请参见上方的[单文件多语言文档格式](#单文件多语言文档格式)部分。
4. 进行修改并保存文件（`Ctrl+S`）。

> 📝 **应该编辑什么：**
> - 修改错别字或错误：找到 `@en` 行进行编辑，对应的 `@zh` 行就在其下方。
> - 添加新段落：为每个段落写 `@en 你的英文文本`，紧接着下面写 `@zh 你的中文文本`。
> - 对于大块内容（警告框、表格等），请使用 `@@@en` / `@@@zh` / `@@@` 块格式。

---

#### 第三步：本地预览修改

在推送之前，你可以在自己的电脑上预览网站，看看修改后的效果。

1. 如果你有 **VS Code**，请按 <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>B</kbd>，然后选择 `npm: dev`。
   > 如果你没有 VS Code，请：
   > 1. 在项目文件夹中打开终端：\
   >    **在 GitHub Desktop 中：** 点击 **Repository（仓库）→ Open in Terminal（在终端中打开）**。
   > 2. 输入以下命令并按回车：
   >    ```bash
   >    pnpm run dev
   >    ```
2. 稍等片刻。你会看到类似以下输出：
   ```
   vitepress vX.X.X
   ➜  Local:   http://localhost:7000/
   ```
3. 按住 <kbd>Ctrl</kbd>（也有可能是 <kbd>Alt</kbd>）键不放并单击链接 **`http://localhost:7000/`**。
4. 导航到你编辑过的页面，检查所有内容是否正确显示。

> 💡 **提示：** 预览会随着你保存文件而自动更新——只需在编辑器中保存，然后刷新浏览器即可。

---

#### 第四步：提交并推送你的更改

"提交（Commit）"是将你的更改保存到本地仓库。"推送（Push）"是将更改上传到 GitHub。

1. 打开 **GitHub Desktop**。你会在左侧看到已更改的文件列表。
2. 在左下角找到 **"Summary（摘要）"** 字段。用简短清晰的文字描述你改了什么。例如：
   - `修复安装指南中的错别字`
   - `在音频页面添加新功能说明`
   - `为FAQ页面补充中文翻译`
3. （可选）在下面的 **"Description（说明）"** 字段中添加更多细节。
4. 点击 **"Commit to docs（提交到docs）"** 按钮。
5. 提交完成后，点击 **"Push origin（推送远端）"** 按钮（右上角），将你的更改上传到 GitHub。

> 🎉 **大功告成！** 推送完成后，Read the Docs 会在几分钟内自动重新构建网站。你可以在 [https://otomadhelper.readthedocs.io/zh-CN/](https://otomadhelper.readthedocs.io/zh-CN/) 查看更新后的在线文档。

---

## 项目结构

```
OtomadHelper_docs/                     # 仓库根目录（docs 分支）
├── .gitignore                         # Git 忽略的文件
├── .readthedocs.yaml                  # Read the Docs 构建配置
├── .vscode/                           # VS Code 编辑器配置
│   ├── extensions.json                # 推荐安装的扩展
│   ├── settings.json                  # 编辑器设置（自动格式化等）
│   └── tasks.json                     # 预配置的任务（dev/build）
├── package.json                       # 项目依赖库与脚本
├── pnpm-lock.yaml                     # 依赖库版本锁定文件
├── tsconfig.json                      # TypeScript 配置
├── oxfmt.config.ts                    # 代码格式化工具配置
├── README.md                          # 英文版 README
├── README_zh-CN.md                    # 中文版 README（你正在读的这个）
└── docs/                              # 文档源码（VitePress 根目录）
    ├── .vitepress/                    # VitePress 配置与主题
    │   ├── config.ts                  # 主站点配置
    │   ├── components/                # 自定义 Vue 组件
    │   ├── plugins/                   # 自定义 Markdown 与构建插件
    │   ├── theme/                     # 自定义主题覆盖
    │   └── use-i18n.ts                # 国际化辅助工具
    ├── assets/                        # 静态资源（图片、字体等）
    ├── img/                           # 文档图片
    ├── public/                        # 公共静态文件
    │   ├── favicon.svg                # 网站图标
    │   └── favicon_1.ico              # 备用图标（用于规避 RtD 图标替换）
    ├── index.md                       # 主页（英文）
    ├── introduction.md                # 介绍页面（单文件多语言）
    ├── installation.md                # 安装指南（单文件多语言）
    ├── usage.md                       # 使用指南（单文件多语言）
    ├── faq.md                         # 常见问题（单文件多语言）
    ├── audio.md, visual.md, ...       # 各功能页面（单文件多语言）
    ├── v4/                            # 旧版 v4 文档页面
    │   ├── introduction.md
    │   ├── installation.md
    │   └── ...
    └── zh-CN/                         # 中文专属页面
        ├── index.md                   # 主页（中文）
        └── v4/                        # 中文 v4 页面入口
```

---

## 构建与部署原理

```mermaid
graph LR
    A[编辑 .md 文件] --> B[提交并推送到 docs 分支]
    B --> C[GitHub]
    C --> D[Read the Docs 检测到推送]
    D --> E[RtD 运行 pnpm build]
    E --> F[生成静态 HTML]
    F --> G[在 otomadhelper.readthedocs.io 上呈现]
```

1. 你编辑 Markdown 文件并推送到 GitHub 上的 `docs` 分支。
2. Read the Docs 自动检测到推送。
3. RtD 运行 `npm run build`（由 `.readthedocs.yaml` 配置），构建 VitePress 站点。
4. 生成的静态 HTML 文件被托管在 [https://otomadhelper.readthedocs.io/zh-CN/](https://otomadhelper.readthedocs.io/zh-CN/)。
5. 构建通常在推送后的 **2-5分钟** 内完成。

### 可用脚本

| 命令 | 说明 |
|---------|-------------|
| `pnpm run dev` | 启动本地预览服务器，访问 `http://localhost:7000/` |
| `pnpm run build` | 构建生产环境静态站点 |
| `pnpm run preview` | 在本地预览生产构建结果 |
| `pnpm run fmt` | 使用 oxfmt 格式化代码 |

---

## 许可证

本文档基于 [GPL 3.0 许可证](https://www.gnu.org/licenses/gpl-3.0.html) 发布。

版权所有 © 2021~至今 兰音
