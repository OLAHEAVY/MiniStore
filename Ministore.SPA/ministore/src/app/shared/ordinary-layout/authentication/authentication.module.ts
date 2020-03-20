import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { HttpClientModule } from "@angular/common/http";
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgxLoadingModule } from 'ngx-loading';
import { BsDatepickerModule, BsDropdownModule, CarouselModule, ModalModule} from 'ngx-bootstrap';



import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ToastrModule } from 'ngx-toastr';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { JwtHelper } from 'angular2-jwt';
import { OrdinaryHeaderComponent } from './ordinary-header/ordinary-header.component';
import { OrdinaryFooterComponent } from './ordinary-footer/ordinary-footer.component';
import { HomeComponent } from './home/home.component';
import { ContactModalComponent } from './contact-modal/contact-modal.component';
import { ConfirmAccountResolver } from 'src/app/_resolver/confirm-account.resolver';
import { ConfirmAccountComponent } from './confirm-account/confirm-account.component';
import { SendEmailconfirmationComponent } from './send-emailconfirmation/send-emailconfirmation.component';

@NgModule({
    imports:[CommonModule, 
                  HttpClientModule,
                  ReactiveFormsModule,
                  FormsModule,
                  NgxLoadingModule,
                  BsDropdownModule.forRoot(),
                  BsDatepickerModule.forRoot(),
                  CarouselModule.forRoot(),
                  ModalModule.forRoot(),
                  RouterModule.forChild([
                      {path:'login', component: LoginComponent},
                      {path:'register', component: RegisterComponent},
                      {path: 'home', component: HomeComponent},
                      {path: 'resend-confirmation', component: SendEmailconfirmationComponent},
                      {
                        path: 'authentication/confirm-account',
                        resolve: { data: ConfirmAccountResolver },
                        component: ConfirmAccountComponent
                    }
                  ])],
    declarations: [LoginComponent, RegisterComponent, OrdinaryHeaderComponent, ContactModalComponent, OrdinaryFooterComponent, HomeComponent, ConfirmAccountComponent,SendEmailconfirmationComponent],
    entryComponents: [
        ContactModalComponent
     ],
    exports: [OrdinaryHeaderComponent,OrdinaryFooterComponent],
    providers: [AuthenticationService, JwtHelper,ConfirmAccountResolver]
})

export class AuthenticationModule{}