using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Dto
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string? Local { get; set; }
        public string? DataEvento { get; set; }

        [Required(ErrorMessage = "O {0} e Obrigatorio"),StringLength(50, MinimumLength = 4, ErrorMessage ="Intervalo permitido e de 4 a 50 caracteres.")]
        
        public string? Tema { get; set; }
        [Display(Name ="Qtd Pessoas")]
        [Range(1, 12000, ErrorMessage ="{0} Não pode ser menor quue 1 e maior que 120.000")]
        public int? QtdPessoas { get; set; }
        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$",        
                          ErrorMessage ="Não e uma imagom valida! (gif, jpg, jpeg,  bmp, ou png )")]
        public string? ImagemURL { get; set; }
        [Required(ErrorMessage ="O campo {0} e obrigatorio")]
        [Phone(ErrorMessage ="O campo {0} esta com numero Invalido!")]
        public string? Telefone { get; set; }
        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [Display(Name = "e-mail")]
        [EmailAddress(ErrorMessage = "o campo {0} deve ser valido!")]
        public string? Email { get; set; }
        public IEnumerable<LoteDto>? Lotes { get; set; }
        public IEnumerable<RedesSocialDto>? RedesSociais { get; set; }
        public IEnumerable<PalestranteDto>? PalestrantesEventos { get; set; }
    }
}