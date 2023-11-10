import { Component, EnvironmentInjector, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environments/environment';
import { Evento } from '@pp/models/evento';
import { Lote } from '@pp/models/lote';
import { EventoService } from '@pp/services/evento.service';
import { LoteService } from '@pp/services/lote.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  modalRef: BsModalRef;
  form!: FormGroup;
  evento = {} as Evento;
  estadoSalvar = 'post';
  eventoId!: number;
  loteAtual = {id: 0, nome: '', indice: 0}
  imagemURL = 'assets/img/upload.png';
  file: File;



  get modoEditar(): boolean {
    return this.estadoSalvar === 'put';
  }
  // adcionar lotes
  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

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
  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,

    private modalService: BsModalService,
    private loteService: LoteService) {

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
      imagemURL: [''],
      lotes: this.fb.array([]),
    });
  }

  adicionarLote(): void {
    this.lotes.push(this.criarLote({ id: 0 } as Lote));
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    });
  }
  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any {
    return { "is-invalid": campoForm.errors && campoForm.touched }
  }
  public carregarEvento(): void {
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');

    if (this.eventoId !== null && this.eventoId !== 0) {
      
      this.estadoSalvar = 'put';

      this.eventoService.getEventoById(this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
          if (this.evento.imagemURL !== '') {
            this.imagemURL = environment.apiURL + 'resources/images/' + this.evento.imagemURL;
          }
          this.carregarLotes();
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao tentar Carregar Evento', 'Error!');
          console.error(error)
        },

      ),
        () => this.spinner.hide();
    }
  }

  public carregarLotes(): void{
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
     (lotesRetorno: Lote[]) =>{
lotesRetorno.forEach(lote =>{
  this.lotes.push(this.criarLote(lote));
});
     },
     (error: any) =>{
      this.toastr.error('Error ao tenta Carregar Lotes', 'Error');
      console.error(error)
     }
    ).add(()=> this.spinner.hide());
  }

  public salvarEvento(): void {
    this.spinner.show();
    if (this.form.valid) {
      this.evento = (this.estadoSalvar === 'post')
        ? { ...this.form.value }
        : { id: this.evento.id, ...this.form.value };
      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.toastr.success('Evento salvo com Sucesso!', 'Sucesso');
          this.router.navigate([`evento/detalhe/${eventoRetorno.id}`]);
        },
        (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Error ao salvar evento', 'Erro');
        },
        () => this.spinner.hide()
      );
    }
  }
  public salvarLotes(): void {
       if (this.form.controls['lotes'].valid) {
        this.spinner.show();
      
      this.loteService
        .saveLotes(this.eventoId, this.form.value.lotes)
        .subscribe(
          () => {
            this.toastr.success('Lotes salvos com Sucesso!', 'Sucesso!');
          },
          (error: any) => {
            this.toastr.error('Erro ao tentar salvar lotes.', 'Erro');
            console.error(error)

          }
        )
        .add(() => this.spinner.hide());
    }
    
  }
  public removerLote(template: TemplateRef<any>, indice: number): void {
    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }
  confirmDeleteLote(): void{
    this.modalRef.hide();
    this.spinner.show();

    this.loteService.deleteLote(this.eventoId, this.loteAtual.id)
    .subscribe(
      () =>{
        this.toastr.success('Lote detelado com Sucesso', 'Sucesso')
        this.lotes.removeAt(this.loteAtual.indice);
      },
      (error: any) => {
        this.toastr.error(`Error ao tenta Deletar o Lote  ${this.loteAtual.id}`, 'Error')
        console.error(error)
      }
    ) .add(() => this.spinner.hide());

  }
  declineDeleteLote(): void{
    this.modalRef.hide();

  }
  public retornaTituloLote(nome: string): string{
 return nome == null || nome == '' ? 'Nome do Lote' : nome;

  }
  onFileChange(ev: any): void{
    const reader = new FileReader();
    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImagen();
  }
  uploadImagen(): void{
    this.spinner.show();
    this.eventoService.postUpload(this.eventoId, this.file).subscribe(
() => {
  this.carregarEvento();
  this.toastr.success('Imagem atualizada com Sucesso','Sucessos!')
},
(error: any) => {
  this.toastr.error('Erro ao carregar imagem', 'Erro!')
  console.error(error)
}
    )
    .add(() => this.spinner.hide());
  }
}

