@echo off

rem Check arguments
IF [%1] == [] GOTO ENDERROR
IF [%2] == [] GOTO ENDERROR

rem Run butler with configured arguments
setlocal
d:
cd d:\Games\butler
echo Pushing with arguments %1 %2
butler.exe push "E:\Dev\DAG\Itch Builds\Darskerry Redemption" %1:%2
echo Butler command complete
rem End
:END
endlocal

:ENDERROR
echo Arguments not found! Usage: upload-release <Game Name> <Game Stage>
