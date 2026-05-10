@echo off
setlocal
set REPO_ROOT=%~dp0
pushd "%REPO_ROOT%"
dotnet run --project .\build\_build.csproj -- %*
set EXIT_CODE=%ERRORLEVEL%
popd
exit /b %EXIT_CODE%
