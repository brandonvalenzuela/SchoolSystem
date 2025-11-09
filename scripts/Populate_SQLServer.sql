USE school_system;
GO

-- Desactivar temporalmente los triggers para evitar errores al insertar datos
-- (SQL Server no lo permite, por lo que nos enfocaremos en transacciones y orden de inserción)
-- SET NOCOUNT ON;
-- GO

-- Declaración de variables para almacenar IDs generados
DECLARE @id_escuela INT;
DECLARE @id_usuario_dir INT, @id_usuario_maestro INT, @id_usuario_padre INT, @id_usuario_alumno INT;
DECLARE @id_maestro INT, @id_padre INT, @id_alumno INT;
DECLARE @id_nivel_primaria INT, @id_grado_1ro INT, @id_grupo_a INT;
DECLARE @id_materia_mat INT, @id_materia_esp INT, @id_materia_ciencias INT;
DECLARE @id_periodo_t1 INT;

-- IDs de Permisos (Asumimos que ya existen por el script de creación de la BD)
DECLARE @id_permiso_ver_calif INT, @id_permiso_cap_calif INT, @id_permiso_ver_asis INT;

-- Capturar IDs de Permisos
SELECT @id_permiso_ver_calif = id FROM permisos WHERE nombre = 'ver_calificaciones';
SELECT @id_permiso_cap_calif = id FROM permisos WHERE nombre = 'capturar_calificaciones';
SELECT @id_permiso_ver_asis = id FROM permisos WHERE nombre = 'ver_asistencias';

PRINT '--- INICIO DE INSERCIÓN DE DATOS DE PRUEBA (SQL Server) ---';

-- =============================================
-- 1. ESCUELA
-- =============================================
PRINT '1. Insertando Escuela: Colegio Everest...';
INSERT INTO escuelas (codigo, nombre, razon_social, rfc, direccion, telefono, email, activo) 
VALUES ('EVR001', 'Colegio Everest', 'Colegio Everest S.C.', 'CEV010101ABC', 'Calle Falsa 123', '5511223344', 'info@everest.edu', 1);
SET @id_escuela = SCOPE_IDENTITY();

-- Insertar Configuración de Escuela
INSERT INTO configuraciones_escuela (escuela_id, escala_calificaciones, calificacion_minima_aprobatoria, idioma_predeterminado)
VALUES (@id_escuela, '0-10', 6.00, 'es');

-- =============================================
-- 2. USUARIOS Y ROLES
-- =============================================
PRINT '2. Insertando 4 Usuarios (Director, Maestro, Padre, Alumno)...';
-- Director
INSERT INTO usuarios (escuela_id, username, email, password_hash, rol, nombre, apellido_paterno, activo)
VALUES (@id_escuela, 'eva.dir', 'eva.director@everest.edu', 'hash_dir', 'director', 'Eva', 'Méndez', 1);
SET @id_usuario_dir = SCOPE_IDENTITY();

-- Maestro
INSERT INTO usuarios (escuela_id, username, email, password_hash, rol, nombre, apellido_paterno, activo)
VALUES (@id_escuela, 'prof.ana', 'ana.maestra@everest.edu', 'hash_maestro', 'maestro', 'Ana', 'García', 1);
SET @id_usuario_maestro = SCOPE_IDENTITY();

-- Padre
INSERT INTO usuarios (escuela_id, username, email, password_hash, rol, nombre, apellido_paterno, activo)
VALUES (@id_escuela, 'pablo.padre', 'pablo.padre@mail.com', 'hash_padre', 'padre', 'Pablo', 'López', 1);
SET @id_usuario_padre = SCOPE_IDENTITY();

-- Alumno (con login)
INSERT INTO usuarios (escuela_id, username, email, password_hash, rol, nombre, apellido_paterno, activo)
VALUES (@id_escuela, 'alumno.fer', 'fernando@mail.com', 'hash_alumno', 'alumno', 'Fernando', 'Ramos', 1);
SET @id_usuario_alumno = SCOPE_IDENTITY();

