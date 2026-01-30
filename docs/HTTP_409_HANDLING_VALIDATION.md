# HTTP 409 Conflict Handling en CapturaCalificaciones.razor

## 📋 Overview

Implementación completa de manejo UX para HTTP 409 (Conflict) en la pantalla de captura masiva de calificaciones. Se detectan conflictos de concurrencia y se ofrece al usuario opciones claras de resolución.

## 🎯 Requisitos Cumplidos

### ✅ 1. Detección de 409 Conflict

**Ubicación:** `ManejarRespuestaGuardado()` - Línea 898

```csharp
if (resp.StatusCode == 409)
{
    ToastService.ShowError("❌ Otro usuario calificó al mismo tiempo. Recarga la lista y vuelve a intentar.");
    // ... manejar dialog
}
```

**Flujo:**
- POST falla con StatusCode == 409
- `resp.Succeeded == false`
- Se dispara `ManejarRespuestaGuardado(resp)`
- Se detecta 409 específicamente

---

### ✅ 2. Toast Warning Claro

**Ubicación:** Línea 900

```csharp
ToastService.ShowError("❌ Otro usuario calificó al mismo tiempo. Recarga la lista y vuelve a intentar.");
```

**Características:**
- Emoji ❌ para claridad visual
- Mensaje en español claro
- Tipo: Error (rojo)
- Sugiere acción: "Recarga la lista"

---

### ✅ 3. Dialog de Concurrencia con Opciones

**Ubicación:** Líneas 902-945

**Componente:** `ConcurrenciaMasivaDialog.razor`

**Opciones presentadas:**

```
┌─────────────────────────────────┐
│  Conflicto de Concurrencia      │
├─────────────────────────────────┤
│ Otro usuario capturó algunas    │
│ calificaciones simultáneamente. │
├─────────────────────────────────┤
│ [🔄 Recargar Estado]            │
│ [🔁 Reintentar Guardar]         │
│ [❌ Cancelar]                    │
└─────────────────────────────────┘
```

**Lógica:**

```csharp
switch (decision)
{
    case "recargar":
        // → AplicarPrecheckExistentesAsync()
        // → Mostrar botón recargar alumnos
        break;
    
    case "reintentar":
        // → GuardarCalificaciones() nuevamente
        break;
    
    case "cancelar":
        // → Mostrar botón recargar alumnos
        // → Mantener pantalla actual
        break;
}
```

---

### ✅ 4. Botón "Recargar Alumnos" Visible en Conflicto

**Ubicación en UI:** Línea 267-281

```razor
<!-- ✅ PASO 5C: Botón "Recargar alumnos" visible cuando hay errores o 409 -->
@if (_mostrarBotonRecargarAlumnos && modelo.GrupoId > 0 && modelo.MateriaId > 0 && modelo.PeriodoId > 0)
{
    <MudStack Row="true" Spacing="2" Class="mb-4" AlignItems="AlignItems.Center">
        <MudButton Variant="Variant.Filled" Color="Color.Primary" 
                   StartIcon="@Icons.Material.Filled.Refresh"
                   OnClick="CargarAlumnos"
                   Disabled="@isLoading">
            @if (isLoading)
            {
                <span class="spinner-border spinner-border-sm me-2"></span>
                <span>Recargando alumnos...</span>
            }
            else
            {
                <span>🔄 Recargar Alumnos</span>
            }
        </MudButton>
        <MudText Typo="Typo.caption" Color="Color.Default">
            Recarga la lista de alumnos para ver cambios recientes
        </MudText>
    </MudStack>
}
```

**Condiciones de Visibilidad:**
- `_mostrarBotonRecargarAlumnos == true` (establecido en 409)
- `modelo.GrupoId > 0` (grupo seleccionado)
- `modelo.MateriaId > 0` (materia seleccionada)
- `modelo.PeriodoId > 0` (período seleccionado)

