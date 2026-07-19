# 获取当前分支名称
$currentBranch = (git symbolic-ref --short HEAD).Trim()

# 确保处于仓库根目录
$toplevel = (git rev-parse --show-toplevel).Trim()
Set-Location $toplevel

# 抓取远程最新分支信息
git fetch origin --prune

# 获取所有远程分支并遍历
git branch -r | ForEach-Object {
	$remoteBranch = $_.Trim()
	if ($remoteBranch -and -not ($remoteBranch -like "*HEAD ->*")) {
		# 提取纯粹的分支名称
		$branchName = $remoteBranch -replace '^origin/', ''

		# 排除当前分支
		if ($branchName -ne $currentBranch) {
			Write-Host "正在合并分支: $remoteBranch ..."

			# 执行合并
			git merge $remoteBranch -s ours --allow-unrelated-histories --no-edit `
				-m "Automated merge of $remoteBranch into $currentBranch using ours strategy"

			# 检查合并状态
			if ($LASTEXITCODE -ne 0) {
				Write-Warning "合并 $remoteBranch 失败，正在跳过..."
				git merge --abort
			}
		}
	}
}

Write-Host "所有其它分支已处理完毕！"
