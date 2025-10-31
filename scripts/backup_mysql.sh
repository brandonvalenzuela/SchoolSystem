#!/bin/bash

# ============================================
# Script de Backup MySQL
# Sistema de Gestión Escolar
# ============================================

# Colores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
MAGENTA='\033[0;35m'
NC='\033[0m' # No Color

# Configuración
DB_NAME="school_system"
BACKUP_DIR="./backups/mysql"
DATE=$(date +"%Y%m%d_%H%M%S")
BACKUP_FILE="$BACKUP_DIR/${DB_NAME}_backup_$DATE.sql"
COMPRESSED_FILE="$BACKUP_FILE.gz"
RETENTION_DAYS=30

# Función para imprimir encabezados
print_header() {
    echo -e "${MAGENTA}"
    echo "╔════════════════════════════════════════════════════════╗"
    echo "║  $1"
    echo "╚════════════════════════════════════════════════════════╝"
    echo -e "${NC}"
}

# Función para imprimir mensajes de éxito
print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

# Función para imprimir mensajes de información
print_info() {
    echo -e "${CYAN}ℹ $1${NC}"
}

# Función para imprimir mensajes de advertencia
print_warning() {
    echo -e "${YELLOW}⚠ $1${NC}"
}

# Función para imprimir mensajes de error
print_error() {
    echo -e "${RED}✗ $1${NC}"
}

# Función para solicitar credenciales
get_credentials() {
    print_info "Ingresa las credenciales de MySQL:"
    read -p "Host (default: localhost): " DB_HOST
    DB_HOST=${DB_HOST:-localhost}

    read -p "Puerto (default: 3306): " DB_PORT
    DB_PORT=${DB_PORT:-3306}

    read -p "Usuario: " DB_USER

    read -sp "Contraseña: " DB_PASS
    echo
}

# Función para crear directorio de backups
create_backup_dir() {
    if [ ! -d "$BACKUP_DIR" ]; then
        mkdir -p "$BACKUP_DIR"
        print_success "Directorio de backups creado: $BACKUP_DIR"
    fi
}

# Función para realizar backup
perform_backup() {
    print_header "INICIANDO BACKUP DE MYSQL"

    print_info "Base de datos: $DB_NAME"
    print_info "Archivo: $BACKUP_FILE"
    print_info "Fecha: $(date '+%Y-%m-%d %H:%M:%S')"
    echo

    # Realizar backup
    print_info "Ejecutando mysqldump..."

    mysqldump --host="$DB_HOST" \
              --port="$DB_PORT" \
              --user="$DB_USER" \
              --password="$DB_PASS" \
              --single-transaction \
              --routines \
              --triggers \
              --events \
              --databases "$DB_NAME" \
              --result-file="$BACKUP_FILE"

    if [ $? -eq 0 ]; then
        print_success "Backup completado exitosamente"

        # Obtener tamaño del archivo
        FILE_SIZE=$(du -h "$BACKUP_FILE" | cut -f1)
        print_info "Tamaño del backup: $FILE_SIZE"

        # Comprimir backup
        print_info "Comprimiendo backup..."
        gzip -f "$BACKUP_FILE"

        if [ $? -eq 0 ]; then
            COMPRESSED_SIZE=$(du -h "$COMPRESSED_FILE" | cut -f1)
            print_success "Backup comprimido exitosamente"
            print_info "Tamaño comprimido: $COMPRESSED_SIZE"
            print_info "Archivo: $COMPRESSED_FILE"
        else
            print_error "Error al comprimir el backup"
            return 1
        fi
    else
        print_error "Error al realizar el backup"
        return 1
    fi
}

# Función para limpiar backups antiguos
cleanup_old_backups() {
    print_header "LIMPIANDO BACKUPS ANTIGUOS"

    print_info "Buscando backups con más de $RETENTION_DAYS días..."

    DELETED_COUNT=0
    while IFS= read -r -d '' file; do
        rm "$file"
        DELETED_COUNT=$((DELETED_COUNT + 1))
        print_info "Eliminado: $(basename "$file")"
    done < <(find "$BACKUP_DIR" -name "${DB_NAME}_backup_*.sql.gz" -type f -mtime +$RETENTION_DAYS -print0)

    if [ $DELETED_COUNT -eq 0 ]; then
        print_info "No hay backups antiguos para eliminar"
    else
        print_success "Se eliminaron $DELETED_COUNT backup(s) antiguo(s)"
    fi
}

