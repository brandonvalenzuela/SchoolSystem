-- ============================================
-- SISTEMA DE GESTIÓN ESCOLAR MULTI-TENANT
-- Base de Datos T-SQL (SQL Server)
-- ============================================

-- Configuración inicial
USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'school_system')
BEGIN
    ALTER DATABASE school_system SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE school_system;
END
GO

CREATE DATABASE school_system COLLATE Latin1_General_100_CI_AS;
GO

USE school_system;
GO

-- ============================================
-- MÓDULO 1: MULTI-TENANCY Y SEGURIDAD
-- ============================================

-- Tabla de Escuelas (Tenants)
CREATE TABLE escuelas (
    id INT IDENTITY(1,1) PRIMARY KEY,
    codigo NVARCHAR(20) UNIQUE NOT NULL,
    nombre NVARCHAR(200) NOT NULL,
    razon_social NVARCHAR(250),
    rfc NVARCHAR(13),
    direccion NVARCHAR(300),
    telefono NVARCHAR(20),
    email NVARCHAR(100),
    logo_url NVARCHAR(500),
    plan_id INT,
    fecha_registro DATETIME2 DEFAULT GETDATE(),
    fecha_expiracion DATETIME2,
    activo BIT DEFAULT 1,
    configuracion NVARCHAR(MAX) CHECK (ISJSON(configuracion) = 1),
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE()
);
GO

CREATE NONCLUSTERED INDEX idx_codigo ON escuelas(codigo);
CREATE NONCLUSTERED INDEX idx_activo ON escuelas(activo);
GO

CREATE TRIGGER trg_escuelas_update
ON escuelas
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE e
    SET updated_at = GETDATE()
    FROM escuelas e
    INNER JOIN inserted i ON e.id = i.id;
END
GO

-- Tabla de Usuarios (Multi-rol)
CREATE TABLE usuarios (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    username NVARCHAR(50) UNIQUE NOT NULL,
    email NVARCHAR(100) UNIQUE NOT NULL,
    password_hash NVARCHAR(255) NOT NULL,
    rol NVARCHAR(20) NOT NULL CHECK (rol IN ('super_admin', 'director', 'subdirector', 'administrativo', 'maestro', 'padre', 'alumno')),
    nombre NVARCHAR(100) NOT NULL,
    apellido_paterno NVARCHAR(100) NOT NULL,
    apellido_materno NVARCHAR(100),
    telefono NVARCHAR(20),
    telefono_emergencia NVARCHAR(20),
    foto_url NVARCHAR(500),
    fecha_nacimiento DATE,
    genero NVARCHAR(20) CHECK (genero IN ('M', 'F', 'Otro', 'Prefiero no decir')),
    activo BIT DEFAULT 1,
    ultimo_acceso DATETIME2,
    token_recuperacion NVARCHAR(255),
    token_expiracion DATETIME2,
    intentos_fallidos INT DEFAULT 0,
    bloqueado_hasta DATETIME2,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela_rol ON usuarios(escuela_id, rol);
CREATE NONCLUSTERED INDEX idx_email ON usuarios(email);
CREATE NONCLUSTERED INDEX idx_activo ON usuarios(activo);
GO

CREATE TRIGGER trg_usuarios_update
ON usuarios
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE u
    SET updated_at = GETDATE()
    FROM usuarios u
    INNER JOIN inserted i ON u.id = i.id;
END
GO

-- Tabla de Permisos Específicos (para granularidad fina)
CREATE TABLE permisos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100) UNIQUE NOT NULL,
    descripcion NVARCHAR(255),
    modulo NVARCHAR(50),
    created_at DATETIME2 DEFAULT GETDATE()
);
GO

-- Tabla de Roles-Permisos
CREATE TABLE rol_permisos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    rol NVARCHAR(20) NOT NULL CHECK (rol IN ('super_admin', 'director', 'subdirector', 'administrativo', 'maestro', 'padre', 'alumno')),
    permiso_id INT NOT NULL,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (permiso_id) REFERENCES permisos(id) ON DELETE CASCADE,
    UNIQUE (escuela_id, rol, permiso_id)
);
GO

-- ============================================
-- MÓDULO 2: ESTRUCTURA ACADÉMICA
-- ============================================

-- Niveles Educativos (Kinder, Primaria, Secundaria, Preparatoria)
CREATE TABLE niveles_educativos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(50) NOT NULL,
    abreviatura NVARCHAR(10),
    orden INT,
    activo BIT DEFAULT 1,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    UNIQUE (escuela_id, nombre)
);
GO

-- Grados (1°, 2°, 3°, etc. por nivel)
CREATE TABLE grados (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nivel_educativo_id INT NOT NULL,
    nombre NVARCHAR(50) NOT NULL,
    orden INT,
    activo BIT DEFAULT 1,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (nivel_educativo_id) REFERENCES niveles_educativos(id) ON DELETE CASCADE,
    UNIQUE (escuela_id, nivel_educativo_id, nombre)
);
GO

CREATE NONCLUSTERED INDEX idx_nivel ON grados(nivel_educativo_id);
GO

-- Grupos (Secciones: A, B, C, etc.)
CREATE TABLE grupos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    grado_id INT NOT NULL,
    nombre NVARCHAR(50) NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    capacidad_maxima INT DEFAULT 40,
    maestro_titular_id INT,
    aula NVARCHAR(50),
    turno NVARCHAR(10) CHECK (turno IN ('matutino', 'vespertino', 'nocturno')),
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (grado_id) REFERENCES grados(id) ON DELETE CASCADE,
    FOREIGN KEY (maestro_titular_id) REFERENCES usuarios(id) ON DELETE SET NULL,
    UNIQUE (escuela_id, grado_id, nombre, ciclo_escolar)
);
GO

CREATE NONCLUSTERED INDEX idx_ciclo ON grupos(ciclo_escolar);
CREATE NONCLUSTERED INDEX idx_maestro ON grupos(maestro_titular_id);
GO

-- Materias/Asignaturas
CREATE TABLE materias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    clave NVARCHAR(20),
    descripcion NVARCHAR(MAX),
    color NVARCHAR(7),
    activo BIT DEFAULT 1,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    UNIQUE (escuela_id, nombre)
);
GO

-- Relación Materias por Grado
CREATE TABLE grado_materias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    grado_id INT NOT NULL,
    materia_id INT NOT NULL,
    horas_semanales INT,
    orden INT,
    obligatoria BIT DEFAULT 1,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (grado_id) REFERENCES grados(id) ON DELETE CASCADE,
    FOREIGN KEY (materia_id) REFERENCES materias(id) ON DELETE CASCADE,
    UNIQUE (grado_id, materia_id)
);
GO

