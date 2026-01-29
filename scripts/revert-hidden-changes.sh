#!/bin/bash
# Reverts files where the only changes are non-functional:
# - BOM (Byte Order Mark) added/removed
# - Trailing newline added/removed
# - Combinations of the above
#
# Usage: ./scripts/revert-hidden-changes.sh

reverted=0
checked=0

for file in $(git diff --name-only); do
    if [[ ! -f "$file" ]]; then
        continue
    fi

    ((checked++))

    # Get the diff content (excluding headers)
    diff_content=$(git diff "$file" | tail -n +5)

    # Get added and removed lines (excluding diff markers like +++ and ---)
    added_lines=$(echo "$diff_content" | grep "^+" | grep -v "^+++" || true)
    removed_lines=$(echo "$diff_content" | grep "^-" | grep -v "^---" || true)

    # Count actual change lines
    added_count=$(echo "$added_lines" | grep -c "^+" 2>/dev/null || true)
    removed_count=$(echo "$removed_lines" | grep -c "^-" 2>/dev/null || true)
    [[ -z "$added_count" || "$added_count" == "0" ]] && added_count=0
    [[ -z "$removed_count" || "$removed_count" == "0" ]] && removed_count=0

    # Skip if too many changes (more than 2 each for BOM + newline combo)
    if [[ "$added_count" -gt 2 || "$removed_count" -gt 2 ]]; then
        continue
    fi

    # Get content without the +/- prefix
    added_content=$(echo "$added_lines" | sed 's/^+//')
    removed_content=$(echo "$removed_lines" | sed 's/^-//')

    # Strip BOM (UTF-8 BOM is EF BB BF)
    added_no_bom=$(echo "$added_content" | sed 's/^\xEF\xBB\xBF//' | sed 's/^﻿//')
    removed_no_bom=$(echo "$removed_content" | sed 's/^\xEF\xBB\xBF//' | sed 's/^﻿//')

    # Strip empty lines (newline-only changes)
    added_trimmed=$(echo "$added_no_bom" | grep -v '^$' || true)
    removed_trimmed=$(echo "$removed_no_bom" | grep -v '^$' || true)

    # If content is identical after stripping BOM and empty lines, revert
    if [[ "$added_trimmed" == "$removed_trimmed" ]]; then
        echo "Reverting hidden change: $file"
        git checkout -- "$file"
        ((reverted++))
    fi
done

echo ""
echo "Checked $checked files, reverted $reverted hidden changes"
