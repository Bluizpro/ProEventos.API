using Microsoft.AspNetCore.Mvc;
using ProEventos.Persistence;
using ProEventos.Domain;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{  
  
private readonly ProEventosContext _context;

public EventosController (ProEventosContext context)
{
        
    _context = context;
}
  

    [HttpGet]
    public IEnumerable<Evento> get()
    {
        return _context.Eventos;
       
      
    }
  
       [HttpGet("{id}")]
    public Evento GetById(int id)
    {
        return _context.Eventos.FirstOrDefault
        (evento => evento.Id == id);
       
      
    }
}
