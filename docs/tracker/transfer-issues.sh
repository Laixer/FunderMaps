#!/usr/bin/env bash
# Transfer the OPEN issues of Laixer/FunderMaps to another repository in the
# same organisation, keeping labels and milestones.
#
# GitHub's issue transfer (GraphQL transferIssue) keeps title, body, comments,
# author attribution and timestamps, and leaves a redirect behind on the old
# number. Labels and milestones are only kept when the target repo has ones
# with the SAME NAME, so step 1 recreates them from labels.json / milestones.json.
#
# Usage:
#   docs/tracker/transfer-issues.sh Laixer/<TargetRepo>            # dry run: prints what it would do
#   docs/tracker/transfer-issues.sh Laixer/<TargetRepo> --apply    # do it
#
# Needs: gh (authenticated with issues:write on both repos), jq.
# Source of truth for the issue list is the live tracker, not open_issues.json,
# so a re-run after new issues are filed picks them up too.
set -euo pipefail

SRC="Laixer/FunderMaps"
DST="${1:?target repo, e.g. Laixer/FunderMapsApi}"
APPLY="${2:-}"
HERE="$(cd "$(dirname "$0")" && pwd)"

run() { if [[ "$APPLY" == "--apply" ]]; then "$@"; else echo "DRY: $*"; fi; }

echo "== 1. labels ($SRC -> $DST)"
jq -c '.[] | {name, color, description}' "$HERE/labels.json" | while read -r l; do
  name=$(jq -r .name <<<"$l"); color=$(jq -r .color <<<"$l"); desc=$(jq -r '.description // ""' <<<"$l")
  if gh label list -R "$DST" --json name --jq '.[].name' | grep -qxF "$name"; then
    echo "   exists: $name"
  else
    run gh label create "$name" -R "$DST" --color "$color" --description "$desc"
  fi
done

echo "== 2. milestones (only the ones still referenced by an open issue)"
gh issue list -R "$SRC" --state open --limit 200 --json milestone --jq '.[].milestone.title | select(. != null)' | sort -u | while read -r m; do
  if gh api "repos/$DST/milestones?state=all&per_page=100" --jq '.[].title' | grep -qxF "$m"; then
    echo "   exists: $m"
  else
    desc=$(jq -r --arg t "$m" '.[] | select(.title==$t) | .description // ""' "$HERE/milestones.json")
    run gh api -X POST "repos/$DST/milestones" -f title="$m" -f description="$desc" >/dev/null
  fi
done

echo "== 3. transfer open issues"
DST_ID=$(gh api "repos/$DST" --jq .node_id)
gh issue list -R "$SRC" --state open --limit 200 --json number,title,id --jq '.[] | "\(.number)\t\(.id)\t\(.title)"' | sort -n | while IFS=$'\t' read -r num id title; do
  echo "   #$num  $title"
  run gh api graphql -f query='mutation($issue:ID!,$repo:ID!){ transferIssue(input:{issueId:$issue, repositoryId:$repo}){ issue { number url } } }' -f issue="$id" -f repo="$DST_ID" --jq '.data.transferIssue.issue.url'
done

echo "done. ${APPLY:-(dry run; pass --apply to execute)}"
