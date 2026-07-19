$currentBranch = (git symbolic-ref --short HEAD).Trim()

$toplevel = (git rev-parse --show-toplevel).Trim()
Set-Location $toplevel

git fetch origin --prune

git branch -r | ForEach-Object {
	$remoteBranch = $_.Trim()
	if ($remoteBranch -and -not ($remoteBranch -like "*HEAD ->*")) {
		$branchName = $remoteBranch -replace '^origin/', ''

		if ($branchName -ne $currentBranch) {
			Write-Host "Merging: $remoteBranch ..."

			git merge $remoteBranch -s ours --allow-unrelated-histories --no-edit `
				-m "Automated merge of $remoteBranch into $currentBranch using ours strategy"

			if ($LASTEXITCODE -ne 0) {
				Write-Warning "Merge $remoteBranch failed. Skipping..."
				git merge --abort
			}
		}
	}
}

Write-Host "Done!"
