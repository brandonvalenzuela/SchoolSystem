USE school_system;

-- =============================================
-- CONFIGURACIÓN INICIAL
-- =============================================
SET FOREIGN_KEY_CHECKS = 0; -- Deshabilitar FKs para inserciones masivas (opcional)
START TRANSACTION;

-- Declaración de variables para almacenar IDs generados
SELECT '--- INICIO DE INSERCIÓN DE DATOS DE PRUEBA (MySQL) ---' AS Paso;

SET @id_escuela = NULL;
SET @id_usuario_dir = NULL;
SET @id_usuario_maestro = NULL;
SET @id_usuario_padre = NULL;
SET @id_usuario_alumno = NULL;
SET @id_maestro = NULL;
SET @id_padre = NULL;
SET @id_alumno = NULL;
SET @id_nivel_primaria = NULL;
SET @id_grado_1ro = NULL;
SET @id_grupo_a = NULL;
SET @id_materia_mat = NULL;
SET @id_materia_esp = NULL;
SET @id_materia_ciencias = NULL;
SET @id_periodo_t1 = NULL;
SET @id_concepto_insc = NULL;
SET @id_concepto_col = NULL;
SET @id_cargo_insc = NULL;

-- Capturar IDs de Permisos
SELECT id INTO @id_permiso_ver_calif FROM permisos WHERE nombre = 'ver_calificaciones';
SELECT id INTO @id_permiso_cap_calif FROM permisos WHERE nombre = 'capturar_calificaciones';
SELECT id INTO @id_permiso_ver_asis FROM permisos WHERE nombre = 'ver_asistencias';


-- =============================================
-- 1. ESCUELA
-- =============================================
SELECT '1. Insertando Escuela: Colegio Everest...' AS Paso;
INSERT INTO escuelas (codigo, nombre, razon_social, rfc, direccion, telefono, email, activo) 
VALUES ('EVR001', 'Colegio Everest', 'Colegio Everest S.C.', 'CEV010101ABC', 'Calle Falsa 123', '5511223344', 'info@everest.edu', 1);
SET @id_escuela = LAST_INSERT_ID();

-- Insertar Configuración de Escuela
INSERT INTO configuraciones_escuela (escuela_id, escala_calificaciones, calificacion_minima_aprobatoria, idioma_predeterminado)
VALUES (@id_escuela, '0-10', 6.00, 'es');

-- =============================================
-- 2. USUARIOS Y ROLES
-- =============================================
SELECT '2. Insertando 4 Usuarios (Director, Maestro, Padre, Alumno)...' AS Paso;
-- Director
INSERT INTO usuarios (escuela_id, username, email, password_hash, rol, nombre, apellido_paterno, activo)
VALUES (@id_escuela, 'eva.dir', 'eva.director@everest.edu', 'hash_dir', 'director', 'Eva', 'Méndez', 1);
SET @id_usuario_dir = LAST_INSERT_ID();

-- Maestro
INSERT INTO usuarios (escuela_id, username, email, password_hash, rol, nombre, apellido_paterno, activo)
VALUES (@id_escuela, 'prof.ana', 'ana.maestra@everest.edu', 'hash_maestro', 'maestro', 'Ana', 'García', 1);
SET @id_usuario_maestro = LAST_INSERT_ID();

-- Padre
INSERT INTO usuarios (escuela_id, username, email, password_hash, rol, nombre, apellido_paterno, activo)
VALUES (@id_escuela, 'pablo.padre', 'pablo.padre@mail.com', 'hash_padre', 'padre', 'Pablo', 'López', 1);
SET @id_usuario_padre = LAST_INSERT_ID();

-- Alumno (con login)
INSERT INTO usuarios (escuela_id, username, email, password_hash, rol, nombre, apellido_paterno, activo)
VALUES (@id_escuela, 'alumno.fer', 'fernando@mail.com', 'hash_alumno', 'alumno', 'Fernando', 'Ramos', 1);
SET @id_usuario_alumno = LAST_INSERT_ID();

-- Crear datos de MAESTRO y PADRE
INSERT INTO maestros (escuela_id, usuario_id, numero_empleado, especialidad, estatus)
VALUES (@id_escuela, @id_usuario_maestro, 'EVE005', 'Pedagogía', 'activo');
SET @id_maestro = LAST_INSERT_ID();

INSERT INTO padres (escuela_id, usuario_id, ocupacion)
VALUES (@id_escuela, @id_usuario_padre, 'Arquitecto');
SET @id_padre = LAST_INSERT_ID();

-- Asignar Permisos Específicos para la Escuela
INSERT INTO rol_permisos (escuela_id, rol, permiso_id) VALUES 
(@id_escuela, 'maestro', @id_permiso_cap_calif),
(@id_escuela, 'maestro', @id_permiso_ver_asis);

-- =============================================
-- 3. ESTRUCTURA ACADÉMICA
-- =============================================
SELECT '3. Insertando Niveles, Grados y Grupos...' AS Paso;
-- Nivel
INSERT INTO niveles_educativos (escuela_id, nombre, orden)
VALUES (@id_escuela, 'Primaria', 1);
SET @id_nivel_primaria = LAST_INSERT_ID();

-- Grado
INSERT INTO grados (escuela_id, nivel_educativo_id, nombre, orden)
VALUES (@id_escuela, @id_nivel_primaria, '1°', 1);
SET @id_grado_1ro = LAST_INSERT_ID();

