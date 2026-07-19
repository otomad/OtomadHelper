# 是的，你没有看错，这就是一个~~垃圾~~分支

## 为什么会有此分支？

GitHub 的个人资料总览页面的贡献统计图表（俗称绿瓷砖）有一个尴尬的原生统计机制，它只会统计各仓库的主分支和 `gh-pages` 分支的所有提交。对于其它分支（如旧版代码），它看都不会看一眼。

## 此仓库的作用？

尽管将所有提交都合并到主分支或 GitHub Pages 分支会污染这些分支的提交历史，有些不便。但是我发现，这个所谓的 `gh-pages` 真的就只是名字是 `gh-pages` 分支的这个分支，并不是 GitHub Pages 所使用的真正分支。那么就简单了，如果只需要将名为 `gh-pages` 的分支用来传播“数字泔水”，一切都将完美解决。

## 被使用方法？

其它分支进行任意提交后，在此分支中使用以下命令：

```bash
git merge origin/其它分支名 -s ours --allow-unrelated-histories
```

就可以将其它所有分支的提交全部扔到这个分支中，只要不在意提交历史里有“奇怪”的绿节点。

## 为什么仓库中是空的？

这就是为什么前面的命令中使用了 `ours` 策略。它的意思是：

> 创建一个合并提交，告诉 Git “我已经把旧版合并了，但保留我现在的（新版）代码，完全忽略旧版代码的变化”。

其它分支的文件关我 `gh-pages` 分支啥事。

## 新：自动化合并脚本

现在已新增自动化合并，可以一键执行合并操作了。

### Windows

#### PowerShell

```powershell
powershell scripts\merge.ps1
```

#### Command Prompt

```bat
scripts\merge.bat
```

### macOS/Linux/Unix

#### Bash

```bash
./scripts/merge.sh
```
