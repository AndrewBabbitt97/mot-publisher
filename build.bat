@ECHO OFF

IF EXIST %SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319 (
    SET MSBUILDPATH=%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319
) ELSE (
    ECHO .Net Framework 4.5 Not Found!
	EXIT
)

SET MSBUILD=%MSBUILDPATH%\msbuild.exe
MKDIR packages
curl -o packages/nuget.exe https://api.nuget.org/downloads/nuget.exe
%~dp0packages/nuget.exe restore -configfile "NuGet.Config" Publisher.sln
%MSBUILD% /nologo /m /p:BuildInParallel=true /p:Configuration=Release /p:Platform="Any CPU" Publisher.sln