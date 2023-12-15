import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';
import { PaginatedResult, Pagination } from '@pp/models/Pagination';
import { Palestrante } from '@pp/models/palestrante';
import { PalestranteService } from '@pp/services/palestrante.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject, debounceTime } from 'rxjs';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss']
})
export class PalestranteListaComponent implements OnInit {
  public pagination = {} as Pagination;
  
  public Palestrantes: Palestrante[] = [];
  public eventoId!: 0;

  constructor(private palestranteService: PalestranteService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }
  termoBuscaChanged: Subject<string> = new Subject<string>();

 public ngOnInit(): void{
  this.pagination = {
    currentPage: 1,
    itemsPerPage: 3,
    totalItems: 1,
  } as Pagination;
  this.getPalestrantes();
  }
  public filtrarPalestrantes(event: any): void {
    if (this.termoBuscaChanged.observers.length == 0) {
      
      this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(
        filtraPor => {
          this.spinner.show();
          this.palestranteService.getPalestrantes(this.pagination.currentPage,
            this.pagination.itemsPerPage,
            filtraPor
          ).subscribe(
            (paginatedResult: PaginatedResult<Palestrante[]>) => {
              this.Palestrantes = paginatedResult.result;

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
  public getImagemURL(imagemName: string): string{
    if(imagemName)
    return environment.apiURL + `resources/perfil/${imagemName}`;
  else{
    return '../../../../assets/img/semImagem.jpeg';
  }
  }
  public getPalestrantes(): void {
    this.spinner.show();

    this.palestranteService
      .getPalestrantes(this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe(
        (paginatedResult: PaginatedResult<Palestrante[]>) => {
          this.Palestrantes = paginatedResult.result;
          this.pagination = paginatedResult.pagination;
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
        }
      )
      .add(() => this.spinner.hide());


  }

}
