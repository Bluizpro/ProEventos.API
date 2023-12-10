using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IRedeSocialPersist : IGeralPersist
    {
       Task<RedesSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int id);
        Task<RedesSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id);
        Task<RedesSocial[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedesSocial[]> GetAllByPalestranteIdAsync(int palestranteId);
    }
}