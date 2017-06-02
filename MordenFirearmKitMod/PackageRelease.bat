@echo off

:: ARGUMENTS
:: PackageRelease.bat <project dir> <dll file> <output dir>

:: Make sure all arguments are specified
if %1.==. goto noProjectDir
set PROJECTDIR=%1

if %2.==. goto noDllFile
set DLLFILE=%2

if %3.==. goto noOutputPath
set OUTPATH=%3

echo Creating new release at %OUTPATH%

:: Create directories
md %OUTPATH%
md %OUTPATH%\Blocks
md %OUTPATH%\Blocks\Obj
md %OUTPATH%\Blocks\Textures
md %OUTPATH%\Blocks\Resources

:: Copy files
copy %DLLFILE% %OUTPATH%\
xcopy %PROJECTDIR%\Resources\obj %OUTPATH%\Blocks\Obj\ /s/h/e/k/f/c
xcopy %PROJECTDIR%\Resources\tex %OUTPATH%\Blocks\Textures\ /s/h/e/k/f/c
xcopy %PROJECTDIR%\Resources\other %OUTPATH%\Blocks\Resources\ /s/h/e/k/f/c

goto :end

:noProjectDir
echo Error: No project directory passed!
goto :end

:noDllFile
echo Error: No path to dll file passed!
goto :end

:noOutputPath
echo Error: No output path specified!
goto :end

:end