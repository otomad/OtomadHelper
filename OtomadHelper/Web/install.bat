@echo off
:: utf-8
chcp >nul 2>nul 65001

:: check admin
net session >nul 2>&1
if %errorlevel% == 0 goto skipAdmin
echo Please run install as an administrator. 
echo 请以管理员身份运行安装程序。 
echo インストールは管理者権限で実行してください。
%1 mshta vbscript:CreateObject("Shell.Application").ShellExecute("cmd.exe","/c %~s0 ::","","runas",1)(window.close)&&exit
:skipAdmin

:: prompt
echo The om midi for After Effects script will be installed soon, press Enter to continue. 
echo 即将安装 om midi for After Effects 脚本，按回车键继续。 
echo om midi for After Effects スクリプトはまもなくインストールされます。Enter キーを押して続行します。
set /p input=
cls

:: add regs
set latestVersion=11
for /l %%i in (4,1,%latestVersion%) do (
	reg >nul 2>nul add HKEY_CURRENT_USER\Software\Adobe\CSXS.%%i /v PlayerDebugMode /t REG_SZ /d "1" /f
)
reg >nul 2>nul add HKEY_CURRENT_USER\Software\Adobe\CSXS.6.1 /v PlayerDebugMode /t REG_SZ /d "1" /f

:: end
echo Installation complete! 
echo 安装完成！ 
echo インストール完了！
pause