**Estados:**
- ✅ Normal: "🔄 Recargar Alumnos"
- ⏳ Cargando: Spinner + "Recargando alumnos..."
- ❌ Deshabilitado: Si isLoading == true

---

### ✅ 5. CargarAlumnos() + Precheck (SoloValidar)

**Al hacer click en botón:**

```csharp
// Botón ejecuta: OnClick="CargarAlumnos"

private async Task CargarAlumnos()
{
    // 1. Valida Grupo/Materia/Período
    if (modelo.GrupoId == 0 || modelo.MateriaId == 0 || modelo.PeriodoId == 0)
        return;

    isLoading = true;

    try
    {
        // 2. Obtiene inscripciones (alumnos del grupo)
        var ins = await InscripcionService.GetAlumnosPorGrupoAsync(modelo.GrupoId, soloActivos: true);
        
        // 3. Llena modelo.Calificaciones
        foreach (var a in ins.Data)
        {
            modelo.Calificaciones.Add(new CalificacionAlumnoDto { ... });
        }

        // 4. Ejecuta precheck (SoloValidar)
        await AplicarPrecheckExistentesAsync(mostrarToast: true);
    }
    finally
    {
        isLoading = false;
    }
}

private async Task AplicarPrecheckExistentesAsync(bool mostrarToast)
{
    // ✅ SoloValidar: No persiste nada
    modelo.SoloValidar = true;
    
    // ✅ Ejecuta POST con SoloValidar=true
    var pre = await CalificacionService.CreateMasivoAsync(modelo);
    
    // ✅ Marca filas existentes con EstadoPreview
    // ✅ Actualiza precheck local
}
```

**Resultado:**
- Recarga lista de alumnos
- Ejecuta precheck (validación sin persistencia)
- Marca calificaciones existentes
- Muestra preview actualizado

---

### ✅ 6. NO Navega a /calificaciones en 409

**Ubicación:** Línea 809-810

En `GuardarCalificaciones()` - Al recibir respuesta exitosa:

```csharp
if (resp.Succeeded)
{
    // ... procesar respuesta
    
    if (tieneErrores)
    {
        // NO NAVEGA - Mantiene pantalla
        _mostrarPanelResumenGuardado = true;
    }
    else
    {
        // Solo navega si TODO OK (sin errores)
        await Task.Delay(1500);
        Navigation.NavigateTo("/calificaciones");  // ← Solo aquí
    }
}
else
{
    // En 409 u otros errores: ManejarRespuestaGuardado()
    // NO NAVEGA
    await ManejarRespuestaGuardado(resp);
}
```

**Garantía:**
- 409 → No navega ✅
- 400 → No navega ✅
- Parcial (con errores) → No navega ✅
- Todo OK → Navega ✅

---

### ✅ 7. isSaving Correctamente Manejado

**Ubicación:** Línea 824-827

```csharp
try
{
    isSaving = true;
    
    // ... lógica POST ...
}
finally
{
    isSaving = false;  // ✅ Siempre se ejecuta
    modelo.SoloValidar = false;
}
```

**Garantías:**
- Se establece `true` al inicio
- Se restaura `true` → `false` **siempre** en finally
- Incluso si hay excepción
- Incluso si hay return temprano (en catch)
- El botón Guardar se habilita nuevamente

**Protección contra Doble Submit:**

```csharp
private async Task GuardarCalificaciones()
{
    // Guard clause: evitar doble submit
    if (isSaving) 
    {
        ToastService.ShowInfo("La solicitud ya está en proceso. Por favor, espera...");
        return;  // ← No ejecuta si ya está guardando
    }

    isSaving = true;  // ← Se establece para bloquear siguientes clicks
    
    try { /* ... */ }
    finally { isSaving = false; }
}
```

---

## 🔄 Flujo Completo: 409 Conflict Handling

