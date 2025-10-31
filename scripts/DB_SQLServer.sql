-- ============================================
-- SISTEMA DE GESTIÓN ESCOLAR MULTI-TENANT
-- Base de Datos SQL Server
-- ============================================

-- Configuración inicial
USE master;
GO

-- Eliminar base de datos si existe
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'school_system')
BEGIN
    ALTER DATABASE school_system SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE school_system;
END
GO

-- Crear base de datos
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

-- Trigger para actualizar updated_at
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

-- Tabla de Dispositivos
CREATE TABLE dispositivos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    nombre_dispositivo NVARCHAR(100),
    token_fcm NVARCHAR(255),
    plataforma NVARCHAR(20) NOT NULL CHECK (plataforma IN ('Android', 'iOS', 'Web')),
    modelo NVARCHAR(100),
    version_os NVARCHAR(50),
    activo BIT DEFAULT 1,
    ultimo_acceso DATETIME2,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE(),
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
    SET updated_at = GETDATE()
    FROM dispositivos d
    INNER JOIN inserted i ON d.id = i.id;
END
GO

-- ============================================
-- MÓDULO 2: ACADÉMICO
-- ============================================

-- Tabla de Niveles Educativos
CREATE TABLE niveles_educativos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(MAX),
    orden INT DEFAULT 0,
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON niveles_educativos(escuela_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON niveles_educativos(is_deleted);
GO

-- Tabla de Grados
CREATE TABLE grados (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nivel_educativo_id INT NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(MAX),
    orden INT DEFAULT 0,
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (nivel_educativo_id) REFERENCES niveles_educativos(id)
);
GO

CREATE NONCLUSTERED INDEX idx_nivel ON grados(nivel_educativo_id);
CREATE NONCLUSTERED INDEX idx_escuela ON grados(escuela_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON grados(is_deleted);
GO

-- Tabla de Materias
CREATE TABLE materias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    clave NVARCHAR(20),
    descripcion NVARCHAR(MAX),
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON materias(escuela_id);
CREATE NONCLUSTERED INDEX idx_clave ON materias(clave);
CREATE NONCLUSTERED INDEX idx_is_deleted ON materias(is_deleted);
GO

-- Tabla de relación Grado-Materia
CREATE TABLE grado_materias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    grado_id INT NOT NULL,
    materia_id INT NOT NULL,
    horas_semana DECIMAL(5, 2) DEFAULT 0,
    obligatoria BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (grado_id) REFERENCES grados(id) ON DELETE CASCADE,
    FOREIGN KEY (materia_id) REFERENCES materias(id),
    CONSTRAINT uk_grado_materia UNIQUE (grado_id, materia_id)
);
GO

CREATE NONCLUSTERED INDEX idx_is_deleted ON grado_materias(is_deleted);
GO

-- Tabla de Grupos
CREATE TABLE grupos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    grado_id INT NOT NULL,
    nombre NVARCHAR(50) NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    capacidad_maxima INT DEFAULT 30,
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (grado_id) REFERENCES grados(id)
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON grupos(escuela_id);
CREATE NONCLUSTERED INDEX idx_grado ON grupos(grado_id);
CREATE NONCLUSTERED INDEX idx_ciclo ON grupos(ciclo_escolar);
CREATE NONCLUSTERED INDEX idx_is_deleted ON grupos(is_deleted);
GO

-- Tabla de Padres/Tutores
CREATE TABLE padres (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    usuario_id INT,
    nombre NVARCHAR(100) NOT NULL,
    apellido_paterno NVARCHAR(100) NOT NULL,
    apellido_materno NVARCHAR(100),
    telefono NVARCHAR(20),
    telefono_emergencia NVARCHAR(20),
    email NVARCHAR(100),
    direccion NVARCHAR(300),
    ocupacion NVARCHAR(100),
    lugar_trabajo NVARCHAR(200),
    telefono_trabajo NVARCHAR(20),
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON padres(escuela_id);
CREATE NONCLUSTERED INDEX idx_email ON padres(email);
CREATE NONCLUSTERED INDEX idx_is_deleted ON padres(is_deleted);
GO

-- Tabla de Alumnos
CREATE TABLE alumnos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    usuario_id INT,
    matricula NVARCHAR(50) NOT NULL,
    curp NVARCHAR(18),
    nombre NVARCHAR(100) NOT NULL,
    apellido_paterno NVARCHAR(100) NOT NULL,
    apellido_materno NVARCHAR(100),
    fecha_nacimiento DATE NOT NULL,
    genero NVARCHAR(20) NOT NULL,
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
    estatus NVARCHAR(20) NOT NULL DEFAULT 'Activo',
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE NO ACTION,
    CONSTRAINT uk_alumno_matricula UNIQUE (matricula),
    CONSTRAINT uk_alumno_curp UNIQUE (curp)
);
GO

CREATE NONCLUSTERED INDEX idx_estatus ON alumnos(estatus);
CREATE NONCLUSTERED INDEX idx_escuela_estatus ON alumnos(escuela_id, estatus);
CREATE NONCLUSTERED INDEX idx_is_deleted ON alumnos(is_deleted);
GO

-- Tabla de relación Alumno-Padre
CREATE TABLE alumno_padres (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    padre_id INT NOT NULL,
    parentesco NVARCHAR(20) NOT NULL CHECK (parentesco IN ('Padre', 'Madre', 'Tutor', 'Abuelo', 'Abuela', 'Tio', 'Tia', 'Otro')),
    es_tutor_principal BIT DEFAULT 0,
    vive_con_alumno BIT DEFAULT 1,
    puede_recoger BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    FOREIGN KEY (padre_id) REFERENCES padres(id) ON DELETE NO ACTION,
    CONSTRAINT uk_alumno_padre UNIQUE (alumno_id, padre_id)
);
GO

CREATE NONCLUSTERED INDEX idx_is_deleted ON alumno_padres(is_deleted);
GO

-- Tabla de Maestros
CREATE TABLE maestros (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    usuario_id INT,
    numero_empleado NVARCHAR(50),
    nombre NVARCHAR(100) NOT NULL,
    apellido_paterno NVARCHAR(100) NOT NULL,
    apellido_materno NVARCHAR(100),
    telefono NVARCHAR(20),
    email NVARCHAR(100),
    fecha_ingreso DATE NOT NULL,
    especialidad NVARCHAR(100),
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE NO ACTION,
    CONSTRAINT uk_numero_empleado UNIQUE (numero_empleado)
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON maestros(escuela_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON maestros(is_deleted);
GO

-- Tabla de asignación Grupo-Materia-Maestro
CREATE TABLE grupo_materia_maestros (
    id INT IDENTITY(1,1) PRIMARY KEY,
    grupo_id INT NOT NULL,
    materia_id INT NOT NULL,
    maestro_id INT NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (grupo_id) REFERENCES grupos(id) ON DELETE NO ACTION,
    FOREIGN KEY (materia_id) REFERENCES materias(id) ON DELETE NO ACTION,
    FOREIGN KEY (maestro_id) REFERENCES maestros(id) ON DELETE NO ACTION,
    CONSTRAINT uk_grupo_materia_maestro UNIQUE (grupo_id, materia_id)
);
GO

CREATE NONCLUSTERED INDEX idx_maestro ON grupo_materia_maestros(maestro_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON grupo_materia_maestros(is_deleted);
GO

-- Tabla de Inscripciones
CREATE TABLE inscripciones (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    grupo_id INT NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    fecha_inscripcion DATE NOT NULL,
    estatus NVARCHAR(20) NOT NULL DEFAULT 'Activa',
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    FOREIGN KEY (grupo_id) REFERENCES grupos(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON inscripciones(alumno_id);
CREATE NONCLUSTERED INDEX idx_grupo ON inscripciones(grupo_id);
CREATE NONCLUSTERED INDEX idx_ciclo ON inscripciones(ciclo_escolar);
CREATE NONCLUSTERED INDEX idx_is_deleted ON inscripciones(is_deleted);
GO

-- ============================================
-- MÓDULO 3: EVALUACIÓN
-- ============================================

-- Tabla de Periodos de Evaluación
CREATE TABLE periodos_evaluacion (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON periodos_evaluacion(escuela_id);
CREATE NONCLUSTERED INDEX idx_ciclo ON periodos_evaluacion(ciclo_escolar);
CREATE NONCLUSTERED INDEX idx_is_deleted ON periodos_evaluacion(is_deleted);
GO

-- Tabla de Calificaciones
CREATE TABLE calificaciones (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    materia_id INT NOT NULL,
    periodo_evaluacion_id INT NOT NULL,
    calificacion DECIMAL(5, 2) NOT NULL,
    calificacion_letra NVARCHAR(5),
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    FOREIGN KEY (materia_id) REFERENCES materias(id) ON DELETE NO ACTION,
    FOREIGN KEY (periodo_evaluacion_id) REFERENCES periodos_evaluacion(id) ON DELETE NO ACTION,
    CONSTRAINT uk_alumno_materia_periodo UNIQUE (alumno_id, materia_id, periodo_evaluacion_id)
);
GO

CREATE NONCLUSTERED INDEX idx_is_deleted ON calificaciones(is_deleted);
GO

-- Tabla de Asistencias
CREATE TABLE asistencias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    grupo_id INT NOT NULL,
    fecha DATE NOT NULL,
    estatus NVARCHAR(20) NOT NULL DEFAULT 'Presente',
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    FOREIGN KEY (grupo_id) REFERENCES grupos(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON asistencias(alumno_id);
CREATE NONCLUSTERED INDEX idx_grupo ON asistencias(grupo_id);
CREATE NONCLUSTERED INDEX idx_fecha ON asistencias(fecha);
CREATE NONCLUSTERED INDEX idx_is_deleted ON asistencias(is_deleted);
GO

-- ============================================
-- MÓDULO 4: CONDUCTA
-- ============================================

-- Tabla de Registros de Conducta
CREATE TABLE registros_conducta (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    maestro_id INT NOT NULL,
    fecha DATETIME2 NOT NULL,
    tipo NVARCHAR(20) NOT NULL,
    gravedad NVARCHAR(20),
    descripcion NVARCHAR(MAX) NOT NULL,
    accion_tomada NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    FOREIGN KEY (maestro_id) REFERENCES maestros(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON registros_conducta(alumno_id);
CREATE NONCLUSTERED INDEX idx_maestro ON registros_conducta(maestro_id);
CREATE NONCLUSTERED INDEX idx_fecha ON registros_conducta(fecha);
CREATE NONCLUSTERED INDEX idx_is_deleted ON registros_conducta(is_deleted);
GO

-- Tabla de Puntos por Alumno (Gamificación)
CREATE TABLE alumno_puntos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    puntos_totales INT DEFAULT 0,
    nivel INT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    CONSTRAINT uk_alumno_puntos UNIQUE (alumno_id)
);
GO

CREATE NONCLUSTERED INDEX idx_is_deleted ON alumno_puntos(is_deleted);
GO

-- Tabla de Historial de Puntos
CREATE TABLE historial_puntos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    puntos INT NOT NULL,
    motivo NVARCHAR(200) NOT NULL,
    descripcion NVARCHAR(MAX),
    tipo NVARCHAR(20) NOT NULL,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON historial_puntos(alumno_id);
CREATE NONCLUSTERED INDEX idx_fecha ON historial_puntos(created_at);
GO

-- Tabla de Insignias
CREATE TABLE insignias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(MAX),
    icono_url NVARCHAR(500),
    puntos_requeridos INT DEFAULT 0,
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON insignias(escuela_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON insignias(is_deleted);
GO

-- Tabla de Insignias por Alumno
CREATE TABLE alumno_insignias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    insignia_id INT NOT NULL,
    fecha_obtencion DATETIME2 DEFAULT GETDATE(),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    FOREIGN KEY (insignia_id) REFERENCES insignias(id) ON DELETE NO ACTION,
    CONSTRAINT uk_alumno_insignia UNIQUE (alumno_id, insignia_id)
);
GO

CREATE NONCLUSTERED INDEX idx_fecha ON alumno_insignias(fecha_obtencion);
GO

-- Tabla de Sanciones
CREATE TABLE sanciones (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    tipo_sancion NVARCHAR(50) NOT NULL,
    descripcion NVARCHAR(MAX) NOT NULL,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE,
    activa BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON sanciones(alumno_id);
CREATE NONCLUSTERED INDEX idx_activa ON sanciones(activa);
CREATE NONCLUSTERED INDEX idx_is_deleted ON sanciones(is_deleted);
GO

-- ============================================
-- MÓDULO 5: FINANZAS
-- ============================================

-- Tabla de Conceptos de Pago
CREATE TABLE conceptos_pago (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(MAX),
    monto_default DECIMAL(10, 2) NOT NULL,
    tipo NVARCHAR(50) NOT NULL,
    recurrente BIT DEFAULT 0,
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON conceptos_pago(escuela_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON conceptos_pago(is_deleted);
GO

-- Tabla de Cargos
CREATE TABLE cargos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    concepto_pago_id INT NOT NULL,
    monto DECIMAL(10, 2) NOT NULL,
    fecha_cargo DATE NOT NULL,
    fecha_vencimiento DATE,
    estatus NVARCHAR(20) NOT NULL DEFAULT 'Pendiente',
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    FOREIGN KEY (concepto_pago_id) REFERENCES conceptos_pago(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON cargos(alumno_id);
CREATE NONCLUSTERED INDEX idx_estatus ON cargos(estatus);
CREATE NONCLUSTERED INDEX idx_is_deleted ON cargos(is_deleted);
GO

-- Tabla de Pagos
CREATE TABLE pagos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    cargo_id INT,
    monto DECIMAL(10, 2) NOT NULL,
    fecha_pago DATE NOT NULL,
    metodo_pago NVARCHAR(50) NOT NULL,
    referencia NVARCHAR(100),
    comprobante_url NVARCHAR(500),
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    FOREIGN KEY (cargo_id) REFERENCES cargos(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON pagos(alumno_id);
CREATE NONCLUSTERED INDEX idx_fecha ON pagos(fecha_pago);
CREATE NONCLUSTERED INDEX idx_is_deleted ON pagos(is_deleted);
GO

-- Tabla de Estados de Cuenta
CREATE TABLE estados_cuenta (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    ciclo_escolar NVARCHAR(20) NOT NULL,
    total_cargos DECIMAL(10, 2) DEFAULT 0,
    total_pagos DECIMAL(10, 2) DEFAULT 0,
    saldo DECIMAL(10, 2) DEFAULT 0,
    ultima_actualizacion DATETIME2 DEFAULT GETDATE(),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    CONSTRAINT uk_alumno_ciclo UNIQUE (alumno_id, ciclo_escolar)
);
GO

CREATE NONCLUSTERED INDEX idx_is_deleted ON estados_cuenta(is_deleted);
GO

-- ============================================
-- MÓDULO 6: BIBLIOTECA
-- ============================================

-- Tabla de Categorías de Recursos
CREATE TABLE categorias_recurso (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(MAX),
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON categorias_recurso(escuela_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON categorias_recurso(is_deleted);
GO

-- Tabla de Libros
CREATE TABLE libros (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    categoria_id INT,
    titulo NVARCHAR(200) NOT NULL,
    autor NVARCHAR(200),
    editorial NVARCHAR(100),
    isbn NVARCHAR(20),
    año_publicacion INT,
    idioma NVARCHAR(50),
    numero_paginas INT,
    ubicacion NVARCHAR(100),
    cantidad_total INT DEFAULT 1,
    cantidad_disponible INT DEFAULT 1,
    portada_url NVARCHAR(500),
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (categoria_id) REFERENCES categorias_recurso(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_titulo ON libros(titulo);
CREATE NONCLUSTERED INDEX idx_isbn ON libros(isbn);
CREATE NONCLUSTERED INDEX idx_escuela ON libros(escuela_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON libros(is_deleted);
GO

-- Tabla de Préstamos
CREATE TABLE prestamos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    libro_id INT NOT NULL,
    usuario_id INT NOT NULL,
    fecha_prestamo DATE NOT NULL,
    fecha_devolucion_estimada DATE NOT NULL,
    fecha_devolucion_real DATE,
    estatus NVARCHAR(20) NOT NULL DEFAULT 'Activo',
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (libro_id) REFERENCES libros(id) ON DELETE NO ACTION,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_libro ON prestamos(libro_id);
CREATE NONCLUSTERED INDEX idx_usuario ON prestamos(usuario_id);
CREATE NONCLUSTERED INDEX idx_estatus ON prestamos(estatus);
CREATE NONCLUSTERED INDEX idx_is_deleted ON prestamos(is_deleted);
GO

-- ============================================
-- MÓDULO 7: MÉDICO
-- ============================================

-- Tabla de Expedientes Médicos
CREATE TABLE expedientes_medicos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alumno_id INT NOT NULL,
    tipo_sangre NVARCHAR(5),
    peso DECIMAL(5, 2),
    altura DECIMAL(5, 2),
    imc DECIMAL(5, 2),
    observaciones NVARCHAR(MAX),
    ultima_revision DATE,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    CONSTRAINT uk_expediente_alumno UNIQUE (alumno_id)
);
GO

CREATE NONCLUSTERED INDEX idx_is_deleted ON expedientes_medicos(is_deleted);
GO

-- Tabla de Vacunas
CREATE TABLE vacunas (
    id INT IDENTITY(1,1) PRIMARY KEY,
    expediente_medico_id INT NOT NULL,
    nombre_vacuna NVARCHAR(100) NOT NULL,
    fecha_aplicacion DATE NOT NULL,
    dosis NVARCHAR(50),
    observaciones NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (expediente_medico_id) REFERENCES expedientes_medicos(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_expediente ON vacunas(expediente_medico_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON vacunas(is_deleted);
GO

-- Tabla de Alergias
CREATE TABLE alergias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    expediente_medico_id INT NOT NULL,
    alergia NVARCHAR(200) NOT NULL,
    gravedad NVARCHAR(20),
    tratamiento NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (expediente_medico_id) REFERENCES expedientes_medicos(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_expediente ON alergias(expediente_medico_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON alergias(is_deleted);
GO

-- Tabla de Medicamentos
CREATE TABLE medicamentos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    expediente_medico_id INT NOT NULL,
    nombre_medicamento NVARCHAR(200) NOT NULL,
    dosis NVARCHAR(100),
    frecuencia NVARCHAR(100),
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE,
    motivo NVARCHAR(MAX),
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (expediente_medico_id) REFERENCES expedientes_medicos(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_expediente ON medicamentos(expediente_medico_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON medicamentos(is_deleted);
GO

-- Tabla de Historial Médico
CREATE TABLE historial_medico (
    id INT IDENTITY(1,1) PRIMARY KEY,
    expediente_medico_id INT NOT NULL,
    fecha DATETIME2 NOT NULL,
    motivo NVARCHAR(200) NOT NULL,
    diagnostico NVARCHAR(MAX),
    tratamiento NVARCHAR(MAX),
    medico NVARCHAR(200),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (expediente_medico_id) REFERENCES expedientes_medicos(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_expediente ON historial_medico(expediente_medico_id);
CREATE NONCLUSTERED INDEX idx_fecha ON historial_medico(fecha);
CREATE NONCLUSTERED INDEX idx_is_deleted ON historial_medico(is_deleted);
GO

-- ============================================
-- MÓDULO 8: COMUNICACIÓN
-- ============================================

-- Tabla de Mensajes
CREATE TABLE mensajes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    remitente_id INT NOT NULL,
    destinatario_id INT NOT NULL,
    asunto NVARCHAR(200),
    cuerpo NVARCHAR(MAX) NOT NULL,
    leido BIT DEFAULT 0,
    fecha_lectura DATETIME2,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (remitente_id) REFERENCES usuarios(id) ON DELETE NO ACTION,
    FOREIGN KEY (destinatario_id) REFERENCES usuarios(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_remitente ON mensajes(remitente_id);
CREATE NONCLUSTERED INDEX idx_destinatario ON mensajes(destinatario_id);
CREATE NONCLUSTERED INDEX idx_leido ON mensajes(leido);
CREATE NONCLUSTERED INDEX idx_is_deleted ON mensajes(is_deleted);
GO

-- Tabla de Notificaciones
CREATE TABLE notificaciones (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    tipo NVARCHAR(50) NOT NULL,
    titulo NVARCHAR(200) NOT NULL,
    mensaje NVARCHAR(MAX) NOT NULL,
    leida BIT DEFAULT 0,
    fecha_lectura DATETIME2,
    data NVARCHAR(MAX) CHECK (ISJSON(data) = 1),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_usuario_leida ON notificaciones(usuario_id, leida);
CREATE NONCLUSTERED INDEX idx_fecha ON notificaciones(created_at);
GO

-- Tabla de Comunicados
CREATE TABLE comunicados (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    titulo NVARCHAR(200) NOT NULL,
    contenido NVARCHAR(MAX) NOT NULL,
    tipo NVARCHAR(50) NOT NULL,
    destinatarios NVARCHAR(50) NOT NULL,
    fecha_publicacion DATETIME2 NOT NULL,
    fecha_expiracion DATETIME2,
    activo BIT DEFAULT 1,
    adjunto_url NVARCHAR(500),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON comunicados(escuela_id);
CREATE NONCLUSTERED INDEX idx_fecha_pub ON comunicados(fecha_publicacion);
CREATE NONCLUSTERED INDEX idx_is_deleted ON comunicados(is_deleted);
GO

-- Tabla de Lectura de Comunicados
CREATE TABLE comunicados_lectura (
    id INT IDENTITY(1,1) PRIMARY KEY,
    comunicado_id INT NOT NULL,
    usuario_id INT NOT NULL,
    fecha_lectura DATETIME2 DEFAULT GETDATE(),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (comunicado_id) REFERENCES comunicados(id) ON DELETE NO ACTION,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE NO ACTION,
    CONSTRAINT uk_comunicado_usuario UNIQUE (comunicado_id, usuario_id)
);
GO

CREATE NONCLUSTERED INDEX idx_comunicado ON comunicados_lectura(comunicado_id);
CREATE NONCLUSTERED INDEX idx_usuario ON comunicados_lectura(usuario_id);
GO

-- Tabla de Log de SMS
CREATE TABLE notificaciones_sms_log (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    numero_telefono NVARCHAR(20) NOT NULL,
    mensaje NVARCHAR(MAX) NOT NULL,
    estatus NVARCHAR(20) NOT NULL,
    proveedor NVARCHAR(50),
    costo DECIMAL(10, 4),
    respuesta_proveedor NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_usuario ON notificaciones_sms_log(usuario_id);
CREATE NONCLUSTERED INDEX idx_estatus ON notificaciones_sms_log(estatus);
CREATE NONCLUSTERED INDEX idx_fecha ON notificaciones_sms_log(created_at);
GO

-- ============================================
-- MÓDULO 9: DOCUMENTOS
-- ============================================

-- Tabla de Plantillas de Documentos
CREATE TABLE plantillas_documento (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(200) NOT NULL,
    tipo_documento NVARCHAR(100) NOT NULL,
    contenido_plantilla NVARCHAR(MAX) NOT NULL,
    variables NVARCHAR(MAX) CHECK (ISJSON(variables) = 1),
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON plantillas_documento(escuela_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON plantillas_documento(is_deleted);
GO

-- Tabla de Documentos Generados
CREATE TABLE documentos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    plantilla_id INT,
    tipo_entidad NVARCHAR(50) NOT NULL,
    entidad_id INT NOT NULL,
    nombre_archivo NVARCHAR(200) NOT NULL,
    ruta_archivo NVARCHAR(500) NOT NULL,
    tipo_archivo NVARCHAR(50),
    tamaño INT,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (plantilla_id) REFERENCES plantillas_documento(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_entidad ON documentos(tipo_entidad, entidad_id);
CREATE NONCLUSTERED INDEX idx_escuela ON documentos(escuela_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON documentos(is_deleted);
GO

-- Tabla de Reportes Personalizados
CREATE TABLE reportes_personalizados (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    nombre NVARCHAR(200) NOT NULL,
    descripcion NVARCHAR(MAX),
    tipo_reporte NVARCHAR(100) NOT NULL,
    parametros NVARCHAR(MAX) CHECK (ISJSON(parametros) = 1),
    query_sql NVARCHAR(MAX),
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON reportes_personalizados(escuela_id);
CREATE NONCLUSTERED INDEX idx_is_deleted ON reportes_personalizados(is_deleted);
GO

-- ============================================
-- MÓDULO 10: CALENDARIO
-- ============================================

-- Tabla de Eventos
CREATE TABLE eventos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    titulo NVARCHAR(200) NOT NULL,
    descripcion NVARCHAR(MAX),
    tipo_evento NVARCHAR(50) NOT NULL,
    fecha_inicio DATETIME2 NOT NULL,
    fecha_fin DATETIME2 NOT NULL,
    todo_el_dia BIT DEFAULT 0,
    ubicacion NVARCHAR(200),
    color NVARCHAR(20),
    notificar BIT DEFAULT 0,
    minutos_notificacion INT,
    activo BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON eventos(escuela_id);
CREATE NONCLUSTERED INDEX idx_fecha_inicio ON eventos(fecha_inicio);
CREATE NONCLUSTERED INDEX idx_is_deleted ON eventos(is_deleted);
GO

-- ============================================
-- MÓDULO 11: AUDITORÍA
-- ============================================

-- Tabla de Logs de Auditoría
CREATE TABLE logs_auditoria (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT,
    usuario_id INT,
    accion NVARCHAR(100) NOT NULL,
    entidad NVARCHAR(100) NOT NULL,
    entidad_id INT,
    descripcion NVARCHAR(MAX),
    ip_address NVARCHAR(50),
    user_agent NVARCHAR(500),
    datos_antes NVARCHAR(MAX) CHECK (ISJSON(datos_antes) = 1),
    datos_despues NVARCHAR(MAX) CHECK (ISJSON(datos_despues) = 1),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE NO ACTION,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_usuario ON logs_auditoria(usuario_id);
CREATE NONCLUSTERED INDEX idx_entidad ON logs_auditoria(entidad, entidad_id);
CREATE NONCLUSTERED INDEX idx_fecha ON logs_auditoria(created_at);
GO

-- Tabla de Cambios en Entidades
CREATE TABLE cambios_entidad (
    id INT IDENTITY(1,1) PRIMARY KEY,
    log_auditoria_id INT NOT NULL,
    campo NVARCHAR(100) NOT NULL,
    valor_anterior NVARCHAR(MAX),
    valor_nuevo NVARCHAR(MAX),
    tipo_cambio NVARCHAR(20),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (log_auditoria_id) REFERENCES logs_auditoria(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_log ON cambios_entidad(log_auditoria_id);
GO

-- Tabla de Sincronizaciones
CREATE TABLE sincronizaciones (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    dispositivo_id INT,
    tipo_sincronizacion NVARCHAR(50) NOT NULL,
    entidad NVARCHAR(100) NOT NULL,
    registros_enviados INT DEFAULT 0,
    registros_recibidos INT DEFAULT 0,
    exitosa BIT DEFAULT 1,
    mensaje_error NVARCHAR(MAX),
    fecha_inicio DATETIME2 NOT NULL,
    fecha_fin DATETIME2,
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    FOREIGN KEY (dispositivo_id) REFERENCES dispositivos(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_escuela ON sincronizaciones(escuela_id);
CREATE NONCLUSTERED INDEX idx_fecha ON sincronizaciones(fecha_inicio);
GO

-- ============================================
-- MÓDULO 12: CONFIGURACIÓN
-- ============================================

-- Tabla de Configuración por Escuela
CREATE TABLE configuraciones_escuela (
    id INT IDENTITY(1,1) PRIMARY KEY,
    escuela_id INT NOT NULL,
    clave NVARCHAR(100) NOT NULL,
    valor NVARCHAR(MAX),
    tipo_dato NVARCHAR(50) NOT NULL,
    descripcion NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (escuela_id) REFERENCES escuelas(id) ON DELETE CASCADE,
    CONSTRAINT uk_escuela_clave UNIQUE (escuela_id, clave)
);
GO

CREATE NONCLUSTERED INDEX idx_is_deleted ON configuraciones_escuela(is_deleted);
GO

-- Tabla de Parámetros del Sistema
CREATE TABLE parametros_sistema (
    id INT IDENTITY(1,1) PRIMARY KEY,
    clave NVARCHAR(100) UNIQUE NOT NULL,
    valor NVARCHAR(MAX),
    tipo_dato NVARCHAR(50) NOT NULL,
    descripcion NVARCHAR(MAX),
    editable BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT
);
GO

CREATE NONCLUSTERED INDEX idx_is_deleted ON parametros_sistema(is_deleted);
GO

-- Tabla de Preferencias de Usuario
CREATE TABLE preferencias_usuario (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    clave NVARCHAR(100) NOT NULL,
    valor NVARCHAR(MAX),
    tipo_dato NVARCHAR(50) NOT NULL,
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    CONSTRAINT uk_usuario_clave UNIQUE (usuario_id, clave)
);
GO

CREATE NONCLUSTERED INDEX idx_is_deleted ON preferencias_usuario(is_deleted);
GO

-- ============================================
-- MÓDULO 13: TAREAS
-- ============================================

-- Tabla de Tareas
CREATE TABLE tareas (
    id INT IDENTITY(1,1) PRIMARY KEY,
    maestro_id INT NOT NULL,
    grupo_id INT NOT NULL,
    materia_id INT NOT NULL,
    titulo NVARCHAR(200) NOT NULL,
    descripcion NVARCHAR(MAX) NOT NULL,
    fecha_asignacion DATE NOT NULL,
    fecha_entrega DATE NOT NULL,
    puntos DECIMAL(5, 2) DEFAULT 0,
    tipo_tarea NVARCHAR(50),
    archivos_adjuntos NVARCHAR(MAX) CHECK (ISJSON(archivos_adjuntos) = 1),
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (maestro_id) REFERENCES maestros(id) ON DELETE NO ACTION,
    FOREIGN KEY (grupo_id) REFERENCES grupos(id) ON DELETE NO ACTION,
    FOREIGN KEY (materia_id) REFERENCES materias(id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX idx_grupo ON tareas(grupo_id);
CREATE NONCLUSTERED INDEX idx_maestro ON tareas(maestro_id);
CREATE NONCLUSTERED INDEX idx_fecha_entrega ON tareas(fecha_entrega);
CREATE NONCLUSTERED INDEX idx_is_deleted ON tareas(is_deleted);
GO

-- Tabla de Entregas de Tareas
CREATE TABLE tareas_entregas (
    id INT IDENTITY(1,1) PRIMARY KEY,
    tarea_id INT NOT NULL,
    alumno_id INT NOT NULL,
    fecha_entrega DATETIME2,
    calificacion DECIMAL(5, 2),
    comentarios NVARCHAR(MAX),
    archivos_adjuntos NVARCHAR(MAX) CHECK (ISJSON(archivos_adjuntos) = 1),
    estatus NVARCHAR(20) NOT NULL DEFAULT 'Pendiente',
    created_at DATETIME2 DEFAULT GETDATE(),
    created_by INT,
    updated_at DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    is_deleted BIT DEFAULT 0,
    deleted_at DATETIME2,
    deleted_by INT,
    FOREIGN KEY (tarea_id) REFERENCES tareas(id) ON DELETE NO ACTION,
    FOREIGN KEY (alumno_id) REFERENCES alumnos(id) ON DELETE NO ACTION,
    CONSTRAINT uk_tarea_alumno UNIQUE (tarea_id, alumno_id)
);
GO

CREATE NONCLUSTERED INDEX idx_alumno ON tareas_entregas(alumno_id);
CREATE NONCLUSTERED INDEX idx_estatus ON tareas_entregas(estatus);
CREATE NONCLUSTERED INDEX idx_is_deleted ON tareas_entregas(is_deleted);
GO

-- ============================================
-- MENSAJE DE CONFIRMACIÓN
-- ============================================
PRINT 'Base de datos SQL Server creada exitosamente';
GO
