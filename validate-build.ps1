Get-ChildItem -Path .\ -Filter *.csproj -Recurse | ForEach-Object {
    Write-Host $_.FullName
    dotnet build $_.FullName
    $status = $LASTEXITCODE
    if ($status -eq 0) {
        Write-Host $_.FullName + "No errors found"
    }
}