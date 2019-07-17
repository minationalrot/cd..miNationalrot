rmdir /Q /S publish
git rm docs -r
rmdir /Q /S docs
dotnet publish ./src/minationalrot.sln -c Release -o ./publish
xcopy publish\minationalrot\dist docs\ /s
git add docs
git commit -m "deploy ***NO_CI***"
git push
pause