-- Grupo
INSERT INTO grupos (escuela_id, grado_id, nombre, ciclo_escolar, maestro_titular_id, turno)
VALUES (@id_escuela, @id_grado_1ro, 'A', '2025-2026', @id_usuario_maestro, 'matutino');
SET @id_grupo_a = LAST_INSERT_ID();

-- =============================================
-- 4. MATERIAS Y ASIGNACIONES
-- =============================================
SELECT '4. Insertando Materias y Asignando a 1° A...' AS Paso;
-- Materias
INSERT INTO materias (escuela_id, nombre, clave) VALUES (@id_escuela, 'Matemáticas', 'MAT101');
SET @id_materia_mat = LAST_INSERT_ID();

INSERT INTO materias (escuela_id, nombre, clave) VALUES (@id_escuela, 'Español', 'ESP101');
SET @id_materia_esp = LAST_INSERT_ID();

INSERT INTO materias (escuela_id, nombre, clave) VALUES (@id_escuela, 'Ciencias Naturales', 'CIE101');
SET @id_materia_ciencias = LAST_INSERT_ID();

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
SELECT '5. Insertando Alumno (Fernando Ramos) e Inscripción...' AS Paso;
-- Alumno
INSERT INTO alumnos (escuela_id, usuario_id, matricula, nombre, apellido_paterno, apellido_materno, fecha_nacimiento, fecha_ingreso, estatus)
VALUES (@id_escuela, @id_usuario_alumno, 'A25001', 'Fernando', 'Ramos', 'López', '2019-03-15', '2025-08-20', 'activo');
SET @id_alumno = LAST_INSERT_ID();

-- Inscripción
INSERT INTO inscripciones (escuela_id, alumno_id, grupo_id, ciclo_escolar, fecha_inscripcion)
VALUES (@id_escuela, @id_alumno, @id_grupo_a, '2025-2026', CURDATE());

-- Vínculo Alumno-Padre
INSERT INTO alumno_padres (alumno_id, padre_id, relacion, es_tutor_principal)
VALUES (@id_alumno, @id_padre, 'padre', 1);

-- =============================================
-- 6. CALIFICACIONES Y PERÍODOS
-- =============================================
SELECT '6. Registrando Período y Calificaciones...' AS Paso;
-- Período de evaluación
INSERT INTO periodos_evaluacion (escuela_id, ciclo_escolar, nombre, numero, fecha_inicio, fecha_fin)
VALUES (@id_escuela, '2025-2026', 'Primer Trimestre', 1, '2025-08-20', '2025-11-20');
SET @id_periodo_t1 = LAST_INSERT_ID();

-- Calificaciones para Fernando Ramos (Capturadas por la Maestra Ana García)
INSERT INTO calificaciones (escuela_id, alumno_id, materia_id, grupo_id, periodo_id, calificacion, capturado_por)
VALUES (@id_escuela, @id_alumno, @id_materia_mat, @id_grupo_a, @id_periodo_t1, 9.5, @id_usuario_maestro); -- Matemáticas

INSERT INTO calificaciones (escuela_id, alumno_id, materia_id, grupo_id, periodo_id, calificacion, capturado_por)
VALUES (@id_escuela, @id_alumno, @id_materia_esp, @id_grupo_a, @id_periodo_t1, 8.2, @id_usuario_maestro); -- Español

-- =============================================
-- 7. FINANZAS (Cargos y Pagos)
-- =============================================
SELECT '7. Registrando Finanzas (Inscripción y Colegiatura)...' AS Paso;

INSERT INTO conceptos_pago (escuela_id, nombre, monto_base, tipo, recurrente, periodicidad, activo)
VALUES (@id_escuela, 'Inscripción Ciclo 25-26', 5000.00, 'inscripcion', 0, NULL, 1);
SET @id_concepto_insc = LAST_INSERT_ID();

INSERT INTO conceptos_pago (escuela_id, nombre, monto_base, tipo, recurrente, periodicidad, activo)
VALUES (@id_escuela, 'Colegiatura Octubre 2025', 3500.00, 'colegiatura', 1, 'mensual', 1);
SET @id_concepto_col = LAST_INSERT_ID();

-- Cargo de Inscripción a Fernando
INSERT INTO cargos (escuela_id, alumno_id, concepto_pago_id, ciclo_escolar, monto, monto_final, fecha_vencimiento, estatus)
VALUES (@id_escuela, @id_alumno, @id_concepto_insc, '2025-2026', 5000.00, 5000.00, '2025-08-30', 'pendiente');
SET @id_cargo_insc = LAST_INSERT_ID();

-- Pago de Inscripción (Activa trigger trg_after_pago_insert)
INSERT INTO pagos (escuela_id, cargo_id, alumno_id, monto, metodo_pago, recibido_por)
VALUES (@id_escuela, @id_cargo_insc, @id_alumno, 5000.00, 'transferencia', @id_usuario_dir);

-- Cargo de Colegiatura (Queda pendiente de pago)
INSERT INTO cargos (escuela_id, alumno_id, concepto_pago_id, ciclo_escolar, monto, monto_final, fecha_vencimiento, estatus)
VALUES (@id_escuela, @id_alumno, @id_concepto_col, '2025-2026', 3500.00, 3500.00, '2025-10-31', 'pendiente');

COMMIT;
SET FOREIGN_KEY_CHECKS = 1;

SELECT '--- DATOS DE PRUEBA INSERTADOS COMPLETAMENTE (MySQL) ---' AS Paso;

-- Verificación de estado de cuenta (Debe tener 3500.00 pendientes)
SELECT * FROM v_alumnos_adeudos;