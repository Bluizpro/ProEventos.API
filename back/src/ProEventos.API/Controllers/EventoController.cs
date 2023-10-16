using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Data;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{  
  
private readonly DataContext _context;

public EventosController (DataContext context)
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
        return _context.Eventos.FirstOrDefault(evento => evento.EventoId == id);
       
      
    }
}
