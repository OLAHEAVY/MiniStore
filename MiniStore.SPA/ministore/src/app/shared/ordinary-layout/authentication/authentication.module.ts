import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgxLoadingModule } from 'ngx-loading';
import { ShowHidePasswordModule } from 'ngx-show-hide-password';
import { RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';
import {JwtHelper} from 'angular-jwt';


import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthenticationService } from 'src/app/service/authentication.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgxLoadingModule,
    FormsModule,
    ShowHidePasswordModule,
    RouterModule.forChild([
        { path: 'login', component: LoginComponent },
        { path: 'register', component: RegisterComponent}
    ]),
    ToastrModule.forRoot({
      preventDuplicates: false,
      timeOut: 10000,
      positionClass: "toast-top-center"
  })
  ],
  exports: [],
  declarations: [LoginComponent, RegisterComponent],
  providers: [JwtHelper, AuthenticationService]
})
export class AuthenticationModule {}