# Función para listar backups existentes
list_backups() {
    print_header "BACKUPS DISPONIBLES"

    if [ -d "$BACKUP_DIR" ]; then
        BACKUP_COUNT=$(find "$BACKUP_DIR" -name "${DB_NAME}_backup_*.sql.gz" -type f | wc -l)

        if [ $BACKUP_COUNT -eq 0 ]; then
            print_warning "No hay backups disponibles"
        else
            print_info "Total de backups: $BACKUP_COUNT"
            echo
            ls -lh "$BACKUP_DIR"/${DB_NAME}_backup_*.sql.gz | awk '{print $9, "(" $5 ")", $6, $7, $8}'
        fi
    else
        print_warning "El directorio de backups no existe"
    fi
}

# Función para restaurar backup
restore_backup() {
    print_header "RESTAURAR BACKUP"

    list_backups
    echo

    read -p "Ingresa el nombre del archivo a restaurar (o 'cancelar' para salir): " RESTORE_FILE

    if [ "$RESTORE_FILE" == "cancelar" ]; then
        print_info "Operación cancelada"
        return 0
    fi

    RESTORE_PATH="$BACKUP_DIR/$RESTORE_FILE"

    if [ ! -f "$RESTORE_PATH" ]; then
        print_error "El archivo no existe: $RESTORE_PATH"
        return 1
    fi

    print_warning "ADVERTENCIA: Esta operación sobrescribirá la base de datos actual"
    read -p "¿Estás seguro? (escriba 'SI' para confirmar): " CONFIRM

    if [ "$CONFIRM" != "SI" ]; then
        print_info "Operación cancelada"
        return 0
    fi

    print_info "Descomprimiendo backup..."
    TEMP_FILE="${RESTORE_PATH%.gz}"
    gunzip -c "$RESTORE_PATH" > "$TEMP_FILE"

    if [ $? -ne 0 ]; then
        print_error "Error al descomprimir el backup"
        return 1
    fi

    print_info "Restaurando base de datos..."
    mysql --host="$DB_HOST" \
          --port="$DB_PORT" \
          --user="$DB_USER" \
          --password="$DB_PASS" \
          < "$TEMP_FILE"

    if [ $? -eq 0 ]; then
        print_success "Base de datos restaurada exitosamente"
        rm "$TEMP_FILE"
    else
        print_error "Error al restaurar la base de datos"
        rm "$TEMP_FILE"
        return 1
    fi
}

# Función para verificar backup
verify_backup() {
    print_header "VERIFICAR BACKUP"

    if [ ! -f "$COMPRESSED_FILE" ]; then
        print_error "No se encontró el backup reciente: $COMPRESSED_FILE"
        return 1
    fi

    print_info "Verificando integridad del archivo..."

    gzip -t "$COMPRESSED_FILE"

    if [ $? -eq 0 ]; then
        print_success "El backup es válido y está íntegro"

        # Mostrar información del backup
        print_info "Información del backup:"
        echo "  - Archivo: $(basename "$COMPRESSED_FILE")"
        echo "  - Tamaño: $(du -h "$COMPRESSED_FILE" | cut -f1)"
        echo "  - Fecha: $(stat -c %y "$COMPRESSED_FILE" | cut -d'.' -f1)"
    else
        print_error "El backup está corrupto o es inválido"
        return 1
    fi
}

# Menú principal
show_menu() {
    clear
    echo -e "${CYAN}"
    echo "╔════════════════════════════════════════════════════════════╗"
    echo "║                                                            ║"
    echo "║          BACKUP Y RESTAURACIÓN MYSQL                       ║"
    echo "║          Sistema de Gestión Escolar                        ║"
    echo "║                                                            ║"
    echo "╚════════════════════════════════════════════════════════════╝"
    echo -e "${NC}"
    echo
    echo "  1. Realizar backup"
    echo "  2. Listar backups disponibles"
    echo "  3. Restaurar backup"
    echo "  4. Verificar último backup"
    echo "  5. Limpiar backups antiguos"
    echo "  6. Salir"
    echo
}

# Script principal
main() {
    # Verificar que mysqldump está instalado
    if ! command -v mysqldump &> /dev/null; then
        print_error "mysqldump no está instalado. Por favor, instala MySQL client."
        exit 1
    fi

    # Obtener credenciales
    get_credentials

    # Crear directorio de backups
    create_backup_dir

    # Menú principal
    while true; do
        show_menu
        read -p "Selecciona una opción: " option

        case $option in
            1)
                perform_backup
                if [ $? -eq 0 ]; then
                    verify_backup
                fi
                ;;
            2)
                list_backups
                ;;
            3)
                restore_backup
                ;;
            4)
                verify_backup
                ;;
            5)
                cleanup_old_backups
                ;;
            6)
                print_success "¡Hasta luego!"
                exit 0
                ;;
            *)
                print_error "Opción no válida"
                ;;
        esac

        echo
        read -p "Presiona Enter para continuar..."
    done
}

# Ejecutar script principal
main
