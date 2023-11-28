using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;

using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.models;

namespace ProEventos.Persistence
{
    public class EventoPersist : IEventosPersist
    {
        private readonly ProEventosContext _context;
      
        public EventoPersist(ProEventosContext context)
        {
        _context = context;  
       // _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;    
            
        }      
        

        public  async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
           IQueryable<Evento> query = _context.Eventos
           .Include(e => e.Lotes)
           .Include(e => e.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                .Include(e => e.PalestrantesEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking()
             .Where(e => (e.Tema.ToLower().Contains(pageParams.Term.ToLower()) ||
                                      e.Local.ToLower().Contains(pageParams.Term.ToLower())) &&
                                     e.UserId == userId);

            return await PageList<Evento>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }

        
         public async Task<Evento> GetEventoByIdAsync( int userId, int eventoId, bool includePalestrantes = false)
        {
             IQueryable<Evento> query = _context.Eventos.Include(e =>
            e.Lotes).Include(e => e.Lotes).Include(e => e.RedesSociais);
            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id)
            .Where(e => e.Id == eventoId && e.UserId == userId);

            return await query.FirstAsync();
        }

       
    }
       
}