-- Asignación de Maestros a Materias por Grupo
CREATE TABLE grupo_materia_maestro (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    grupo_id INT NOT NULL,
    materia_id INT NOT NULL,
    maestro_id INT NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (grupo_id) REFERENCES grupos(id) ON DELETE CASCADE,
    FOREIGN KEY (materia_id) REFERENCES materias(id) ON DELETE CASCADE,
    FOREIGN KEY (maestro_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    UNIQUE (grupo_id, materia_id, ciclo_escolar)
);
GO

CREATE NONCLUSTERED INDEX idx_maestro ON grupo_materia_maestro(maestro_id);
GO

-- ============================================
-- MÓDULO 3: ALUMNOS Y FAMILIAS
-- ============================================

-- Alumnos
CREATE TABLE alumnos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    usuario_id INT UNIQUE,
    matricula NVARCHAR(50) UNIQUE NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    apellido_paterno NVARCHAR(100) NOT NULL,
    apellido_materno NVARCHAR(100),
    fecha_nacimiento DATE NOT NULL,
    curp NVARCHAR(18) UNIQUE,
    genero NVARCHAR(10) CHECK (genero IN ('M', 'F', 'Otro')),
    foto_url NVARCHAR(500),
    direccion NVARCHAR(300),
    telefono NVARCHAR(20),
    email NVARCHAR(100),
    tipo_sangre NVARCHAR(5),
    alergias NVARCHAR(MAX),
    condiciones_medicas NVARCHAR(MAX),
    medicamentos NVARCHAR(MAX),
    contacto_emergencia_nombre NVARCHAR(100),
    contacto_emergencia_telefono NVARCHAR(20),
    contacto_emergencia_relacion NVARCHAR(50),
    fecha_ingreso DATE NOT NULL,
    fecha_baja DATE,
    motivo_baja NVARCHAR(MAX),
    estatus NVARCHAR(10) DEFAULT 'activo' CHECK (estatus IN ('activo', 'inactivo', 'egresado', 'baja')),
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE SET NULL
);
GO

CREATE NONCLUSTERED INDEX idx_matricula ON alumnos(matricula);
CREATE NONCLUSTERED INDEX idx_estatus ON alumnos(estatus);
CREATE NONCLUSTERED INDEX idx_escuela_estatus ON alumnos(escuela_id, estatus);
GO

CREATE TRIGGER trg_alumnos_update
ON alumnos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE a
    SET updated_at = GETDATE()
    FROM alumnos a
    INNER JOIN inserted i ON a.id = i.id;
END
GO

-- Inscripciones (Historial de grupos por ciclo escolar)
CREATE TABLE inscripciones (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT NOT NULL,
    grupo_id INT NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    fecha_inscripcion DATE NOT NULL,
    numero_lista INT,
    estatus NVARCHAR(20) DEFAULT 'inscrito' CHECK (estatus IN ('inscrito', 'baja_temporal', 'baja_definitiva', 'finalizado')),
    promedio_final DECIMAL(4,2),
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (grupo_id) REFERENCES grupos(id) ON DELETE CASCADE,
    UNIQUE (alumno_id, ciclo_escolar)
);
GO

CREATE NONCLUSTERED INDEX idx_grupo_ciclo ON inscripciones(grupo_id, ciclo_escolar);
GO

