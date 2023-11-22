using Microsoft.AspNetCore.Mvc;
using ProEventos.Persistence;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Application.Contratos;


using ProEventos.Application.Dto;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;


namespace ProEventos.API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly IEventoService _eventoService;
    private readonly IAccountService _accountService;
    private readonly IWebHostEnvironment _hostEnvironment;

    public EventosController(IEventoService eventoService,
    IAccountService accountService, IWebHostEnvironment hostEnvironment )
    {
    _hostEnvironment = hostEnvironment;
        _eventoService = eventoService;
        _accountService = accountService;

    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos =await _eventoService.GetAllEventosAsync(User.GetUserId(), true);
            if (eventos == null)return NoContent();
           
           

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
            var evento =await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
            if (evento ==null)return NoContent();
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
            var evento =await _eventoService.GetAllEventosByTemaAsync(User.GetUserId(), tema, true);
            if (evento ==null)return NoContent();
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar Temas. Erro: {ex.Message}");
        }
       
      
    }
    [HttpPost("upload-image/{eventoId}")]

    public async Task <IActionResult> UploadImage(int eventoId)
    {
        try
        {
           var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
            if (evento == null)return NoContent();

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                 DeleteImage(evento.ImagemURL);
                evento.ImagemURL = await SaveImage(file);

            }
            var EventoRetorno = await _eventoService.UpdateEventos(User.GetUserId(), eventoId, evento);
            return Ok(EventoRetorno);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar Temas. Erro: {ex.Message}");
        }
    }

    [HttpPost]

    public async Task <IActionResult> Post(EventoDto model)
    {
        try
        {
            var evento =await _eventoService.AddEventos(User.GetUserId(), model);
            if (evento == null)return NoContent();
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar Temas. Erro: {ex.Message}");
        }
    }


    [HttpPut("{id}")]

    public async Task <IActionResult> Put(int id,EventoDto model)
    {
        try
        {
            var evento =await _eventoService.UpdateEventos(User.GetUserId(), id,model);
            if (evento == null)return NoContent();
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar atualizar evento. Erro: {ex.Message}");
        }
    }
    [HttpDelete("{id}")]

   public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();

              if(await _eventoService.DeleteEventos(User.GetUserId(), id)) 
              {
                DeleteImage(evento.ImagemURL);
              return Ok(new { message = "Deletado" });  

              }
              else
              {
                throw new Exception("Ocorreu um problem não específico ao tentar deletar Evento.");

              }                    
                 
                 
                
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar eventos. Erro: {ex.Message}");
            }
            
        }
         [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ','-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);
            using(var fileStream = new FileStream(imagePath, FileMode.Create)){
                await imageFile.CopyToAsync(fileStream);
            }
           return imageName;
        }
        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);
            if(System.IO.File.Exists(imagePath))
            System.IO.File.Delete(imagePath);
        }

        
}

    
