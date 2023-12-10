using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class RedeSocialPersist : GeralPersist, IRedeSocialPersist
    {
        private readonly ProEventosContext _context;
        public RedeSocialPersist(ProEventosContext context) : base(context)
        {
           _context = context;
            
        }
          public async Task<RedesSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int id)
          {
           IQueryable<RedesSocial> query = _context.RedesSociais;
           query = query.AsNoTracking()
                        .Where(rs => rs.EventoId == eventoId && rs.Id == id );
           return await query.FirstOrDefaultAsync();
          }
          public async Task<RedesSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id)
          {
            IQueryable<RedesSocial> query = _context.RedesSociais;
            query = query.AsNoTracking()
                        .Where(rs => rs.PalestranteId
                         == palestranteId && rs.Id == id );
                          return await query.FirstOrDefaultAsync();
          }
          public async Task<RedesSocial[]> GetAllByEventoIdAsync(int eventoId)
         {
            IQueryable<RedesSocial> query = _context.RedesSociais;
             query = query.AsNoTracking()
                        .Where(rs => rs.EventoId == eventoId );
                          return await query.ToArrayAsync();
          }
          public async Task<RedesSocial[]> GetAllByPalestranteIdAsync(int palestranteId)
          {
            IQueryable<RedesSocial> query = _context.RedesSociais;

            query = query.AsNoTracking()
                        .Where(rs => rs.PalestranteId == palestranteId );
                          return await query.ToArrayAsync();

          }        

        }

    }
