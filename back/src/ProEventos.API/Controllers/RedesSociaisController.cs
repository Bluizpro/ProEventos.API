using Microsoft.AspNetCore.Mvc;
using ProEventos.Persistence;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Application.Contratos;

using ProEventos.Application.Dto;
using ProEventos.Application;
using Microsoft.AspNetCore.Authorization;
using ProEventos.API.Extensions;


namespace ProEventos.API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RedesSociaisController : ControllerBase
{
    private readonly IRedeSocialService _redeSocialService;
    private readonly IEventoService _eventoService;
    private readonly IPalestranteService _palestranteService;

    public RedesSociaisController(IRedeSocialService redeSocialService,
                                  IEventoService eventoService, IPalestranteService palestranteService)
    {
        _palestranteService = palestranteService;
        _redeSocialService = redeSocialService;
        _eventoService = eventoService;
    }

    [HttpGet("evento/{eventoId}")]
    public async Task<IActionResult> GetByEvento(int eventoId)
    {
        try
        {
            if (!(await AutorEvento(eventoId)))
            return Unauthorized();
            var redeSocial = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
            if (redeSocial == null) return NoContent();

            return Ok(redeSocial);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar Evento. Erro: {ex.Message}");
        }
    }
     [HttpGet("palestrante")]
    public async Task<IActionResult> GetByPalestrante()
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if(palestrante == null) return Unauthorized();

            var redeSocial = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
            if (redeSocial == null) return NoContent();

            return Ok(redeSocial);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar Palestrante. Erro: {ex.Message}");
        }
    }

    [HttpPut("evento/{eventoId}")]
    public async Task<IActionResult> SaveByEvento(int eventoId, RedesSocialDto[] models)
    {
        try
        {
              if (!(await AutorEvento(eventoId)))
            return Unauthorized();
            var redeSocial = await _redeSocialService.SaveByEvento(eventoId, models);
            if (redeSocial == null) return NoContent();

            return Ok(redeSocial);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar salvar Evento. Erro: {ex.Message}");
        }
    }

    [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedesSocialDto[] models)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null) return Unauthorized();

                var redeSocial = await _redeSocialService.SaveByPalestrante(palestrante.Id, models);
                if (redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar Rede Social por Palestrante. Erro: {ex.Message}");
            }
        }
    

    [HttpDelete("evento/{eventoId}/{redeSocialId}")]
    public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
    {
        try
        {
              if (!(await AutorEvento(eventoId)))
            return Unauthorized();
            var redesSocial = await _redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
            if (redesSocial == null) return NoContent();

            return await _redeSocialService.DeleteByEvento(eventoId, redeSocialId)
                   ? Ok(new { message = "Rede Social Deletada" })
                   : throw new Exception("Ocorreu um problem não específico ao tentar deletar rede Social por evento.");
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar rede Social. Erro: {ex.Message}");
        }
        
    }
     [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null) return Unauthorized();

                var RedeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, redeSocialId);
                if (RedeSocial == null) return NoContent();

                return await _redeSocialService.DeleteByPalestrante(palestrante.Id, redeSocialId) 
                       ? Ok(new { message = "Rede Social Deletada" }) 
                       : throw new Exception("Ocorreu um problem não específico ao tentar deletar Rede Social por Palestrante.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar Rede Social por Palestrante. Erro: {ex.Message}");
            }
        }

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId)
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
            if (evento == null) return false;

            return true;
        }
    }
        
    



    




