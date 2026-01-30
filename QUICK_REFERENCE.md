# 🔖 Quick Reference - CapturaCalificaciones.razor Improvements

## 📍 Ubicaciones Clave

### HTTP 409 Handling
```
Línea 898:     Detección (if (resp.StatusCode == 409))
Línea 900:     Toast Error
Línea 912:     Dialog (DialogService.ShowAsync)
Línea 926-941: Opciones (switch decision)
Línea 267-281: Botón Recargar Alumnos (UI)
```

### Errores Parciales
```
Línea 841-856:    Marcado de alumnos con error
Línea 248-284:    Tabla MudTable de errores
Línea 334-347:    Alert Warning guardado parcial
Línea 715-720:    Variables de estado nuevas
Línea 945-960:    Método GetClaseFilaAlumno()
Línea 354-358:    Aplicación CSS dinámico
```

---

## 🎯 Variables de Estado

```csharp
// Conflicto 409
_mostrarAlertaConcurrencia           bool
_mostrarAlertaConflictoGuardado      bool
_mostrarBotonRecargarAlumnos         bool
_precheck                            CalificacionMasivaResultadoDto?

// Errores Parciales (NUEVAS)
_mostrarPanelResumenGuardado         bool
_mostrarTablaErrores                 bool      ← NUEVA
_alumnosConError                     HashSet<int>  ← NUEVA
_ultimaRespuestaGuardado             CalificacionMasivaResultadoDto?

// En CalificacionAlumnoDto
TieneError                           bool      ← NUEVA
ErrorMotivo                          string?   ← NUEVA
```

---

## 🔧 Métodos Principales

### GetClaseFilaAlumno() - NUEVO
```csharp
private string GetClaseFilaAlumno(CalificacionAlumnoDto item)
{
    if (item.TieneError) return "table-danger";
    if (item.EstadoPreview == "Error") return "table-danger";
    if (item.YaTieneCalificacion && _mostrarAlertaConcurrencia) return "table-warning";
    return "";
}
```

### GuardarCalificaciones() - ACTUALIZADO
```csharp
// Línea 841-856: Marca alumnos con error
if (tieneErrores)
{
    _alumnosConError.Clear();
    foreach (var error in resp.Data.Errores)
    {
        _alumnosConError.Add(error.AlumnoId);
        var item = modelo.Calificaciones?.FirstOrDefault(c => c.AlumnoId == error.AlumnoId);
        if (item != null)
        {
            item.TieneError = true;
            item.ErrorMotivo = error.Motivo;
        }
    }
    _mostrarTablaErrores = true;  // Auto-expandir
}
```

---

## 📋 Flujos Rápidos

### 409 Conflict
```
resp.StatusCode == 409 
→ Toast.ShowError() 
→ Dialog (3 opciones) 
→ Acción usuario
```

### Guardado Parcial
```
resp.Succeeded && Errores.Any()
→ Alert Warning
→ Panel Resumen (expandido)
→ Tabla de errores (expandida)
→ Grid con filas rojas
→ Usuario corrige + Reintenta
```

### Guardado OK
```
resp.Succeeded && !Errores.Any()
→ Toast Success
→ Navega /calificaciones (1.5s)
```

---

## 🎨 CSS Classes

```
table-danger    → Rojo (errores)
table-warning   → Amarillo (conflictos)
(vacío)         → Normal
```

---

## 📊 Componentes UI

### MudAlert
```razor
<MudAlert Severity="Severity.Warning" Icon="@Icons.Material.Filled.ErrorOutline">
    @message
</MudAlert>
```

### MudTable
```razor
<MudTable Items="@_ultimaRespuestaGuardado.Errores" Dense="true">
    <HeaderContent> ... </HeaderContent>
    <RowTemplate> ... </RowTemplate>
</MudTable>
```

### MudExpansionPanel
```razor
<MudExpansionPanel @bind-Expanded="_mostrarTablaErrores">
    <TitleContent> Detalles de Errores </TitleContent>
    <ChildContent> Tabla </ChildContent>
</MudExpansionPanel>
```

---

## ✅ Testing Rápido

### Caso 1: 409 (via mock)
```
POST falla con 409
→ Verificar: Toast, Dialog, Botón visible
```

### Caso 2: Errores Parciales (18 OK, 2 error)
```
POST OK con Errores.Any()
→ Verificar: Alert, Panel, Tabla, Filas rojas
```

### Caso 3: Guardado Exitoso
```
POST OK sin errores
→ Verificar: Toast + Navigate
```

---

## 🐛 Debugging

### Problema: Variables no actualizan
**Solución:** Agregar `StateHasChanged()` si necesario

### Problema: Tabla no muestra datos
**Solución:** Verificar `_ultimaRespuestaGuardado.Errores != null`

### Problema: Filas no resaltadas
**Solución:** Verificar `GetClaseFilaAlumno()` retorna clase correcta

### Problema: Dialog no cierra
**Solución:** Verificar `dialog.Result` se asigna correctamente

---

## 📱 Responsive

Todos los componentes son responsive usando:
- MudBlazor (auto-responsive)
- Bootstrap grid (row, col-md-*)
- CSS media queries

---

## ♿ Accessibility

- [x] Colors: Rojo/Amarillo para diferenciadores
- [x] Icons: Emoji + MudIcon para visual
- [x] Text: Descripción clara en alerts
- [x] Hover: Table hover effect
- [x] Expand: MudExpansionPanel accesible

---

## 🔐 Seguridad

- [x] Validación servidor (SoloValidar)
- [x] UNIQUE constraint (evita duplicados)
- [x] Transacciones (atomicidad)
- [x] Guards (evita doble submit)
- [x] Authorization (existente)

---

## 📦 Deployment

```bash
dotnet build
dotnet run
# Verificar en http://localhost:5000
```

---

## 📞 Quick Help

| Problema | Línea | Solución |
|----------|-------|----------|
| 409 no detecta | 898 | Verificar StatusCode |
| Dialog no muestra | 912 | Verificar DialogService |
| Tabla no expande | 248 | Verificar MudExpansionPanel |
| Filas no resaltan | 945 | Verificar GetClaseFilaAlumno() |
| isSaving no se restaura | 824 | Verificar finally block |

---

**Referencia:** v1.0 (2024)
