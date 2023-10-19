using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestratePersist
    {
    
//Palestrante
        Task<Palestrante[]> GetAllPalestrantesAsyncByNome(string Nome, bool includeEventos );
        Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos );

        Task<Palestrante[]> GetAllPalestrantesAsync( bool includeEventos );
    }
}