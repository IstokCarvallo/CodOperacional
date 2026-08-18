namespace APISegura.Dtos.Folios
{
    public class FolioBusquedaDto
    {
        public int PlantaC { get; set; }

        public string? Planta { get; set; }

        public int ClienteC { get; set; }

        public string? Cliente { get; set; }

        public int NroDespacho { get; set; }

        public DateTime? FechaDespacho { get; set; }

        public int? GuiaDespacho { get; set; }

        public string? Embarque { get; set; }

        public int? Consignatario { get; set; }

        public int Folio { get; set; }

        public string? Operacion { get; set; }

        public string? Nave { get; set; }

        public DateTime? FechaZarpe { get; set; }

        public int? PuertoDestino { get; set; }

        public string? Puerto { get; set; }

        public int? PuertoOrigen { get; set; }

        public string? Puerto_Origen { get; set; }

        public string? Contenedor { get; set; }

        public int EspecieC { get; set; }

        public string? Especie { get; set; }

        public int VariedadC { get; set; }

        public string? Variedad { get; set; }

        public string? Embalaje { get; set; }

        public int ProductorC { get; set; }

        public string? Productor { get; set; }

        public int EtiquetaC { get; set; }

        public string? Etiqueta { get; set; }

        public string? Calibre { get; set; }

        public DateTime? FechaEmbalaje { get; set; }

        public int PredioC { get; set; }

        public string? SCG { get; set; }

        public string? Cuartel { get; set; }

        public string? SDP { get; set; }

        public int CategoriaC { get; set; }

        public string? Categoria { get; set; }

        public int PackingC { get; set; }

        public string? Packing { get; set; }

        public int CantidadCajas { get; set; }

        public string? Busqueda { get; set; }

        public int TotalRegistros { get; set; }
    }
}
