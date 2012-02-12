@echo off
if exist "C:\Program Files\Microsoft Visual Studio 9.0\VC\vcvarsall.bat" call "C:\Program Files\Microsoft Visual Studio 9.0\VC\vcvarsall.bat"
..\Tools\nant\nant publish %*
pause
