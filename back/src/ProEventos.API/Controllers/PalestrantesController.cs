using Microsoft.AspNetCore.Mvc;
using ProEventos.Persistence;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Application.Contratos;


using ProEventos.Application.Dto;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Persistence.models;
using ProEventos.API.Helpers;


namespace ProEventos.API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PalestrantesController : ControllerBase
{
    private readonly IPalestranteService _palestranteService;

    private readonly IAccountService _accountService;
    private readonly IUtil _util;

    private readonly string _destino = "images";

    public  PalestrantesController(IPalestranteService palestranteService,
    IAccountService accountService, IUtil util )
    {
    _util = util;
        _palestranteService = palestranteService;

        _accountService = accountService;

    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams)
    {
        try
        {
            var palestrantes = await _palestranteService.GetAllPalestrantesAsync(pageParams, true);
            if (palestrantes == null)return NoContent();
           
           Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPages);

            return Ok(palestrantes);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
        }
       
      
    }
  
       [HttpGet()]
    public async Task<IActionResult>GetPalestrante()
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), true);
            if (palestrante ==null)return NoContent();
            return Ok(palestrante);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
        }
       
      
    }   

    [HttpPost]

    public async Task <IActionResult> Post(PalestranteAddDto model)
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);
            if(palestrante == null)
             palestrante = await _palestranteService.AddPalestrantes(User.GetUserId(), model);
         
            return Ok(palestrante);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar Palestrante. Erro: {ex.Message}");
        }
    }


    [HttpPut]

    public async Task <IActionResult> Put(PalestranteUpdateDto model)
    {
        try
        {
            var palestrante =await _palestranteService.UpdatePalestrantes(User.GetUserId(), model);
            if (palestrante == null)return NoContent();
            return Ok(palestrante);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar atualizar palestrante. Erro: {ex.Message}");
        }
    }    
         

        
}

    
