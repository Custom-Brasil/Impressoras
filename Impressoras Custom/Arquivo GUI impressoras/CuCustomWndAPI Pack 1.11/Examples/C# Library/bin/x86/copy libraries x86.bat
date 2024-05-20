@echo off

rem ******************
rem *DLL Library
rem ******************
copy "..\..\..\..\Library\C++ Library\x86\*.dll" . /Y

rem ******************
rem *.NET Wrapper
rem ******************
copy "..\..\..\..\Library\.NET Library\x86\*.dll" . /Y