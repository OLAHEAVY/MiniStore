import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { HttpClientModule } from "@angular/common/http";
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgxLoadingModule } from 'ngx-loading';
import { BsDatepickerModule} from 'ngx-bootstrap';



import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ToastrModule } from 'ngx-toastr';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { JwtHelper } from 'angular2-jwt';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
    imports:[CommonModule, 
                  HttpClientModule,
                  ReactiveFormsModule,
                  FormsModule,
                  NgxLoadingModule,
                //   ToastrModule.forRoot({
                //     preventDuplicates: false,
                //     timeOut: 10000,
                //     positionClass: "toast-top-center"
                // }),
                  BsDatepickerModule.forRoot(),
                  RouterModule.forChild([
                      {path:'login', component: LoginComponent},
                      {path:'register', component: RegisterComponent}
                  ])],
    declarations: [LoginComponent, RegisterComponent],
    exports: [],
    providers: [AuthenticationService, JwtHelper]
})

export class AuthenticationModule{}