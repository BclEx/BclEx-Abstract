@echo off
echo Refreshing source files:

::
echo FromCoreEx
pushd +FromCoreEx
set SRC=..\..\..\BclEx-Extend\src\System.CoreEx
xcopy %SRC%\+Kludge\Collections\HashHelpers.cs +Kludge\Collections\ /Y/Q
xcopy %SRC%\+Kludge\IO\* +Kludge\IO\ /Y/S/Q
xcopy %SRC%\+Kludge\IO+Enumerate\* +Kludge\IO+Enumerate\ /Y/S/Q
xcopy %SRC%\+Kludge\Microsoft.Win32_\* +Kludge\Microsoft.Win32_\ /Y/S/Q
xcopy %SRC%\+Kludge\Runtime\* +Kludge\Runtime\ /Y/S/Q
xcopy %SRC%\Data\DataReaderExtensions.cs Data\ /Y/Q
xcopy %SRC%\IO\WrapTextReader.cs IO\ /Y/Q
xcopy %SRC%\Linq\Expressions\ExpressionEx.cs Linq\Expressions\ /Y/Q
xcopy %SRC%\Reflection\AssemblyExtensions.cs Reflection\ /Y/Q
xcopy %SRC%\CoreExtensions+Lazy.cs .\ /Y/Q
xcopy %SRC%\ExceptionEx.cs .\ /Y/Q
popd
pause