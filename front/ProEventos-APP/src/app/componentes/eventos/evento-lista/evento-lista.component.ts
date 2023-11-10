import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/evento';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {
  modalRef!: BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public eventoId!: number 
  widthImg: number = 150;
  marginImg: number = 2;
  exibirImagem: boolean = true;
  private _filtroLista: string = '';

  public get filtroLista() {
    return this._filtroLista;
  }
  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }
  public filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string; local: string }) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  constructor(private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
    ) { }

  public ngOnInit(): void {
    this.spinner.show();
    this.getEventos();
  
  }
  
  public retornaImagem(imagemURL: string): string{
    return(imagemURL !== '')
    ? `${environment.apiURL}resources/images/${imagemURL}`
    : 'assets/img/semimagem.jpeg';    
  }
  public alterarImagem(): void {
    this.exibirImagem = !this.exibirImagem;
  }
  public getEventos(): void {
   this.eventoService.getEventos().subscribe({
    next: (eventos: Evento[]) =>{
      this.eventos = eventos;
      this.eventosFiltrados = this.eventos;
    },
    error: (error: any) => {
      this.spinner.hide();
      this.toastr.error('Erro ao Carregar os Eventos', 'Error!')
    },
    complete: () => this.spinner.hide()
   });


  }
  openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirm(): void {
    this.modalRef.hide();
    this.spinner.show();


    this.eventoService.deleteEvento(this.eventoId).subscribe(
   (result : any) =>{
      if (result.message === 'Deletado'){
        this.toastr.success('O Evento FOI deletado com Sucesso.','Deletado!')
        this.getEventos();
      }    
   },
   (error: any) => {
    console.error(error)
    this.toastr.error(`Erro ao tentar deleta o evento ${this.eventoId}`,'ERROR');
   }   
    ).add(() => this.spinner.hide());
    
  }

  decline(): void {
   
    this.modalRef?.hide();
  }
  detalheEvento(id: number): void {
    this.router.navigate([`evento/detalhe/${id}`]);
}

}
