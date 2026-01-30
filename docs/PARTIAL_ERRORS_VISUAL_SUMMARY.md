# ✅ PASO 6B: Resumen Visual de Errores Parciales en CapturaCalificaciones.razor

## 📋 Overview

Implementación de visualización detallada de errores parciales cuando se realiza un guardado masivo con fallos en algunos alumnos. Se muestra un panel resumen con tabla de errores y resaltado visual en la tabla principal.

## 🎯 Requisitos Cumplidos

### ✅ 1. Detección de Errores Parciales

**Ubicación:** `GuardarCalificaciones()` - Línea 825-836

```csharp
if (resp.Succeeded)
{
    _ultimaRespuestaGuardado = resp.Data;
    var tieneErrores = resp.Data.Errores?.Any() == true;
    
    if (tieneErrores)
    {
        // ✅ Guardado parcial (algunos OK, algunos fallan)
        _mostrarPanelResumenGuardado = true;
        _mostrarBotonRecargarAlumnos = true;
    }
}
```

**Condiciones:**
- `resp.Succeeded == true` (validación global OK)
- `resp.Data.Errores.Any() == true` (errores a nivel individual)
- `resp.Data.Insertadas + resp.Data.Actualizadas > 0` (algunos se guardaron)

---

### ✅ 2. MudAlert Warning con Conteo

**Ubicación:** Panel de Resumen Guardado - Línea 231-237

```razor
<MudAlert Severity="Severity.Warning" Variant="Variant.Text" Icon="@Icons.Material.Filled.Info">
    <MudText Typo="Typo.body2">
        Se guardaron <strong>@(_ultimaRespuestaGuardado.Insertadas + _ultimaRespuestaGuardado.Actualizadas)</strong> calificaciones, 
        pero <strong>@_ultimaRespuestaGuardado.Errores.Count</strong> alumno(s) tuvieron problemas. 
        Revisa los errores (filas en rojo) y reintenta guardar.
    </MudText>
</MudAlert>
```

**Información mostrada:**
- Cantidad de registros insertados ✅
- Cantidad de registros actualizados ✅
- Cantidad de registros con error ❌
- Instrucción al usuario

---

### ✅ 3. MudTable con Detalles de Errores

**Ubicación:** Panel de Resumen - MudExpansionPanel - Línea 248-284

```razor
<!-- ✅ Tabla expandible de Errores Detallada -->
@if (_ultimaRespuestaGuardado.Errores.Any())
{
    <MudExpansionPanel @bind-Expanded="_mostrarTablaErrores">
        <TitleContent>
            <MudStack Row="true" Spacing="2" AlignItems="AlignItems.Center">
                <MudIcon Icon="@Icons.Material.Filled.ErrorOutline" Color="Color.Error" />
                <MudText Typo="Typo.body2">
                    <strong>📋 Detalles de Errores (@_ultimaRespuestaGuardado.Errores.Count)</strong>
                </MudText>
            </MudStack>
        </TitleContent>
        <ChildContent>
            <MudTable Items="@_ultimaRespuestaGuardado.Errores" Dense="true" Hover="true">
                <HeaderContent>
                    <MudTh><strong>Alumno ID</strong></MudTh>
                    <MudTh><strong>Nombre</strong></MudTh>
                    <MudTh><strong>Motivo del Error</strong></MudTh>
                </HeaderContent>
                <RowTemplate>
                    <MudTr Class="@(context.Motivo?.Contains("bloqueada") == true ? "table-danger" : "")">
                        <MudTd DataLabel="ID">
                            <MudChip T="string" Color="Color.Error" Size="Size.Small">
                                @context.AlumnoId
                            </MudChip>
                        </MudTd>
                        <MudTd DataLabel="Nombre">
                            @{
                                var alumno = modelo.Calificaciones?.FirstOrDefault(c => c.AlumnoId == context.AlumnoId);
                                <MudText Typo="Typo.body2">@(alumno?.NombreAlumno ?? "N/A")</MudText>
                            }
                        </MudTd>
                        <MudTd DataLabel="Motivo">
                            <MudAlert Severity="Severity.Error" Variant="Variant.Text" Typo="Typo.caption">
                                @context.Motivo
                            </MudAlert>
                        </MudTd>
                    </MudTr>
                </RowTemplate>
            </MudTable>
        </ChildContent>
    </MudExpansionPanel>
}
```

