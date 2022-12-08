Get-ChildItem ./ -Recurse -Filter README.mdpp |
Foreach-Object {
    markdown-pp --output ($_.DirectoryName + '/README.md') $_.FullName
}