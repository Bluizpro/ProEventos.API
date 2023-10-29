using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Dto
{
    public class RedesSocialDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? URL { get; set; }
        public int? EventoId { get; set; }
        public EventoDto? Evento { get; }
        public int? PalestranteId { get; set; }
        public PalestranteDto? Palestrante { get; }
    }
}