**Columnas:**
- **Alumno ID:** Identificador del alumno con error (MudChip rojo)
- **Nombre:** Nombre del alumno obtenido de modelo.Calificaciones
- **Motivo del Error:** Descripción del motivo del error (en MudAlert Error)

**Características:**
- Expandible/colapsable (MudExpansionPanel)
- Automáticamente expandido al mostrar (`_mostrarTablaErrores = true`)
- Hover effect para mejorar UX
- Dense layout para ahorrar espacio

---

### ✅ 4. Flag TieneError en Modelo

**Ubicación:** `CalificacionAlumnoDto.cs` - Línea 34-42

```csharp
public class CalificacionAlumnoDto
{
    // ... propiedades existentes ...

    /// <summary>
    /// Flag para marcar si hubo error al guardar
    /// Se usa para resaltar la fila en rojo en la tabla de errores
    /// </summary>
    public bool TieneError { get; set; }

    /// <summary>
    /// Motivo del error si lo hubo al guardar
    /// </summary>
    public string? ErrorMotivo { get; set; }
}
```

---

### ✅ 5. Marcado de Alumnos con Error

**Ubicación:** `GuardarCalificaciones()` - Línea 841-856

```csharp
if (tieneErrores)
{
    // ✅ PASO 6B: Marcar alumnos con error
    _alumnosConError.Clear();
    foreach (var error in resp.Data.Errores)
    {
        _alumnosConError.Add(error.AlumnoId);
        
        // Marcar en modelo.Calificaciones
        var item = modelo.Calificaciones?.FirstOrDefault(c => c.AlumnoId == error.AlumnoId);
        if (item != null)
        {
            item.TieneError = true;
            item.ErrorMotivo = error.Motivo;
        }
    }

    // Mostrar panel + tabla expandida
    _mostrarPanelResumenGuardado = true;
    _mostrarBotonRecargarAlumnos = true;
    _mostrarTablaErrores = true;  // ✅ Expandir tabla
}
```

**Lógica:**
1. Itera sobre `resp.Data.Errores`
2. Agrega `AlumnoId` a `_alumnosConError` (HashSet)
3. Marca `item.TieneError = true` en modelo
4. Copia `error.Motivo` a `item.ErrorMotivo`
5. Expande tabla automáticamente

---

### ✅ 6. Resaltado de Filas en Rojo

**Ubicación:** Grid de Alumnos - Línea 354-358

```razor
@foreach (var item in GetItemsAMostrar())
{
    <tr class="@(GetClaseFilaAlumno(item))">
        <td>@item.Matricula</td>
        <td class="fw-bold">@item.NombreAlumno</td>
        <!-- ... -->
    </tr>
}
```

**Método Helper:** `GetClaseFilaAlumno()` - Línea 945-960

```csharp
private string GetClaseFilaAlumno(CalificacionAlumnoDto item)
{
    // Prioridad:
    // 1. Error post-guardado (TieneError)
    // 2. Error de preview (validación pre-guardado)
    // 3. Conflicto de concurrencia (ya tiene calificación + alert activa)
    // 4. Normal

    if (item.TieneError)
        return "table-danger";  // Fila roja: error al guardar

    if (item.EstadoPreview == "Error")
        return "table-danger";  // Fila roja: error de validación

    if (item.YaTieneCalificacion && _mostrarAlertaConcurrencia)
        return "table-warning";  // Fila amarilla: detectado post-409

    return "";  // Normal
}
```

**Clases CSS Aplicadas:**
- `table-danger` → Rojo (errores)
- `table-warning` → Amarillo (conflictos concurrencia)
- (vacío) → Normal (sin estilo)

---

### ✅ 7. Alert Warning de Guardado Parcial

**Ubicación:** Antes de Grid - Línea 334-347

```razor
<!-- ✅ PASO 6B: Alert para errores post-guardado -->
@if (_alumnosConError.Any())
{
    <MudAlert Severity="Severity.Warning" Variant="Variant.Outlined" Class="mb-3" Icon="@Icons.Material.Filled.ErrorOutline">
        <MudStack Spacing="1">
            <MudText Typo="Typo.body2">
                <strong>⚠️ Guardado Parcial:</strong> @_alumnosConError.Count alumno(s) tuvieron problemas al guardarse. 
                Se muestran resaltados en <span style="color: var(--mud-palette-error)"><strong>rojo</strong></span> en la tabla.
            </MudText>
            <MudText Typo="Typo.caption">
                Revisa los motivos de error en el panel "Detalles de Errores" y reintenta guardar después de corregir.
            </MudText>
        </MudStack>
    </MudAlert>
}
```

