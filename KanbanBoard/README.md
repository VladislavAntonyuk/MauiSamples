# KanbanBoard

[![Buy Me A Coffee](https://ik.imagekit.io/VladislavAntonyuk/vladislavantonyuk/misc/bmc-button.png)](https://www.buymeacoffee.com/vlad.antonyuk)

Article: https://vladislavantonyuk.azurewebsites.net/articles/Creating-Kanban-Board-using-Xamarin-Forms-5

## Build
```pwsh
dotnet build KanbanBoard.csproj -t:Run -f net8.0-android

xcrun simctl list devices
dotnet build KanbanBoard.csproj -t:Run -f net8.0-ios -p:_DeviceName=:v2:udid=XXX
dotnet build KanbanBoard.csproj -t:Run -f net8.0-maccatalyst
dotnet build KanbanBoard.csproj -t:Run -f net8.0-windows10.0.19041.0
dotnet build KanbanBoard.csproj -t:Run -f net8.0-tizen
```

## Images

### Add columns and cards

![kanban1](https://user-images.githubusercontent.com/33021114/109400008-ddc4bc00-794e-11eb-9909-58e6403b29e1.png)

### Draw & Drop cards

![Drag & Drop](https://user-images.githubusercontent.com/33021114/109400009-de5d5280-794e-11eb-97a9-cc980dd74a93.png)

### Check WIP limit

![WIP is reached](https://user-images.githubusercontent.com/33021114/109400010-de5d5280-794e-11eb-8600-643220c150d7.png)

### Delete cards

![Delete card](https://user-images.githubusercontent.com/33021114/109400005-dc938f00-794e-11eb-8fce-3c8ac6f12502.png)

### Delete columns

![Delete column](https://user-images.githubusercontent.com/33021114/109400007-ddc4bc00-794e-11eb-94a4-ebf858198c6d.png)

[![Stand With Ukraine](https://img.shields.io/badge/made_in-ukraine-ffd700.svg?labelColor=0057b7)](https://stand-with-ukraine.pp.ua)