using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using ProEventos.Domain;
using ProEventos.Persistence.models;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantePersist : IGeralPersist
    {
    
//Palestrante
       // Task<Palestrante[]> GetAllPalestrantesAsyncByNome(string Nome, bool includeEventos );
        Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);

        Task<PageList<Palestrante>> GetAllPalestrantesAsync( PageParams pageParams, bool includeEventos = false );
    }
}