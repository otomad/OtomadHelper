Set-location $PSScriptRoot

function Get-ContentRaw {
	Param (
		[String] $file
	)

	return (Get-Content -Path $file -Encoding UTF8 -Raw).Trim()
}

function Set-ContentRaw {
	Param (
		[String] $file,
		[String] $content
	)
	# Set-Content -Path $file -Value $content -Encoding UTF8
	$utf8NoBomEncoding = New-Object Text.UTF8Encoding $False
	[IO.File]::WriteAllLines((Join-Path $pwd $file), $content, $utf8NoBomEncoding)
}

function Update-ToReplaceIfNeeded {
	Param (
		[String] $inputString,
		[String] $pattern,
		[String] $replacement
	)
	$current = if ($inputString -match $pattern) { $Matches[0] }
	if ($Matches -ne $null -and $current -ne $replacement) {
		return $inputString -replace $pattern, $replacement
	}
	return $null
}

$file = ".\version.txt"
$version = (Get-ContentRaw $file).Split(".") | % {iex $_}
# $version[2]++
$version = $version -Join "."
Write-Host $version
Set-ContentRaw $file $version

$file = ".\OtomadHelper\OtomadHelper.csproj"
$csAssemblyInfo = Get-ContentRaw $file
$csAssemblyInfo = Update-ToReplaceIfNeeded $csAssemblyInfo "(?<=<AssemblyVersion>)[^\*]+?(?=</)" $version
if ($csAssemblyInfo) { Set-ContentRaw $file $csAssemblyInfo }

$file = ".\OtomadHelper\Web\package.json"
$jsPackage = Get-ContentRaw $file
$jsPackage = Update-ToReplaceIfNeeded $jsPackage "(?<=`"version`": `").*(?=`")" $version
if ($jsPackage) { Set-ContentRaw $file $jsPackage }
