@echo off
echo Refreshing source files:

::
echo FromCoreEx
pushd +FromCoreEx
set SRC=..\..\..\BclEx-Extend\src\System.CoreEx
xcopy %SRC%\+Kludge\Collections\HashHelpers.cs +Polyfill\Collections\ /Y/Q
xcopy %SRC%\+Kludge\IO\* +Polyfill\IO\ /Y/S/Q
xcopy %SRC%\+Kludge\IO+Enumerate\* +Polyfill\IO+Enumerate\ /Y/S/Q
xcopy %SRC%\+Kludge\Microsoft.Win32_\* +Polyfill\Microsoft.Win32_\ /Y/S/Q
xcopy %SRC%\+Kludge\Runtime\* +Polyfill\Runtime\ /Y/S/Q
xcopy %SRC%\Data\DataReaderExtensions.cs Data\ /Y/Q
xcopy %SRC%\IO\WrapTextReader.cs IO\ /Y/Q
xcopy %SRC%\Linq\Expressions\ExpressionEx.cs Linq\Expressions\ /Y/Q
xcopy %SRC%\Reflection\AssemblyExtensions.cs Reflection\ /Y/Q
xcopy %SRC%\CoreExtensions+Lazy.cs .\ /Y/Q
xcopy %SRC%\ExceptionEx.cs .\ /Y/Q
popd
pause