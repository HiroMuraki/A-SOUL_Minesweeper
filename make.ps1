dotnet publish -c release -r win10-x64 --self-contained=false /p:PublishSingleFile=True

$hasPath = Test-Path "Output/A-SOULɨ��.exe"
if ($hasPath) {
    Move-Item -Force "Output/A-SOULɨ��.exe" "Output/A-SOULɨ��.exe.old"
}
Copy-Item "ASMinesweeperGame\bin\Release\net5.0-windows\win10-x64\publish\ASMinesweeperGame.exe" "Output/A-SOULɨ��.exe"