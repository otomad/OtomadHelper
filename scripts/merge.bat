@echo off
setlocal enabledelayedexpansion

for /f "tokens=*" %%i in ('git symbolic-ref --short HEAD') do set CURRENT_BRANCH=%%i

for /f "tokens=*" %%i in ('git rev-parse --show-toplevel') do cd /d "%%i"

git fetch origin --prune

for /f "tokens=*" %%i in ('git branch -r') do (
	set "REMOTE_BRANCH=%%i"
	for /f "tokens=*" %%a in ("!REMOTE_BRANCH!") do set "REMOTE_BRANCH=%%a"

	echo !REMOTE_BRANCH! | findstr /v "HEAD ->" >nul
	if !errorlevel! equ 0 (
		set "BRANCH_NAME=!REMOTE_BRANCH:origin/=!"

		if "!BRANCH_NAME!" neq "!CURRENT_BRANCH!" (
			echo Merging: !REMOTE_BRANCH! ...
			git merge "!REMOTE_BRANCH!" -s ours --allow-unrelated-histories --no-edit -m "Automated merge of !REMOTE_BRANCH! into !CURRENT_BRANCH! using ours strategy"

			if !errorlevel! neq 0 (
				echo Warning: Merge !REMOTE_BRANCH! failed. Skipping...
				git merge --abort
			)
		)
	)
)

echo Done!
pause
