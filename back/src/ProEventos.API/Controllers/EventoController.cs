using Microsoft.AspNetCore.Mvc;
using ProEventos.Persistence;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Application.Contratos;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly IEventoService _eventoService;

    public EventosController(IEventoService eventoService)
    {
        _eventoService = eventoService;
    }

    [HttpGet]
    public async Task<IActionResult> get()
    {
        try
        {
            var eventos =await _eventoService.GetAllEventosAsync(true);
            if (eventos ==null)return NotFound("Nenhum evento encontrado");
            return Ok(eventos);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
        }
       
      
    }
  
       [HttpGet("{id}")]
    public async Task<IActionResult>GetById(int id)
    {
        try
        {
            var evento =await _eventoService.GetEventoByIdAsync(id, true);
            if (evento ==null)return NotFound("Nenhum evento encontrado");
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
        }
       
      
    }

         [HttpGet("{tema}/tema")]
    public async Task<IActionResult>GetByTema(string tema)
    {
        try
        {
            var evento =await _eventoService.GetAllEventosByTemaAsync(tema, true);
            if (evento ==null)return NotFound("Nenhum Tema encontrado");
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar Temas. Erro: {ex.Message}");
        }
       
      
    }
    [HttpPost]

    public async Task <IActionResult> Post(Evento model)
    {
        try
        {
            var evento =await _eventoService.AddEventos(model);
            if (evento == null)return BadRequest("Erro ao tentar adicionar evento");
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar Temas. Erro: {ex.Message}");
        }
    }
    [HttpPut("{id}")]

    public async Task <IActionResult> Put(int id,Evento model)
    {
        try
        {
            var evento =await _eventoService.UpdateEventos(id,model);
            if (evento == null)return BadRequest("Erro ao tentar atualizar evento");
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar atualizar evento. Erro: {ex.Message}");
        }
    }
    [HttpDelete("{id}")]

    public async Task <IActionResult> Delete(int id)
    {
        try
        {
            if( await _eventoService.DeleteEventos(id))   
             return Ok("Evento deletado");
            else
             return BadRequest("Evento não deletado");                  
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar deletar evento. Erro: {ex.Message}");
        }
    }
}
