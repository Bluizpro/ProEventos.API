import { CurrencyPipe } from '@angular/common';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';
import { PaginatedResult, Pagination } from '@pp/models/Pagination';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject, debounceTime } from 'rxjs';
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
  public eventoId!: number
  widthImg: number = 150;
  marginImg: number = 2;
  exibirImagem: boolean = true;
  public pagination = {} as Pagination

  termoBuscaChanged: Subject<string> = new Subject<string>();


  public filtrarEventos(event: any): void {
    if (this.termoBuscaChanged.observers.length == 0) {
      
      this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(
        filtraPor => {
          this.spinner.show();
          this.eventoService.getEventos(this.pagination.currentPage,
            this.pagination.itemsPerPage,
            filtraPor
          ).subscribe(
            (paginatedResult: PaginatedResult<Evento[]>) => {
              this.eventos = paginatedResult.result;

              this.pagination = paginatedResult.pagination;
            },
            (error: any) => {
              this.spinner.hide();
              this.toastr.error('Erro ao Carregar os Eventos', 'Error!')
            }
          ).add(() => this.spinner.hide())
        }); 
    }
    this.termoBuscaChanged.next(event.value);
  }


  constructor(private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 3,
      totalItems: 1,
    } as Pagination;
    this.getEventos();
  }

  public retornaImagem(imagemURL: string): string {
    return (imagemURL !== '')
      ? `${environment.apiURL}resources/images/${imagemURL}`
      : 'assets/img/semimagem.jpeg';
  }
  public alterarImagem(): void {
    this.exibirImagem = !this.exibirImagem;
  }
  public getEventos(): void {
    this.spinner.show();

    this.eventoService.getEventos(this.pagination.currentPage,
      this.pagination.itemsPerPage).subscribe({
        next: (paginatedResult: PaginatedResult<Evento[]>) => {
          this.eventos = paginatedResult.result;

          this.pagination = paginatedResult.pagination;
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
  public pageChanged(event): void {
    this.pagination.currentPage = event.page;
    this.getEventos();

  }

  confirm(): void {
    this.modalRef.hide();
    this.spinner.show();


    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        if (result.message === 'Deletado') {
          this.toastr.success('O Evento FOI deletado com Sucesso.', 'Deletado!')
          this.getEventos();
        }
      },
      (error: any) => {
        console.error(error)
        this.toastr.error(`Erro ao tentar deleta o evento ${this.eventoId}`, 'ERROR');
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