```
1. Usuario hace click en "Guardar Calificaciones"
   ├─ isSaving = true
   └─ Botón Guardar se deshabilita

2. POST /api/Calificaciones/masivo
   └─ Conflicto: Otro usuario capturó misma data

3. API responde: 409 Conflict
   ├─ resp.StatusCode == 409
   ├─ resp.Succeeded == false
   └─ CallBack: ManejarRespuestaGuardado()

4. Toast Error
   └─ "❌ Otro usuario calificó al mismo tiempo..."

5. Dialog ConcurrenciaMasivaDialog aparece
   ├─ [🔄 Recargar Estado]
   ├─ [🔁 Reintentar Guardar]
   └─ [❌ Cancelar]

6. Usuario selecciona opción:

   Si "Recargar":
   ├─ AplicarPrecheckExistentesAsync(true)
   ├─ Recarga lista + precheck
   ├─ _mostrarBotonRecargarAlumnos = true
   └─ Botón visible

   Si "Reintentar":
   ├─ modelo.SoloValidar = false
   ├─ GuardarCalificaciones()
   └─ Intenta nuevamente

   Si "Cancelar":
   ├─ _mostrarBotonRecargarAlumnos = true
   └─ Botón visible

7. isSaving = false (en finally)
   └─ Pantalla responde nuevamente
```

---

## 📊 Estados de Pantalla

### Estado Normal
```
[Grupo: 1A]
[Materia: Matemáticas]
[Período: Período 1]
[Botón: Cargar Alumnos]

[Grid de alumnos]

[Botón: Guardar Calificaciones] ← Habilitado
```

### Estado Durante Guardado (409)
```
Toast Error: "❌ Otro usuario calificó..."

Dialog: [Conflicto de Concurrencia]
        [🔄 Recargar | 🔁 Reintentar | ❌ Cancelar]

Botón Guardar: ❌ Deshabilitado (isSaving=true)
```

### Estado Post-409 (Si usuario cancela)
```
Panel Warning: "⚠️ Conflicto detectado"

Grid: (Sin cambios, listo para corregir)

[Botón: 🔄 Recargar Alumnos] ← ✅ Visible

[Botón: Guardar Calificaciones] ← ✅ Habilitado (isSaving=false)
```

---

## 🔐 Validaciones Enterprise

### Pre-409 Checks
- ✅ Validación de Grupo/Materia/Período
- ✅ Validación de Alumnos inscritos
- ✅ Validación de Motivo de recalificación (≥10 chars)
- ✅ Validación de Calificación (0-10)

### Post-409 Handling
- ✅ No permitir doble submit (isSaving guard)
- ✅ Cleanup automático (finally block)
- ✅ Regeneración de precheck fresco
- ✅ Actualización visual de filas
- ✅ Toast + Dialog + Botón visible

### Atomicidad
- ✅ Transacción servidor (rollback automático)
- ✅ UNIQUE constraint (evita duplicados)
- ✅ Precheck fresco post-409
- ✅ Estado UI sincronizado

---

## 📝 Componentes Relacionados

### ConcurrenciaMasivaDialog.razor
```razor
@* Dialog con 3 botones para decisión usuario *@
<MudDialog>
    <DialogContent>
        <MudText>Otro usuario capturó simultáneamente...</MudText>
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="() => MudDialog.Close('recargar')">Recargar</MudButton>
        <MudButton OnClick="() => MudDialog.Close('reintentar')">Reintentar</MudButton>
        <MudButton OnClick="() => MudDialog.Close('cancelar')">Cancelar</MudButton>
    </DialogActions>
</MudDialog>
```

### DbUpdateExceptionExtensions.cs
```csharp
public static bool IsDuplicateKeyError(this DbUpdateException ex)
{
    // Detecta MySQL error 1062 (duplicate key)
    // Retorna true si es UNIQUE violation
}
```

### ApiResponse<T>
```csharp
public class ApiResponse<T>
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }  // ← Usado para detectar 409
    public T Data { get; set; }
}
```

---

