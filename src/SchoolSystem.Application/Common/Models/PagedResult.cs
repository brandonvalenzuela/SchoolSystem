namespace SchoolSystem.Application.Common.Models
{
		/// <summary>
		/// Clase genérica para envolver resultados paginados.
		/// </summary>
		/// <typeparam name="T">El tipo de dato que contiene la lista (ej: AlumnoDto, EscuelaDto)</typeparam>
		public class PagedResult<T>
		{
			/// <summary>
			/// La lista de elementos de la página actual.
			/// </summary>
			public IEnumerable<T> Items { get; set; }

			/// <summary>
			/// El número total de elementos en la base de datos (sin paginar).
			/// </summary>
			public int TotalItems { get; set; }

			/// <summary>
			/// El número de página actual.
			/// </summary>
			public int PageNumber { get; set; }

			/// <summary>
			/// La cantidad de elementos por página.
			/// </summary>
			public int PageSize { get; set; }

			/// <summary>
			/// El total de páginas calculado.
			/// </summary>
			public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

			/// <summary>
			/// Indica si hay una página anterior.
			/// </summary>
			public bool HasPreviousPage => PageNumber > 1;

			/// <summary>
			/// Indica si hay una página siguiente.
			/// </summary>
			public bool HasNextPage => PageNumber < TotalPages;

			/// <summary>
			/// Constructor vacío
			/// </summary>
			public PagedResult()
			{
				Items = new List<T>();
			}

			/// <summary>
			/// Constructor con datos iniciales
			/// </summary>
			public PagedResult(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
			{
				Items = items;
				TotalItems = totalItems;
				PageNumber = pageNumber;
				PageSize = pageSize;
			}

		}
}