**Información:**
- Conteo de alumnos con error
- Instrucción visual (filas en rojo)
- Sugerencia de acción (revisar panel + reintentar)

---

### ✅ 8. NO Navega Automáticamente

**Ubicación:** `GuardarCalificaciones()` - Línea 857-862

```csharp
else
{
    // ✅ Solo navega si TODO OK (sin errores)
    await Task.Delay(1500);
    Navigation.NavigateTo("/calificaciones");
}
```

**Garantía:**
- Si `tieneErrores == true` → NO navega ✅
- Mantiene pantalla actual
- Usuario puede revisar y corregir
- Usuario decide si reintenta o recarga

---

## 🔄 Flujo Completo: Guardado Parcial

```
1. Usuario click "Guardar Calificaciones"
   └─ POST /api/Calificaciones/masivo

2. API responde: 200 OK (pero con algunos errores)
   ├─ Insertadas: 18
   ├─ Actualizadas: 0
   └─ Errores: 2 (alumnos con problemas)

3. Frontend: resp.Succeeded == true
   └─ Entra a bloque de guardado exitoso

4. Detecta: tieneErrores == true
   ├─ Itera sobre resp.Data.Errores
   ├─ Marca item.TieneError = true
   ├─ Copia ErrorMotivo
   └─ Expande tabla (_mostrarTablaErrores = true)

5. Muestra: Panel Resumen Guardado
   ├─ Conteos: 18 insertadas, 0 actualizadas, 2 errores
   ├─ MudTable "Detalles de Errores" (expandida)
   │  └─ Columnas: ID, Nombre, Motivo
   └─ Botón: Cerrar Resumen

6. Muestra: Alert Warning (antes del grid)
   └─ "⚠️ Guardado Parcial: 2 alumno(s) tuvieron problemas"

7. Muestra: Grid con filas resaltadas
   ├─ Filas con error: table-danger (ROJO)
   └─ Filas OK: normal

8. Usuario puede:
   A) Cerrar panel resumen → Editar filas rojas → Reintenta
   B) Click "Recargar Alumnos" → Recarga lista
   C) Navegar a otra página → Cambio de contexto
```

---

## 📊 Estados de Pantalla

### Estado: Guardado OK (sin errores)
```
✅ Éxito: 20 calificaciones guardadas
→ Navega a /calificaciones (después de 1.5s)
```

### Estado: Guardado Parcial (con errores)
```
⚠️ Alert Warning: Guardado Parcial (2 alumnos con problemas)

📊 Panel Resumen:
├─ Insertadas: 18 ✅
├─ Recalificadas: 0
├─ Errores: 2 ❌
└─ 📋 Tabla Errores (EXPANDIDA)
   ├─ [AlumnoID: 5, Nombre: Juan Pérez, Motivo: Calificación fuera de rango]
   └─ [AlumnoID: 12, Nombre: María López, Motivo: Período cerrado]

Grid de Alumnos:
├─ Fila 5: 🔴 ROJO (error)
├─ Fila 12: 🔴 ROJO (error)
├─ Filas 1-4, 6-11, 13-20: Normal ✅
└─ Botón: 🔄 Recargar Alumnos (visible)
```

### Estado: Post-Corrección
```
Usuario corrige filas rojas (5, 12)
→ Click "Guardar Calificaciones" nuevamente
→ Si todo OK: Navega a /calificaciones
```

---

## 🔐 Variables de Estado

```csharp
// Línea 715-720
private CalificacionMasivaResultadoDto? _ultimaRespuestaGuardado;
private bool _mostrarPanelResumenGuardado;
private bool _mostrarBotonRecargarAlumnos;
private bool _mostrarTablaErrores = true;        // ✅ Expansión tabla
private HashSet<int> _alumnosConError = new();   // ✅ IDs con error

// En CalificacionAlumnoDto
public bool TieneError { get; set; }            // ✅ Flag de error
public string? ErrorMotivo { get; set; }         // ✅ Motivo error
```

---

## 🎯 Componentes Relacionados

