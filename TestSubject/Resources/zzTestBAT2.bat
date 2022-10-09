@ECHO OFF
setlocal

SET HEADERFILE=%~dp0CopyrightHeader.txt

FOR /R %%F in (*.cs) DO CALL :AddHeader %%F

EXIT /B 0

:AddHeader
   CALL :IsDesigner %~n1
   if %DESIGNER%==1 GOTO :EOF
   echo Adding header text to CS file %1
   copy %HEADERFILE% %HEADERFILE%.tmp >NUL
   fart.exe -- %HEADERFILE%.tmp "{fileName}" "%~nx1" >NUL
   type %HEADERFILE%.tmp "%1" > "%~1.tmp" 2>NUL
   move /y  "%~1.tmp" "%~1" >NUL
   del %HEADERFILE%.tmp
GOTO :EOF

:IsDesigner
  set DESIGNER=0
  IF /i "%~x1"==".Designer" set DESIGNER=1
GOTO :EOF

