import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidationField } from '@pp/helpers/ValidationField';
import { UserUpdate } from '@pp/models/identity/UserUpdate';
import { AccountService } from '@pp/services/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  userUpdate = {} as UserUpdate;
  form = {} as FormGroup;


  
  constructor(
    private fb: FormBuilder,
    public accountService: AccountService,
    private router: Router,
    private toaster: ToastrService,
    private spinner: NgxSpinnerService
  ) {}

  get f(): any {
    return this.form.controls;
  }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService.getUser().subscribe({
      next: (userRetorno: UserUpdate) => {
        console.log(userRetorno);
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
        this.toaster.success('Usuário Carregado', 'Sucesso');
      },
      error: (error: any) => {
        console.error(error);
        this.toaster.error('Usuário não Carregado', 'Erro');
        this.router.navigate(['/dashboard']);
      },
      complete: () => this.spinner.hide(),
    });
  }

  private validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidationField.MustMatch('password', 'confirmePassword'),
    };
    this.form = this.fb.group({
      userName: [''],
      imagemURL: [''],
      titulo: ['NaoInformado', Validators.required],
      primeiroNome: ['', Validators.required],
      ultimoNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
      descricao: ['', Validators.required],
      funcao: ['NaoInformado', Validators.required],
      password: ['', [Validators.minLength(4)]],
      confirmePassword: ['', Validators.required],
    }, formOptions);
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  onSubmit(): void {
  this.atualizarUsuario();
    }
public atualizarUsuario() {
  this.userUpdate = { ...this.form.value}
  this.spinner.show();

  this.accountService.updateUser(this.userUpdate).subscribe({
    next: () =>{
      this.toaster.success('Usuario atualizado!', 'Sucesso')
    },
    error: (error: any) => {      
      this.toaster.error(error.error);
     
     
    },
    complete: () => this.spinner.hide(),
  });
  }
}
  




