using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dto;

namespace ProEventos.Application.Contratos
{
    public interface IRedeSocialService
    {
      Task<RedesSocialDto[]> SaveByEvento(int eventoId, RedesSocialDto[] models);
        Task<bool> DeleteByEvento(int eventoId, int redeSocialId);

        Task<RedesSocialDto[]> SaveByPalestrante(int palestranteId, RedesSocialDto[] models);
        Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId);

        Task<RedesSocialDto[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedesSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId);

        Task<RedesSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int RedeSocialId);
        Task<RedesSocialDto> GetRedeSocialPalestranteByIdsAsync(int PalestranteId, int RedeSocialId);


    }
}