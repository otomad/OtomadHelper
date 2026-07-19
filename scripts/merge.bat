@echo off
setlocal enabledelayedexpansion

:: 获取当前分支名称
for /f "tokens=*" %%i in ('git symbolic-ref --short HEAD') do set CURRENT_BRANCH=%%i

:: 确保处于仓库根目录
for /f "tokens=*" %%i in ('git rev-parse --show-toplevel') do cd /d "%%i"

:: 抓取远程最新分支信息
git fetch origin --prune

:: 遍历所有远程分支
for /f "tokens=*" %%i in ('git branch -r') do (
	set "REMOTE_BRANCH=%%i"
	:: 去除行首空格
	for /f "tokens=*" %%a in ("!REMOTE_BRANCH!") do set "REMOTE_BRANCH=%%a"

	:: 排除 HEAD 指针
	echo !REMOTE_BRANCH! | findstr /v "HEAD ->" >nul
	if !errorlevel! equ 0 (
		:: 提取纯粹的分支名称（去掉 origin/）
		set "BRANCH_NAME=!REMOTE_BRANCH:origin/=!"

		:: 排除当前分支自身
		if "!BRANCH_NAME!" neq "!CURRENT_BRANCH!" (
			echo 正在合并分支: !REMOTE_BRANCH! ...
			git merge "!REMOTE_BRANCH!" -s ours --allow-unrelated-histories --no-edit -m "Automated merge of !REMOTE_BRANCH! into !CURRENT_BRANCH! using ours strategy"

			if !errorlevel! neq 0 (
				echo 警告: 合并 !REMOTE_BRANCH! 失败，正在跳过...
				git merge --abort
			)
		)
	)
)

echo 所有其它分支已处理完毕！
pause
