import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Palestrante } from '@pp/models/palestrante';
import { PalestranteService } from '@pp/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map, tap } from 'rxjs';

@Component({
  selector: 'app-palestrante-detalhe',
  templateUrl: './palestrante-detalhe.component.html',
  styleUrls: ['./palestrante-detalhe.component.scss']
})
export class PalestranteDetalheComponent implements OnInit {
  public form!: FormGroup;
  public situacaoDoForm = '';
  public corDaDescricao = '';


  constructor(
    private fb: FormBuilder,
    public palestranteService: PalestranteService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.validation();
    this.verificaForm();
    this.carregarConvidado();
  }
  private validation(): void{
    this.form = this.fb.group({
      miniCurriculo: ['']
    })
  }
  public get f(): any{
    return this.form.controls;
  }

  private carregarConvidado(): void{
  this.spinner.show();
  this.palestranteService
  .getPalestrante()
  .subscribe({
    next: (palestrante: Palestrante) =>{
      this.form.patchValue(palestrante);
    },
    error: (error: any) =>{
      this.toastr.error('Erro ao Carregar os convidados', 'Error')
    }
  })
  }
  private verificaForm(): void{
    this.form.valueChanges
    .pipe(
      map(() =>{
        this.situacaoDoForm = 'MiniCurriculo está sendo Atualizado!'
        this.corDaDescricao = 'text-warning'
      }),
      debounceTime(1000),
      tap(() => this.spinner.show())
    ).subscribe({
      next: () =>{
        this.palestranteService
        .put({ ...this.form.value})
        .subscribe({
          next: () =>{
            this.situacaoDoForm = 'minicurriculo foi Atualizado'
            this.corDaDescricao = 'text-success'; 
            setTimeout(() =>{
              this.situacaoDoForm = 'minicurriculo foi Carregado'
              this.corDaDescricao = 'text-muted'; 

            }, 2000);           
          },
          error:() =>{
            this.toastr.error('Error ao tentar atualizar Palestrante', 'Erro');
            
          },
          complete: () =>{
            this.spinner.hide()
          }
        })     
      }
        
      
    })
  }


}
