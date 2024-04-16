@echo off
setlocal
:PROMPT
SET /P AREYOUSURE=Are you sure (Y/[N])?
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END:
d:
cd d:\Games\butler
butler.exe push "E:\Dev\DAG\Itch Builds\Darskerry Redemption" DaftAppleGames/darskerry-redemption:windows-alpha
pause
:END
endlocal