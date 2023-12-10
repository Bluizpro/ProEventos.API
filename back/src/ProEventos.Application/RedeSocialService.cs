using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dto;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class RedeSocialService : IRedeSocialService
    {
      
        private readonly IRedeSocialPersist _redeSocialPersist;
        private readonly IMapper _mapper;
        public RedeSocialService(IRedeSocialPersist redeSocialPersist,
        IMapper mapper)
        {
            _mapper = mapper;
            _redeSocialPersist = redeSocialPersist;
            

        }
       public async Task AddRedeSocial(int Id, RedesSocialDto model, bool isEvento)
        {
            try
            {
                var redeSocial = _mapper.Map<RedesSocial>(model);
                if (isEvento)
                {
                     redeSocial.EventoId = Id;
                     redeSocial.PalestranteId  = null;
                }
                else
                {
                    redeSocial.EventoId = null;
                     redeSocial.PalestranteId = Id;
                }           
                _redeSocialPersist.Add<RedesSocial>(redeSocial);

                await _redeSocialPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedesSocialDto[]> SaveByEvento(int eventoId, RedesSocialDto[] models)
        {
            try
            {
                var redesSocials = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);
                if (redesSocials == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, model, true);
                    }
                    else
                    {
                        var redeSocial = redesSocials.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);
                        model.EventoId = eventoId;

                        _mapper.Map(model, redeSocial);

                        _redeSocialPersist.Update<RedesSocial>(redeSocial);

                        await _redeSocialPersist.SaveChangesAsync();
                    }
                }

                var redeSocialRetorno = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);

                return _mapper.Map<RedesSocialDto[]>(redeSocialRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


       public async Task<RedesSocialDto[]> SaveByPalestrante(int palestranteId, RedesSocialDto[] models)
        {
            try
            {
                var redeSociais = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);
                if (redeSociais == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(palestranteId, model, false);
                    }
                    else
                    {
                         var redeSocial = redeSociais.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);
                        model.PalestranteId = palestranteId;

                        _mapper.Map(model, redeSocial);

                        _redeSocialPersist.Update<RedesSocial>(redeSocial);

                        await _redeSocialPersist.SaveChangesAsync();
                    }
                }

                var redeSocialRetorno = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);

                return _mapper.Map<RedesSocialDto[]>(redeSocialRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (redeSocial == null) throw new Exception("Rede Social por evento para deleta não encontrado.");

                _redeSocialPersist.Delete<RedesSocial>(redeSocial);
                return await _redeSocialPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (redeSocial == null) throw new Exception("Rede Social por palestrante para deleta não encontrado.");

                _redeSocialPersist.Delete<RedesSocial>(redeSocial);
                return await _redeSocialPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedesSocialDto[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);
                if (redeSocial == null) return null;

                var resultado = _mapper.Map<RedesSocialDto[]>(redeSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<RedesSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);
                if (redeSocial == null) return null;

                var resultado = _mapper.Map<RedesSocialDto[]>(redeSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public async Task<RedesSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var redesSocial = await _redeSocialPersist.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (redesSocial == null) return null;

                var resultado = _mapper.Map<RedesSocialDto>(redesSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

          public async Task<RedesSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redesSocial = await _redeSocialPersist.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (redesSocial == null) return null;

                var resultado = _mapper.Map<RedesSocialDto>(redesSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }     

    }
}