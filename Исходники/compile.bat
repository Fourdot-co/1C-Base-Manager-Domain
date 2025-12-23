@echo off
:: Указываем путь к компилятору Windows
set "MSBUILD=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"

:: Проверка, есть ли компилятор
if not exist "%MSBUILD%" (
    echo ОШИБКА: Не найден MSBuild.
    echo У вас странная версия Windows или вырезаны компоненты .NET.
    pause
    exit
)

echo.
echo ==========================================
echo Начинаем сборку программы...
echo ==========================================
echo.

:: Запускаем сборку
"%MSBUILD%" "1C8 Base Manager.sln" /p:Configuration=Release /t:Rebuild

:: Проверяем результат
if %ERRORLEVEL% EQU 0 (
    echo.
    echo ==========================================
    echo УСПЕХ! Программа собрана.
    echo ==========================================
    echo Ищите файл .exe здесь:
    echo 1C8 Base Manager\bin\Release\1C8 Base Manager.exe
    echo.
) else (
    echo.
    echo ==========================================
    echo ОШИБКА СБОРКИ
    echo ==========================================
    echo Прочитайте красный текст выше, чтобы понять причину.
)

pause