-- Crear datos de MAESTRO y PADRE
INSERT INTO maestros (escuela_id, usuario_id, numero_empleado, especialidad, estatus)
VALUES (@id_escuela, @id_usuario_maestro, 'EVE005', 'Pedagogía', 'activo');
SET @id_maestro = SCOPE_IDENTITY();

INSERT INTO padres (escuela_id, usuario_id, ocupacion)
VALUES (@id_escuela, @id_usuario_padre, 'Arquitecto');
SET @id_padre = SCOPE_IDENTITY();

-- Asignar Permisos Específicos para la Escuela
IF @id_permiso_cap_calif IS NOT NULL AND @id_permiso_ver_asis IS NOT NULL
BEGIN
    INSERT INTO rol_permisos (escuela_id, rol, permiso_id) VALUES 
    (@id_escuela, 'maestro', @id_permiso_cap_calif),
    (@id_escuela, 'maestro', @id_permiso_ver_asis);
END

-- =============================================
-- 3. ESTRUCTURA ACADÉMICA
-- =============================================
PRINT '3. Insertando Niveles, Grados y Grupos...';
-- Nivel
INSERT INTO niveles_educativos (escuela_id, nombre, orden)
VALUES (@id_escuela, 'Primaria', 1);
SET @id_nivel_primaria = SCOPE_IDENTITY();

-- Grado
INSERT INTO grados (escuela_id, nivel_educativo_id, nombre, orden)
VALUES (@id_escuela, @id_nivel_primaria, '1°', 1);
SET @id_grado_1ro = SCOPE_IDENTITY();

-- Grupo
INSERT INTO grupos (escuela_id, grado_id, nombre, ciclo_escolar, maestro_titular_id, turno)
VALUES (@id_escuela, @id_grado_1ro, 'A', '2025-2026', @id_usuario_maestro, 'matutino');
SET @id_grupo_a = SCOPE_IDENTITY();

-- =============================================
-- 4. MATERIAS Y ASIGNACIONES
-- =============================================
PRINT '4. Insertando Materias y Asignando a 1° A...';
-- Materias
INSERT INTO materias (escuela_id, nombre, clave) VALUES (@id_escuela, 'Matemáticas', 'MAT101');
SET @id_materia_mat = SCOPE_IDENTITY();

INSERT INTO materias (escuela_id, nombre, clave) VALUES (@id_escuela, 'Español', 'ESP101');
SET @id_materia_esp = SCOPE_IDENTITY();

INSERT INTO materias (escuela_id, nombre, clave) VALUES (@id_escuela, 'Ciencias Naturales', 'CIE101');
SET @id_materia_ciencias = SCOPE_IDENTITY();

-- Asignar materias al grado 1°
INSERT INTO grado_materias (escuela_id, grado_id, materia_id) VALUES 
(@id_escuela, @id_grado_1ro, @id_materia_mat),
(@id_escuela, @id_grado_1ro, @id_materia_esp),
(@id_escuela, @id_grado_1ro, @id_materia_ciencias);

-- Asignar Maestro Ana García al grupo 1° A y sus materias
INSERT INTO grupo_materia_maestro (escuela_id, grupo_id, materia_id, maestro_id, ciclo_escolar) VALUES
(@id_escuela, @id_grupo_a, @id_materia_mat, @id_usuario_maestro, '2025-2026'),
(@id_escuela, @id_grupo_a, @id_materia_esp, @id_usuario_maestro, '2025-2026'),
(@id_escuela, @id_grupo_a, @id_materia_ciencias, @id_usuario_maestro, '2025-2026');

-- =============================================
-- 5. ALUMNO, INSCRIPCIÓN Y FAMILIA
-- =============================================
PRINT '5. Insertando Alumno (Fernando Ramos) e Inscripción...';
-- Alumno
INSERT INTO alumnos (escuela_id, usuario_id, matricula, nombre, apellido_paterno, apellido_materno, fecha_nacimiento, fecha_ingreso, estatus)
VALUES (@id_escuela, @id_usuario_alumno, 'A25001', 'Fernando', 'Ramos', 'López', '2019-03-15', '2025-08-20', 'activo');
SET @id_alumno = SCOPE_IDENTITY();

