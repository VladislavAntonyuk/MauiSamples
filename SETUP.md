## Build solution

1. Install .NET 8
2. Install .NET MAUI Workloads
```pwsh
dotnet workload install maui
```
3. Install Tizen

> If you don't want to run sample on Tizen you need to comment the next line: `<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);$(NetVersion)-tizen</TargetFrameworks>`. It exists in `Directory.Build.props` and `PizzaStore.Mobile.csproj`.

```pwsh
Invoke-WebRequest 'https://raw.githubusercontent.com/Samsung/Tizen.NET/main/workload/scripts/workload-install.ps1' -OutFile 'workload-install.ps1'
.\workload-install.ps1
```
4. Build
```pwsh
dotnet restore --configfile NuGet.config
dotnet build
```

## Update ReadMe

1. Install Python 3
2. Clone `http://github.com/amyreese/markdown-pp`
3. Run `pip install MarkdownPP`
4. Optional. You may need to add env variables to path: `C:\Users\{USERNAME}\AppData\Roaming\Python\Python311\Scripts`. This path should contain `markdown-pp`.
5. Run PowerShell script `./md/run.ps1`