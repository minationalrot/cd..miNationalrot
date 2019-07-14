rmdir /Q /S publish
rmdir /Q /S docs
dotnet publish ./src/minationalrot.sln -c Release -o ./publish
xcopy publish\minationalrot\dist docs\ /s
pause