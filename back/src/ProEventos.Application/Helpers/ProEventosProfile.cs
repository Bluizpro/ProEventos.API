using AutoMapper;
using ProEventos.Application.Dto;
using ProEventos.Domain;

namespace ProEventos.Application.Helpers
{
    public class ProEventosProfile : Profile
    {
        
        public ProEventosProfile(){
            CreateMap<Evento, EventoDto>().ReverseMap();
             CreateMap<Lote, LoteDto>().ReverseMap();
              CreateMap<RedesSocial, RedesSocialDto>().ReverseMap();
               CreateMap<Palestrante, PalestranteDto>().ReverseMap();
        }
    }
}