using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Finanzas;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Finanzas;
using SchoolSystem.Domain.Enums.Finanzas;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class PagoService : IPagoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PagoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<PagoDto>> GetPagosAsync(int page, int size)
        {
            // Incluimos Cargo (y Concepto) y Alumno para mostrar nombres
            var query = _unitOfWork.Pagos.GetAllIncludingAsync(
                p => p.Cargo,
                p => p.Cargo.ConceptoPago,
                p => p.Alumno
            );

            var allItems = await query;
            var total = allItems.Count();

            var items = allItems
                .OrderByDescending(p => p.FechaPago)
                .Skip((page - 1) * size)
                .Take(size);

            return new PagedResult<PagoDto>
            {
                Items = _mapper.Map<IEnumerable<PagoDto>>(items),
                TotalItems = total,
                PageNumber = page,
                PageSize = size
            };
        }

        public async Task<List<CargoDto>> GetCargosPendientesPorAlumnoAsync(int alumnoId)
        {
            // Buscamos cargos que NO estén pagados ni cancelados
            var cargos = await _unitOfWork.Cargos.FindAsync(c =>
                c.AlumnoId == alumnoId &&
                c.Estatus != EstatusCargo.Pagado &&
                c.Estatus != EstatusCargo.Cancelado,
                c => c.ConceptoPago);

            // Nota: Aquí podrías incluir ConceptoPago si lo necesitas en el mapeo

            return _mapper.Map<List<CargoDto>>(cargos);
        }

        public async Task<int> RegistrarPagoAsync(CreatePagoDto dto)
        {
            // 1. Obtener el Cargo
            var cargo = await _unitOfWork.Cargos.GetByIdAsync(dto.CargoId);
            if (cargo == null)
                throw new KeyNotFoundException("El cargo no existe.");

            // 2. Validaciones de Negocio
            if (cargo.EstaPagado)
                throw new InvalidOperationException("El cargo ya está pagado por completo.");

            if (dto.Monto > cargo.SaldoPendiente)
                throw new InvalidOperationException($"El monto (${dto.Monto}) excede el saldo pendiente (${cargo.SaldoPendiente}).");

            // 3. Crear la entidad Pago
            var pago = _mapper.Map<Pago>(dto);
            pago.FechaPago = DateTime.Now;

            // Generar Folio (Lógica simple, idealmente usar una secuencia de BD o tabla de folios)
            // Aquí usamos un GUID corto o timestamp para simular
            pago.SerieRecibo = "A";
            pago.FolioRecibo = DateTime.Now.ToString("yyyyMMdd-HHmmss");

            // 4. Actualizar el Cargo (Usando método de dominio)
            cargo.RegistrarPago(dto.Monto);

            // 5. Guardar todo (Transacción implícita por UnitOfWork)
            await _unitOfWork.Pagos.AddAsync(pago);
            await _unitOfWork.Cargos.UpdateAsync(cargo);
            await _unitOfWork.SaveChangesAsync();

            return pago.Id;
        }

        public async Task CancelarPagoAsync(int pagoId, string motivo, int usuarioId)
        {
            var pago = await _unitOfWork.Pagos.GetByIdAsync(pagoId);
            if (pago == null)
                throw new KeyNotFoundException("Pago no encontrado.");

            if (pago.Cancelado)
                throw new InvalidOperationException("El pago ya fue cancelado anteriormente.");

            // 1. Obtener el cargo relacionado para reversar el saldo
            var cargo = await _unitOfWork.Cargos.GetByIdAsync(pago.CargoId);

            // 2. Cancelar el pago (Método de dominio)
            pago.Cancelar(motivo, usuarioId);

            // 3. Revertir saldo en el cargo
            // Como no tenemos un método "RevertirPago" en la entidad Cargo, lo hacemos manualmente 
            // o agregamos el método a la entidad Cargo.cs (Recomendado).
            // Por ahora, lógica manual simple:
            cargo.MontoPagado -= pago.Monto;
            cargo.SaldoPendiente += pago.Monto;

            // Recalcular estatus del cargo
            if (cargo.MontoPagado == 0)
                cargo.Estatus = EstatusCargo.Pendiente;
            else
                cargo.Estatus = EstatusCargo.Parcial;

            // Si estaba vencido, vuelve a estarlo
            if (cargo.FechaVencimiento < DateTime.Now && cargo.SaldoPendiente > 0)
                cargo.Estatus = EstatusCargo.Vencido;

            await _unitOfWork.Pagos.UpdateAsync(pago);
            await _unitOfWork.Cargos.UpdateAsync(cargo);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task GenerarCargosMensualesAsync(int escuelaId, int mes, int anio)
        {
            // Esta lógica es compleja:
            // 1. Buscar alumnos activos
            // 2. Buscar conceptos recurrentes (Colegiaturas)
            // 3. Generar un cargo por alumno

            // Por brevedad, dejaremos este método como esqueleto para futura implementación
            throw new NotImplementedException("La generación masiva se implementará en la siguiente fase.");
        }
    }
}
