using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dto;
using ProEventos.Domain;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.models;

namespace ProEventos.Application.Helpers
{
    public class ProEventosProfile : Profile
    {
        
        public ProEventosProfile(){
            CreateMap<Evento, EventoDto>().ReverseMap();
             CreateMap<Lote, LoteDto>().ReverseMap();
              CreateMap<RedesSocial, RedesSocialDto>().ReverseMap();
               CreateMap<Palestrante, PalestranteDto>().ReverseMap();       
               CreateMap<Palestrante, PalestranteAddDto>().ReverseMap();  
               CreateMap<Palestrante, PalestranteUpdateDto>().ReverseMap();         


               CreateMap<User, UserDto>().ReverseMap();
               CreateMap<User, UserUpdateDto>().ReverseMap();
               CreateMap<User, UserLoginDto>().ReverseMap();
        }
    }
}