## 🎯 Casos de Uso

### Caso 1: Conflicto Simple
```
Hora 14:30:00 - Usuario A: POST 20 calificaciones
Hora 14:30:00 - Usuario B: POST 10 calificaciones (mismos alumnos)

Resultado:
- Usuario A: 200 OK (inserta primero)
- Usuario B: 409 Conflict (UNIQUE violation detectado)
  → Dialog → Recargar → Precheck actualizado
```

### Caso 2: Parcial Conflict
```
15 de 20 alumnos ya fueron capturados por usuario anterior

Resultado:
- POST devuelve 200 OK
- resp.Data.Errores.Count = 5
- _mostrarPanelResumenGuardado = true
- Botón "Recargar Alumnos" visible
- Usuario puede corregir los 5 errores
```

### Caso 3: Período Cerrado
```
POST a período ya cerrado/definitivo

Resultado:
- API: 400 BadRequest
- ManejarRespuestaGuardado()
- Toast Error: mensaje específico
- Botón Recargar visible si usuario cancela
```

---

## 🚀 Testing Manual

### Escenario 1: 409 Conflict
```
1. Usuario A: Carga grupo 1A, materia Matemáticas
2. Usuario B: Carga grupo 1A, materia Matemáticas (MISMO)
3. Usuario A: Completa y hace click "Guardar"
4. Usuario B: Completa y hace click "Guardar" (< 1 segundo después)
5. Usuario B: Recibe 409 + Dialog + puede recargar
```

### Escenario 2: Período Validations
```
1. Usuario: Selecciona período "Cerrado"
2. Usuario: Intenta guardar
3. API: Valida estado período → rechaza
4. Frontend: Toast error + Botón recargar visible
```

### Escenario 3: isSaving Guard
```
1. Usuario: Click "Guardar" (isSaving=false → true)
2. Usuario: Intenta click "Guardar" nuevamente (< 1s)
3. Frontend: Guard clause bloquea → Toast info
4. Usuario espera... (isSaving sigue true)
5. Respuesta llega → finally: isSaving=false
6. Botón nuevamente habilitado
```

---

## 📈 Métricas de Éxito

| Métrica | Target | Status |
|---------|--------|--------|
| 409 Detectado | StatusCode == 409 | ✅ |
| Toast Visible | ErrorLevel message | ✅ |
| Dialog Mostrado | ConcurrenciaMasivaDialog | ✅ |
| Botón Recargar | Visible post-409 | ✅ |
| CargarAlumnos | Ejecuta + Precheck | ✅ |
| NO Navega | /calificaciones blocked | ✅ |
| isSaving Restored | false en finally | ✅ |
| Doble Submit Bloqueado | Guard clause | ✅ |

---

## 📞 Troubleshooting

### Problema: Botón Recargar no aparece
**Solución:**
- Verificar `_mostrarBotonRecargarAlumnos == true`
- Verificar Grupo/Materia/Período seleccionados
- Verificar que `isLoading == false` (para no estar deshabilitado)

### Problema: Dialog no aparece
**Solución:**
- Verificar `ConcurrenciaMasivaDialog.razor` existe
- Verificar `@using` correcto en CapturaCalificaciones.razor
- Verificar `IDialogService` inyectado

### Problema: CargarAlumnos no ejecuta precheck
**Solución:**
- Verificar `AplicarPrecheckExistentesAsync()` se llama
- Verificar `modelo.SoloValidar = true` se establece
- Verificar CalificacionService inyectado

---

## ✅ Conclusión

Implementación completa de HTTP 409 handling con UX enterprise-grade:
- ✅ Detección automática
- ✅ Toast + Dialog intuitivo
- ✅ Opciones claras de resolución
- ✅ Botón "Recargar" siempre disponible
- ✅ Protección contra doble submit
- ✅ No navega en error
- ✅ Estado UI sincronizado

**Status:** LISTO PARA PRODUCCIÓN