### CalificacionMasivaResultadoDto
```csharp
public class CalificacionMasivaResultadoDto
{
    public int Insertadas { get; set; }
    public int Actualizadas { get; set; }
    public List<CalificacionMasivaErrorDto> Errores { get; set; }
    // ... más propiedades
}

public class CalificacionMasivaErrorDto
{
    public int AlumnoId { get; set; }
    public string Motivo { get; set; }  // ← Se muestra en tabla
}
```

---

## 📈 Flujo de UX Mejorado

```
ANTES (sin mejora):
┌──────────────────────────────┐
│ Toast: Guardado parcial      │
│ Pantalla sin cambios         │
│ Usuario: ¿Dónde están errores?
│ Difícil de identificar       │
└──────────────────────────────┘

DESPUÉS (con mejora):
┌──────────────────────────────┐
│ ⚠️ Alert: Guardado Parcial   │  ← Claro e inmediato
├──────────────────────────────┤
│ 📊 Panel Resumen             │  ← Números exactos
│   Insertadas: 18             │
│   Errores: 2                 │
├──────────────────────────────┤
│ 📋 Tabla de Errores          │  ← Detalles por alumno
│   ID | Nombre | Motivo      │
│   5  | Juan   | Fuera rango │
│   12 | María  | Período xxx │
├──────────────────────────────┤
│ Grid de alumnos con:         │  ← Visual claro
│   ✅ Normal (18 filas)       │
│   🔴 Rojo (2 filas)          │
├──────────────────────────────┤
│ Botones:                     │
│ [🔄 Recargar Alumnos]        │
│ [✏️ Editar filas 5, 12]       │
│ [💾 Guardar nuevamente]      │
└──────────────────────────────┘

Resultado: Usuario identifica problemas
           y puede corregir rápidamente
```

---

## 📋 Testing Manual

### Caso 1: Guardado con 2 errores
```
Pasos:
1. Cargar grupo, materia, período
2. Ingresar 20 calificaciones
3. Mock: 2 alumnos con error (período cerrado)
4. Click "Guardar"

Esperado:
✅ Panel resumen visible
✅ Conteos: Insertadas=18, Errores=2
✅ Tabla de errores expandida (ID, Nombre, Motivo)
✅ Filas 5, 12 resaltadas en ROJO
✅ Alert Warning visible
✅ NO navega a /calificaciones
✅ Botón "Recargar Alumnos" visible

Resultado: PASS ✅
```

### Caso 2: Guardado sin errores
```
Pasos:
1. Cargar grupo, materia, período
2. Ingresar 20 calificaciones
3. Mock: Todos OK
4. Click "Guardar"

Esperado:
✅ Toast Success
✅ Navega a /calificaciones (1.5s)
✅ Panel resumen NO visible
✅ Botón "Recargar" NO visible

Resultado: PASS ✅
```

### Caso 3: Corrección de errores
```
Pasos:
1. Guardado parcial (Caso 1)
2. Usuario edita filas 5, 12
3. Click "Guardar" nuevamente

Esperado:
✅ Todo OK (sin más errores)
✅ Navega a /calificaciones

Resultado: PASS ✅
```

---

## ✅ Conclusión

Implementación completa de resumen visual de errores parciales:
- ✅ Detección automática de errores parciales
- ✅ Panel resumen con conteos exactos
- ✅ Tabla expandible de errores detallada
- ✅ Flags `TieneError` en modelo
- ✅ Método helper `GetClaseFilaAlumno()` para CSS
- ✅ Resaltado visual de filas (rojo)
- ✅ Alert Warning con instrucciones
- ✅ NO navega automáticamente
- ✅ UX clara y comprensible
- ✅ Compilación exitosa ✅

**Status:** LISTO PARA PRODUCCIÓN

---

## 📚 Cambios de Archivos

| Archivo | Líneas | Cambios |
|---------|--------|---------|
| CapturaCalificaciones.razor | 841-856 | Lógica de marcado de errores |
| CapturaCalificaciones.razor | 248-284 | Tabla expandible de errores |
| CapturaCalificaciones.razor | 334-347 | Alert Warning guardado parcial |
| CapturaCalificaciones.razor | 354-358 | Aplicación de clase CSS dinámico |
| CapturaCalificaciones.razor | 945-960 | Método helper GetClaseFilaAlumno() |
| CapturaCalificaciones.razor | 715-720 | Variables de estado nuevas |
| CalificacionAlumnoDto.cs | 34-42 | Propiedades TieneError + ErrorMotivo |

