using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoriasRecurso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Color = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icono = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Orden = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CantidadRecursos = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasRecurso", x => x.Id);
                    table.CheckConstraint("CK_CategoriasRecurso_CantidadRecursos", "`CantidadRecursos` >= 0");
                    table.CheckConstraint("CK_CategoriasRecurso_Orden", "`Orden` >= 0");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConfiguracionesEscuela",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    NombreInstitucion = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreCorto = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lema = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mision = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Vision = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valores = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ciudad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodigoPostal = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pais = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelefonoAlternativo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SitioWeb = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogoPequenoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColorPrimario = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false, defaultValue: "#1976D2")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColorSecundario = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false, defaultValue: "#424242")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColorAcento = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false, defaultValue: "#FF9800")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImagenFondoLoginUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SistemaCalificacion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Numerico")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CalificacionMinimaAprobatoria = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 6.0m),
                    CalificacionMaxima = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 10.0m),
                    DecimalesCalificacion = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    PeriodosPorCiclo = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    DuracionClaseMinutos = table.Column<int>(type: "int", nullable: false, defaultValue: 50),
                    DuracionRecesoMinutos = table.Column<int>(type: "int", nullable: false, defaultValue: 20),
                    PorcentajeMinimoAsistencia = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 80.0m),
                    PermiteReprobacion = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    MaximaMateriasReprobadas = table.Column<int>(type: "int", nullable: true),
                    NotificacionesEmailHabilitadas = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    NotificacionesSMSHabilitadas = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    NotificacionesPushHabilitadas = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    NotificarCalificacionesAutomaticamente = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    NotificarAsistenciaAutomaticamente = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    NotificarTareasAutomaticamente = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    NotificarEventosAutomaticamente = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    RequiereCambioPasswordPrimerLogin = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    DiasVigenciaPassword = table.Column<int>(type: "int", nullable: false, defaultValue: 90),
                    LongitudMinimaPassword = table.Column<int>(type: "int", nullable: false, defaultValue: 8),
                    RequiereMayusculasPassword = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    RequiereNumerosPassword = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    RequiereCaracteresEspecialesPassword = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IntentosLoginFallidosAntesBloQueo = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    MinutosBloqueoPorIntentosFallidos = table.Column<int>(type: "int", nullable: false, defaultValue: 15),
                    Autenticacion2FactoresHabilitada = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    TiempoSesionMinutos = table.Column<int>(type: "int", nullable: false, defaultValue: 60),
                    FormatoPredeterminadoReportes = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "PDF")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncluirLogoEnReportes = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    IncluirFirmaDigitalEnDocumentos = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    PlantillaBoletaPredeterminadaId = table.Column<int>(type: "int", nullable: true),
                    PlantillaConstanciaPredeterminadaId = table.Column<int>(type: "int", nullable: true),
                    ModuloPagosHabilitado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    PermitePagosEnLinea = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    ProveedorPagos = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MonedaPredeterminada = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "MXN")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SimboloMoneda = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "$")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiasToleranciaParaPagos = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    PorcentajeRecargoMora = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ZonaHoraria = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, defaultValue: "America/Mexico_City")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdiomaPredeterminado = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "es-MX")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormatoFecha = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "dd/MM/yyyy")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormatoHora = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "HH:mm")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrimerDiaSemana = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    GoogleApiKey = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IntegracionGoogleClassroomHabilitada = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IntegracionMicrosoftTeamsHabilitada = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IntegracionZoomHabilitada = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    ConfiguracionSMTP = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConfiguracionSMS = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoPlan = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Basic")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaExpiracionLicencia = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LimiteAlumnos = table.Column<int>(type: "int", nullable: true),
                    LimiteMaestros = table.Column<int>(type: "int", nullable: true),
                    LimiteAlmacenamientoGB = table.Column<int>(type: "int", nullable: true),
                    DatosAdicionales = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionesEscuela", x => x.Id);
                    table.CheckConstraint("CK_ConfiguracionesEscuela_Calificaciones", "`CalificacionMinimaAprobatoria` < `CalificacionMaxima`");
                    table.CheckConstraint("CK_ConfiguracionesEscuela_DecimalesCalificacion", "`DecimalesCalificacion` >= 0 AND `DecimalesCalificacion` <= 2");
                    table.CheckConstraint("CK_ConfiguracionesEscuela_DuracionClase", "`DuracionClaseMinutos` >= 30 AND `DuracionClaseMinutos` <= 180");
                    table.CheckConstraint("CK_ConfiguracionesEscuela_IntentosLogin", "`IntentosLoginFallidosAntesBloQueo` >= 3 AND `IntentosLoginFallidosAntesBloQueo` <= 10");
                    table.CheckConstraint("CK_ConfiguracionesEscuela_Limites", "(`LimiteAlumnos` IS NULL OR `LimiteAlumnos` > 0) AND (`LimiteMaestros` IS NULL OR `LimiteMaestros` > 0) AND (`LimiteAlmacenamientoGB` IS NULL OR `LimiteAlmacenamientoGB` > 0)");
                    table.CheckConstraint("CK_ConfiguracionesEscuela_LongitudPassword", "`LongitudMinimaPassword` >= 6 AND `LongitudMinimaPassword` <= 32");
                    table.CheckConstraint("CK_ConfiguracionesEscuela_PeriodosPorCiclo", "`PeriodosPorCiclo` >= 1 AND `PeriodosPorCiclo` <= 6");
                    table.CheckConstraint("CK_ConfiguracionesEscuela_PorcentajeAsistencia", "`PorcentajeMinimoAsistencia` >= 0 AND `PorcentajeMinimoAsistencia` <= 100");
                    table.CheckConstraint("CK_ConfiguracionesEscuela_PrimerDiaSemana", "`PrimerDiaSemana` >= 0 AND `PrimerDiaSemana` <= 6");
                    table.CheckConstraint("CK_ConfiguracionesEscuela_TiempoSesion", "`TiempoSesionMinutos` >= 15 AND `TiempoSesionMinutos` <= 480");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Escuelas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RazonSocial = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RFC = table.Column<string>(type: "varchar(13)", maxLength: 13, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ciudad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodigoPostal = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pais = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelefonoAlternativo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SitioWeb = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoPlan = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    MaxAlumnos = table.Column<int>(type: "int", nullable: true),
                    MaxMaestros = table.Column<int>(type: "int", nullable: true),
                    EspacioAlmacenamiento = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escuelas", x => x.Id);
                    table.CheckConstraint("CK_Escuelas_EspacioAlmacenamiento", "`EspacioAlmacenamiento` IS NULL OR `EspacioAlmacenamiento` > 0");
                    table.CheckConstraint("CK_Escuelas_MaxAlumnos", "`MaxAlumnos` IS NULL OR `MaxAlumnos` > 0");
                    table.CheckConstraint("CK_Escuelas_MaxMaestros", "`MaxMaestros` IS NULL OR `MaxMaestros` > 0");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Insignias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icono = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Criterios = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rareza = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PuntosOtorgados = table.Column<int>(type: "int", nullable: false),
                    Requisitos = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsRecurrente = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insignias", x => x.Id);
                    table.CheckConstraint("CK_Insignias_PuntosOtorgados", "`PuntosOtorgados` >= 0");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlantillasDocumento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDocumento = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContenidoHtml = table.Column<string>(type: "LONGTEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstilosCSS = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VariablesDisponibles = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TamanioPagina = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "A4")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Orientacion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Vertical")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MargenSuperior = table.Column<int>(type: "int", nullable: false, defaultValue: 20),
                    MargenInferior = table.Column<int>(type: "int", nullable: false, defaultValue: 20),
                    MargenIzquierdo = table.Column<int>(type: "int", nullable: false, defaultValue: 20),
                    MargenDerecho = table.Column<int>(type: "int", nullable: false, defaultValue: 20),
                    TieneEncabezado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    EncabezadoHtml = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AlturaEncabezado = table.Column<int>(type: "int", nullable: true),
                    TienePiePagina = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    PiePaginaHtml = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AlturaPiePagina = table.Column<int>(type: "int", nullable: true),
                    TieneMarcaAgua = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    TextoMarcaAgua = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImagenMarcaAguaUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OpacidadMarcaAgua = table.Column<int>(type: "int", nullable: true),
                    EsPlantillaPorDefecto = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RequiereFirmaDigital = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RequiereFolio = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    PrefijoFolio = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConsecutivoFolio = table.Column<int>(type: "int", nullable: true),
                    Activa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Version = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "1.0")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NotasVersion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Categoria = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tags = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VecesUsada = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    FechaUltimaGeneracion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantillasDocumento", x => x.Id);
                    table.CheckConstraint("CK_PlantillasDocumento_ConsecutivoFolio", "`ConsecutivoFolio` IS NULL OR `ConsecutivoFolio` >= 1");
                    table.CheckConstraint("CK_PlantillasDocumento_Margenes", "`MargenSuperior` >= 0 AND `MargenInferior` >= 0 AND `MargenIzquierdo` >= 0 AND `MargenDerecho` >= 0");
                    table.CheckConstraint("CK_PlantillasDocumento_OpacidadMarcaAgua", "`OpacidadMarcaAgua` IS NULL OR (`OpacidadMarcaAgua` >= 0 AND `OpacidadMarcaAgua` <= 100)");
                    table.CheckConstraint("CK_PlantillasDocumento_VecesUsada", "`VecesUsada` >= 0");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Autor = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISBN = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Editorial = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnioPublicacion = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoriaId = table.Column<int>(type: "int", nullable: true),
                    CodigoClasificacion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NumeroPaginas = table.Column<int>(type: "int", nullable: true),
                    Edicion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Idioma = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Español")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CantidadTotal = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CantidadDisponible = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CantidadPrestada = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CantidadExtraviada = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CantidadDaniada = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Ubicacion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estante = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Disponible")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisponiblePrestamo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    ImagenPortadaUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecursoDigitalUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CalificacionPromedio = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    VecesPrestado = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Popularidad = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PrecioAdquisicion = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    FechaAdquisicion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Proveedor = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notas = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.Id);
                    table.CheckConstraint("CK_Libros_AnioPublicacion", "`AnioPublicacion` IS NULL OR (`AnioPublicacion` >= 1000 AND `AnioPublicacion` <= 2100 + 1)");
                    table.CheckConstraint("CK_Libros_CalificacionPromedio", "`CalificacionPromedio` IS NULL OR (`CalificacionPromedio` >= 1 AND `CalificacionPromedio` <= 5)");
                    table.CheckConstraint("CK_Libros_CantidadDisponible", "`CantidadDisponible` >= 0");
                    table.CheckConstraint("CK_Libros_CantidadesInventario", "`CantidadDisponible` + `CantidadPrestada` + `CantidadExtraviada` + `CantidadDaniada` = `CantidadTotal`");
                    table.CheckConstraint("CK_Libros_CantidadPrestada", "`CantidadPrestada` >= 0");
                    table.CheckConstraint("CK_Libros_CantidadTotal", "`CantidadTotal` >= 0");
                    table.CheckConstraint("CK_Libros_PrecioAdquisicion", "`PrecioAdquisicion` IS NULL OR `PrecioAdquisicion` >= 0");
                    table.ForeignKey(
                        name: "FK_Libros_CategoriasRecurso_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriasRecurso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CiclosEscolares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaInicio = table.Column<DateTime>(type: "date", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "date", nullable: true),
                    EsActual = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiclosEscolares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CiclosEscolares_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Clave = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icono = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Color = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Area = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NivelDificultad = table.Column<int>(type: "int", nullable: true),
                    RequiereMateriales = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    MaterialesRequeridos = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequiereInstalacionesEspeciales = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    InstalacionesRequeridas = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Objetivos = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Competencias = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContenidoTematico = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bibliografia = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id);
                    table.CheckConstraint("CK_Materias_NivelDificultad", "`NivelDificultad` IS NULL OR (`NivelDificultad` >= 1 AND `NivelDificultad` <= 5)");
                    table.ForeignKey(
                        name: "FK_Materias_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NivelesEducativos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Abreviatura = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    EdadMinima = table.Column<int>(type: "int", nullable: true),
                    EdadMaxima = table.Column<int>(type: "int", nullable: true),
                    DuracionAños = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelesEducativos", x => x.Id);
                    table.CheckConstraint("CK_NivelesEducativos_DuracionAños", "`DuracionAños` IS NULL OR `DuracionAños` > 0");
                    table.CheckConstraint("CK_NivelesEducativos_Edades", "`EdadMinima` IS NULL OR `EdadMaxima` IS NULL OR `EdadMinima` <= `EdadMaxima`");
                    table.CheckConstraint("CK_NivelesEducativos_EdadMaxima", "`EdadMaxima` IS NULL OR `EdadMaxima` >= 0");
                    table.CheckConstraint("CK_NivelesEducativos_EdadMinima", "`EdadMinima` IS NULL OR `EdadMinima` >= 0");
                    table.CheckConstraint("CK_NivelesEducativos_Orden", "`Orden` >= 0");
                    table.ForeignKey(
                        name: "FK_NivelesEducativos_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rol = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoPaterno = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoMaterno = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelefonoEmergencia = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FotoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaNacimiento = table.Column<DateTime>(type: "DATE", nullable: true),
                    Genero = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    UltimoAcceso = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    TokenRecuperacion = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TokenExpiracion = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    IntentosFallidos = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BloqueadoHasta = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PeriodosEvaluacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    CicloEscolarId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    FechaInicio = table.Column<DateTime>(type: "DATE", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "DATE", nullable: false),
                    FechaLimiteCaptura = table.Column<DateTime>(type: "DATE", nullable: true),
                    FechaPublicacion = table.Column<DateTime>(type: "DATE", nullable: true),
                    Porcentaje = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TipoPeriodo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CalificacionesDefinitivas = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    PermiteRecalificacion = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaLimiteRecalificacion = table.Column<DateTime>(type: "DATE", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodosEvaluacion", x => x.Id);
                    table.CheckConstraint("CK_PeriodosEvaluacion_FechaLimiteCaptura", "`FechaLimiteCaptura` IS NULL OR `FechaLimiteCaptura` >= `FechaFin`");
                    table.CheckConstraint("CK_PeriodosEvaluacion_FechaPublicacion", "`FechaPublicacion` IS NULL OR `FechaPublicacion` >= `FechaFin`");
                    table.CheckConstraint("CK_PeriodosEvaluacion_Fechas", "`FechaInicio` < `FechaFin`");
                    table.CheckConstraint("CK_PeriodosEvaluacion_Numero", "`Numero` > 0");
                    table.CheckConstraint("CK_PeriodosEvaluacion_Porcentaje", "`Porcentaje` >= 0 AND `Porcentaje` <= 100");
                    table.ForeignKey(
                        name: "FK_PeriodosEvaluacion_CiclosEscolares_CicloEscolarId",
                        column: x => x.CicloEscolarId,
                        principalTable: "CiclosEscolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PeriodosEvaluacion_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Grados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    NivelEducativoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    EdadRecomendada = table.Column<int>(type: "int", nullable: true),
                    CapacidadMaximaPorGrupo = table.Column<int>(type: "int", nullable: true),
                    HorasSemanales = table.Column<int>(type: "int", nullable: true),
                    Requisitos = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grados", x => x.Id);
                    table.CheckConstraint("CK_Grados_CapacidadMaximaPorGrupo", "`CapacidadMaximaPorGrupo` IS NULL OR `CapacidadMaximaPorGrupo` > 0");
                    table.CheckConstraint("CK_Grados_EdadRecomendada", "`EdadRecomendada` IS NULL OR (`EdadRecomendada` >= 0 AND `EdadRecomendada` <= 25)");
                    table.CheckConstraint("CK_Grados_HorasSemanales", "`HorasSemanales` IS NULL OR `HorasSemanales` > 0");
                    table.CheckConstraint("CK_Grados_Orden", "`Orden` >= 0");
                    table.ForeignKey(
                        name: "FK_Grados_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grados_NivelesEducativos_NivelEducativoId",
                        column: x => x.NivelEducativoId,
                        principalTable: "NivelesEducativos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Alumnos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    Matricula = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CURP = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoPaterno = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoMaterno = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaNacimiento = table.Column<DateTime>(type: "DATE", nullable: false),
                    Genero = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FotoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoSangre = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Alergias = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CondicionesMedicas = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Medicamentos = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactoEmergenciaNombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactoEmergenciaTelefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactoEmergenciaRelacion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaIngreso = table.Column<DateTime>(type: "DATE", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "DATE", nullable: true),
                    MotivoBaja = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumnos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alumnos_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumnos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Dispositivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeviceName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SO = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Navegador = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TokenFCM = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IpUltimaConexion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UltimaActividad = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dispositivos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    PlantillaDocumentoId = table.Column<int>(type: "int", nullable: true),
                    TipoDocumento = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Titulo = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Folio = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoEntidad = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntidadRelacionadaId = table.Column<int>(type: "int", nullable: true),
                    NombreEntidadRelacionada = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContenidoHtml = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreArchivo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TamanioArchivo = table.Column<long>(type: "bigint", nullable: true),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaVigencia = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Borrador")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CorreosEnvio = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TieneFirmaDigital = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    HashFirma = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaFirma = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FirmadoPorId = table.Column<int>(type: "int", nullable: true),
                    CertificadoFirma = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeneradoPorId = table.Column<int>(type: "int", nullable: false),
                    GeneradoAutomaticamente = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Descripcion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tags = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatosAdicionales = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CantidadDescargas = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    FechaUltimaDescarga = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EsPublico = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Archivado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaArchivado = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                    table.CheckConstraint("CK_Documentos_CantidadDescargas", "`CantidadDescargas` >= 0");
                    table.CheckConstraint("CK_Documentos_FechaVencimiento", "`FechaVencimiento` IS NULL OR (`FechaVigencia` IS NOT NULL AND `FechaVencimiento` >= `FechaVigencia`)");
                    table.CheckConstraint("CK_Documentos_FirmaDigital", "(`TieneFirmaDigital` = 0) OR (`TieneFirmaDigital` = 1 AND `HashFirma` IS NOT NULL)");
                    table.CheckConstraint("CK_Documentos_TamanioArchivo", "`TamanioArchivo` IS NULL OR `TamanioArchivo` >= 0");
                    table.ForeignKey(
                        name: "FK_Documentos_PlantillasDocumento_PlantillaDocumentoId",
                        column: x => x.PlantillaDocumentoId,
                        principalTable: "PlantillasDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Documentos_Usuarios_FirmadoPorId",
                        column: x => x.FirmadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Documentos_Usuarios_GeneradoPorId",
                        column: x => x.GeneradoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TodoElDia = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    GruposAfectadosJson = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AplicaATodos = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Ubicacion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecordatorioMinutos = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prioridad = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "Normal")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreadoPorId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    RecordatoriosEnviados = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaEnvioRecordatorios = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EsRecurrente = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    ConfiguracionRecurrencia = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoAdjuntoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoAdjuntoNombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                    table.CheckConstraint("CK_Eventos_Fechas", "`FechaFin` IS NULL OR `FechaFin` >= `FechaInicio`");
                    table.CheckConstraint("CK_Eventos_RecordatorioMinutos", "`RecordatorioMinutos` IS NULL OR `RecordatorioMinutos` >= 0");
                    table.ForeignKey(
                        name: "FK_Eventos_Usuarios_CreadoPorId",
                        column: x => x.CreadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LogsAuditoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    NombreUsuario = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailUsuario = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoAccion = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaHora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EntidadAfectada = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntidadAfectadaId = table.Column<int>(type: "int", nullable: true),
                    TipoEntidad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValoresAnteriores = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValoresNuevos = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CamposModificados = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DireccionIP = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAgent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Navegador = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SistemaOperativo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dispositivo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Exitoso = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CodigoResultado = table.Column<int>(type: "int", nullable: true),
                    MensajeError = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StackTrace = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DuracionMs = table.Column<int>(type: "int", nullable: true),
                    Modulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Funcionalidad = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Controlador = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Metodo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatosAdicionales = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Severidad = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tags = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAuditoria", x => x.Id);
                    table.CheckConstraint("CK_LogsAuditoria_CodigoResultado", "`CodigoResultado` IS NULL OR (`CodigoResultado` >= 100 AND `CodigoResultado` <= 599)");
                    table.CheckConstraint("CK_LogsAuditoria_DuracionMs", "`DuracionMs` IS NULL OR `DuracionMs` >= 0");
                    table.ForeignKey(
                        name: "FK_LogsAuditoria_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Maestros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    NumeroEmpleado = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaIngreso = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TipoContrato = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Salario = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CedulaProfesional = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Especialidad = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TituloAcademico = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Universidad = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AñoGraduacion = table.Column<int>(type: "int", nullable: true),
                    AñosExperiencia = table.Column<int>(type: "int", nullable: true),
                    Certificaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Capacitaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Idiomas = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HorarioAtencion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisponibleExtracurriculares = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maestros", x => x.Id);
                    table.CheckConstraint("CK_Maestros_AñoGraduacion", "`AñoGraduacion` IS NULL OR (`AñoGraduacion` >= 1900 AND `AñoGraduacion` <= 2100)");
                    table.CheckConstraint("CK_Maestros_AñosExperiencia", "`AñosExperiencia` IS NULL OR `AñosExperiencia` >= 0");
                    table.CheckConstraint("CK_Maestros_Fechas", "`FechaBaja` IS NULL OR `FechaIngreso` IS NULL OR (`FechaBaja` >= `FechaIngreso`)");
                    table.CheckConstraint("CK_Maestros_Salario", "`Salario` IS NULL OR `Salario` >= 0");
                    table.ForeignKey(
                        name: "FK_Maestros_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maestros_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioDestinatarioId = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prioridad = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "Normal")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Titulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mensaje = table.Column<string>(type: "LONGTEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UrlAccion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnviadoPorId = table.Column<int>(type: "int", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaProgramada = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaLectura = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Leida = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Canal = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "Sistema")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Metadata = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icono = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Color = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReproducirSonido = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Usuarios_EnviadoPorId",
                        column: x => x.EnviadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Usuarios_UsuarioDestinatarioId",
                        column: x => x.UsuarioDestinatarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Padres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Ocupacion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LugarTrabajo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelefonoTrabajo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DireccionTrabajo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Puesto = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NivelEstudios = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Carrera = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstadoCivil = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AceptaSMS = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    AceptaEmail = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    AceptaPush = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Padres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Padres_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Padres_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ParametrosSistema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: true),
                    Clave = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor = table.Column<string>(type: "LONGTEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDato = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "String")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Categoria = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorPredeterminado = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValoresPermitidos = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorMinimo = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ValorMaximo = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ExpresionValidacion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsGlobal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    EsConfigurable = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    EsVisible = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    EsSoloLectura = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RequiereReinicio = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    EsSensible = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    EstaEncriptado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Grupo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Orden = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Etiquetas = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Unidad = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UrlAyuda = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    FechaUltimoCambio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioUltimoCambioId = table.Column<int>(type: "int", nullable: true),
                    ValorAnterior = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HabilitarCache = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    TiempoCacheMinutos = table.Column<int>(type: "int", nullable: true, defaultValue: 60),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametrosSistema", x => x.Id);
                    table.CheckConstraint("CK_ParametrosSistema_EsGlobal", "(`EsGlobal` = 0 AND `EscuelaId` IS NOT NULL) OR (`EsGlobal` = 1 AND `EscuelaId` IS NULL)");
                    table.CheckConstraint("CK_ParametrosSistema_Orden", "`Orden` >= 0");
                    table.CheckConstraint("CK_ParametrosSistema_TiempoCache", "`TiempoCacheMinutos` IS NULL OR `TiempoCacheMinutos` > 0");
                    table.CheckConstraint("CK_ParametrosSistema_ValorMinMax", "`ValorMinimo` IS NULL OR `ValorMaximo` IS NULL OR `ValorMinimo` <= `ValorMaximo`");
                    table.ForeignKey(
                        name: "FK_ParametrosSistema_Usuarios_UsuarioUltimoCambioId",
                        column: x => x.UsuarioUltimoCambioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PreferenciasUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor = table.Column<string>(type: "LONGTEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, defaultValue: "Personalizacion")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Categoria = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Grupo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDato = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "String")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorPredeterminado = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsSincronizable = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    FechaUltimaSincronizacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DispositivoOrigen = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HashSincronizacion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Alcance = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Usuario")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsPreferenciaSistema = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    EsPrivada = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Activa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    FechaUltimoCambio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ValorAnterior = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequiereValidacion = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    ExpresionValidacion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValoresPermitidos = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Etiquetas = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Orden = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Icono = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferenciasUsuario", x => x.Id);
                    table.CheckConstraint("CK_PreferenciasUsuario_AlcanceEscuela", "(`Alcance` != 'Escuela') OR (`Alcance` = 'Escuela' AND `EscuelaId` IS NOT NULL)");
                    table.CheckConstraint("CK_PreferenciasUsuario_AlcanceGlobal", "(`Alcance` != 'Global') OR (`Alcance` = 'Global' AND `EscuelaId` IS NULL)");
                    table.CheckConstraint("CK_PreferenciasUsuario_Orden", "`Orden` >= 0");
                    table.CheckConstraint("CK_PreferenciasUsuario_UsuarioId", "`UsuarioId` > 0");
                    table.ForeignKey(
                        name: "FK_PreferenciasUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReportesPersonalizados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoReporte = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConsultaSQL = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConfiguracionJSON = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrigenDatos = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "SQL")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StoredProcedure = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParametrosJSON = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequiereParametros = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    ParametrosObligatorios = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormatoSalida = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "PDF")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormatosAdicionales = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlantillaHTML = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConfiguracionColumnas = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TieneGraficas = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    ConfiguracionGraficas = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProgramacionAutomatica = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Frecuencia = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpresionCron = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiaMes = table.Column<int>(type: "int", nullable: true),
                    DiaSemana = table.Column<int>(type: "int", nullable: true),
                    HoraEjecucion = table.Column<int>(type: "int", nullable: true),
                    ProximaEjecucion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UltimaEjecucion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EnvioAutomatico = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CorreosDestinatarios = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AsuntoCorreo = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CuerpoCorreo = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FiltrosPredeterminados = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrdenamientoPredeterminado = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermiteOrdenamientoDinamico = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    LimiteRegistros = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HabilitarCache = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    TiempoCacheMinutos = table.Column<int>(type: "int", nullable: true),
                    TimeoutSegundos = table.Column<int>(type: "int", nullable: true),
                    EsPrivado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RolesPermitidos = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequiereAprobacion = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    EsReporteSistema = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    VecesEjecutado = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    FechaUltimaEjecucionManual = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TiempoPromedioEjecucion = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Categoria = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tags = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icono = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Orden = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportesPersonalizados", x => x.Id);
                    table.CheckConstraint("CK_ReportesPersonalizados_DiaMes", "`DiaMes` IS NULL OR (`DiaMes` >= 1 AND `DiaMes` <= 31)");
                    table.CheckConstraint("CK_ReportesPersonalizados_DiaSemana", "`DiaSemana` IS NULL OR (`DiaSemana` >= 0 AND `DiaSemana` <= 6)");
                    table.CheckConstraint("CK_ReportesPersonalizados_HoraEjecucion", "`HoraEjecucion` IS NULL OR (`HoraEjecucion` >= 0 AND `HoraEjecucion` <= 23)");
                    table.CheckConstraint("CK_ReportesPersonalizados_LimiteRegistros", "`LimiteRegistros` >= 0");
                    table.CheckConstraint("CK_ReportesPersonalizados_TiempoCacheMinutos", "`TiempoCacheMinutos` IS NULL OR `TiempoCacheMinutos` > 0");
                    table.CheckConstraint("CK_ReportesPersonalizados_TiempoPromedioEjecucion", "`TiempoPromedioEjecucion` IS NULL OR `TiempoPromedioEjecucion` >= 0");
                    table.CheckConstraint("CK_ReportesPersonalizados_TimeoutSegundos", "`TimeoutSegundos` IS NULL OR `TimeoutSegundos` > 0");
                    table.CheckConstraint("CK_ReportesPersonalizados_VecesEjecutado", "`VecesEjecutado` >= 0");
                    table.ForeignKey(
                        name: "FK_ReportesPersonalizados_Usuarios_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sincronizaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    NombreUsuario = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DispositivoId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreDispositivo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDispositivo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SistemaOperativo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VersionCliente = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Manual")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Bidireccional")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DuracionMs = table.Column<int>(type: "int", nullable: true),
                    UltimaSincronizacionExitosa = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EntidadesSincronizadas = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalEntidades = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RegistrosCreados = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RegistrosActualizados = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RegistrosEliminados = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RegistrosSinCambios = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CantidadErrores = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TamanioDatos = table.Column<long>(type: "bigint", nullable: true),
                    TuvoErrores = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    MensajeError = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DetalleErrores = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StackTrace = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HashVerificacion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HashCliente = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VerificacionExitosa = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CantidadConflictos = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DetalleConflictos = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstrategiaResolucion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DireccionIP = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatosAdicionales = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModoSincronizacion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Incremental")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prioridad = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    Reintentos = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sincronizaciones", x => x.Id);
                    table.CheckConstraint("CK_Sincronizaciones_Contadores", "`RegistrosCreados` >= 0 AND `RegistrosActualizados` >= 0 AND `RegistrosEliminados` >= 0 AND `RegistrosSinCambios` >= 0");
                    table.CheckConstraint("CK_Sincronizaciones_DuracionMs", "`DuracionMs` IS NULL OR `DuracionMs` >= 0");
                    table.CheckConstraint("CK_Sincronizaciones_Errores", "`CantidadErrores` >= 0 AND `CantidadConflictos` >= 0");
                    table.CheckConstraint("CK_Sincronizaciones_FechaFin", "`FechaFin` IS NULL OR `FechaFin` >= `FechaInicio`");
                    table.CheckConstraint("CK_Sincronizaciones_Prioridad", "`Prioridad` >= 1 AND `Prioridad` <= 10");
                    table.CheckConstraint("CK_Sincronizaciones_Reintentos", "`Reintentos` >= 0");
                    table.CheckConstraint("CK_Sincronizaciones_TamanioDatos", "`TamanioDatos` IS NULL OR `TamanioDatos` >= 0");
                    table.CheckConstraint("CK_Sincronizaciones_TotalEntidades", "`TotalEntidades` >= 0");
                    table.ForeignKey(
                        name: "FK_Sincronizaciones_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConceptosPago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MontoBase = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Recurrente = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Periodicidad = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Codigo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AplicaDescuentos = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    PorcentajeMaximoDescuento = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    TieneMora = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    PorcentajeMora = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    DiasGracia = table.Column<int>(type: "int", nullable: true),
                    NivelEducativoId = table.Column<int>(type: "int", nullable: true),
                    GradoId = table.Column<int>(type: "int", nullable: true),
                    CicloEscolarId = table.Column<int>(type: "int", nullable: false),
                    CuentaContable = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoriaFiscal = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notas = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptosPago", x => x.Id);
                    table.CheckConstraint("CK_ConceptosPago_DiasGracia", "`DiasGracia` IS NULL OR `DiasGracia` >= 0");
                    table.CheckConstraint("CK_ConceptosPago_MontoBase", "`MontoBase` >= 0");
                    table.CheckConstraint("CK_ConceptosPago_PorcentajeDescuento", "`PorcentajeMaximoDescuento` IS NULL OR (`PorcentajeMaximoDescuento` >= 0 AND `PorcentajeMaximoDescuento` <= 100)");
                    table.CheckConstraint("CK_ConceptosPago_PorcentajeMora", "`PorcentajeMora` IS NULL OR `PorcentajeMora` >= 0");
                    table.ForeignKey(
                        name: "FK_ConceptosPago_CiclosEscolares_CicloEscolarId",
                        column: x => x.CicloEscolarId,
                        principalTable: "CiclosEscolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConceptosPago_Grados_GradoId",
                        column: x => x.GradoId,
                        principalTable: "Grados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConceptosPago_NivelesEducativos_NivelEducativoId",
                        column: x => x.NivelEducativoId,
                        principalTable: "NivelesEducativos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GradoMaterias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    GradoId = table.Column<int>(type: "int", nullable: false),
                    MateriaId = table.Column<int>(type: "int", nullable: false),
                    HorasSemanales = table.Column<int>(type: "int", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: true),
                    Obligatoria = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    PorcentajePeso = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradoMaterias", x => x.Id);
                    table.CheckConstraint("CK_GradoMaterias_HorasSemanales", "`HorasSemanales` IS NULL OR `HorasSemanales` > 0");
                    table.CheckConstraint("CK_GradoMaterias_Orden", "`Orden` IS NULL OR `Orden` >= 0");
                    table.CheckConstraint("CK_GradoMaterias_PorcentajePeso", "`PorcentajePeso` IS NULL OR (`PorcentajePeso` >= 0 AND `PorcentajePeso` <= 100)");
                    table.ForeignKey(
                        name: "FK_GradoMaterias_Grados_GradoId",
                        column: x => x.GradoId,
                        principalTable: "Grados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradoMaterias_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AlumnoPuntos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    PeriodoEscolarId = table.Column<int>(type: "int", nullable: true),
                    CicloEscolarId = table.Column<int>(type: "int", nullable: false),
                    PuntosTotales = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PuntosPeriodoActual = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PuntosAcademicos = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PuntosConducta = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PuntosDeportivos = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PuntosCulturales = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PuntosSociales = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PuntosAsistencia = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PuntosExtra = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NivelActual = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ExperienciaActual = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ExperienciaSiguienteNivel = table.Column<int>(type: "int", nullable: false, defaultValue: 100),
                    TituloNivel = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Principiante")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColorNivel = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "#4CAF50")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RankingGrupo = table.Column<int>(type: "int", nullable: true),
                    TotalAlumnosGrupo = table.Column<int>(type: "int", nullable: true),
                    RankingGrado = table.Column<int>(type: "int", nullable: true),
                    TotalAlumnosGrado = table.Column<int>(type: "int", nullable: true),
                    RankingEscuela = table.Column<int>(type: "int", nullable: true),
                    TotalAlumnosEscuela = table.Column<int>(type: "int", nullable: true),
                    CambioRanking = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RachaAsistencia = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RachaBuenaConducta = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RachaTareas = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MejorRachaAsistencia = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MejorRachaConducta = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MejorRachaTareas = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PromedioPuntosDiario = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    PromedioPuntosSemanal = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    Tendencia = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Estable")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PorcentajeMejora = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    UltimaActualizacionPuntos = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaUltimoNivel = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaUltimoLogro = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaReinicioPeriodo = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NotificacionesActivas = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    MostrarEnRankings = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    AvatarUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lema = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlumnoPuntos", x => x.Id);
                    table.CheckConstraint("CK_AlumnoPuntos_ExperienciaActual", "`ExperienciaActual` >= 0");
                    table.CheckConstraint("CK_AlumnoPuntos_NivelActual", "`NivelActual` >= 1");
                    table.CheckConstraint("CK_AlumnoPuntos_Promedios", "`PromedioPuntosDiario` >= 0 AND `PromedioPuntosSemanal` >= 0");
                    table.CheckConstraint("CK_AlumnoPuntos_PuntosTotales", "`PuntosTotales` >= 0");
                    table.CheckConstraint("CK_AlumnoPuntos_Rachas", "`RachaAsistencia` >= 0 AND `RachaBuenaConducta` >= 0 AND `RachaTareas` >= 0");
                    table.CheckConstraint("CK_AlumnoPuntos_Rankings", "(`RankingGrupo` IS NULL OR `RankingGrupo` > 0) AND (`RankingGrado` IS NULL OR `RankingGrado` > 0) AND (`RankingEscuela` IS NULL OR `RankingEscuela` > 0)");
                    table.ForeignKey(
                        name: "FK_AlumnoPuntos_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlumnoPuntos_CiclosEscolares_CicloEscolarId",
                        column: x => x.CicloEscolarId,
                        principalTable: "CiclosEscolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EstadosCuenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    CicloEscolarId = table.Column<int>(type: "int", nullable: false),
                    TotalCargos = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    TotalDescuentos = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    TotalMora = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    TotalPagos = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    SaldoPendiente = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    SaldoAFavor = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    CargosPendientes = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CargosPagados = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CargosVencidos = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CargosParciales = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalCargosCantidad = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalPagosCantidad = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TieneAdeudos = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    TieneCargosVencidos = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    AlCorriente = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    FechaUltimoCargo = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaUltimoPago = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaProximoVencimiento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequiereAtencion = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    NotasAtencion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCuenta", x => x.Id);
                    table.CheckConstraint("CK_EstadosCuenta_Contadores", "`CargosPendientes` >= 0 AND `CargosPagados` >= 0 AND `CargosVencidos` >= 0 AND `CargosParciales` >= 0");
                    table.CheckConstraint("CK_EstadosCuenta_SaldoAFavor", "`SaldoAFavor` >= 0");
                    table.CheckConstraint("CK_EstadosCuenta_SaldoPendiente", "`SaldoPendiente` >= 0");
                    table.CheckConstraint("CK_EstadosCuenta_TotalCargos", "`TotalCargos` >= 0");
                    table.CheckConstraint("CK_EstadosCuenta_TotalDescuentos", "`TotalDescuentos` >= 0 AND `TotalDescuentos` <= `TotalCargos`");
                    table.CheckConstraint("CK_EstadosCuenta_TotalPagos", "`TotalPagos` >= 0");
                    table.ForeignKey(
                        name: "FK_EstadosCuenta_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EstadosCuenta_CiclosEscolares_CicloEscolarId",
                        column: x => x.CicloEscolarId,
                        principalTable: "CiclosEscolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExpedientesMedicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    TipoSangre = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Peso = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Estatura = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    IMC = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Alergias = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CondicionesMedicas = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedicamentosRegulares = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Restricciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequiereAtencionEspecial = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DetallesAtencionEspecial = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactoEmergenciaNombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactoEmergenciaTelefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactoEmergenciaTelefonoAlt = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactoEmergenciaParentesco = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TieneSeguro = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    SeguroNombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeguroNumeroPoliza = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeguroVigencia = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SeguroTelefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedicoNombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedicoEspecialidad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedicoTelefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedicoDireccion = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VacunacionCompleta = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    VacunacionObservaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaUltimaRevision = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpedienteCompleto = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpedientesMedicos", x => x.Id);
                    table.CheckConstraint("CK_ExpedientesMedicos_Estatura", "`Estatura` IS NULL OR `Estatura` > 0");
                    table.CheckConstraint("CK_ExpedientesMedicos_IMC", "`IMC` IS NULL OR `IMC` > 0");
                    table.CheckConstraint("CK_ExpedientesMedicos_Peso", "`Peso` IS NULL OR `Peso` > 0");
                    table.ForeignKey(
                        name: "FK_ExpedientesMedicos_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mensajes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    EmisorId = table.Column<int>(type: "int", nullable: false),
                    ReceptorId = table.Column<int>(type: "int", nullable: false),
                    AlumnoRelacionadoId = table.Column<int>(type: "int", nullable: true),
                    Asunto = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contenido = table.Column<string>(type: "LONGTEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Leido = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaLectura = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ArchivoAdjuntoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoAdjuntoNombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoAdjuntoTamano = table.Column<long>(type: "bigint", nullable: true),
                    ArchivoAdjuntoTipo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MensajePadreId = table.Column<int>(type: "int", nullable: true),
                    EliminadoPorEmisor = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    EliminadoPorReceptor = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Importante = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Archivado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensajes", x => x.Id);
                    table.CheckConstraint("CK_Mensajes_Emisor_Receptor", "`EmisorId` <> `ReceptorId`");
                    table.ForeignKey(
                        name: "FK_Mensajes_Alumnos_AlumnoRelacionadoId",
                        column: x => x.AlumnoRelacionadoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Mensajes_Mensajes_MensajePadreId",
                        column: x => x.MensajePadreId,
                        principalTable: "Mensajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mensajes_Usuarios_EmisorId",
                        column: x => x.EmisorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mensajes_Usuarios_ReceptorId",
                        column: x => x.ReceptorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CambiosEntidad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: true),
                    LogAuditoriaId = table.Column<int>(type: "int", nullable: true),
                    NombreEntidad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntidadId = table.Column<int>(type: "int", nullable: false),
                    NombreCampo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreDescriptivo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDato = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorAnterior = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorNuevo = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorAnteriorFormateado = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorNuevoFormateado = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    NombreUsuario = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCambio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EsCampoSensible = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Categoria = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Etiquetas = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notas = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CambiosEntidad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CambiosEntidad_LogsAuditoria_LogAuditoriaId",
                        column: x => x.LogAuditoriaId,
                        principalTable: "LogsAuditoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CambiosEntidad_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    GradoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CicloEscolarId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CapacidadMaxima = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    MaestroTitularId = table.Column<int>(type: "int", nullable: true),
                    Aula = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Turno = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HoraInicio = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    HoraFin = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    DiasClase = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, defaultValue: "Lunes a Viernes")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.Id);
                    table.CheckConstraint("CK_Grupos_CapacidadMaxima", "`CapacidadMaxima` > 0");
                    table.CheckConstraint("CK_Grupos_Horarios", "`HoraInicio` IS NULL OR `HoraFin` IS NULL OR `HoraInicio` < `HoraFin`");
                    table.ForeignKey(
                        name: "FK_Grupos_CiclosEscolares_CicloEscolarId",
                        column: x => x.CicloEscolarId,
                        principalTable: "CiclosEscolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grupos_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grupos_Grados_GradoId",
                        column: x => x.GradoId,
                        principalTable: "Grados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grupos_Maestros_MaestroTitularId",
                        column: x => x.MaestroTitularId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: true),
                    MaestroId = table.Column<int>(type: "int", nullable: true),
                    RegistradoPorId = table.Column<int>(type: "int", nullable: false),
                    FechaPrestamo = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaDevolucionProgramada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaDevolucionReal = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ObservacionesDevolucion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MontoMulta = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    MultaPagada = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaPagoMulta = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DevueltoPorId = table.Column<int>(type: "int", nullable: true),
                    CondicionDevolucion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReportadoExtraviado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaReporteExtravio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReportadoDaniado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CantidadRenovaciones = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    FechaUltimaRenovacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Folio = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrestamoUrgente = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.Id);
                    table.CheckConstraint("CK_Prestamos_CantidadRenovaciones", "`CantidadRenovaciones` >= 0");
                    table.CheckConstraint("CK_Prestamos_FechaDevolucionProgramada", "`FechaDevolucionProgramada` > `FechaPrestamo`");
                    table.CheckConstraint("CK_Prestamos_FechaDevolucionReal", "`FechaDevolucionReal` IS NULL OR `FechaDevolucionReal` >= `FechaPrestamo`");
                    table.CheckConstraint("CK_Prestamos_MontoMulta", "`MontoMulta` >= 0");
                    table.CheckConstraint("CK_Prestamos_SolicitanteUnico", "(`AlumnoId` IS NOT NULL AND `MaestroId` IS NULL) OR (`AlumnoId` IS NULL AND `MaestroId` IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Prestamos_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prestamos_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prestamos_Maestros_MaestroId",
                        column: x => x.MaestroId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prestamos_Usuarios_DevueltoPorId",
                        column: x => x.DevueltoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Prestamos_Usuarios_RegistradoPorId",
                        column: x => x.RegistradoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NotificacionSmsLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NotificacionId = table.Column<int>(type: "int", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mensaje = table.Column<string>(type: "LONGTEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Proveedor = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estatus = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "Pendiente")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SidProveedor = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Costo = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    Moneda = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "MXN")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaEntrega = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ErrorMensaje = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodigoError = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NumeroIntentos = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    FechaUltimoIntento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Metadata = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificacionSmsLog", x => x.Id);
                    table.CheckConstraint("CK_NotificacionSmsLog_NumeroIntentos", "`NumeroIntentos` >= 0");
                    table.ForeignKey(
                        name: "FK_NotificacionSmsLog_Notificaciones_NotificacionId",
                        column: x => x.NotificacionId,
                        principalTable: "Notificaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AlumnoPadres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    PadreId = table.Column<int>(type: "int", nullable: false),
                    Relacion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsTutorPrincipal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    AutorizadoRecoger = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    RecibeNotificaciones = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    ViveConAlumno = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlumnoPadres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlumnoPadres_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlumnoPadres_Padres_PadreId",
                        column: x => x.PadreId,
                        principalTable: "Padres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cargos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    ConceptoPagoId = table.Column<int>(type: "int", nullable: false),
                    CicloEscolarId = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    PorcentajeDescuento = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MontoFinal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Mora = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    MontoPagado = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    SaldoPendiente = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaPagoCompleto = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Estatus = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "Pendiente")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotivoCancelacion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCancelacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CanceladoPorId = table.Column<int>(type: "int", nullable: true),
                    NumeroRecibo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReferenciaExterna = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeneradoAutomaticamente = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    MesCorrespondiente = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargos", x => x.Id);
                    table.CheckConstraint("CK_Cargos_Descuento", "`Descuento` >= 0 AND `Descuento` <= `Monto`");
                    table.CheckConstraint("CK_Cargos_FechaVencimiento", "`FechaVencimiento` >= `FechaCreacion`");
                    table.CheckConstraint("CK_Cargos_Monto", "`Monto` >= 0");
                    table.CheckConstraint("CK_Cargos_MontoFinal", "`MontoFinal` >= 0");
                    table.CheckConstraint("CK_Cargos_MontoPagado", "`MontoPagado` >= 0");
                    table.CheckConstraint("CK_Cargos_Mora", "`Mora` >= 0");
                    table.CheckConstraint("CK_Cargos_SaldoPendiente", "`SaldoPendiente` >= 0");
                    table.ForeignKey(
                        name: "FK_Cargos_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cargos_CiclosEscolares_CicloEscolarId",
                        column: x => x.CicloEscolarId,
                        principalTable: "CiclosEscolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cargos_ConceptosPago_ConceptoPagoId",
                        column: x => x.ConceptoPagoId,
                        principalTable: "ConceptosPago",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cargos_Usuarios_CanceladoPorId",
                        column: x => x.CanceladoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AlumnosInsignias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AlumnoPuntosId = table.Column<int>(type: "int", nullable: false),
                    InsigniaId = table.Column<int>(type: "int", nullable: false),
                    FechaObtencion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Motivo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsFavorita = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    VecesObtenida = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlumnosInsignias", x => x.Id);
                    table.CheckConstraint("CK_AlumnosInsignias_VecesObtenida", "`VecesObtenida` >= 1");
                    table.ForeignKey(
                        name: "FK_AlumnosInsignias_AlumnoPuntos_AlumnoPuntosId",
                        column: x => x.AlumnoPuntosId,
                        principalTable: "AlumnoPuntos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlumnosInsignias_Insignias_InsigniaId",
                        column: x => x.InsigniaId,
                        principalTable: "Insignias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistorialPuntos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AlumnoPuntosId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PuntosObtenidos = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrigenId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoOrigen = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PuntosAcumulados = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialPuntos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialPuntos_AlumnoPuntos_AlumnoPuntosId",
                        column: x => x.AlumnoPuntosId,
                        principalTable: "AlumnoPuntos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Alergias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    ExpedienteMedicoId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreAlergeno = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gravedad = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sintomas = table.Column<string>(type: "LONGTEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PuedeSerAnafilactica = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaDiagnostico = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MedicoDiagnostico = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoPrueba = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TratamientoRecomendado = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedicamentoEmergencia = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequiereAutoinyector = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    InstruccionesEmergencia = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    FechaSuperacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alergias", x => x.Id);
                    table.CheckConstraint("CK_Alergias_FechaSuperacion", "`FechaSuperacion` IS NULL OR (`FechaSuperacion` >= `FechaDiagnostico` AND `Activa` = 0)");
                    table.ForeignKey(
                        name: "FK_Alergias_ExpedientesMedicos_ExpedienteMedicoId",
                        column: x => x.ExpedienteMedicoId,
                        principalTable: "ExpedientesMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistorialMedico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    ExpedienteMedicoId = table.Column<int>(type: "int", nullable: false),
                    TipoIncidente = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaIncidente = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Descripcion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sintomas = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LugarIncidente = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OcurrioEnEscuela = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Diagnostico = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gravedad = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TratamientoAplicado = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedicoAtencion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EspecialidadMedico = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LugarAtencion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstitucionAtencion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequirioHospitalizacion = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaIngresoHospital = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaAltaHospital = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DiasHospitalizado = table.Column<int>(type: "int", nullable: true),
                    MotivoHospitalizacion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedicamentosRecetados = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProcedimientosRealizados = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequirioCirugia = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DescripcionCirugia = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequiereSeguimiento = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaProximaConsulta = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IndicacionesSeguimiento = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CasoCerrado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaCierreCaso = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PadresNotificados = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaNotificacionPadres = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PersonaQueNotifico = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedioNotificacion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentosUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegistradoPorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialMedico", x => x.Id);
                    table.CheckConstraint("CK_HistorialMedico_DiasHospitalizado", "`DiasHospitalizado` IS NULL OR `DiasHospitalizado` >= 0");
                    table.CheckConstraint("CK_HistorialMedico_FechaAltaHospital", "`FechaAltaHospital` IS NULL OR `FechaAltaHospital` >= `FechaIngresoHospital`");
                    table.CheckConstraint("CK_HistorialMedico_Hospitalizacion", "(`RequirioHospitalizacion` = 0) OR (`RequirioHospitalizacion` = 1 AND `FechaIngresoHospital` IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_HistorialMedico_ExpedientesMedicos_ExpedienteMedicoId",
                        column: x => x.ExpedienteMedicoId,
                        principalTable: "ExpedientesMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialMedico_Usuarios_RegistradoPorId",
                        column: x => x.RegistradoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Medicamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    ExpedienteMedicoId = table.Column<int>(type: "int", nullable: false),
                    NombreMedicamento = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreGenerico = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dosis = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Frecuencia = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Via = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Indicacion = table.Column<string>(type: "LONGTEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedicoPrescriptor = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EspecialidadMedico = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CedulaMedico = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TratamientoCronico = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaSuspension = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MotivoSuspension = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstruccionesEspeciales = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Precauciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EfectosSecundarios = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdministrarEnEscuela = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    HorarioEscolar = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequiereSupervision = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    PuedeAutoAdministrar = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RecetaUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicamentos", x => x.Id);
                    table.CheckConstraint("CK_Medicamentos_FechaFin", "`FechaFin` IS NULL OR `FechaFin` >= `FechaInicio`");
                    table.CheckConstraint("CK_Medicamentos_FechaSuspension", "`FechaSuspension` IS NULL OR (`FechaSuspension` >= `FechaInicio` AND `Estado` = 'Suspendido')");
                    table.ForeignKey(
                        name: "FK_Medicamentos_ExpedientesMedicos_ExpedienteMedicoId",
                        column: x => x.ExpedienteMedicoId,
                        principalTable: "ExpedientesMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vacunas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    ExpedienteMedicoId = table.Column<int>(type: "int", nullable: false),
                    NombreVacuna = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dosis = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaAplicacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaProximaDosis = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Lote = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Marca = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCaducidad = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    InstitucionAplicacion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PersonalAplicacion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LugarAnatomico = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TuvoReacciones = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DescripcionReacciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ComprobanteUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacunas", x => x.Id);
                    table.CheckConstraint("CK_Vacunas_FechaCaducidad", "`FechaCaducidad` IS NULL OR `FechaCaducidad` >= `FechaAplicacion`");
                    table.CheckConstraint("CK_Vacunas_FechaProximaDosis", "`FechaProximaDosis` IS NULL OR `FechaProximaDosis` > `FechaAplicacion`");
                    table.ForeignKey(
                        name: "FK_Vacunas_ExpedientesMedicos_ExpedienteMedicoId",
                        column: x => x.ExpedienteMedicoId,
                        principalTable: "ExpedientesMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Asistencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "DATE", nullable: false),
                    Estatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HoraEntrada = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    HoraSalida = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    MinutosRetardo = table.Column<int>(type: "int", nullable: true),
                    Justificado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Motivo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JustificanteUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaJustificacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AproboJustificanteId = table.Column<int>(type: "int", nullable: true),
                    Observaciones = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PadresNotificados = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaNotificacionPadres = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RegistradoPor = table.Column<int>(type: "int", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FueModificada = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MotivoModificacion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencias", x => x.Id);
                    table.CheckConstraint("CK_Asistencias_Horarios", "`HoraEntrada` IS NULL OR `HoraSalida` IS NULL OR `HoraEntrada` < `HoraSalida`");
                    table.CheckConstraint("CK_Asistencias_MinutosRetardo", "`MinutosRetardo` IS NULL OR `MinutosRetardo` >= 0");
                    table.ForeignKey(
                        name: "FK_Asistencias_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asistencias_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Asistencias_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Asistencias_Usuarios_RegistradoPor",
                        column: x => x.RegistradoPor,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Calificaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    MateriaId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    PeriodoId = table.Column<int>(type: "int", nullable: false),
                    CalificacionNumerica = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: false),
                    CalificacionLetra = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aprobado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CalificacionMinima = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: true),
                    TipoEvaluacion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Peso = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: true),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fortalezas = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AreasOportunidad = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Recomendaciones = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCaptura = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CapturadoPor = table.Column<int>(type: "int", nullable: true),
                    FueModificada = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModificadoPor = table.Column<int>(type: "int", nullable: true),
                    MotivoModificacion = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsRecalificacion = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CalificacionOriginal = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: true),
                    FechaRecalificacion = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    TipoRecalificacion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bloqueada = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaBloqueo = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    VisibleParaPadres = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificaciones", x => x.Id);
                    table.CheckConstraint("CHK_Calificacion_Rango", "CalificacionNumerica >=0 AND CalificacionNumerica <=10");
                    table.CheckConstraint("CHK_Peso_Rango", "Peso IS NULL OR (Peso >=0 AND Peso <=100)");
                    table.ForeignKey(
                        name: "FK_Calificaciones_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Maestros_CapturadoPor",
                        column: x => x.CapturadoPor,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Calificaciones_PeriodosEvaluacion_PeriodoId",
                        column: x => x.PeriodoId,
                        principalTable: "PeriodosEvaluacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Comunicados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contenido = table.Column<string>(type: "LONGTEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Destinatarios = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GrupoId = table.Column<int>(type: "int", nullable: true),
                    ArchivoAdjuntoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoAdjuntoNombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoAdjuntoTamano = table.Column<long>(type: "bigint", nullable: true),
                    ArchivoAdjuntoTipo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublicadoPorId = table.Column<int>(type: "int", nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RequiereConfirmacion = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Prioridad = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "Normal")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Categoria = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermiteComentarios = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    TotalDestinatarios = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalLecturas = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalConfirmaciones = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comunicados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comunicados_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comunicados_Usuarios_PublicadoPorId",
                        column: x => x.PublicadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GrupoMateriaMaestros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    MateriaId = table.Column<int>(type: "int", nullable: false),
                    MaestroId = table.Column<int>(type: "int", nullable: false),
                    CicloEscolarId = table.Column<int>(type: "int", nullable: false),
                    Horario = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoMateriaMaestros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrupoMateriaMaestros_CiclosEscolares_CicloEscolarId",
                        column: x => x.CicloEscolarId,
                        principalTable: "CiclosEscolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrupoMateriaMaestros_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GrupoMateriaMaestros_Maestros_MaestroId",
                        column: x => x.MaestroId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrupoMateriaMaestros_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Inscripciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    CicloEscolarId = table.Column<int>(type: "int", nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NumeroLista = table.Column<int>(type: "int", nullable: true),
                    Estatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Inscrito")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PromedioFinal = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PromedioAcumulado = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Aprobado = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    MateriasReprobadas = table.Column<int>(type: "int", nullable: true),
                    LugarEnGrupo = table.Column<int>(type: "int", nullable: true),
                    TotalDiasClase = table.Column<int>(type: "int", nullable: true),
                    DiasAsistidos = table.Column<int>(type: "int", nullable: true),
                    DiasFaltados = table.Column<int>(type: "int", nullable: true),
                    DiasRetardo = table.Column<int>(type: "int", nullable: true),
                    PorcentajeAsistencia = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MotivoBaja = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GrupoAnteriorId = table.Column<int>(type: "int", nullable: true),
                    FechaCambioGrupo = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MotivoCambioGrupo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Becado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    TipoBeca = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PorcentajeBeca = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Repetidor = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscripciones", x => x.Id);
                    table.CheckConstraint("CK_Inscripciones_Asistencias", "(`TotalDiasClase` IS NULL OR `TotalDiasClase` >= 0) AND (`DiasAsistidos` IS NULL OR `DiasAsistidos` >= 0) AND (`DiasFaltados` IS NULL OR `DiasFaltados` >= 0) AND (`DiasRetardo` IS NULL OR `DiasRetardo` >= 0)");
                    table.CheckConstraint("CK_Inscripciones_Fechas", "`FechaInicio` IS NULL OR `FechaFin` IS NULL OR `FechaInicio` <= `FechaFin`");
                    table.CheckConstraint("CK_Inscripciones_LugarEnGrupo", "`LugarEnGrupo` IS NULL OR `LugarEnGrupo` > 0");
                    table.CheckConstraint("CK_Inscripciones_MateriasReprobadas", "`MateriasReprobadas` IS NULL OR `MateriasReprobadas` >= 0");
                    table.CheckConstraint("CK_Inscripciones_NumeroLista", "`NumeroLista` IS NULL OR `NumeroLista` > 0");
                    table.CheckConstraint("CK_Inscripciones_PorcentajeAsistencia", "`PorcentajeAsistencia` IS NULL OR (`PorcentajeAsistencia` >= 0 AND `PorcentajeAsistencia` <= 100)");
                    table.CheckConstraint("CK_Inscripciones_PorcentajeBeca", "`PorcentajeBeca` IS NULL OR (`PorcentajeBeca` >= 0 AND `PorcentajeBeca` <= 100)");
                    table.CheckConstraint("CK_Inscripciones_PromedioAcumulado", "`PromedioAcumulado` IS NULL OR (`PromedioAcumulado` >= 0 AND `PromedioAcumulado` <= 10)");
                    table.CheckConstraint("CK_Inscripciones_PromedioFinal", "`PromedioFinal` IS NULL OR (`PromedioFinal` >= 0 AND `PromedioFinal` <= 10)");
                    table.ForeignKey(
                        name: "FK_Inscripciones_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscripciones_CiclosEscolares_CicloEscolarId",
                        column: x => x.CicloEscolarId,
                        principalTable: "CiclosEscolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inscripciones_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inscripciones_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RegistrosConducta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    MaestroId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Categoria = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gravedad = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Titulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaHoraIncidente = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Lugar = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Puntos = table.Column<int>(type: "int", nullable: false),
                    Testigos = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EvidenciaUrls = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SancionId = table.Column<int>(type: "int", nullable: true),
                    AccionesTomadas = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PadresNotificados = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaNotificacionPadres = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MetodoNotificacion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RespuestaPadres = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequiereSeguimiento = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaSeguimiento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NotasSeguimiento = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PeriodoId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosConducta", x => x.Id);
                    table.CheckConstraint("CK_RegistrosConducta_FechaSeguimiento", "`FechaSeguimiento` IS NULL OR `FechaSeguimiento` >= `FechaHoraIncidente`");
                    table.ForeignKey(
                        name: "FK_RegistrosConducta_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrosConducta_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrosConducta_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RegistrosConducta_Maestros_MaestroId",
                        column: x => x.MaestroId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    MateriaId = table.Column<int>(type: "int", nullable: false),
                    MaestroId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaEntrega = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaLimiteTardia = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Tipo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorPuntos = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PermiteEntregaTardia = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PenalizacionTardia = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ArchivoAdjuntoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoAdjuntoNombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoAdjuntoTamano = table.Column<long>(type: "bigint", nullable: true),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.Id);
                    table.CheckConstraint("CK_Tareas_Fechas", "(`FechaEntrega` > `FechaAsignacion`)");
                    table.CheckConstraint("CK_Tareas_Penalizacion", "(`PenalizacionTardia` IS NULL OR (`PenalizacionTardia` >= 0 AND `PenalizacionTardia` <= 100))");
                    table.CheckConstraint("CK_Tareas_ValorPuntos", "(`ValorPuntos` > 0)");
                    table.ForeignKey(
                        name: "FK_Tareas_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tareas_Maestros_MaestroId",
                        column: x => x.MaestroId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tareas_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    CargoId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MetodoPago = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Referencia = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Banco = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UltimosDigitosTarjeta = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaPago = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaAplicacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FolioRecibo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SerieRecibo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReciboUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecibidoPorId = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cancelado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaCancelacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MotivoCancelacion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CanceladoPorId = table.Column<int>(type: "int", nullable: true),
                    Facturado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    UuidFactura = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FacturaXmlUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FacturaPdfUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaFacturacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReferenciaExterna = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatosAdicionales = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DireccionIp = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.CheckConstraint("CK_Pagos_FechaAplicacion", "`FechaAplicacion` IS NULL OR `FechaAplicacion` >= `FechaPago`");
                    table.CheckConstraint("CK_Pagos_Monto", "`Monto` > 0");
                    table.CheckConstraint("CK_Pagos_UltimosDigitos", "`UltimosDigitosTarjeta` IS NULL OR CHAR_LENGTH(`UltimosDigitosTarjeta`) = 4");
                    table.ForeignKey(
                        name: "FK_Pagos_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_Cargos_CargoId",
                        column: x => x.CargoId,
                        principalTable: "Cargos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_Usuarios_CanceladoPorId",
                        column: x => x.CanceladoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Pagos_Usuarios_RecibidoPorId",
                        column: x => x.RecibidoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComunicadoLecturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ComunicadoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FechaLectura = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Confirmado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaConfirmacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Comentario = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComunicadoLecturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComunicadoLecturas_Comunicados_ComunicadoId",
                        column: x => x.ComunicadoId,
                        principalTable: "Comunicados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComunicadoLecturas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sanciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    ConductaId = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaInicio = table.Column<DateTime>(type: "DATE", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "DATE", nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Motivo = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AutorizadoPor = table.Column<int>(type: "int", nullable: false),
                    FechaAutorizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Cumplida = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaCumplimiento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ObservacionesCumplimiento = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VerificadoPor = table.Column<int>(type: "int", nullable: true),
                    Apelada = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaApelacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MotivoApelacion = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResultadoApelacion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaResolucionApelacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PadresNotificados = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaNotificacionPadres = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MedioNotificacion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirmaEnterado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FechaFirmaEnterado = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DocumentoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sanciones", x => x.Id);
                    table.CheckConstraint("CK_Sanciones_FechaCumplimiento", "`FechaCumplimiento` IS NULL OR `FechaCumplimiento` >= `FechaInicio`");
                    table.CheckConstraint("CK_Sanciones_Fechas", "`FechaFin` IS NULL OR `FechaFin` >= `FechaInicio`");
                    table.ForeignKey(
                        name: "FK_Sanciones_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sanciones_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sanciones_RegistrosConducta_ConductaId",
                        column: x => x.ConductaId,
                        principalTable: "RegistrosConducta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Sanciones_Usuarios_AutorizadoPor",
                        column: x => x.AutorizadoPor,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TareaEntregas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    TareaId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    FechaEntrega = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ComentariosAlumno = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoNombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoTamano = table.Column<long>(type: "bigint", nullable: true),
                    Estatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsTardia = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    NumeroIntento = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Calificacion = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CalificacionOriginal = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PenalizacionAplicada = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Retroalimentacion = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RevisadoPorId = table.Column<int>(type: "int", nullable: true),
                    FechaRevision = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ArchivoRetroalimentacionUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivoRetroalimentacionNombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareaEntregas", x => x.Id);
                    table.CheckConstraint("CK_TareaEntregas_Calificacion", "`Calificacion` IS NULL OR (`Calificacion` >= 0 AND `Calificacion` <= 100)");
                    table.CheckConstraint("CK_TareaEntregas_CalificacionOriginal", "`CalificacionOriginal` IS NULL OR (`CalificacionOriginal` >= 0 AND `CalificacionOriginal` <= 100)");
                    table.CheckConstraint("CK_TareaEntregas_NumeroIntento", "`NumeroIntento` > 0");
                    table.CheckConstraint("CK_TareaEntregas_Penalizacion", "`PenalizacionAplicada` IS NULL OR `PenalizacionAplicada` >= 0");
                    table.ForeignKey(
                        name: "FK_TareaEntregas_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TareaEntregas_Maestros_RevisadoPorId",
                        column: x => x.RevisadoPorId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TareaEntregas_Tareas_TareaId",
                        column: x => x.TareaId,
                        principalTable: "Tareas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_Activa",
                table: "Alergias",
                column: "Activa");

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_EscuelaId",
                table: "Alergias",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_Expediente_Activa",
                table: "Alergias",
                columns: new[] { "ExpedienteMedicoId", "Activa" });

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_ExpedienteMedicoId",
                table: "Alergias",
                column: "ExpedienteMedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_Gravedad",
                table: "Alergias",
                column: "Gravedad");

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_PuedeSerAnafilactica",
                table: "Alergias",
                column: "PuedeSerAnafilactica");

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_Tipo",
                table: "Alergias",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_Tipo_Gravedad_Activa",
                table: "Alergias",
                columns: new[] { "Tipo", "Gravedad", "Activa" });

            migrationBuilder.CreateIndex(
                name: "IX_Alumno_Padre_Padre_Id",
                table: "AlumnoPadres",
                column: "PadreId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumno_Padre_Unique",
                table: "AlumnoPadres",
                columns: new[] { "AlumnoId", "PadreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_Alumno_Ciclo_Unique",
                table: "AlumnoPuntos",
                columns: new[] { "AlumnoId", "CicloEscolarId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_AlumnoId",
                table: "AlumnoPuntos",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_CicloEscolarId",
                table: "AlumnoPuntos",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_Escuela_Ciclo_Puntos",
                table: "AlumnoPuntos",
                columns: new[] { "EscuelaId", "CicloEscolarId", "PuntosTotales" });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_Escuela_CicloEscolarId",
                table: "AlumnoPuntos",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_Escuela_Ranking",
                table: "AlumnoPuntos",
                columns: new[] { "EscuelaId", "RankingEscuela" });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_EscuelaId",
                table: "AlumnoPuntos",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_Nivel_Puntos",
                table: "AlumnoPuntos",
                columns: new[] { "NivelActual", "PuntosTotales" });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_NivelActual",
                table: "AlumnoPuntos",
                column: "NivelActual");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_PeriodoEscolarId",
                table: "AlumnoPuntos",
                column: "PeriodoEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_PuntosTotales",
                table: "AlumnoPuntos",
                column: "PuntosTotales");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_RankingEscuela",
                table: "AlumnoPuntos",
                column: "RankingEscuela");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_RankingGrado",
                table: "AlumnoPuntos",
                column: "RankingGrado");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_RankingGrupo",
                table: "AlumnoPuntos",
                column: "RankingGrupo");

            migrationBuilder.CreateIndex(
                name: "IX_Alumno_Curp",
                table: "Alumnos",
                column: "CURP",
                unique: true,
                filter: "curp IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Alumno_Escuela_Estatus",
                table: "Alumnos",
                columns: new[] { "EscuelaId", "Estatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Alumno_Estatus",
                table: "Alumnos",
                column: "Estatus");

            migrationBuilder.CreateIndex(
                name: "IX_Alumno_Is_Deleted",
                table: "Alumnos",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Alumno_Matricula",
                table: "Alumnos",
                column: "Matricula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_UsuarioId",
                table: "Alumnos",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlumnosInsignias_AlumnoPuntos_Favorita",
                table: "AlumnosInsignias",
                columns: new[] { "AlumnoPuntosId", "EsFavorita" });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnosInsignias_AlumnoPuntos_Fecha",
                table: "AlumnosInsignias",
                columns: new[] { "AlumnoPuntosId", "FechaObtencion" });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnosInsignias_AlumnoPuntos_Insignia",
                table: "AlumnosInsignias",
                columns: new[] { "AlumnoPuntosId", "InsigniaId" });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnosInsignias_AlumnoPuntosId",
                table: "AlumnosInsignias",
                column: "AlumnoPuntosId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnosInsignias_EsFavorita",
                table: "AlumnosInsignias",
                column: "EsFavorita");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnosInsignias_FechaObtencion",
                table: "AlumnosInsignias",
                column: "FechaObtencion");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnosInsignias_Insignia_Fecha",
                table: "AlumnosInsignias",
                columns: new[] { "InsigniaId", "FechaObtencion" });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnosInsignias_InsigniaId",
                table: "AlumnosInsignias",
                column: "InsigniaId");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Alumno_Estatus",
                table: "Asistencias",
                columns: new[] { "AlumnoId", "Estatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Alumno_Fecha_Estatus",
                table: "Asistencias",
                columns: new[] { "AlumnoId", "Fecha", "Estatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Alumno_Fecha_Unique",
                table: "Asistencias",
                columns: new[] { "AlumnoId", "Fecha" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_AlumnoId",
                table: "Asistencias",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Escuela_Fecha_Estatus",
                table: "Asistencias",
                columns: new[] { "EscuelaId", "Fecha", "Estatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_EscuelaId",
                table: "Asistencias",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Estatus",
                table: "Asistencias",
                column: "Estatus");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Fecha",
                table: "Asistencias",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_FechaRegistro",
                table: "Asistencias",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Grupo_Fecha",
                table: "Asistencias",
                columns: new[] { "GrupoId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_GrupoId",
                table: "Asistencias",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Justificado",
                table: "Asistencias",
                column: "Justificado");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_RegistradoPor",
                table: "Asistencias",
                column: "RegistradoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Alumno",
                table: "Calificaciones",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Alumno_Materia_Periodo",
                table: "Calificaciones",
                columns: new[] { "AlumnoId", "MateriaId", "PeriodoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Bloqueada",
                table: "Calificaciones",
                column: "Bloqueada");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Grupo_Periodo",
                table: "Calificaciones",
                columns: new[] { "GrupoId", "PeriodoId" });

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Visible_Padres",
                table: "Calificaciones",
                column: "VisibleParaPadres");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_CapturadoPor",
                table: "Calificaciones",
                column: "CapturadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_EscuelaId",
                table: "Calificaciones",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_MateriaId",
                table: "Calificaciones",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_PeriodoId",
                table: "Calificaciones",
                column: "PeriodoId");

            migrationBuilder.CreateIndex(
                name: "IX_CambiosEntidad_Entidad_Campo_Fecha",
                table: "CambiosEntidad",
                columns: new[] { "NombreEntidad", "NombreCampo", "FechaCambio" });

            migrationBuilder.CreateIndex(
                name: "IX_CambiosEntidad_Entidad_EntidadId",
                table: "CambiosEntidad",
                columns: new[] { "NombreEntidad", "EntidadId" });

            migrationBuilder.CreateIndex(
                name: "IX_CambiosEntidad_Entidad_EntidadId_Fecha",
                table: "CambiosEntidad",
                columns: new[] { "NombreEntidad", "EntidadId", "FechaCambio" });

            migrationBuilder.CreateIndex(
                name: "IX_CambiosEntidad_EntidadId",
                table: "CambiosEntidad",
                column: "EntidadId");

            migrationBuilder.CreateIndex(
                name: "IX_CambiosEntidad_EsCampoSensible",
                table: "CambiosEntidad",
                column: "EsCampoSensible");

            migrationBuilder.CreateIndex(
                name: "IX_CambiosEntidad_EscuelaId",
                table: "CambiosEntidad",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_CambiosEntidad_FechaCambio",
                table: "CambiosEntidad",
                column: "FechaCambio");

            migrationBuilder.CreateIndex(
                name: "IX_CambiosEntidad_LogAuditoriaId",
                table: "CambiosEntidad",
                column: "LogAuditoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CambiosEntidad_NombreCampo",
                table: "CambiosEntidad",
                column: "NombreCampo");

            migrationBuilder.CreateIndex(
                name: "IX_CambiosEntidad_NombreEntidad",
                table: "CambiosEntidad",
                column: "NombreEntidad");

            migrationBuilder.CreateIndex(
                name: "IX_CambiosEntidad_UsuarioId",
                table: "CambiosEntidad",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_Alumno_Ciclo",
                table: "Cargos",
                columns: new[] { "AlumnoId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_Alumno_Estatus",
                table: "Cargos",
                columns: new[] { "AlumnoId", "Estatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_AlumnoId",
                table: "Cargos",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_CanceladoPorId",
                table: "Cargos",
                column: "CanceladoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_CicloEscolarId",
                table: "Cargos",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_ConceptoPagoId",
                table: "Cargos",
                column: "ConceptoPagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_Escuela_CicloEscolarId",
                table: "Cargos",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_Escuela_Fecha_Estatus",
                table: "Cargos",
                columns: new[] { "EscuelaId", "FechaVencimiento", "Estatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_EscuelaId",
                table: "Cargos",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_Estatus",
                table: "Cargos",
                column: "Estatus");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_FechaVencimiento",
                table: "Cargos",
                column: "FechaVencimiento");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_NumeroRecibo",
                table: "Cargos",
                column: "NumeroRecibo");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasRecurso_Activo",
                table: "CategoriasRecurso",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasRecurso_Codigo",
                table: "CategoriasRecurso",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasRecurso_Escuela_Activo_Orden",
                table: "CategoriasRecurso",
                columns: new[] { "EscuelaId", "Activo", "Orden" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasRecurso_EscuelaId",
                table: "CategoriasRecurso",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasRecurso_Nombre",
                table: "CategoriasRecurso",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_CiclosEscolares_Escuela_Actual",
                table: "CiclosEscolares",
                columns: new[] { "EscuelaId", "EsActual" });

            migrationBuilder.CreateIndex(
                name: "IX_CiclosEscolares_Escuela_Clave_Unique",
                table: "CiclosEscolares",
                columns: new[] { "EscuelaId", "Clave" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComunicadoLecturas_Comunicado_Confirmado",
                table: "ComunicadoLecturas",
                columns: new[] { "ComunicadoId", "Confirmado" });

            migrationBuilder.CreateIndex(
                name: "IX_ComunicadoLecturas_Comunicado_Usuario_Unique",
                table: "ComunicadoLecturas",
                columns: new[] { "ComunicadoId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComunicadoLecturas_ComunicadoId",
                table: "ComunicadoLecturas",
                column: "ComunicadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ComunicadoLecturas_UsuarioId",
                table: "ComunicadoLecturas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Comunicados_Activo",
                table: "Comunicados",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Comunicados_Destinatarios",
                table: "Comunicados",
                column: "Destinatarios");

            migrationBuilder.CreateIndex(
                name: "IX_Comunicados_Escuela_Activo_Fecha",
                table: "Comunicados",
                columns: new[] { "EscuelaId", "Activo", "FechaPublicacion" });

            migrationBuilder.CreateIndex(
                name: "IX_Comunicados_EscuelaId",
                table: "Comunicados",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Comunicados_FechaPublicacion",
                table: "Comunicados",
                column: "FechaPublicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Comunicados_GrupoId",
                table: "Comunicados",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Comunicados_PublicadoPorId",
                table: "Comunicados",
                column: "PublicadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_Activo",
                table: "ConceptosPago",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_CicloEscolarId",
                table: "ConceptosPago",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_Codigo",
                table: "ConceptosPago",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_Escuela_Activo",
                table: "ConceptosPago",
                columns: new[] { "EscuelaId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_Escuela_CicloEscolarId",
                table: "ConceptosPago",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_EscuelaId",
                table: "ConceptosPago",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_GradoId",
                table: "ConceptosPago",
                column: "GradoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_NivelEducativoId",
                table: "ConceptosPago",
                column: "NivelEducativoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_Recurrente_Periodicidad",
                table: "ConceptosPago",
                columns: new[] { "Recurrente", "Periodicidad" });

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_Tipo",
                table: "ConceptosPago",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesEscuela_EscuelaId_Unique",
                table: "ConfiguracionesEscuela",
                column: "EscuelaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesEscuela_FechaExpiracionLicencia",
                table: "ConfiguracionesEscuela",
                column: "FechaExpiracionLicencia");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesEscuela_NombreInstitucion",
                table: "ConfiguracionesEscuela",
                column: "NombreInstitucion");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesEscuela_TipoPlan",
                table: "ConfiguracionesEscuela",
                column: "TipoPlan");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivo_Device_Id",
                table: "Dispositivos",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivo_Usuario_Activo",
                table: "Dispositivos",
                columns: new[] { "UsuarioId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_Archivado",
                table: "Documentos",
                column: "Archivado");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_Escuela_Estado_Fecha",
                table: "Documentos",
                columns: new[] { "EscuelaId", "Estado", "FechaGeneracion" });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_Escuela_Tipo_Estado",
                table: "Documentos",
                columns: new[] { "EscuelaId", "TipoDocumento", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_EscuelaId",
                table: "Documentos",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_EsPublico",
                table: "Documentos",
                column: "EsPublico");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_Estado",
                table: "Documentos",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_FechaGeneracion",
                table: "Documentos",
                column: "FechaGeneracion");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_FechaVencimiento",
                table: "Documentos",
                column: "FechaVencimiento");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_FirmadoPorId",
                table: "Documentos",
                column: "FirmadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_Folio",
                table: "Documentos",
                column: "Folio");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_GeneradoPorId",
                table: "Documentos",
                column: "GeneradoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_PlantillaDocumentoId",
                table: "Documentos",
                column: "PlantillaDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_TieneFirmaDigital",
                table: "Documentos",
                column: "TieneFirmaDigital");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_TipoDocumento",
                table: "Documentos",
                column: "TipoDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_TipoEntidad_EntidadId",
                table: "Documentos",
                columns: new[] { "TipoEntidad", "EntidadRelacionadaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Escuelas_Activo",
                table: "Escuelas",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Escuelas_Codigo_Unique",
                table: "Escuelas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Escuelas_Email",
                table: "Escuelas",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Escuelas_RFC",
                table: "Escuelas",
                column: "RFC");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_AlCorriente",
                table: "EstadosCuenta",
                column: "AlCorriente");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_Alumno_Ciclo_Unique",
                table: "EstadosCuenta",
                columns: new[] { "AlumnoId", "CicloEscolarId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_AlumnoId",
                table: "EstadosCuenta",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_CicloEscolarId",
                table: "EstadosCuenta",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_Escuela_Ciclo_Adeudos",
                table: "EstadosCuenta",
                columns: new[] { "EscuelaId", "CicloEscolarId", "TieneAdeudos" });

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_Escuela_CicloEscolarId",
                table: "EstadosCuenta",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_EscuelaId",
                table: "EstadosCuenta",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_FechaActualizacion",
                table: "EstadosCuenta",
                column: "FechaActualizacion");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_RequiereAtencion",
                table: "EstadosCuenta",
                column: "RequiereAtencion");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_TieneAdeudos",
                table: "EstadosCuenta",
                column: "TieneAdeudos");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_TieneCargosVencidos",
                table: "EstadosCuenta",
                column: "TieneCargosVencidos");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_CreadoPorId",
                table: "Eventos",
                column: "CreadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_Escuela_Fecha_Activo",
                table: "Eventos",
                columns: new[] { "EscuelaId", "FechaInicio", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_EscuelaId",
                table: "Eventos",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_FechaInicio",
                table: "Eventos",
                column: "FechaInicio");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_FechaInicio_FechaFin",
                table: "Eventos",
                columns: new[] { "FechaInicio", "FechaFin" });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_Prioridad",
                table: "Eventos",
                column: "Prioridad");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_Recordatorios_Fecha",
                table: "Eventos",
                columns: new[] { "RecordatoriosEnviados", "FechaInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_Tipo",
                table: "Eventos",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicos_Activo",
                table: "ExpedientesMedicos",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicos_AlumnoId_Unique",
                table: "ExpedientesMedicos",
                column: "AlumnoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicos_Escuela_Activo",
                table: "ExpedientesMedicos",
                columns: new[] { "EscuelaId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicos_EscuelaId",
                table: "ExpedientesMedicos",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicos_FechaUltimaActualizacion",
                table: "ExpedientesMedicos",
                column: "FechaUltimaActualizacion");

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicos_RequiereAtencionEspecial",
                table: "ExpedientesMedicos",
                column: "RequiereAtencionEspecial");

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicos_SeguroVigencia",
                table: "ExpedientesMedicos",
                column: "SeguroVigencia");

            migrationBuilder.CreateIndex(
                name: "IX_ExpedientesMedicos_TieneSeguro",
                table: "ExpedientesMedicos",
                column: "TieneSeguro");

            migrationBuilder.CreateIndex(
                name: "IX_GradoMaterias_Escuela_Grado_Materia_Unique",
                table: "GradoMaterias",
                columns: new[] { "EscuelaId", "GradoId", "MateriaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GradoMaterias_EscuelaId",
                table: "GradoMaterias",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_GradoMaterias_Grado_Orden",
                table: "GradoMaterias",
                columns: new[] { "GradoId", "Orden" });

            migrationBuilder.CreateIndex(
                name: "IX_GradoMaterias_GradoId",
                table: "GradoMaterias",
                column: "GradoId");

            migrationBuilder.CreateIndex(
                name: "IX_GradoMaterias_MateriaId",
                table: "GradoMaterias",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_GradoMaterias_Obligatoria",
                table: "GradoMaterias",
                column: "Obligatoria");

            migrationBuilder.CreateIndex(
                name: "IX_Grados_Activo",
                table: "Grados",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Grados_Escuela_Activo",
                table: "Grados",
                columns: new[] { "EscuelaId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_Grados_Escuela_Nivel_Nombre_Unique",
                table: "Grados",
                columns: new[] { "EscuelaId", "NivelEducativoId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grados_Escuela_Nivel_Orden_Unique",
                table: "Grados",
                columns: new[] { "EscuelaId", "NivelEducativoId", "Orden" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grados_EscuelaId",
                table: "Grados",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Grados_Nivel_Activo_Orden",
                table: "Grados",
                columns: new[] { "NivelEducativoId", "Activo", "Orden" });

            migrationBuilder.CreateIndex(
                name: "IX_Grados_NivelEducativoId",
                table: "Grados",
                column: "NivelEducativoId");

            migrationBuilder.CreateIndex(
                name: "IX_Grados_Nombre",
                table: "Grados",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Grados_Orden",
                table: "Grados",
                column: "Orden");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMateriaMaestros_CicloEscolarId",
                table: "GrupoMateriaMaestros",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMateriaMaestros_Escuela_CicloEscolarId",
                table: "GrupoMateriaMaestros",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMateriaMaestros_Escuela_Grupo_Materia_Ciclo_Unique",
                table: "GrupoMateriaMaestros",
                columns: new[] { "EscuelaId", "GrupoId", "MateriaId", "CicloEscolarId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMateriaMaestros_EscuelaId",
                table: "GrupoMateriaMaestros",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMateriaMaestros_Grupo_Ciclo",
                table: "GrupoMateriaMaestros",
                columns: new[] { "GrupoId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMateriaMaestros_GrupoId",
                table: "GrupoMateriaMaestros",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMateriaMaestros_Maestro_Ciclo",
                table: "GrupoMateriaMaestros",
                columns: new[] { "MaestroId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMateriaMaestros_MaestroId",
                table: "GrupoMateriaMaestros",
                column: "MaestroId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMateriaMaestros_MateriaId",
                table: "GrupoMateriaMaestros",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_Activo",
                table: "Grupos",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_CicloEscolarId",
                table: "Grupos",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_Escuela_Ciclo_Activo",
                table: "Grupos",
                columns: new[] { "EscuelaId", "CicloEscolarId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_Escuela_CicloEscolarId",
                table: "Grupos",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_Escuela_Grado_Nombre_Ciclo_Unique",
                table: "Grupos",
                columns: new[] { "EscuelaId", "GradoId", "Nombre", "CicloEscolarId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_EscuelaId",
                table: "Grupos",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_Grado_Ciclo_Activo",
                table: "Grupos",
                columns: new[] { "GradoId", "CicloEscolarId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_GradoId",
                table: "Grupos",
                column: "GradoId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_MaestroTitularId",
                table: "Grupos",
                column: "MaestroTitularId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_Turno",
                table: "Grupos",
                column: "Turno");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_CasoCerrado",
                table: "HistorialMedico",
                column: "CasoCerrado");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_EscuelaId",
                table: "HistorialMedico",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_Expediente_Fecha",
                table: "HistorialMedico",
                columns: new[] { "ExpedienteMedicoId", "FechaIncidente" });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_ExpedienteMedicoId",
                table: "HistorialMedico",
                column: "ExpedienteMedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_FechaIncidente",
                table: "HistorialMedico",
                column: "FechaIncidente");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_FechaProximaConsulta",
                table: "HistorialMedico",
                column: "FechaProximaConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_Gravedad",
                table: "HistorialMedico",
                column: "Gravedad");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_OcurrioEnEscuela",
                table: "HistorialMedico",
                column: "OcurrioEnEscuela");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_RegistradoPorId",
                table: "HistorialMedico",
                column: "RegistradoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_RequiereSeguimiento",
                table: "HistorialMedico",
                column: "RequiereSeguimiento");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_RequirioHospitalizacion",
                table: "HistorialMedico",
                column: "RequirioHospitalizacion");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_Tipo_Gravedad_Fecha",
                table: "HistorialMedico",
                columns: new[] { "TipoIncidente", "Gravedad", "FechaIncidente" });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMedico_TipoIncidente",
                table: "HistorialMedico",
                column: "TipoIncidente");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPuntos_AlumnoPuntos_Categoria",
                table: "HistorialPuntos",
                columns: new[] { "AlumnoPuntosId", "Categoria" });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPuntos_AlumnoPuntos_Fecha",
                table: "HistorialPuntos",
                columns: new[] { "AlumnoPuntosId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPuntos_AlumnoPuntosId",
                table: "HistorialPuntos",
                column: "AlumnoPuntosId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPuntos_Categoria",
                table: "HistorialPuntos",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPuntos_Fecha",
                table: "HistorialPuntos",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPuntos_Fecha_Categoria",
                table: "HistorialPuntos",
                columns: new[] { "Fecha", "Categoria" });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPuntos_PuntosObtenidos",
                table: "HistorialPuntos",
                column: "PuntosObtenidos");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPuntos_TipoOrigen",
                table: "HistorialPuntos",
                column: "TipoOrigen");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPuntos_TipoOrigen_OrigenId",
                table: "HistorialPuntos",
                columns: new[] { "TipoOrigen", "OrigenId" });

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_Alumno_Ciclo",
                table: "Inscripciones",
                columns: new[] { "AlumnoId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_Alumno_Grupo_Ciclo_Unique",
                table: "Inscripciones",
                columns: new[] { "AlumnoId", "GrupoId", "CicloEscolarId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_AlumnoId",
                table: "Inscripciones",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_Becado",
                table: "Inscripciones",
                column: "Becado");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_CicloEscolarId",
                table: "Inscripciones",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_Escuela_Ciclo_Estatus",
                table: "Inscripciones",
                columns: new[] { "EscuelaId", "CicloEscolarId", "Estatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_Escuela_CicloEscolarId",
                table: "Inscripciones",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_EscuelaId",
                table: "Inscripciones",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_Estatus",
                table: "Inscripciones",
                column: "Estatus");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_FechaInscripcion",
                table: "Inscripciones",
                column: "FechaInscripcion");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_Grupo_Ciclo_Estatus",
                table: "Inscripciones",
                columns: new[] { "GrupoId", "CicloEscolarId", "Estatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_Grupo_NumeroLista",
                table: "Inscripciones",
                columns: new[] { "GrupoId", "NumeroLista" });

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_GrupoId",
                table: "Inscripciones",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_Activo",
                table: "Insignias",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_Escuela_Activo",
                table: "Insignias",
                columns: new[] { "EscuelaId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_Escuela_Nombre_Unique",
                table: "Insignias",
                columns: new[] { "EscuelaId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_Escuela_Tipo_Activo",
                table: "Insignias",
                columns: new[] { "EscuelaId", "Tipo", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_EscuelaId",
                table: "Insignias",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_Nombre",
                table: "Insignias",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_PuntosOtorgados",
                table: "Insignias",
                column: "PuntosOtorgados");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_Rareza",
                table: "Insignias",
                column: "Rareza");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_Tipo",
                table: "Insignias",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_Tipo_Rareza",
                table: "Insignias",
                columns: new[] { "Tipo", "Rareza" });

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Activo",
                table: "Libros",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_CategoriaId",
                table: "Libros",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_CodigoClasificacion",
                table: "Libros",
                column: "CodigoClasificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_DisponiblePrestamo",
                table: "Libros",
                column: "DisponiblePrestamo");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Escuela_Activo_Estado",
                table: "Libros",
                columns: new[] { "EscuelaId", "Activo", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Libros_EscuelaId",
                table: "Libros",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Estado",
                table: "Libros",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_ISBN",
                table: "Libros",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Tipo",
                table: "Libros",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Titulo",
                table: "Libros",
                column: "Titulo");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_DireccionIP",
                table: "LogsAuditoria",
                column: "DireccionIP");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Entidad_EntidadId",
                table: "LogsAuditoria",
                columns: new[] { "EntidadAfectada", "EntidadAfectadaId" });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_EntidadAfectada",
                table: "LogsAuditoria",
                column: "EntidadAfectada");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Escuela_Fecha",
                table: "LogsAuditoria",
                columns: new[] { "EscuelaId", "FechaHora" });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Escuela_Tipo_Fecha",
                table: "LogsAuditoria",
                columns: new[] { "EscuelaId", "TipoAccion", "FechaHora" });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_EscuelaId",
                table: "LogsAuditoria",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Exitoso",
                table: "LogsAuditoria",
                column: "Exitoso");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Exitoso_Fecha",
                table: "LogsAuditoria",
                columns: new[] { "Exitoso", "FechaHora" });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_FechaHora",
                table: "LogsAuditoria",
                column: "FechaHora");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Modulo",
                table: "LogsAuditoria",
                column: "Modulo");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Severidad",
                table: "LogsAuditoria",
                column: "Severidad");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_TipoAccion",
                table: "LogsAuditoria",
                column: "TipoAccion");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_TipoAccion_Fecha",
                table: "LogsAuditoria",
                columns: new[] { "TipoAccion", "FechaHora" });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_Usuario_Fecha",
                table: "LogsAuditoria",
                columns: new[] { "UsuarioId", "FechaHora" });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_UsuarioId",
                table: "LogsAuditoria",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Maestros_CedulaProfesional",
                table: "Maestros",
                column: "CedulaProfesional");

            migrationBuilder.CreateIndex(
                name: "IX_Maestros_Escuela_Estatus",
                table: "Maestros",
                columns: new[] { "EscuelaId", "Estatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Maestros_Escuela_NumeroEmpleado_Unique",
                table: "Maestros",
                columns: new[] { "EscuelaId", "NumeroEmpleado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maestros_Escuela_Usuario_Unique",
                table: "Maestros",
                columns: new[] { "EscuelaId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maestros_EscuelaId",
                table: "Maestros",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Maestros_Especialidad",
                table: "Maestros",
                column: "Especialidad");

            migrationBuilder.CreateIndex(
                name: "IX_Maestros_Estatus",
                table: "Maestros",
                column: "Estatus");

            migrationBuilder.CreateIndex(
                name: "IX_Maestros_NumeroEmpleado",
                table: "Maestros",
                column: "NumeroEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Maestros_UsuarioId",
                table: "Maestros",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materias_Activo",
                table: "Materias",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_Area",
                table: "Materias",
                column: "Area");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_Clave",
                table: "Materias",
                column: "Clave");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_Escuela_Activo",
                table: "Materias",
                columns: new[] { "EscuelaId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_Materias_Escuela_Area_Activo",
                table: "Materias",
                columns: new[] { "EscuelaId", "Area", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_Materias_Escuela_Clave_Unique",
                table: "Materias",
                columns: new[] { "EscuelaId", "Clave" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materias_EscuelaId",
                table: "Materias",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_Nombre",
                table: "Materias",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_Tipo",
                table: "Materias",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_AdministrarEnEscuela",
                table: "Medicamentos",
                column: "AdministrarEnEscuela");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_EscuelaId",
                table: "Medicamentos",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_Estado",
                table: "Medicamentos",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_Estado_FechaFin",
                table: "Medicamentos",
                columns: new[] { "Estado", "FechaFin" });

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_Expediente_Estado",
                table: "Medicamentos",
                columns: new[] { "ExpedienteMedicoId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_ExpedienteMedicoId",
                table: "Medicamentos",
                column: "ExpedienteMedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_FechaFin",
                table: "Medicamentos",
                column: "FechaFin");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_FechaInicio",
                table: "Medicamentos",
                column: "FechaInicio");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_NombreMedicamento",
                table: "Medicamentos",
                column: "NombreMedicamento");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_TratamientoCronico",
                table: "Medicamentos",
                column: "TratamientoCronico");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_AlumnoRelacionadoId",
                table: "Mensajes",
                column: "AlumnoRelacionadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_Emisor_Receptor_Fecha",
                table: "Mensajes",
                columns: new[] { "EmisorId", "ReceptorId", "FechaEnvio" });

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_EmisorId",
                table: "Mensajes",
                column: "EmisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_EscuelaId",
                table: "Mensajes",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_FechaEnvio",
                table: "Mensajes",
                column: "FechaEnvio");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_MensajePadreId",
                table: "Mensajes",
                column: "MensajePadreId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_Receptor_Leido",
                table: "Mensajes",
                columns: new[] { "ReceptorId", "Leido" });

            migrationBuilder.CreateIndex(
                name: "IX_NivelesEducativos_Activo",
                table: "NivelesEducativos",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_NivelesEducativos_Escuela_Activo_Orden",
                table: "NivelesEducativos",
                columns: new[] { "EscuelaId", "Activo", "Orden" });

            migrationBuilder.CreateIndex(
                name: "IX_NivelesEducativos_Escuela_Nombre_Unique",
                table: "NivelesEducativos",
                columns: new[] { "EscuelaId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NivelesEducativos_EscuelaId",
                table: "NivelesEducativos",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_NivelesEducativos_Nombre",
                table: "NivelesEducativos",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_NivelesEducativos_Orden",
                table: "NivelesEducativos",
                column: "Orden");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_EnviadoPorId",
                table: "Notificaciones",
                column: "EnviadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_EscuelaId",
                table: "Notificaciones",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_FechaEnvio",
                table: "Notificaciones",
                column: "FechaEnvio");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_FechaProgramada",
                table: "Notificaciones",
                column: "FechaProgramada");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_Prioridad",
                table: "Notificaciones",
                column: "Prioridad");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_Tipo",
                table: "Notificaciones",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_Usuario_Fecha",
                table: "Notificaciones",
                columns: new[] { "UsuarioDestinatarioId", "FechaEnvio" });

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_Usuario_Leida",
                table: "Notificaciones",
                columns: new[] { "UsuarioDestinatarioId", "Leida" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionSmsLog_Estatus",
                table: "NotificacionSmsLog",
                column: "Estatus");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionSmsLog_Estatus_FechaIntento",
                table: "NotificacionSmsLog",
                columns: new[] { "Estatus", "FechaUltimoIntento" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionSmsLog_FechaEnvio",
                table: "NotificacionSmsLog",
                column: "FechaEnvio");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionSmsLog_NotificacionId",
                table: "NotificacionSmsLog",
                column: "NotificacionId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionSmsLog_Telefono",
                table: "NotificacionSmsLog",
                column: "Telefono");

            migrationBuilder.CreateIndex(
                name: "IX_Padres_Escuela_AceptaEmail",
                table: "Padres",
                columns: new[] { "EscuelaId", "AceptaEmail" });

            migrationBuilder.CreateIndex(
                name: "IX_Padres_Escuela_AceptaPush",
                table: "Padres",
                columns: new[] { "EscuelaId", "AceptaPush" });

            migrationBuilder.CreateIndex(
                name: "IX_Padres_Escuela_AceptaSMS",
                table: "Padres",
                columns: new[] { "EscuelaId", "AceptaSMS" });

            migrationBuilder.CreateIndex(
                name: "IX_Padres_Escuela_Usuario_Unique",
                table: "Padres",
                columns: new[] { "EscuelaId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Padres_EscuelaId",
                table: "Padres",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Padres_NivelEstudios",
                table: "Padres",
                column: "NivelEstudios");

            migrationBuilder.CreateIndex(
                name: "IX_Padres_Ocupacion",
                table: "Padres",
                column: "Ocupacion");

            migrationBuilder.CreateIndex(
                name: "IX_Padres_UsuarioId",
                table: "Padres",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_Alumno_Fecha",
                table: "Pagos",
                columns: new[] { "AlumnoId", "FechaPago" });

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_AlumnoId",
                table: "Pagos",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_Cancelado",
                table: "Pagos",
                column: "Cancelado");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CanceladoPorId",
                table: "Pagos",
                column: "CanceladoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CargoId",
                table: "Pagos",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_Escuela_Fecha_Cancelado",
                table: "Pagos",
                columns: new[] { "EscuelaId", "FechaPago", "Cancelado" });

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_EscuelaId",
                table: "Pagos",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_Facturado",
                table: "Pagos",
                column: "Facturado");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_FechaPago",
                table: "Pagos",
                column: "FechaPago");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_FolioRecibo_Unique",
                table: "Pagos",
                column: "FolioRecibo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_MetodoPago",
                table: "Pagos",
                column: "MetodoPago");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_RecibidoPorId",
                table: "Pagos",
                column: "RecibidoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_UuidFactura",
                table: "Pagos",
                column: "UuidFactura");

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_Activo",
                table: "ParametrosSistema",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_Categoria",
                table: "ParametrosSistema",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_Categoria_Grupo_Orden",
                table: "ParametrosSistema",
                columns: new[] { "Categoria", "Grupo", "Orden" });

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_Clave_EscuelaId_Unique",
                table: "ParametrosSistema",
                columns: new[] { "Clave", "EscuelaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_EsConfigurable",
                table: "ParametrosSistema",
                column: "EsConfigurable");

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_Escuela_Categoria_Activo",
                table: "ParametrosSistema",
                columns: new[] { "EscuelaId", "Categoria", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_EscuelaId",
                table: "ParametrosSistema",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_EsGlobal",
                table: "ParametrosSistema",
                column: "EsGlobal");

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_FechaUltimoCambio",
                table: "ParametrosSistema",
                column: "FechaUltimoCambio");

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_TipoDato",
                table: "ParametrosSistema",
                column: "TipoDato");

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_UsuarioUltimoCambioId",
                table: "ParametrosSistema",
                column: "UsuarioUltimoCambioId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_Activo",
                table: "PeriodosEvaluacion",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_CalificacionesDefinitivas",
                table: "PeriodosEvaluacion",
                column: "CalificacionesDefinitivas");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_Ciclo_Numero",
                table: "PeriodosEvaluacion",
                columns: new[] { "CicloEscolarId", "Numero" });

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_CicloEscolarId",
                table: "PeriodosEvaluacion",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_Escuela_Ciclo_Activo",
                table: "PeriodosEvaluacion",
                columns: new[] { "EscuelaId", "CicloEscolarId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_Escuela_Ciclo_Numero_Unique",
                table: "PeriodosEvaluacion",
                columns: new[] { "EscuelaId", "CicloEscolarId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_Escuela_CicloEscolarId",
                table: "PeriodosEvaluacion",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_Escuela_Fechas",
                table: "PeriodosEvaluacion",
                columns: new[] { "EscuelaId", "FechaInicio", "FechaFin" });

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_EscuelaId",
                table: "PeriodosEvaluacion",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_FechaFin",
                table: "PeriodosEvaluacion",
                column: "FechaFin");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_FechaInicio",
                table: "PeriodosEvaluacion",
                column: "FechaInicio");

            migrationBuilder.CreateIndex(
                name: "IX_PlantillasDocumento_Activa",
                table: "PlantillasDocumento",
                column: "Activa");

            migrationBuilder.CreateIndex(
                name: "IX_PlantillasDocumento_Categoria",
                table: "PlantillasDocumento",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_PlantillasDocumento_Escuela_Tipo_Activa",
                table: "PlantillasDocumento",
                columns: new[] { "EscuelaId", "TipoDocumento", "Activa" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantillasDocumento_Escuela_Tipo_Default",
                table: "PlantillasDocumento",
                columns: new[] { "EscuelaId", "TipoDocumento", "EsPlantillaPorDefecto" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantillasDocumento_EscuelaId",
                table: "PlantillasDocumento",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantillasDocumento_EsPlantillaPorDefecto",
                table: "PlantillasDocumento",
                column: "EsPlantillaPorDefecto");

            migrationBuilder.CreateIndex(
                name: "IX_PlantillasDocumento_Nombre",
                table: "PlantillasDocumento",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_PlantillasDocumento_TipoDocumento",
                table: "PlantillasDocumento",
                column: "TipoDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_Activa",
                table: "PreferenciasUsuario",
                column: "Activa");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_Alcance",
                table: "PreferenciasUsuario",
                column: "Alcance");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_Categoria",
                table: "PreferenciasUsuario",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_Categoria_Grupo_Orden",
                table: "PreferenciasUsuario",
                columns: new[] { "Categoria", "Grupo", "Orden" });

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_EscuelaId",
                table: "PreferenciasUsuario",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_EsSincronizable",
                table: "PreferenciasUsuario",
                column: "EsSincronizable");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_FechaUltimaSincronizacion",
                table: "PreferenciasUsuario",
                column: "FechaUltimaSincronizacion");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_FechaUltimoCambio",
                table: "PreferenciasUsuario",
                column: "FechaUltimoCambio");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_Tipo",
                table: "PreferenciasUsuario",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_Usuario_Categoria_Activa",
                table: "PreferenciasUsuario",
                columns: new[] { "UsuarioId", "Categoria", "Activa" });

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_Usuario_Clave_Unique",
                table: "PreferenciasUsuario",
                columns: new[] { "UsuarioId", "Clave" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_Usuario_Tipo_Activa",
                table: "PreferenciasUsuario",
                columns: new[] { "UsuarioId", "Tipo", "Activa" });

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasUsuario_UsuarioId",
                table: "PreferenciasUsuario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_Alumno_Estado",
                table: "Prestamos",
                columns: new[] { "AlumnoId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_AlumnoId",
                table: "Prestamos",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_DevueltoPorId",
                table: "Prestamos",
                column: "DevueltoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_Escuela_Estado_Fecha",
                table: "Prestamos",
                columns: new[] { "EscuelaId", "Estado", "FechaPrestamo" });

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_EscuelaId",
                table: "Prestamos",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_Estado",
                table: "Prestamos",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_Estado_FechaDevolucion",
                table: "Prestamos",
                columns: new[] { "Estado", "FechaDevolucionProgramada" });

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_FechaDevolucionProgramada",
                table: "Prestamos",
                column: "FechaDevolucionProgramada");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_FechaPrestamo",
                table: "Prestamos",
                column: "FechaPrestamo");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_Folio",
                table: "Prestamos",
                column: "Folio");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_MaestroId",
                table: "Prestamos",
                column: "MaestroId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_MultaPagada",
                table: "Prestamos",
                column: "MultaPagada");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_RegistradoPorId",
                table: "Prestamos",
                column: "RegistradoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_Alumno_Fecha",
                table: "RegistrosConducta",
                columns: new[] { "AlumnoId", "FechaHoraIncidente" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_Alumno_Tipo_Estado",
                table: "RegistrosConducta",
                columns: new[] { "AlumnoId", "Tipo", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_AlumnoId",
                table: "RegistrosConducta",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_Categoria",
                table: "RegistrosConducta",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_Escuela_Estado_Deleted",
                table: "RegistrosConducta",
                columns: new[] { "EscuelaId", "Estado", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_EscuelaId",
                table: "RegistrosConducta",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_Estado",
                table: "RegistrosConducta",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_FechaHoraIncidente",
                table: "RegistrosConducta",
                column: "FechaHoraIncidente");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_FechaSeguimiento",
                table: "RegistrosConducta",
                column: "FechaSeguimiento");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_Gravedad",
                table: "RegistrosConducta",
                column: "Gravedad");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_GrupoId",
                table: "RegistrosConducta",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_IsDeleted",
                table: "RegistrosConducta",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_Maestro_Fecha",
                table: "RegistrosConducta",
                columns: new[] { "MaestroId", "FechaHoraIncidente" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_MaestroId",
                table: "RegistrosConducta",
                column: "MaestroId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_PadresNotificados",
                table: "RegistrosConducta",
                column: "PadresNotificados");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_PeriodoId",
                table: "RegistrosConducta",
                column: "PeriodoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_RequiereSeguimiento",
                table: "RegistrosConducta",
                column: "RequiereSeguimiento");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_SancionId",
                table: "RegistrosConducta",
                column: "SancionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_Seguimiento_Fecha",
                table: "RegistrosConducta",
                columns: new[] { "RequiereSeguimiento", "FechaSeguimiento" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_Tipo",
                table: "RegistrosConducta",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosConducta_Tipo_Gravedad",
                table: "RegistrosConducta",
                columns: new[] { "Tipo", "Gravedad" });

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_Activo",
                table: "ReportesPersonalizados",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_Categoria",
                table: "ReportesPersonalizados",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_CreatedBy",
                table: "ReportesPersonalizados",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_Escuela_Tipo_Activo",
                table: "ReportesPersonalizados",
                columns: new[] { "EscuelaId", "TipoReporte", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_EscuelaId",
                table: "ReportesPersonalizados",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_EsPrivado",
                table: "ReportesPersonalizados",
                column: "EsPrivado");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_EsReporteSistema",
                table: "ReportesPersonalizados",
                column: "EsReporteSistema");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_Nombre",
                table: "ReportesPersonalizados",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_Programacion_Proxima",
                table: "ReportesPersonalizados",
                columns: new[] { "ProgramacionAutomatica", "ProximaEjecucion" });

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_ProgramacionAutomatica",
                table: "ReportesPersonalizados",
                column: "ProgramacionAutomatica");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_ProximaEjecucion",
                table: "ReportesPersonalizados",
                column: "ProximaEjecucion");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesPersonalizados_TipoReporte",
                table: "ReportesPersonalizados",
                column: "TipoReporte");

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_Alumno_FechaInicio",
                table: "Sanciones",
                columns: new[] { "AlumnoId", "FechaInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_Alumno_Tipo_Cumplida",
                table: "Sanciones",
                columns: new[] { "AlumnoId", "Tipo", "Cumplida" });

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_AlumnoId",
                table: "Sanciones",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_Apelada",
                table: "Sanciones",
                column: "Apelada");

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_AutorizadoPor",
                table: "Sanciones",
                column: "AutorizadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_ConductaId",
                table: "Sanciones",
                column: "ConductaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_Cumplida",
                table: "Sanciones",
                column: "Cumplida");

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_Cumplida_FechaFin",
                table: "Sanciones",
                columns: new[] { "Cumplida", "FechaFin" });

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_Escuela_Cumplida",
                table: "Sanciones",
                columns: new[] { "EscuelaId", "Cumplida" });

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_Escuela_Fechas",
                table: "Sanciones",
                columns: new[] { "EscuelaId", "FechaInicio", "FechaFin" });

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_EscuelaId",
                table: "Sanciones",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_FechaFin",
                table: "Sanciones",
                column: "FechaFin");

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_FechaInicio",
                table: "Sanciones",
                column: "FechaInicio");

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_FirmaEnterado",
                table: "Sanciones",
                column: "FirmaEnterado");

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_Notificacion_Firma",
                table: "Sanciones",
                columns: new[] { "PadresNotificados", "FirmaEnterado" });

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_PadresNotificados",
                table: "Sanciones",
                column: "PadresNotificados");

            migrationBuilder.CreateIndex(
                name: "IX_Sanciones_Tipo",
                table: "Sanciones",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_Dispositivo_Estado",
                table: "Sincronizaciones",
                columns: new[] { "DispositivoId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_Dispositivo_Fecha",
                table: "Sincronizaciones",
                columns: new[] { "DispositivoId", "FechaInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_DispositivoId",
                table: "Sincronizaciones",
                column: "DispositivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_Escuela_Estado_Fecha",
                table: "Sincronizaciones",
                columns: new[] { "EscuelaId", "Estado", "FechaInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_EscuelaId",
                table: "Sincronizaciones",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_Estado",
                table: "Sincronizaciones",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_Estado_Fecha",
                table: "Sincronizaciones",
                columns: new[] { "Estado", "FechaInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_FechaFin",
                table: "Sincronizaciones",
                column: "FechaFin");

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_FechaInicio",
                table: "Sincronizaciones",
                column: "FechaInicio");

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_Tipo",
                table: "Sincronizaciones",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_TipoDispositivo",
                table: "Sincronizaciones",
                column: "TipoDispositivo");

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_TuvoErrores",
                table: "Sincronizaciones",
                column: "TuvoErrores");

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_UltimaSincronizacionExitosa",
                table: "Sincronizaciones",
                column: "UltimaSincronizacionExitosa");

            migrationBuilder.CreateIndex(
                name: "IX_Sincronizaciones_UsuarioId",
                table: "Sincronizaciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TareaEntregas_Alumno_Estatus",
                table: "TareaEntregas",
                columns: new[] { "AlumnoId", "Estatus" });

            migrationBuilder.CreateIndex(
                name: "IX_TareaEntregas_AlumnoId",
                table: "TareaEntregas",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_TareaEntregas_EscuelaId",
                table: "TareaEntregas",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_TareaEntregas_Estatus",
                table: "TareaEntregas",
                column: "Estatus");

            migrationBuilder.CreateIndex(
                name: "IX_TareaEntregas_FechaEntrega",
                table: "TareaEntregas",
                column: "FechaEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_TareaEntregas_RevisadoPorId",
                table: "TareaEntregas",
                column: "RevisadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_TareaEntregas_Tarea_Alumno_Unique",
                table: "TareaEntregas",
                columns: new[] { "TareaId", "AlumnoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TareaEntregas_Tarea_Estatus",
                table: "TareaEntregas",
                columns: new[] { "TareaId", "Estatus" });

            migrationBuilder.CreateIndex(
                name: "IX_TareaEntregas_TareaId",
                table: "TareaEntregas",
                column: "TareaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_Activo",
                table: "Tareas",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_EscuelaId",
                table: "Tareas",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_FechaEntrega",
                table: "Tareas",
                column: "FechaEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_Grupo_Fecha_Activo",
                table: "Tareas",
                columns: new[] { "GrupoId", "FechaEntrega", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_Grupo_Materia",
                table: "Tareas",
                columns: new[] { "GrupoId", "MateriaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_MaestroId",
                table: "Tareas",
                column: "MaestroId");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_MateriaId",
                table: "Tareas",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Activo",
                table: "Usuarios",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Escuela_Rol",
                table: "Usuarios",
                columns: new[] { "EscuelaId", "Rol" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Username",
                table: "Usuarios",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacunas_EscuelaId",
                table: "Vacunas",
                column: "EscuelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacunas_Expediente_Nombre_Fecha",
                table: "Vacunas",
                columns: new[] { "ExpedienteMedicoId", "NombreVacuna", "FechaAplicacion" });

            migrationBuilder.CreateIndex(
                name: "IX_Vacunas_ExpedienteMedicoId",
                table: "Vacunas",
                column: "ExpedienteMedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacunas_FechaAplicacion",
                table: "Vacunas",
                column: "FechaAplicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Vacunas_FechaProximaDosis",
                table: "Vacunas",
                column: "FechaProximaDosis");

            migrationBuilder.CreateIndex(
                name: "IX_Vacunas_NombreVacuna",
                table: "Vacunas",
                column: "NombreVacuna");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alergias");

            migrationBuilder.DropTable(
                name: "AlumnoPadres");

            migrationBuilder.DropTable(
                name: "AlumnosInsignias");

            migrationBuilder.DropTable(
                name: "Asistencias");

            migrationBuilder.DropTable(
                name: "Calificaciones");

            migrationBuilder.DropTable(
                name: "CambiosEntidad");

            migrationBuilder.DropTable(
                name: "ComunicadoLecturas");

            migrationBuilder.DropTable(
                name: "ConfiguracionesEscuela");

            migrationBuilder.DropTable(
                name: "Dispositivos");

            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "EstadosCuenta");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "GradoMaterias");

            migrationBuilder.DropTable(
                name: "GrupoMateriaMaestros");

            migrationBuilder.DropTable(
                name: "HistorialMedico");

            migrationBuilder.DropTable(
                name: "HistorialPuntos");

            migrationBuilder.DropTable(
                name: "Inscripciones");

            migrationBuilder.DropTable(
                name: "Medicamentos");

            migrationBuilder.DropTable(
                name: "Mensajes");

            migrationBuilder.DropTable(
                name: "NotificacionSmsLog");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "ParametrosSistema");

            migrationBuilder.DropTable(
                name: "PreferenciasUsuario");

            migrationBuilder.DropTable(
                name: "Prestamos");

            migrationBuilder.DropTable(
                name: "ReportesPersonalizados");

            migrationBuilder.DropTable(
                name: "Sanciones");

            migrationBuilder.DropTable(
                name: "Sincronizaciones");

            migrationBuilder.DropTable(
                name: "TareaEntregas");

            migrationBuilder.DropTable(
                name: "Vacunas");

            migrationBuilder.DropTable(
                name: "Padres");

            migrationBuilder.DropTable(
                name: "Insignias");

            migrationBuilder.DropTable(
                name: "PeriodosEvaluacion");

            migrationBuilder.DropTable(
                name: "LogsAuditoria");

            migrationBuilder.DropTable(
                name: "Comunicados");

            migrationBuilder.DropTable(
                name: "PlantillasDocumento");

            migrationBuilder.DropTable(
                name: "AlumnoPuntos");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "Cargos");

            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "RegistrosConducta");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "ExpedientesMedicos");

            migrationBuilder.DropTable(
                name: "ConceptosPago");

            migrationBuilder.DropTable(
                name: "CategoriasRecurso");

            migrationBuilder.DropTable(
                name: "Grupos");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Alumnos");

            migrationBuilder.DropTable(
                name: "CiclosEscolares");

            migrationBuilder.DropTable(
                name: "Grados");

            migrationBuilder.DropTable(
                name: "Maestros");

            migrationBuilder.DropTable(
                name: "NivelesEducativos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Escuelas");
        }
    }
}
