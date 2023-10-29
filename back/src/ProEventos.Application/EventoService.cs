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
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventosPersist _eventosPersist;
        private readonly IMapper _mapper;
        public EventoService(IGeralPersist geralPersist, IEventosPersist eventosPersist,      
        IMapper mapper)
        {
            _mapper = mapper;
            _eventosPersist = eventosPersist;
            _geralPersist = geralPersist;

        }
        public async Task<EventoDto> AddEventos(EventoDto model)
        {
      
          try
            {
             var evento = _mapper.Map<Evento>(model);

                _geralPersist.Add<Evento>(evento);
                if (await _geralPersist.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventosPersist.GetEventoByIdAsync(evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public async Task<EventoDto> UpdateEventos(int eventoId, EventoDto model)
        {
          
            try
            {
                var evento = await _eventosPersist.GetEventoByIdAsync(eventoId, false);
                if (evento == null) return null;

                model.Id = evento.Id;

                _mapper.Map(model, evento);

                _geralPersist.Update<Evento>(evento);

                if (await _geralPersist.SaveChangesAsync())
                {
                      var eventoRetorno = await _eventosPersist.GetEventoByIdAsync(evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteEventos(int eventoId)
        {
                  try
            {
                var evento = await _eventosPersist.GetEventoByIdAsync(eventoId, false);
                if (evento == null) throw new Exception("Evento para delete não encontrado.");

                _geralPersist.Delete<Evento>(evento);

                return await _geralPersist.SaveChangesAsync();

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersist.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;
                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
          try
            {
                var eventos = await _eventosPersist.GetAllEventosByTemaAsync(tema,includePalestrantes);
                if (eventos == null) return null;

               var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
          try
            {
                var evento = await _eventosPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
                if (evento == null) return null;

                var resultado = _mapper.Map<EventoDto>(evento);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}