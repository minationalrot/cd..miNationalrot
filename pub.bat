rmdir /Q /S publish
rmdir /Q /S dist
dotnet publish ./src/minationalrot.sln -c Release -o ./publish
copy publish\minationalrot\dist dist\
pause