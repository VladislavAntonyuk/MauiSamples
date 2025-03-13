## Build solution

1. Install .NET 8
1. Install .NET MAUI Workloads
	```pwsh
	dotnet workload install maui
	```

1. Build
	```pwsh
	dotnet restore --configfile NuGet.config
	dotnet build
	```

## Update ReadMe

1. Install Python 3
1. Run `pip install MarkdownPP`
1. Optional. You may need to add env variables to path: `C:\Program Files\Python312\Scripts`. This path should contain `markdown-pp`.
1. Run PowerShell script `./md/run.ps1`

## Clean

```pwsh
find . -name '.DS_Store' -type f -delete

Get-ChildItem .\ -Force -Include Packages,.idea,.vs,bin,obj -Recurse | foreach ($_) { Remove-Item -Path $_.fullname -Force -Recurse }

git clean -dfx
```