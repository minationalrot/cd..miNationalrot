rmdir /Q /S publish
rmdir /Q /S dist
dotnet publish ./src/minationalrot.sln -c Release -o ./publish
xcopy publish\minationalrot\dist dist\
pause