@echo off
if exist "%ProgramFiles%\Microsoft Visual Studio 9.0\VC\vcvarsall.bat" call "%ProgramFiles%\Microsoft Visual Studio 9.0\VC\vcvarsall.bat"
..\Tools\nant\nant integrate %*
pause
