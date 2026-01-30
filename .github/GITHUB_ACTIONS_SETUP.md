# GitHub Actions - Configuration Guide

## 📋 Setup de Secrets y Variables

Este documento describe cómo configurar los secretos y variables de entorno necesarios para los pipelines de CI/CD.

## 🔐 Secrets a Configurar

Ir a: **Settings → Secrets and variables → Actions**

### 1. Base de Datos (MySQL)
```
MYSQL_ROOT_PASSWORD = root
MYSQL_TEST_DATABASE = SchoolSystem_Test
```

### 2. Autenticación
```
GITHUB_TOKEN = (Auto-generado por GitHub)
SONARCLOUD_TOKEN = (Opcional - si usas SonarCloud)
```

### 3. Deployment (cuando sea necesario)
```
DEPLOY_KEY = (SSH key para servidor)
DEPLOY_USER = (Usuario en servidor)
DEPLOY_HOST = (IP o dominio del servidor)
```

## 🌍 Variables de Entorno

Ir a: **Settings → Secrets and variables → Actions → Variables**

```
DOTNET_VERSION = 8.0.x
CONFIGURATION = Release
SOLUTION_PATH = ./SchoolSystem.sln
ASPNETCORE_ENVIRONMENT = Testing
```

## 📝 Archivo de Configuración Local

Crear `.github/workflows/config.env` (opcional, para valores locales):

```bash
# Database
MYSQL_ROOT_PASSWORD=root
MYSQL_PORT=3306
MYSQL_VERSION=8.0

# .NET
DOTNET_VERSION=8.0.x
CONFIGURATION=Release

# Paths
SOLUTION_PATH=./SchoolSystem.sln
TEST_RESULTS_PATH=./test-results
```

## ✅ Checklist de Setup

- [ ] GitHub repository creado
- [ ] MySQL secrets configurados
- [ ] Permisos de Actions habilitados
- [ ] Workflows sincronizados
- [ ] Test local exitoso antes de push
- [ ] Ramas main/develop protegidas
- [ ] Requerimientos de CI en PR habilitados

## 🔄 Workflows Disponibles

### 1. `ci.yml`
- Trigger: Push a main/develop y PRs
- Pasos:
  - Build solution
  - Run Unit Tests
  - Run Integration Tests
  - Code Quality Checks
  - Security Checks
- Timeout: ~15 minutos

### 2. `pr-validation.yml`
- Trigger: Pull Requests abiertos/editados
- Pasos:
  - Validar cambios
  - Ejecutar tests
  - Comentar estado en PR
  - Requerir reviews
- Timeout: ~15 minutos

## 🚀 Ejecución Manual

Para ejecutar un workflow manualmente:

```bash
# Listar workflows
gh workflow list

# Ejecutar workflow específico
gh workflow run ci.yml --ref main

# Ver estado de ejecución
gh run list --workflow ci.yml
```

## 📊 Monitoreo

### Acceder a resultados:
1. GitHub repository
2. Actions tab
3. Seleccionar workflow run
4. Ver logs y artefactos

### Descargar test results:
```bash
# Desde CLI
gh run download <RUN_ID> -n test-results-8.0.x

# Desde UI: Actions → Latest run → Artifacts
```

## 🐛 Troubleshooting

### Problema: MySQL no se conecta
**Solución:**
- Verificar servicio MySQL está corriendo
- Verificar credentials en workflow
- Aumentar timeout de healthcheck

### Problema: Tests no se ejecutan
**Solución:**
- Verificar archivos de test existen
- Verificar .NET SDK instalado
- Ver logs en Actions

### Problema: Timeout en tests
**Solución:**
- Aumentar timeout en workflow
- Revisar tests que son lentos
- Considerar paralelización

## 📝 Mejores Prácticas

1. **Secrets:**
   - Nunca commitear credentials
   - Usar GitHub Secrets
   - Rotar keys regularmente

2. **Workflows:**
   - Usar versiones pinned de actions
   - Documentar dependencias
   - Mantener workflows DRY

3. **Tests:**
   - Ejecutar localmente antes de push
   - Mantener tests rápidos (<5min)
   - Evitar test flakiness

4. **Artifacts:**
   - Borrar después de review
   - Limitar retention
   - Comprimir si es grande

## 🔗 Referencias

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [MySQL Docker Image](https://hub.docker.com/_/mysql)
- [Setup .NET Action](https://github.com/actions/setup-dotnet)
- [Upload Artifacts Action](https://github.com/actions/upload-artifact)

## 📞 Soporte

Para problemas:
1. Revisar GitHub Actions logs
2. Verificar workflow YAML syntax
3. Consultar documentación oficial
4. Crear issue en repositorio

---

**Última actualización:** 2024
**Versión:** 1.0
