#!/usr/bin/env bash

# 获取当前分支名称
CURRENT_BRANCH=$(git symbolic-ref --short HEAD)

# 确保处于仓库根目录
cd "$(git rev-parse --show-toplevel)"

# 抓取远程最新分支信息并清理已删除的远程分支
git fetch origin --prune

# 遍历所有远程分支
for remote_branch in $(git branch -r | grep -v "HEAD ->"); do
	# 提取纯粹的分支名称（去除 origin/ 前缀）
	branch_name=${remote_branch#origin/}

	# 排除当前分支自身
	if [ "$branch_name" != "$CURRENT_BRANCH" ]; then
		echo "正在合并分支: $remote_branch ..."

		# 执行 ours 策略合并，允许合并不相关的历史
		git merge "$remote_branch" -s ours --allow-unrelated-histories --no-edit \
			-m "Automated merge of $remote_branch into $CURRENT_BRANCH using ours strategy"

		# 检查合并是否成功
		if [ $? -ne 0 ]; then
			echo "警告: 合并 $remote_branch 失败，正在跳过..."
			git merge --abort
		fi
	fi
done

echo "所有其它分支已处理完毕！"
