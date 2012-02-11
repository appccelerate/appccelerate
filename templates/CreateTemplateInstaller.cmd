@echo off
set resultDir=..
set zipexe=..\zip.exe
set zip2exe=zip.exe
set librarydir=Appccelerate.Library
set librarytestdir=Appccelerate.Library.Test
set libraryspecificationsdir=Appccelerate.Library.Specification

echo "============================================================"
echo "Creating library project"
echo "============================================================"

cd %librarydir%
%zipexe% -r %resultDir%\Appccelerate.Library.zip *.*  -0 -u -x *.vscontent
cd %resultDir%
copy %librarydir%\Appccelerate.Library.vscontent .
%zip2exe% -r Appccelerate.Library.vsi Appccelerate.Library.zip Appccelerate.Library.vscontent  -0 -u

echo "============================================================"
echo "Creating test library project"
echo "============================================================"

cd %librarytestdir%
%zipexe% -r %resultDir%\Appccelerate.Library.Test.zip *.*  -0 -u -x *.vscontent
cd %resultDir%
copy %librarytestdir%\Appccelerate.Library.Test.vscontent .
%zip2exe% -r Appccelerate.Library.Test.vsi Appccelerate.Library.Test.zip Appccelerate.Library.Test.vscontent  -0 -u

echo "============================================================"
echo "Creating specifications library project"
echo "============================================================"

cd %libraryspecificationsdir%
%zipexe% -r %resultDir%\Appccelerate.Library.Specification.zip *.*  -0 -u -x *.vscontent
cd %resultDir%
copy %libraryspecificationsdir%\Appccelerate.Library.Specification.vscontent .
%zip2exe% -r Appccelerate.Library.Specification.vsi Appccelerate.Library.Specification.zip Appccelerate.Library.Specification.vscontent  -0 -u

del *.zip
del *.vscontent

echo "============================================================"
echo "Finished"
echo "============================================================"