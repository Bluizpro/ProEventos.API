import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@pp/models/evento';
import { EventoService } from '@pp/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  form!: FormGroup;
  evento = {} as Evento;
  modoSalvar = 'post'; 
  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false


    }
  }
  constructor(private fb: FormBuilder, private localeService: BsLocaleService,
    private router: ActivatedRoute, private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService) {
    this.localeService.use('pt-br')
  }




  ngOnInit(): void {
    this.validation();
    this.carregarEvento();

  }
  public validation(): void {
    this.form = this.fb.group({
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemURL: ['', Validators.required],
      lotes: this.fb.array([]),
    });
  }
  public resetForm(): void {
    this.form.reset();
  }
  public salvarEvento(): void {
    this.form.reset();
  }
  public cssValidator(campoForm: FormControl): any {
    return { "is-invalid": campoForm.errors && campoForm.touched }
  }
  public carregarEvento(): void {
    const eventoIdParam = this.router.snapshot.paramMap.get('id');
    if (eventoIdParam !== null) {
      this.spinner.show();
      this.modoSalvar = 'put'; 
      this.eventoService.getEventoById(+eventoIdParam).subscribe(
        {
          next: (evento: Evento) => {
            this.evento = { ...evento };
            this.form.patchValue(this.evento)
          },
          error: (error: any) => {
            this.spinner.hide();
            this.toastr.error('Erro ao tentar carregar Evento')
            console.error(error)
          },
          complete: () => this.spinner.hide(),
        });
    }
  }
  public salvarAlteracao() {
    this.spinner.show();
    if(this.form.valid){
          if(this.modoSalvar === 'post'){
        this.evento = {...this.form.value}
        this.eventoService.postEvento(this.evento).subscribe(
          () => this.toastr.success('Evento salvo com sucesso!', 'Sucessor'),
          (error: any) =>{
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Error ao salvar evento', 'Error')
          },
          () => this.spinner.hide()      
        );
      }else{
        this.evento = {id: this.evento.id, ...this.form.value}
        this.eventoService.putEvento(this.evento.id, this.evento).subscribe(
          () => this.toastr.success('Evento salvo com sucesso!', 'Sucessor'),
          (error: any) =>{
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Error ao salvar evento', 'Error')
          },
          () => this.spinner.hide()
        );
      }   
      
    }
  }

}
