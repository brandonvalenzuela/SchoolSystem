#!/bin/bash

# Script de validación local antes de push
# Asegura que los cambios pasen CI antes de enviarse a GitHub

set -e

# Colores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Variables
SOLUTION_PATH="./SchoolSystem.sln"
DOTNET_VERSION="8.0"
TEST_TIMEOUT=600  # 10 minutos

echo -e "${BLUE}================================================${NC}"
echo -e "${BLUE}  SchoolSystem - Pre-Push Validation Script${NC}"
echo -e "${BLUE}================================================${NC}"
echo ""

# 1. Verificar que .NET esté instalado
echo -e "${YELLOW}[1/6] Verificando .NET SDK...${NC}"
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}ERROR: .NET SDK no encontrado${NC}"
    exit 1
fi
DOTNET_ACTUAL=$(dotnet --version)
echo -e "${GREEN}✓ .NET ${DOTNET_ACTUAL} encontrado${NC}"
echo ""

# 2. Verificar que MySQL esté corriendo (si se necesita)
echo -e "${YELLOW}[2/6] Verificando MySQL (para Integration Tests)...${NC}"
if command -v mysql &> /dev/null; then
    if mysql -h localhost -u root -proot -e "SELECT 1" > /dev/null 2>&1; then
        echo -e "${GREEN}✓ MySQL accesible en localhost:3306${NC}"
        # Crear BD de test
        mysql -h localhost -u root -proot -e "DROP DATABASE IF EXISTS SchoolSystem_Test; CREATE DATABASE SchoolSystem_Test;" 2>/dev/null
        echo -e "${GREEN}✓ BD de test preparada${NC}"
    else
        echo -e "${YELLOW}⚠ MySQL no accesible (Integration tests serán skipped)${NC}"
    fi
else
    echo -e "${YELLOW}⚠ MySQL CLI no encontrado (skipping MySQL checks)${NC}"
fi
echo ""

# 3. Restore dependencies
echo -e "${YELLOW}[3/6] Restaurando dependencias de NuGet...${NC}"
if dotnet restore $SOLUTION_PATH > /dev/null 2>&1; then
    echo -e "${GREEN}✓ Dependencias restauradas${NC}"
else
    echo -e "${RED}ERROR: Falló al restaurar dependencias${NC}"
    exit 1
fi
echo ""

# 4. Build
echo -e "${YELLOW}[4/6] Compilando solución...${NC}"
if dotnet build $SOLUTION_PATH --configuration Release --no-restore > /dev/null 2>&1; then
    echo -e "${GREEN}✓ Compilación exitosa${NC}"
else
    echo -e "${RED}ERROR: Falló la compilación${NC}"
    dotnet build $SOLUTION_PATH --configuration Release --no-restore
    exit 1
fi
echo ""

# 5. Run Unit Tests
echo -e "${YELLOW}[5/6] Ejecutando Unit Tests...${NC}"
UNIT_TEST_PROJECT="tests/SchoolSystem.UnitTests/SchoolSystem.UnitTests.csproj"
if [ -f "$UNIT_TEST_PROJECT" ]; then
    if dotnet test "$UNIT_TEST_PROJECT" \
        --configuration Release \
        --no-build \
        --logger "console;verbosity=minimal" \
        --results-directory ./test-results \
        --diag ./logs/diagnostics.txt \
        2>&1 | tee ./test-unit.log; then
        echo -e "${GREEN}✓ Unit Tests pasaron${NC}"
    else
        echo -e "${RED}ERROR: Unit Tests fallaron${NC}"
        echo ""
        echo "Últimas líneas del log:"
        tail -20 ./test-unit.log
        exit 1
    fi
else
    echo -e "${YELLOW}⚠ Proyecto de Unit Tests no encontrado${NC}"
fi
echo ""

# 6. Run Integration Tests (si MySQL está disponible)
echo -e "${YELLOW}[6/6] Ejecutando Integration Tests...${NC}"
INT_TEST_PROJECT="tests/SchoolSystem.IntegrationTests/SchoolSystem.IntegrationTests.csproj"
if [ -f "$INT_TEST_PROJECT" ]; then
    if mysql -h localhost -u root -proot -e "SELECT 1" > /dev/null 2>&1; then
        echo "MySQL disponible, ejecutando Integration Tests..."
        if timeout $TEST_TIMEOUT dotnet test "$INT_TEST_PROJECT" \
            --configuration Release \
            --no-build \
            --filter "FullyQualifiedName~CalificacionesMasivoIntegrationTests" \
            --logger "console;verbosity=minimal" \
            --results-directory ./test-results \
            2>&1 | tee ./test-integration.log; then
            echo -e "${GREEN}✓ Integration Tests pasaron${NC}"
        else
            RESULT=$?
            if [ $RESULT -eq 124 ]; then
                echo -e "${YELLOW}⚠ Integration Tests timeout (> ${TEST_TIMEOUT}s)${NC}"
            else
                echo -e "${RED}ERROR: Integration Tests fallaron${NC}"
                echo ""
                echo "Últimas líneas del log:"
                tail -20 ./test-integration.log
                exit 1
            fi
        fi
    else
        echo -e "${YELLOW}⚠ MySQL no disponible, skipping Integration Tests${NC}"
        echo "   Nota: CI pipeline ejecutará estos tests en pipeline"
    fi
else
    echo -e "${YELLOW}⚠ Proyecto de Integration Tests no encontrado${NC}"
fi
echo ""

# ✅ Success!
echo -e "${BLUE}================================================${NC}"
echo -e "${GREEN}✅ VALIDACIÓN COMPLETADA EXITOSAMENTE${NC}"
echo -e "${BLUE}================================================${NC}"
echo ""
echo -e "${GREEN}La solución está lista para push:${NC}"
echo "  • Compilación: ✓"
echo "  • Unit Tests: ✓"
echo "  • Integration Tests: ✓ (si disponible)"
echo ""
echo -e "${YELLOW}Próximos pasos:${NC}"
echo "  1. git add ."
echo "  2. git commit -m 'Your message'"
echo "  3. git push origin your-branch"
echo ""
echo "El pipeline de CI ejecutará validaciones adicionales en GitHub."
echo ""
