#!/usr/bin/env bash

CURRENT_BRANCH=$(git symbolic-ref --short HEAD)

cd "$(git rev-parse --show-toplevel)"

git fetch origin --prune

for remote_branch in $(git branch -r | grep -v "HEAD ->"); do
	branch_name=${remote_branch#origin/}

	if [ "$branch_name" != "$CURRENT_BRANCH" ]; then
		echo "Merging: $remote_branch ..."

		git merge "$remote_branch" -s ours --allow-unrelated-histories --no-edit \
			-m "Automated merge of $remote_branch into $CURRENT_BRANCH using ours strategy"

		if [ $? -ne 0 ]; then
			echo "Warning: Merge $remote_branch failed. Skipping..."
			git merge --abort
		fi
	fi
done

echo "Done!"
