rmdir /Q /S c:\publish
dotnet publish ./src/minationalrot.sln -c Release -o c:\publish
cd c:\publish\minationalrot\dist
git init
git add .
git commit -m "Push force deploy ***NO_CI***"
git remote add origin https://RemoOser:b05095333ffd54b99c03390cca83b9796b600e03@github.com/minationalrot/miNationalrot.git
git push --force origin master:gh-pages
pause 