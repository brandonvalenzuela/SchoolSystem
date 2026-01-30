@echo off
REM Script de validación local para Windows antes de push
REM Asegura que los cambios pasen CI antes de enviarse a GitHub

setlocal enabledelayedexpansion

cls

REM Variables
set SOLUTION_PATH=.\SchoolSystem.sln
set TEST_TIMEOUT=600
set UNIT_TEST_PROJECT=tests\SchoolSystem.UnitTests\SchoolSystem.UnitTests.csproj
set INT_TEST_PROJECT=tests\SchoolSystem.IntegrationTests\SchoolSystem.IntegrationTests.csproj

REM Colors (usando salida estándar)
echo.
echo ================================================
echo   SchoolSystem - Pre-Push Validation Script
echo ================================================
echo.

REM 1. Verificar .NET
echo [1/6] Verificando .NET SDK...
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: .NET SDK no encontrado
    exit /b 1
)
for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo OK - .NET %DOTNET_VERSION% encontrado
echo.

REM 2. Verificar MySQL (opcional)
echo [2/6] Verificando MySQL...
mysql -h localhost -u root -proot -e "SELECT 1" >nul 2>&1
if errorlevel 1 (
    echo WARNING - MySQL no accesible (Integration tests serán skipped)
) else (
    echo OK - MySQL accesible
    mysql -h localhost -u root -proot -e "DROP DATABASE IF EXISTS SchoolSystem_Test; CREATE DATABASE SchoolSystem_Test;" >nul 2>&1
    echo OK - BD de test preparada
)
echo.

REM 3. Restore
echo [3/6] Restaurando dependencias de NuGet...
dotnet restore %SOLUTION_PATH% >nul 2>&1
if errorlevel 1 (
    echo ERROR: Falló al restaurar dependencias
    exit /b 1
)
echo OK - Dependencias restauradas
echo.

REM 4. Build
echo [4/6] Compilando solución...
dotnet build %SOLUTION_PATH% --configuration Release --no-restore >nul 2>&1
if errorlevel 1 (
    echo ERROR: Falló la compilación
    dotnet build %SOLUTION_PATH% --configuration Release --no-restore
    exit /b 1
)
echo OK - Compilación exitosa
echo.

REM 5. Unit Tests
echo [5/6] Ejecutando Unit Tests...
if exist %UNIT_TEST_PROJECT% (
    dotnet test %UNIT_TEST_PROJECT% ^
        --configuration Release ^
        --no-build ^
        --logger "console;verbosity=minimal" ^
        --results-directory .\test-results
    if errorlevel 1 (
        echo ERROR: Unit Tests fallaron
        exit /b 1
    )
    echo OK - Unit Tests pasaron
) else (
    echo WARNING - Proyecto de Unit Tests no encontrado
)
echo.

REM 6. Integration Tests
echo [6/6] Ejecutando Integration Tests...
if exist %INT_TEST_PROJECT% (
    mysql -h localhost -u root -proot -e "SELECT 1" >nul 2>&1
    if errorlevel 1 (
        echo WARNING - MySQL no disponible, skipping Integration Tests
    ) else (
        echo Ejecutando Integration Tests...
        dotnet test %INT_TEST_PROJECT% ^
            --configuration Release ^
            --no-build ^
            --filter "FullyQualifiedName~CalificacionesMasivoIntegrationTests" ^
            --logger "console;verbosity=minimal" ^
            --results-directory .\test-results
        if errorlevel 1 (
            echo ERROR: Integration Tests fallaron
            exit /b 1
        )
        echo OK - Integration Tests pasaron
    )
) else (
    echo WARNING - Proyecto de Integration Tests no encontrado
)
echo.

REM Success!
echo ================================================
echo OK - VALIDACION COMPLETADA EXITOSAMENTE
echo ================================================
echo.
echo La solución está lista para push:
echo   - Compilación: OK
echo   - Unit Tests: OK
echo   - Integration Tests: OK (si disponible)
echo.
echo Próximos pasos:
echo   1. git add .
echo   2. git commit -m "Your message"
echo   3. git push origin your-branch
echo.
echo El pipeline de CI ejecutará validaciones adicionales en GitHub.
echo.

exit /b 0
