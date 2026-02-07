@echo off
echo Building VelocityFixer Release...
dotnet publish -c Release
echo.
echo Build complete! Check bin\Release\net10.0\win-x64\publish\
pause