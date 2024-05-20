@echo off

rem ******************
rem *DLL Library
rem ******************
copy "..\..\..\..\Library\C++ Library\x64\*.dll" . /Y

rem ******************
rem *.NET Wrapper
rem ******************
copy "..\..\..\..\Library\.NET Library\x64\*.dll" . /Y