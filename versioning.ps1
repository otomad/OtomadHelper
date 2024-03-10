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

$file = ".\version.txt"
$version = (Get-ContentRaw $file).Split(".") | % {iex $_}
# $version[2]++
$version = $version -Join "."
Write-Output $version
Set-ContentRaw $file $version

$file = ".\OtomadHelper\Properties\AssemblyInfo.cs"
$csAssemblyInfo = Get-ContentRaw $file
$csAssemblyInfo = [Regex]::Replace($csAssemblyInfo, "(?<=Assembly(File)?Version\(`")[^\*]+?(?=`"\))", $version)
Set-ContentRaw $file $csAssemblyInfo

$file = ".\OtomadHelper\Web\package.json"
$jsPackage = Get-ContentRaw $file
$jsPackage = [Regex]::Replace($jsPackage, "(?<=`"version`": `").*(?=`")", $version)
Set-ContentRaw $file $jsPackage
