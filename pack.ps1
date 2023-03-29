Set-location (Join-Path $PSScriptRoot "\distribution")

function Use-SrcPath {
	Param (
		[String] $fileName
	)
	
	return Join-Path $PSScriptRoot "otomad_helper" | Join-Path -ChildPath $fileName
}

function Add-ToZip {
	Param (
		[String] $zipFile,
		[String] $file
	)
	
	$writeTime = $lastWriteTime = (Get-Item $zipFile).LastWriteTime
	& "C:\Program Files\WinRAR\WinRAR" a -afzip $zipFile $file
	
	while ($writeTime -eq $lastWriteTime) {
		Start-Sleep -Milliseconds 100
		$lastWriteTime = (Get-Item $zipFile).LastWriteTime
	}
}

function Update-ToReplaceTextFileOnce {
	Param (
		[String]   $file,
		[String[]] $sourceTexts,
		[String[]] $targetTexts
	)
	
	$content = Get-Content -Path $file -Encoding UTF8 -Raw
	
	for ($i = 0; $i -lt $sourceTexts.Length; $i++) {
		$content = [Regex]::New($sourceTexts[$i]).Replace($content, $targetTexts[$i], 1)
	}
	
	# Set-Content -Path $file -Value $content -Encoding UTF8
	$utf8NoBomEncoding = New-Object Text.UTF8Encoding $False
	[IO.File]::WriteAllLines($file, $content, $utf8NoBomEncoding)
}

function Update-UndefinedSymbols {
	Param (
		[String]   $file,
		[String[]] $undefinedSymbols
	)
	
	if ($undefinedSymbols.Length -ne 0) {
		$sourceTexts = $undefinedSymbols | ForEach-Object { "#define " + $_ }
		$targetTexts = $undefinedSymbols | ForEach-Object { "// #define " + $_ }
		Update-ToReplaceTextFileOnce $file $sourceTexts $targetTexts
	}
}

$infos = @{
	VegasVersions    = 16..20 -join "";
	UndefinedSymbols = @();
}, @{
	VegasVersions    = 14..15 -join "";
	UndefinedSymbols = @("VER_GEQ_16");
}, @{
	VegasVersions    = 13;
	UndefinedSymbols = @("VER_GEQ_16", "VER_GEQ_14");
}

$version = [Regex]::New("(?<=VERSION-)[\d\.]+").Match((Get-Content -Path '..\README.md' -Encoding UTF8)).Value

Remove-Item "*.*"
for ($i = 0; $i -lt $infos.Count; $i++) {
	$info = $infos[$i]
	Copy-Item (Use-SrcPath "release_package_template.zip"), (Use-SrcPath "Otomad Helper.cs") -Destination "."
	Update-UndefinedSymbols "Otomad Helper.cs" $info.UndefinedSymbols
	Add-ToZip "release_package_template.zip" "Otomad Helper.cs"
	Rename-Item -Path "release_package_template.zip" -NewName ([String]$i + "_otomad_helper_v" + $version + "_vegas" + [string]$info.VegasVersions + ".zip")
	Remove-Item "Otomad Helper.cs"
}
Remove-Item "..\Otomad Helper.cs"
