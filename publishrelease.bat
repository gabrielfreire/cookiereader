@ECHO off
dotnet publish CookieReader -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true