-- Padres/Tutores
CREATE TABLE padres (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    usuario_id INT UNIQUE NOT NULL,
    ocupacion NVARCHAR(100),
    lugar_trabajo NVARCHAR(200),
    telefono_trabajo NVARCHAR(20),
    nivel_estudios NVARCHAR(50),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

-- Relación Alumnos-Padres
CREATE TABLE alumno_padres (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    padre_id INT NOT NULL,
    relacion NVARCHAR(20) NOT NULL CHECK (relacion IN ('padre', 'madre', 'tutor', 'abuelo', 'abuela', 'tio', 'tia', 'hermano', 'hermana', 'otro')),
    es_tutor_principal BIT DEFAULT 0,
    autorizado_recoger BIT DEFAULT 1,
    recibe_notificaciones BIT DEFAULT 1,
    vive_con_alumno BIT DEFAULT 1,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (padre_id) REFERENCES padres(id) ON DELETE CASCADE,
    UNIQUE (alumno_id, padre_id)
);
GO

CREATE NONCLUSTERED INDEX idx_padre ON alumno_padres(padre_id);
GO

-- ============================================
-- MÓDULO 4: MAESTROS
-- ============================================

-- Maestros (información adicional)
CREATE TABLE maestros (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    usuario_id INT UNIQUE NOT NULL,
    numero_empleado NVARCHAR(50) UNIQUE,
    fecha_ingreso DATE,
    cedula_profesional NVARCHAR(50),
    especialidad NVARCHAR(100),
    titulo_academico NVARCHAR(100),
    universidad NVARCHAR(200),
    años_experiencia INT,
    tipo_contrato NVARCHAR(20) DEFAULT 'base' CHECK (tipo_contrato IN ('base', 'interino', 'honorarios', 'medio_tiempo')),
    estatus NVARCHAR(10) DEFAULT 'activo' CHECK (estatus IN ('activo', 'licencia', 'baja')),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

-- ============================================
-- MÓDULO 5: ASISTENCIAS
-- ============================================

CREATE TABLE asistencias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT NOT NULL,
    grupo_id INT NOT NULL,
    fecha DATE NOT NULL,
    estatus NVARCHAR(20) NOT NULL CHECK (estatus IN ('presente', 'falta', 'retardo', 'justificada', 'permiso')),
    hora_entrada TIME,
    hora_salida TIME,
    observaciones NVARCHAR(MAX),
    justificante_url NVARCHAR(500),
    registrado_por INT,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (grupo_id) REFERENCES grupos(id) ON DELETE CASCADE,
    FOREIGN KEY (registrado_por) REFERENCES usuarios(id) ON DELETE SET NULL,
    UNIQUE (alumno_id, fecha)
);
GO

CREATE NONCLUSTERED INDEX idx_grupo_fecha ON asistencias(grupo_id, fecha);
CREATE NONCLUSTERED INDEX idx_fecha ON asistencias(fecha);
GO

CREATE TRIGGER trg_asistencias_update
ON asistencias
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE a
    SET updated_at = GETDATE()
    FROM asistencias a
    INNER JOIN inserted i ON a.id = i.id;
END
GO

-- ============================================
-- MÓDULO 6: CALIFICACIONES
-- ============================================

-- Períodos de Evaluación
CREATE TABLE periodos_evaluacion (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    nombre NVARCHAR(50) NOT NULL,
    numero INT NOT NULL,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    porcentaje DECIMAL(5,2),
    activo BIT DEFAULT 1,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    UNIQUE (escuela_id, ciclo_escolar, numero)
);
GO

CREATE NONCLUSTERED INDEX idx_ciclo ON periodos_evaluacion(ciclo_escolar);
GO

-- Calificaciones
CREATE TABLE calificaciones (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT NOT NULL,
    materia_id INT NOT NULL,
    grupo_id INT NOT NULL,
    periodo_id INT NOT NULL,
    calificacion DECIMAL(5,2) NOT NULL,
    calificacion_letra NVARCHAR(5),
    observaciones NVARCHAR(MAX),
    fecha_captura DATETIME2 DEFAULT GETDATE(),
    capturado_por INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (materia_id) REFERENCES materias(id) ON DELETE CASCADE,
    FOREIGN KEY (grupo_id) REFERENCES grupos(id) ON DELETE CASCADE,
    FOREIGN KEY (periodo_id) REFERENCES periodos_evaluacion(id) ON DELETE CASCADE,
    FOREIGN KEY (capturado_por) REFERENCES usuarios(id) ON DELETE SET NULL,
    UNIQUE (alumno_id, materia_id, periodo_id)
);
GO

CREATE NONCLUSTERED INDEX idx_grupo_periodo ON calificaciones(grupo_id, periodo_id);
CREATE NONCLUSTERED INDEX idx_alumno ON calificaciones(alumno_id);
GO

CREATE TRIGGER trg_calificaciones_update
ON calificaciones
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE c
    SET updated_at = GETDATE()
    FROM calificaciones c
    INNER JOIN inserted i ON c.id = i.id;
END
GO

-- ============================================
-- MÓDULO 7: CONDUCTA Y DISCIPLINA
-- ============================================

-- Registros de Conducta
CREATE TABLE conductas (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT NOT NULL,
    fecha DATETIME2 NOT NULL,
    tipo NVARCHAR(10) NOT NULL CHECK (tipo IN ('positiva', 'negativa', 'neutral')),
    categoria NVARCHAR(50),
    descripcion NVARCHAR(MAX) NOT NULL,
    gravedad NVARCHAR(10) CHECK (gravedad IN ('leve', 'moderada', 'grave', 'muy_grave')),
    puntos INT DEFAULT 0,
    reportado_por INT NOT NULL,
    acciones_tomadas NVARCHAR(MAX),
    notificado_padres BIT DEFAULT 0,
    fecha_notificacion DATETIME2,
    seguimiento_requerido BIT DEFAULT 0,
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (reportado_por) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_alumno_fecha ON conductas(alumno_id, fecha);
CREATE NONCLUSTERED INDEX idx_tipo ON conductas(tipo);
GO

-- Sanciones
CREATE TABLE sanciones (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT NOT NULL,
    conducta_id INT,
    tipo NVARCHAR(30) NOT NULL CHECK (tipo IN ('amonestacion_verbal', 'amonestacion_escrita', 'suspension', 'expulsion', 'trabajo_comunitario', 'cita_padres')),
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE,
    descripcion NVARCHAR(MAX) NOT NULL,
    autorizado_por INT NOT NULL,
    cumplida BIT DEFAULT 0,
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (conducta_id) REFERENCES conductas(id) ON DELETE SET NULL,
    FOREIGN KEY (autorizado_por) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON sanciones(alumno_id);
GO

-- ============================================
-- MÓDULO 8: TAREAS Y ACTIVIDADES
-- ============================================

CREATE TABLE tareas (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    grupo_id INT NOT NULL,
    materia_id INT NOT NULL,
    maestro_id INT NOT NULL,
    titulo NVARCHAR(200) NOT NULL,
    descripcion NVARCHAR(MAX),
    fecha_asignacion DATETIME2 DEFAULT GETDATE(),
    fecha_entrega DATETIME2 NOT NULL,
    valor_puntos DECIMAL(5,2),
    permite_entrega_tardia BIT DEFAULT 0,
    fecha_limite_tardia DATETIME2,
    penalizacion_tardia DECIMAL(5,2),
    archivo_adjunto_url NVARCHAR(500),
    tipo NVARCHAR(20) DEFAULT 'tarea' CHECK (tipo IN ('tarea', 'proyecto', 'investigacion', 'examen', 'practica')),
    activo BIT DEFAULT 1,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (grupo_id) REFERENCES grupos(id) ON DELETE CASCADE,
    FOREIGN KEY (materia_id) REFERENCES materias(id) ON DELETE CASCADE,
    FOREIGN KEY (maestro_id) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_grupo_materia ON tareas(grupo_id, materia_id);
CREATE NONCLUSTERED INDEX idx_fecha_entrega ON tareas(fecha_entrega);
GO

-- Entregas de Tareas
CREATE TABLE tarea_entregas (
    id INT IDENTITY(1,1) PRIMARY KEY,
    tarea_id INT NOT NULL,
    alumno_id INT NOT NULL,
    fecha_entrega DATETIME2,
    archivo_url NVARCHAR(500),
    comentarios NVARCHAR(MAX),
    calificacion DECIMAL(5,2),
    retroalimentacion NVARCHAR(MAX),
    estatus NVARCHAR(20) DEFAULT 'pendiente' CHECK (estatus IN ('pendiente', 'entregada', 'revisada', 'tardia', 'no_entregada')),
    revisado_por INT,
    fecha_revision DATETIME2,
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (tarea_id) REFERENCES tareas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (revisado_por) REFERENCES usuarios(id) ON DELETE SET NULL,
    UNIQUE (tarea_id, alumno_id)
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON tarea_entregas(alumno_id);
CREATE NONCLUSTERED INDEX idx_estatus ON tarea_entregas(estatus);
GO

-- ============================================
-- MÓDULO 9: NOTIFICACIONES Y COMUNICACIÓN
-- ============================================

-- Notificaciones
CREATE TABLE notificaciones (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    usuario_destinatario_id INT,
    tipo NVARCHAR(20) NOT NULL CHECK (tipo IN ('general', 'academico', 'disciplinario', 'financiero', 'urgente', 'evento')),
    prioridad NVARCHAR(10) DEFAULT 'normal' CHECK (prioridad IN ('baja', 'normal', 'alta', 'urgente')),
    titulo NVARCHAR(200) NOT NULL,
    mensaje NVARCHAR(MAX) NOT NULL,
    url_accion NVARCHAR(500),
    enviado_por INT NOT NULL,
    fecha_envio DATETIME2 DEFAULT GETDATE(),
    fecha_programada DATETIME2,
    leida BIT DEFAULT 0,
    fecha_lectura DATETIME2,
    canal NVARCHAR(10) NOT NULL CHECK (canal IN ('app', 'sms', 'email', 'push', 'sistema')),
    metadata NVARCHAR(MAX) CHECK (ISJSON(metadata) = 1),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_destinatario_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (enviado_por) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_usuario_leida ON notificaciones(usuario_destinatario_id, leida);
CREATE NONCLUSTERED INDEX idx_fecha_envio ON notificaciones(fecha_envio);
CREATE NONCLUSTERED INDEX idx_prioridad ON notificaciones(prioridad);
GO

-- Log de Notificaciones SMS
CREATE TABLE notificaciones_sms_log (
    id INT IDENTITY(1,1) PRIMARY KEY,
    notificacion_id INT NOT NULL,
    telefono NVARCHAR(20) NOT NULL,
    mensaje NVARCHAR(MAX) NOT NULL,
    proveedor NVARCHAR(50),
    estatus NVARCHAR(10) DEFAULT 'pendiente' CHECK (estatus IN ('pendiente', 'enviado', 'fallido', 'entregado')),
    sid_proveedor NVARCHAR(100),
    costo DECIMAL(10,4),
    fecha_envio DATETIME2,
    fecha_entrega DATETIME2,
    error_mensaje NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (notificacion_id) REFERENCES notificaciones(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_estatus ON notificaciones_sms_log(estatus);
CREATE NONCLUSTERED INDEX idx_telefono ON notificaciones_sms_log(telefono);
GO

-- Comunicados Generales
CREATE TABLE comunicados (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    titulo NVARCHAR(200) NOT NULL,
    contenido NVARCHAR(MAX) NOT NULL,
    destinatarios NVARCHAR(20) NOT NULL CHECK (destinatarios IN ('todos', 'padres', 'maestros', 'alumnos', 'grupo_especifico')),
    grupo_id INT,
    archivo_adjunto_url NVARCHAR(500),
    publicado_por INT NOT NULL,
    fecha_publicacion DATETIME2 DEFAULT GETDATE(),
    fecha_expiracion DATETIME2,
    requiere_confirmacion BIT DEFAULT 0,
    activo BIT DEFAULT 1,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (grupo_id) REFERENCES grupos(id) ON DELETE CASCADE,
    FOREIGN KEY (publicado_por) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_destinatarios ON comunicados(destinatarios);
CREATE NONCLUSTERED INDEX idx_fecha_publicacion ON comunicados(fecha_publicacion);
GO

-- Confirmación de Lectura de Comunicados
CREATE TABLE comunicado_lecturas (
    id INT IDENTITY(1,1) PRIMARY KEY,
    comunicado_id INT NOT NULL,
    usuario_id INT NOT NULL,
    fecha_lectura DATETIME2 DEFAULT GETDATE(),
    confirmado BIT DEFAULT 0,
    FOREIGN KEY (comunicado_id) REFERENCES comunicados(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    UNIQUE (comunicado_id, usuario_id)
);
GO

-- Mensajes Directos (Chat Maestro-Padre)
CREATE TABLE mensajes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    emisor_id INT NOT NULL,
    receptor_id INT NOT NULL,
    alumno_relacionado_id INT,
    asunto NVARCHAR(200),
    mensaje NVARCHAR(MAX) NOT NULL,
    fecha_envio DATETIME2 DEFAULT GETDATE(),
    leido BIT DEFAULT 0,
    fecha_lectura DATETIME2,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (emisor_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (receptor_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_relacionado_id) REFERENCES alumnos(id) ON DELETE SET NULL
);
GO

CREATE NONCLUSTERED INDEX idx_receptor_leido ON mensajes(receptor_id, leido);
CREATE NONCLUSTERED INDEX idx_emisor ON mensajes(emisor_id);
GO

-- ============================================
-- MÓDULO 10: RANKINGS Y GAMIFICACIÓN
-- ============================================

-- Puntos de Alumnos
CREATE TABLE alumno_puntos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    puntos_conducta INT DEFAULT 0,
    puntos_asistencia INT DEFAULT 0,
    puntos_academicos INT DEFAULT 0,
    puntos_participacion INT DEFAULT 0,
    puntos_totales INT DEFAULT 0,
    nivel INT DEFAULT 1,
    insignias NVARCHAR(MAX) CHECK (ISJSON(insignias) = 1),
    ultima_actualizacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    UNIQUE (alumno_id, ciclo_escolar)
);
GO

CREATE NONCLUSTERED INDEX idx_puntos_totales ON alumno_puntos(puntos_totales DESC);
GO

CREATE TRIGGER trg_alumno_puntos_update
ON alumno_puntos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ap
    SET ultima_actualizacion = GETDATE()
    FROM alumno_puntos ap
    INNER JOIN inserted i ON ap.id = i.id;
END
GO

-- Insignias Disponibles
CREATE TABLE insignias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(MAX),
    icono_url NVARCHAR(500),
    criterio NVARCHAR(MAX),
    puntos_requeridos INT,
    tipo NVARCHAR(20) NOT NULL CHECK (tipo IN ('asistencia', 'conducta', 'academico', 'participacion', 'especial')),
    activo BIT DEFAULT 1,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

-- ============================================
-- MÓDULO 11: CALENDARIO Y EVENTOS
-- ============================================

CREATE TABLE eventos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    titulo NVARCHAR(200) NOT NULL,
    descripcion NVARCHAR(MAX),
    tipo NVARCHAR(30) NOT NULL CHECK (tipo IN ('junta', 'examen', 'festivo', 'suspension', 'evento_especial', 'entrega_calificaciones', 'reunion_padres')),
    fecha_inicio DATETIME2 NOT NULL,
    fecha_fin DATETIME2,
    todo_el_dia BIT DEFAULT 0,
    grupos_afectados NVARCHAR(MAX) CHECK (ISJSON(grupos_afectados) = 1),
    ubicacion NVARCHAR(200),
    recordatorio_minutos INT,
    creado_por INT NOT NULL,
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (creado_por) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_fecha_inicio ON eventos(fecha_inicio);
CREATE NONCLUSTERED INDEX idx_tipo ON eventos(tipo);
GO

-- ============================================
-- MÓDULO 12: FINANZAS Y PAGOS
-- ============================================

-- Conceptos de Pago
CREATE TABLE conceptos_pago (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(MAX),
    monto_base DECIMAL(10,2) NOT NULL,
    tipo NVARCHAR(20) NOT NULL CHECK (tipo IN ('inscripcion', 'colegiatura', 'uniforme', 'material', 'evento', 'otro')),
    recurrente BIT DEFAULT 0,
    periodicidad NVARCHAR(10) CHECK (periodicidad IN ('mensual', 'bimestral', 'trimestral', 'semestral', 'anual')),
    activo BIT DEFAULT 1,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_tipo ON conceptos_pago(tipo);
GO

-- Cargos a Alumnos
CREATE TABLE cargos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT NOT NULL,
    concepto_pago_id INT NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    monto DECIMAL(10,2) NOT NULL,
    descuento DECIMAL(10,2) DEFAULT 0,
    monto_final DECIMAL(10,2) NOT NULL,
    fecha_vencimiento DATE NOT NULL,
    estatus NVARCHAR(10) DEFAULT 'pendiente' CHECK (estatus IN ('pendiente', 'pagado', 'parcial', 'vencido', 'cancelado')),
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (concepto_pago_id) REFERENCES conceptos_pago(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_alumno_estatus ON cargos(alumno_id, estatus);
CREATE NONCLUSTERED INDEX idx_fecha_vencimiento ON cargos(fecha_vencimiento);
GO

-- Pagos Realizados
CREATE TABLE pagos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    cargo_id INT NOT NULL,
    alumno_id INT NOT NULL,
    monto DECIMAL(10,2) NOT NULL,
    metodo_pago NVARCHAR(20) NOT NULL CHECK (metodo_pago IN ('efectivo', 'transferencia', 'tarjeta', 'cheque', 'otro')),
    referencia NVARCHAR(100),
    fecha_pago DATETIME2 DEFAULT GETDATE(),
    recibido_por INT NOT NULL,
    folio_recibo NVARCHAR(50) UNIQUE,
    observaciones NVARCHAR(MAX),
    cancelado BIT DEFAULT 0,
    fecha_cancelacion DATETIME2,
    motivo_cancelacion NVARCHAR(MAX),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (cargo_id) REFERENCES cargos(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (recibido_por) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON pagos(alumno_id);
CREATE NONCLUSTERED INDEX idx_fecha_pago ON pagos(fecha_pago);
CREATE NONCLUSTERED INDEX idx_folio ON pagos(folio_recibo);
GO

-- Estados de Cuenta
CREATE TABLE estados_cuenta (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    total_cargos DECIMAL(10,2) DEFAULT 0,
    total_pagos DECIMAL(10,2) DEFAULT 0,
    saldo_pendiente DECIMAL(10,2) DEFAULT 0,
    tiene_adeudos BIT DEFAULT 0,
    ultima_actualizacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    UNIQUE (alumno_id, ciclo_escolar)
);
GO

CREATE NONCLUSTERED INDEX idx_tiene_adeudos ON estados_cuenta(tiene_adeudos);
GO

CREATE TRIGGER trg_estados_cuenta_update
ON estados_cuenta
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ec
    SET ultima_actualizacion = GETDATE()
    FROM estados_cuenta ec
    INNER JOIN inserted i ON ec.id = i.id;
END
GO

-- ============================================
-- MÓDULO 13: BIBLIOTECA Y RECURSOS
-- ============================================

CREATE TABLE biblioteca_recursos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    tipo NVARCHAR(20) NOT NULL CHECK (tipo IN ('libro', 'revista', 'material_didactico', 'equipo', 'otro')),
    codigo NVARCHAR(50) UNIQUE,
    titulo NVARCHAR(200) NOT NULL,
    autor NVARCHAR(200),
    editorial NVARCHAR(200),
    isbn NVARCHAR(20),
    año_publicacion INT,
    categoria NVARCHAR(100),
    ubicacion NVARCHAR(100),
    cantidad_total INT DEFAULT 1,
    cantidad_disponible INT DEFAULT 1,
    valor DECIMAL(10,2),
    descripcion NVARCHAR(MAX),
    foto_url NVARCHAR(500),
    estado NVARCHAR(20) DEFAULT 'bueno' CHECK (estado IN ('excelente', 'bueno', 'regular', 'malo', 'dado_de_baja')),
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_codigo ON biblioteca_recursos(codigo);
CREATE NONCLUSTERED INDEX idx_tipo ON biblioteca_recursos(tipo);
GO

-- Préstamos de Biblioteca
CREATE TABLE prestamos_biblioteca (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    recurso_id INT NOT NULL,
    usuario_id INT NOT NULL,
    fecha_prestamo DATETIME2 DEFAULT GETDATE(),
    fecha_devolucion_esperada DATE NOT NULL,
    fecha_devolucion_real DATETIME2,
    estado_al_prestar NVARCHAR(10) DEFAULT 'bueno' CHECK (estado_al_prestar IN ('excelente', 'bueno', 'regular', 'malo')),
    estado_al_devolver NVARCHAR(10) CHECK (estado_al_devolver IN ('excelente', 'bueno', 'regular', 'malo')),
    observaciones NVARCHAR(MAX),
    multa DECIMAL(10,2) DEFAULT 0,
    estatus NVARCHAR(10) DEFAULT 'activo' CHECK (estatus IN ('activo', 'devuelto', 'vencido', 'extraviado')),
    prestado_por INT NOT NULL,
    recibido_por INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (recurso_id) REFERENCES biblioteca_recursos(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (prestado_por) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (recibido_por) REFERENCES usuarios(id) ON DELETE SET NULL
);
GO

CREATE NONCLUSTERED INDEX idx_usuario_estatus ON prestamos_biblioteca(usuario_id, estatus);
CREATE NONCLUSTERED INDEX idx_fecha_devolucion ON prestamos_biblioteca(fecha_devolucion_esperada);
GO

-- ============================================
-- MÓDULO 14: EXPEDIENTE MÉDICO
-- ============================================

CREATE TABLE expediente_medico (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT UNIQUE NOT NULL,
    tipo_sangre NVARCHAR(5),
    alergias NVARCHAR(MAX),
    condiciones_cronicas NVARCHAR(MAX),
    medicamentos_actuales NVARCHAR(MAX),
    vacunas NVARCHAR(MAX) CHECK (ISJSON(vacunas) = 1),
    seguro_medico NVARCHAR(200),
    numero_poliza NVARCHAR(100),
    medico_familiar NVARCHAR(200),
    telefono_medico NVARCHAR(20),
    observaciones_importantes NVARCHAR(MAX),
    updated_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE
);
GO

CREATE TRIGGER trg_expediente_medico_update
ON expediente_medico
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE em
    SET updated_at = GETDATE()
    FROM expediente_medico em
    INNER JOIN inserted i ON em.id = i.id;
END
GO

-- Incidentes Médicos en la Escuela
CREATE TABLE incidentes_medicos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT NOT NULL,
    fecha_hora DATETIME2 NOT NULL,
    tipo NVARCHAR(10) NOT NULL CHECK (tipo IN ('accidente', 'enfermedad', 'lesion', 'crisis', 'otro')),
    descripcion NVARCHAR(MAX) NOT NULL,
    sintomas NVARCHAR(MAX),
    atencion_proporcionada NVARCHAR(MAX),
    derivado_hospital BIT DEFAULT 0,
    hospital NVARCHAR(200),
    padres_notificados BIT DEFAULT 0,
    fecha_notificacion DATETIME2,
    reportado_por INT NOT NULL,
    seguimiento_requerido BIT DEFAULT 0,
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (reportado_por) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON incidentes_medicos(alumno_id);
CREATE NONCLUSTERED INDEX idx_fecha ON incidentes_medicos(fecha_hora);
GO

-- ============================================
-- MÓDULO 15: REPORTES Y DOCUMENTOS
-- ============================================

CREATE TABLE documentos_alumno (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT NOT NULL,
    tipo NVARCHAR(30) NOT NULL CHECK (tipo IN ('acta_nacimiento', 'curp', 'comprobante_domicilio', 'foto', 'certificado_estudios', 'cartilla_vacunacion', 'otro')),
    nombre_archivo NVARCHAR(200) NOT NULL,
    url_archivo NVARCHAR(500) NOT NULL,
    fecha_subida DATETIME2 DEFAULT GETDATE(),
    subido_por INT NOT NULL,
    verificado BIT DEFAULT 0,
    fecha_verificacion DATETIME2,
    verificado_por INT,
    observaciones NVARCHAR(MAX),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (subido_por) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (verificado_por) REFERENCES usuarios(id) ON DELETE SET NULL
);
GO

CREATE NONCLUSTERED INDEX idx_alumno_tipo ON documentos_alumno(alumno_id, tipo);
GO

-- Boletas Generadas
CREATE TABLE boletas (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    alumno_id INT NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    periodo_id INT,
    promedio_general DECIMAL(5,2),
    url_pdf NVARCHAR(500),
    fecha_generacion DATETIME2 DEFAULT GETDATE(),
    generado_por INT NOT NULL,
    tipo NVARCHAR(10) DEFAULT 'parcial' CHECK (tipo IN ('parcial', 'final')),
    observaciones NVARCHAR(MAX),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE CASCADE,
    FOREIGN KEY (periodo_id) REFERENCES periodos_evaluacion(id) ON DELETE CASCADE,
    FOREIGN KEY (generado_por) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_alumno_ciclo ON boletas(alumno_id, ciclo_escolar);
GO

-- ============================================
-- MÓDULO 16: SINCRONIZACIÓN Y AUDITORÍA
-- ============================================

-- Control de Sincronización (para clientes offline)
CREATE TABLE sync_control (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    tabla NVARCHAR(100) NOT NULL,
    registro_id INT NOT NULL,
    operacion NVARCHAR(10) NOT NULL CHECK (operacion IN ('INSERT', 'UPDATE', 'DELETE')),
    datos_antiguos NVARCHAR(MAX) CHECK (ISJSON(datos_antiguos) = 1),
    datos_nuevos NVARCHAR(MAX) CHECK (ISJSON(datos_nuevos) = 1),
    timestamp_operacion DATETIME2 DEFAULT GETDATE(),
    sincronizado BIT DEFAULT 0,
    timestamp_sincronizacion DATETIME2,
    usuario_id INT,
    dispositivo_id NVARCHAR(100),
    version INT DEFAULT 1,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE SET NULL
);
GO

CREATE NONCLUSTERED INDEX idx_sincronizado ON sync_control(sincronizado);
CREATE NONCLUSTERED INDEX idx_tabla_registro ON sync_control(tabla, registro_id);
CREATE NONCLUSTERED INDEX idx_timestamp ON sync_control(timestamp_operacion);
GO

-- Log de Auditoría
CREATE TABLE auditoria (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    tabla NVARCHAR(100) NOT NULL,
    registro_id INT NOT NULL,
    accion NVARCHAR(10) NOT NULL CHECK (accion IN ('CREATE', 'READ', 'UPDATE', 'DELETE', 'LOGIN', 'LOGOUT')),
    usuario_id INT,
    ip_address NVARCHAR(45),
    user_agent NVARCHAR(500),
    datos_anteriores NVARCHAR(MAX) CHECK (ISJSON(datos_anteriores) = 1),
    datos_nuevos NVARCHAR(MAX) CHECK (ISJSON(datos_nuevos) = 1),
    descripcion NVARCHAR(MAX),
    timestamp DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE SET NULL
);
GO

CREATE NONCLUSTERED INDEX idx_tabla_registro ON auditoria(tabla, registro_id);
CREATE NONCLUSTERED INDEX idx_usuario ON auditoria(usuario_id);
CREATE NONCLUSTERED INDEX idx_timestamp ON auditoria(timestamp);
CREATE NONCLUSTERED INDEX idx_accion ON auditoria(accion);
GO

-- Dispositivos Registrados (para control de sesiones)
CREATE TABLE dispositivos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    device_id NVARCHAR(100) UNIQUE NOT NULL,
    device_name NVARCHAR(200),
    tipo NVARCHAR(10) NOT NULL CHECK (tipo IN ('web', 'mobile', 'desktop')),
    so NVARCHAR(50),
    navegador NVARCHAR(50),
    token_fcm NVARCHAR(500),
    ip_ultima_conexion NVARCHAR(45),
    fecha_registro DATETIME2 DEFAULT GETDATE(),
    ultima_actividad DATETIME2 DEFAULT GETDATE(),
    activo BIT DEFAULT 1,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_usuario_activo ON dispositivos(usuario_id, activo);
GO

CREATE TRIGGER trg_dispositivos_update
ON dispositivos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE d
    SET ultima_actividad = GETDATE()
    FROM dispositivos d
    INNER JOIN inserted i ON d.id = i.id
    WHERE i.ultima_actividad = (SELECT ultima_actividad FROM deleted WHERE id = i.id); -- Solo actualiza si no se proporcionó un valor
END
GO

-- ============================================
-- MÓDULO 17: INTEGRACIÓN GUBERNAMENTAL
-- ============================================

CREATE TABLE integraciones_gobierno (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    pais NVARCHAR(50),
    estado NVARCHAR(50),
    tipo NVARCHAR(10) NOT NULL CHECK (tipo IN ('federal', 'estatal', 'municipal')),
    url_portal NVARCHAR(500),
    usuario_portal NVARCHAR(100),
    credenciales_encriptadas NVARCHAR(MAX),
    configuracion NVARCHAR(MAX) CHECK (ISJSON(configuracion) = 1),
    activo BIT DEFAULT 1,
    ultima_sincronizacion DATETIME2,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela_activo ON integraciones_gobierno(escuela_id, activo);
GO

-- Log de Exportaciones Gubernamentales
CREATE TABLE exportaciones_gobierno (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    integracion_id INT NOT NULL,
    tipo_datos NVARCHAR(100),
    ciclo_escolar NVARCHAR(20),
    fecha_exportacion DATETIME2 DEFAULT GETDATE(),
    registros_exportados INT,
    archivo_generado_url NVARCHAR(500),
    formato NVARCHAR(10) NOT NULL CHECK (formato IN ('excel', 'csv', 'xml', 'json', 'api')),
    estatus NVARCHAR(10) DEFAULT 'pendiente' CHECK (estatus IN ('pendiente', 'generado', 'enviado', 'error')),
    mensaje_error NVARCHAR(MAX),
    exportado_por INT NOT NULL,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (integracion_id) REFERENCES integraciones_gobierno(id) ON DELETE CASCADE,
    FOREIGN KEY (exportado_por) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_fecha ON exportaciones_gobierno(fecha_exportacion);
CREATE NONCLUSTERED INDEX idx_estatus ON exportaciones_gobierno(estatus);
GO

-- ============================================
-- MÓDULO 18: CONFIGURACIONES Y PREFERENCIAS
-- ============================================

CREATE TABLE configuraciones_escuela (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT UNIQUE NOT NULL,
    año_escolar_inicio_mes INT DEFAULT 8,
    año_escolar_inicio_dia INT DEFAULT 1,
    escala_calificaciones NVARCHAR(10) DEFAULT '0-10' CHECK (escala_calificaciones IN ('0-10', '0-100', 'A-F', 'MB-B-S-I')),
    calificacion_minima_aprobatoria DECIMAL(5,2) DEFAULT 6.00,
    permite_calificaciones_decimales BIT DEFAULT 1,
    horario_entrada TIME DEFAULT '08:00:00',
    horario_salida TIME DEFAULT '14:00:00',
    minutos_tolerancia_retardo INT DEFAULT 10,
    logo_url NVARCHAR(500),
    colores_tema NVARCHAR(MAX) CHECK (ISJSON(colores_tema) = 1),
    idioma_predeterminado NVARCHAR(10) DEFAULT 'es',
    zona_horaria NVARCHAR(50) DEFAULT 'America/Mexico_City',
    moneda NVARCHAR(10) DEFAULT 'MXN',
    mensaje_bienvenida NVARCHAR(MAX),
    politicas_privacidad NVARCHAR(MAX),
    terminos_condiciones NVARCHAR(MAX),
    updated_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE TRIGGER trg_configuraciones_escuela_update
ON configuraciones_escuela
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ce
    SET updated_at = GETDATE()
    FROM configuraciones_escuela ce
    INNER JOIN inserted i ON ce.id = i.id;
END
GO

-- Preferencias de Usuario
CREATE TABLE preferencias_usuario (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT UNIQUE NOT NULL,
    idioma NVARCHAR(10) DEFAULT 'es',
    tema NVARCHAR(10) DEFAULT 'claro' CHECK (tema IN ('claro', 'oscuro', 'auto')),
    notificaciones_push BIT DEFAULT 1,
    notificaciones_email BIT DEFAULT 1,
    notificaciones_sms BIT DEFAULT 0,
    configuracion_privacidad NVARCHAR(MAX) CHECK (ISJSON(configuracion_privacidad) = 1),
    updated_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE TRIGGER trg_preferencias_usuario_update
ON preferencias_usuario
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE pu
    SET updated_at = GETDATE()
    FROM preferencias_usuario pu
    INNER JOIN inserted i ON pu.id = i.id;
END
GO

-- ============================================
-- VISTAS ÚTILES
-- ============================================

-- Vista de Alumnos con Adeudos
CREATE VIEW v_alumnos_adeudos AS
SELECT 
    a.id,
    a.escuela_id,
    a.matricula,
    CONCAT(a.nombre, ' ', a.apellido_paterno, ' ', ISNULL(a.apellido_materno, '')) AS nombre_completo,
    ec.saldo_pendiente,
    ec.ciclo_escolar,
    g.nombre AS grado,
    gr.nombre AS grupo
FROM alumnos a
INNER JOIN estados_cuenta ec ON a.id = ec.alumno_id
INNER JOIN inscripciones i ON a.id = i.alumno_id AND i.ciclo_escolar = ec.ciclo_escolar
INNER JOIN grupos gr ON i.grupo_id = gr.id
INNER JOIN grados g ON gr.grado_id = g.id
WHERE ec.tiene_adeudos = 1 AND a.estatus = 'activo';
GO

-- Vista de Rankings por Grupo
CREATE VIEW v_ranking_grupo AS
SELECT 
    i.grupo_id,
    a.id AS alumno_id,
    CONCAT(a.nombre, ' ', a.apellido_paterno) AS nombre_completo,
    ap.puntos_totales,
    ap.puntos_academicos,
    ap.puntos_conducta,
    ap.puntos_asistencia,
    RANK() OVER (PARTITION BY i.grupo_id ORDER BY ap.puntos_totales DESC) AS ranking
FROM alumnos a
INNER JOIN inscripciones i ON a.id = i.alumno_id
INNER JOIN alumno_puntos ap ON a.id = ap.alumno_id AND i.ciclo_escolar = ap.ciclo_escolar
WHERE a.estatus = 'activo' AND i.estatus = 'inscrito';
GO

-- Vista de Asistencias del Mes
CREATE VIEW v_asistencias_mes AS
SELECT 
    a.escuela_id,
    al.id AS alumno_id,
    al.matricula,
    CONCAT(al.nombre, ' ', al.apellido_paterno) AS nombre_completo,
    a.grupo_id,
    YEAR(a.fecha) AS año,
    MONTH(a.fecha) AS mes,
    COUNT(*) AS total_dias,
    SUM(CASE WHEN a.estatus = 'presente' THEN 1 ELSE 0 END) AS dias_presente,
    SUM(CASE WHEN a.estatus = 'falta' THEN 1 ELSE 0 END) AS dias_falta,
    SUM(CASE WHEN a.estatus = 'retardo' THEN 1 ELSE 0 END) AS dias_retardo,
    ROUND((SUM(CASE WHEN a.estatus = 'presente' THEN 1 ELSE 0 END) * 1.0 / COUNT(*)) * 100, 2) AS porcentaje_asistencia
FROM asistencias a
INNER JOIN alumnos al ON a.alumno_id = al.id
GROUP BY a.escuela_id, al.id, al.matricula, CONCAT(al.nombre, ' ', al.apellido_paterno), a.grupo_id, YEAR(a.fecha), MONTH(a.fecha);
GO

-- ============================================
-- TRIGGERS PARA AUTOMATIZACIÓN
-- ============================================

-- Trigger para actualizar estado de cuenta al crear cargo (Usa MERGE)
CREATE TRIGGER trg_after_cargo_insert
ON cargos
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    MERGE INTO estados_cuenta AS target
    USING (
        SELECT 
            escuela_id, 
            alumno_id, 
            ciclo_escolar, 
            SUM(monto_final) AS total_monto_insertado
        FROM inserted
        GROUP BY escuela_id, alumno_id, ciclo_escolar
    ) AS source
    ON (target.alumno_id = source.alumno_id AND target.ciclo_escolar = source.ciclo_escolar AND target.escuela_id = source.escuela_id)
    WHEN MATCHED THEN
        UPDATE SET
            total_cargos = target.total_cargos + source.total_monto_insertado,
            saldo_pendiente = target.saldo_pendiente + source.total_monto_insertado,
            tiene_adeudos = 1
    WHEN NOT MATCHED THEN
        INSERT (escuela_id, alumno_id, ciclo_escolar, total_cargos, saldo_pendiente, tiene_adeudos)
        VALUES (source.escuela_id, source.alumno_id, source.ciclo_escolar, source.total_monto_insertado, source.total_monto_insertado, 1);
END
GO

-- Trigger para actualizar estado de cuenta al crear pago
CREATE TRIGGER trg_after_pago_insert
ON pagos
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Actualizar estados_cuenta (agregado para ser seguro en lotes)
    UPDATE ec
    SET 
        ec.total_pagos = ec.total_pagos + agg_pagos.total_pagado,
        ec.saldo_pendiente = ec.saldo_pendiente - agg_pagos.total_pagado,
        ec.tiene_adeudos = CASE WHEN (ec.saldo_pendiente - agg_pagos.total_pagado) > 0 THEN 1 ELSE 0 END
    FROM estados_cuenta ec
    INNER JOIN (
        SELECT
            p.alumno_id,
            c.ciclo_escolar,
            SUM(p.monto) AS total_pagado
        FROM inserted p
        INNER JOIN cargos c ON p.cargo_id = c.id
        GROUP BY p.alumno_id, c.ciclo_escolar
    ) AS agg_pagos ON ec.alumno_id = agg_pagos.alumno_id AND ec.ciclo_escolar = agg_pagos.ciclo_escolar;

    -- 2. Actualizar estatus del cargo
    UPDATE c
    SET 
        c.estatus = CASE
            WHEN (c.monto_final - COALESCE(p_sum.total_pagado_cargo, 0)) <= 0 THEN 'pagado'
            ELSE 'parcial'
        END
    FROM cargos c
    INNER JOIN (
        -- Obtener el total pagado para este cargo específico, consultando la tabla de pagos
        SELECT cargo_id, SUM(monto) AS total_pagado_cargo
        FROM pagos
        WHERE cargo_id IN (SELECT DISTINCT cargo_id FROM inserted)
        GROUP BY cargo_id
    ) AS p_sum ON c.id = p_sum.cargo_id
    WHERE c.id IN (SELECT DISTINCT cargo_id FROM inserted);
END
GO

-- Trigger para actualizar disponibilidad de recursos en biblioteca
CREATE TRIGGER trg_after_prestamo_insert
ON prestamos_biblioteca
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE br
    SET br.cantidad_disponible = br.cantidad_disponible - agg.total_prestado
    FROM biblioteca_recursos br
    INNER JOIN (
        SELECT recurso_id, COUNT(*) AS total_prestado
        FROM inserted
        GROUP BY recurso_id
    ) AS agg ON br.id = agg.recurso_id;
END
GO

-- Trigger para devolver recurso
CREATE TRIGGER trg_after_prestamo_devolucion
ON prestamos_biblioteca
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE br
    SET br.cantidad_disponible = br.cantidad_disponible + agg.total_devuelto
    FROM biblioteca_recursos br
    INNER JOIN (
        SELECT 
            i.recurso_id,
            COUNT(*) AS total_devuelto
        FROM inserted i
        INNER JOIN deleted d ON i.id = d.id
        WHERE i.estatus = 'devuelto' AND d.estatus != 'devuelto'
        GROUP BY i.recurso_id
    ) AS agg ON br.id = agg.recurso_id;
END
GO

-- Trigger para registro de auditoría automático
CREATE TRIGGER trg_auditoria_usuario_update
ON usuarios
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO auditoria (escuela_id, tabla, registro_id, accion, usuario_id, datos_anteriores, datos_nuevos, descripcion)
    SELECT
        i.escuela_id,
        'usuarios',
        i.id,
        'UPDATE',
        i.id, -- El usuario que se está modificando (no el que modifica)
        (SELECT d.email, d.rol, d.activo FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) AS datos_anteriores,
        (SELECT i.email, i.rol, i.activo FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) AS datos_nuevos,
        'Actualización de usuario'
    FROM inserted i
    INNER JOIN deleted d ON i.id = d.id;
END
GO

-- ============================================
-- DATOS INICIALES
-- ============================================

-- Insertar permisos base
INSERT INTO permisos (nombre, descripcion, modulo) VALUES
('ver_alumnos', 'Ver lista de alumnos', 'alumnos'),
('crear_alumnos', 'Crear nuevos alumnos', 'alumnos'),
('editar_alumnos', 'Editar información de alumnos', 'alumnos'),
('eliminar_alumnos', 'Eliminar alumnos', 'alumnos'),
('ver_calificaciones', 'Ver calificaciones', 'calificaciones'),
('capturar_calificaciones', 'Capturar calificaciones', 'calificaciones'),
('ver_asistencias', 'Ver asistencias', 'asistencias'),
('registrar_asistencias', 'Registrar asistencias', 'asistencias'),
('ver_finanzas', 'Ver información financiera', 'finanzas'),
('gestionar_pagos', 'Gestionar pagos y cobros', 'finanzas'),
('enviar_notificaciones', 'Enviar notificaciones', 'comunicacion'),
('ver_reportes', 'Ver reportes generales', 'reportes'),
('gestionar_maestros', 'Gestionar maestros', 'maestros'),
('gestionar_grupos', 'Gestionar grupos', 'grupos'),
('configurar_sistema', 'Configurar el sistema', 'configuracion');
GO

-- ============================================
-- FIN DEL SCRIPT
-- ============================================