-- Inscripción
INSERT INTO inscripciones (escuela_id, alumno_id, grupo_id, ciclo_escolar, fecha_inscripcion)
VALUES (@id_escuela, @id_alumno, @id_grupo_a, '2025-2026', GETDATE());

-- Vínculo Alumno-Padre
INSERT INTO alumno_padres (alumno_id, padre_id, relacion, es_tutor_principal)
VALUES (@id_alumno, @id_padre, 'padre', 1);

-- =============================================
-- 6. CALIFICACIONES Y PERÍODOS
-- =============================================
PRINT '6. Registrando Período y Calificaciones...';
-- Período de evaluación
INSERT INTO periodos_evaluacion (escuela_id, ciclo_escolar, nombre, numero, fecha_inicio, fecha_fin)
VALUES (@id_escuela, '2025-2026', 'Primer Trimestre', 1, '2025-08-20', '2025-11-20');
SET @id_periodo_t1 = SCOPE_IDENTITY();

-- Calificaciones para Fernando Ramos (Capturadas por la Maestra Ana García)
INSERT INTO calificaciones (escuela_id, alumno_id, materia_id, grupo_id, periodo_id, calificacion, capturado_por)
VALUES (@id_escuela, @id_alumno, @id_materia_mat, @id_grupo_a, @id_periodo_t1, 9.5, @id_usuario_maestro); -- Matemáticas

INSERT INTO calificaciones (escuela_id, alumno_id, materia_id, grupo_id, periodo_id, calificacion, capturado_por)
VALUES (@id_escuela, @id_alumno, @id_materia_esp, @id_grupo_a, @id_periodo_t1, 8.2, @id_usuario_maestro); -- Español

-- =============================================
-- 7. FINANZAS (Cargos y Pagos)
-- =============================================
PRINT '7. Registrando Finanzas (Inscripción y Colegiatura)...';
DECLARE @id_concepto_insc INT, @id_concepto_col INT;

INSERT INTO conceptos_pago (escuela_id, nombre, monto_base, tipo, recurrente, periodicidad, activo)
VALUES (@id_escuela, 'Inscripción Ciclo 25-26', 5000.00, 'inscripcion', 0, NULL, 1);
SET @id_concepto_insc = SCOPE_IDENTITY();

INSERT INTO conceptos_pago (escuela_id, nombre, monto_base, tipo, recurrente, periodicidad, activo)
VALUES (@id_escuela, 'Colegiatura Octubre 2025', 3500.00, 'colegiatura', 1, 'mensual', 1);
SET @id_concepto_col = SCOPE_IDENTITY();

-- Cargo de Inscripción a Fernando
DECLARE @id_cargo_insc INT;
INSERT INTO cargos (escuela_id, alumno_id, concepto_pago_id, ciclo_escolar, monto, monto_final, fecha_vencimiento, estatus)
VALUES (@id_escuela, @id_alumno, @id_concepto_insc, '2025-2026', 5000.00, 5000.00, '2025-08-30', 'pendiente');
SET @id_cargo_insc = SCOPE_IDENTITY();

-- Pago de Inscripción (Activa trigger trg_after_pago_insert)
INSERT INTO pagos (escuela_id, cargo_id, alumno_id, monto, metodo_pago, recibido_por)
VALUES (@id_escuela, @id_cargo_insc, @id_alumno, 5000.00, 'transferencia', @id_usuario_dir);

-- Cargo de Colegiatura (Queda pendiente de pago)
INSERT INTO cargos (escuela_id, alumno_id, concepto_pago_id, ciclo_escolar, monto, monto_final, fecha_vencimiento, estatus)
VALUES (@id_escuela, @id_alumno, @id_concepto_col, '2025-2026', 3500.00, 3500.00, '2025-10-31', 'pendiente');


PRINT '--- DATOS DE PRUEBA INSERTADOS COMPLETAMENTE (SQL Server) ---';
GO

-- Verificación de estado de cuenta (Debe tener 3500.00 pendientes)
SELECT * FROM v_alumnos_adeudos;
GO