rmdir /Q /S c:\publish
dotnet publish ./src/minationalrot.sln -c Release -o c:\publish
cd c:\publish\minationalrot\dist
git init
git add .
git commit -m "Push force deploy ***NO_CI***"
git remote add origin https://github.com/minationalrot/miNationalrot.git
git push --force origin master:gh-pages
pause 