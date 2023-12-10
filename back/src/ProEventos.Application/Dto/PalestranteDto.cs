using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Dto
{
    public class PalestranteDto
    {
        public int Id { get; set; }
        public string? MiniCurriculo { get; set; }
        public int UserId { get; set; }
        public UserUpdateDto? User { get; set; }
        public IEnumerable<RedesSocialDto>? RedesSociais { get; set; }
        public IEnumerable<EventoDto>? Palestrantes { get; set; }
    }
}