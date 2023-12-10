

using ProEventos.Application.Dto;
using ProEventos.Persistence.models;


namespace ProEventos.Application.Contratos
{
    public interface IPalestranteService
    {
        Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model);

        Task<PalestranteDto> UpdatePalestrantes(int userId, PalestranteUpdateDto model);

       
        Task<PageList<PalestranteDto>> GetAllPalestrantesAsync( PageParams pageParams, bool includeEventos = false);

       
        Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId,  bool includeEventos = false );
    }   
  

}

