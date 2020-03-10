import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { HttpClientModule } from "@angular/common/http";
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgxLoadingModule } from 'ngx-loading';


import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

@NgModule({
    imports:[CommonModule, 
                  HttpClientModule,
                  ReactiveFormsModule,
                  FormsModule,
                  NgxLoadingModule,
                  RouterModule.forChild([
                      {path:'login', component: LoginComponent},
                      {path:'register', component: RegisterComponent}
                  ])],
    declarations: [LoginComponent, RegisterComponent],
    exports: [],
    providers: []
})

export class AuthenticationModule{}