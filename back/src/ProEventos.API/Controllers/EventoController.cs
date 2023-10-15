using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{  
    public IEnumerable<Evento> _evento = new Evento[] {
            new Evento(){
                   EventoId = 1,
            Tema = "Angular 18 e .NET 6",
            Local = "Rio de Janeiro",
            Lote = "1º Lote",
            QtdPessoas = 250,
            DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"),
            ImagemURL = "foto.png"

            },
            new Evento(){
            EventoId = 2,
            Tema = "Angular 18 e .NET 6",
            Local = "São Paulo",
            Lote = "2º Lote",
            QtdPessoas = 200,
            DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"),
            ImagemURL = "foto.png"

            }
         
        };

  

    [HttpGet()]
    public IEnumerable< Evento> Get()
    {
        return _evento;
       
      
    }
       [HttpGet("{id})")]
    public IEnumerable< Evento> Get(int id)
    {
        return _evento.Where(evento => evento.EventoId == id);
       
      
    }
}
