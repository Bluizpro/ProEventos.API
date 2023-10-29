import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationField } from '@pp/helpers/ValidationField';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  form!: FormGroup
  
  constructor(public fb: FormBuilder){}  
  get f(): any {return this.form.controls}

  ngOnInit(): void {
    this.validation();
  
  }
  private validation(): void{
    const formOptions: AbstractControlOptions ={
      validators: ValidationField.MustMatch('password',' confirmePassword')
    };
    this.form = this.fb.group(
      {
        userName: [''],
        imagemURL: [''],
        titulo: ['NaoInformado', Validators.required],
        primeiroNome: ['', Validators.required],
        ultimoNome: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phoneNumber: ['', [Validators.required]],
        descricao: ['', Validators.required],
        funcao: ['NaoInformado', Validators.required],
        password: ['', [Validators.required, Validators.minLength(6)]
      ],
      confirmePassword: ['', Validators.required],
    }, formOptions);
    
  }
  public resetForm(event: any): void{
    event.preventDefault();
    this.form.reset();
  }



}
